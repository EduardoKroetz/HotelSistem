using Hotel.Domain.Entities.Interfaces;
using Hotel.Domain.Services.Interfaces;
using Hotel.Domain.ValueObjects;
using Newtonsoft.Json;
using Stripe;

namespace Hotel.Domain.Services;

public class StripeService : IStripeService
{
    private readonly CustomerService _stripeCustomerService;
    private readonly ProductService _stripeProductService;
    private readonly PriceService _stripePriceService;
    private readonly PaymentIntentService _stripePaymentIntentService;
    private readonly PaymentMethodService _stripePaymentMethodService;
    private readonly ILogger<StripeService> _logger;

    public StripeService(ILogger<StripeService> logger)
    {
        _stripeCustomerService = new CustomerService();
        _stripeProductService = new ProductService();
        _stripePriceService = new PriceService();
        _stripePaymentIntentService = new PaymentIntentService();
        _stripePaymentMethodService = new PaymentMethodService();
        _logger = logger;
    }

    //Customer
    public async Task<Customer> CreateCustomerAsync(Name name, Email email, Phone phone, ValueObjects.Address? address)
    {
        var options = new CustomerCreateOptions
        {
            Name = name.GetFullName(),
            Email = email.Address,
            Address = new AddressOptions()
            {
                City = address?.City ?? "",
                Country = address?.Country ?? "",
            },
            Phone = phone.Number
        };

        var customer = await _stripeCustomerService.CreateAsync(options);
        _logger.LogInformation($"Cliente stripe foi criado. Id: {customer.Id}");
        return customer;
    }

    public async Task<bool> DeleteCustomerAsync(string customerId)
    {
        var isDeleted = await _stripeCustomerService.DeleteAsync(customerId) is null ? false : true;
        if (isDeleted)
        {
            _logger.LogInformation("Cliente stripe foi deletado");
        }else
        {
            _logger.LogError("Tentativa de deletar cliente stripe mal sucedida");
        }
        return isDeleted;
    }

    public async Task<Customer> GetCustomerAsync(string customerId)
    {
        return await _stripeCustomerService.GetAsync(customerId);
    }

    public async Task<Customer> UpdateCustomerAsync(string customerId, Name name, Email email, Phone phone, ValueObjects.Address? address)
    {
        var options = new CustomerUpdateOptions
        {
            Name = name.GetFullName(),
            Email = email.Address,
            Address = new AddressOptions()
            {
                City = address?.City ?? "",
                Country = address?.Country ?? "",
            },
            Phone = phone.Number
        };

        var customer = await _stripeCustomerService.UpdateAsync(customerId, options);
        _logger.LogInformation($"Cliente stripe com o id {customer.Id} foi atualizado");
        return customer;
    }


    //Room
    public async Task<Product> CreateProductAsync(string name, string description, decimal price)
    {

        var productOptions = new ProductCreateOptions()
        {
            Active = true,
            Name = name,
            Description = description,
        };

        var product = await _stripeProductService.CreateAsync(productOptions);
        _logger.LogInformation($"Produto criado no Stripe. Id: {product.Id}");

        var priceCreateOptions = new PriceCreateOptions
        {
            Currency = "BRL",
            UnitAmountDecimal = price * 100,
            Product = product.Id
        };

        var stripePrice =  await _stripePriceService.CreateAsync(priceCreateOptions);
        _logger.LogInformation($"Preço criado no Stripe. Id: {stripePrice.Id}");
        return product;
    }

    public async Task<Product> DisableProductAsync(string productId)
    {
        var product = await _stripeProductService.GetAsync(productId);

        var productUpdateOptions = new ProductUpdateOptions
        {
            Active = false,
        };

        await _stripeProductService.UpdateAsync(productId, productUpdateOptions);
        return product;
    }

    public async Task<Product> GetProductAsync(string productId)
    {
        return await _stripeProductService.GetAsync(productId);
    }

    public async Task<Product> UpdateProductAsync(string productId, string name, string description, decimal price, bool isActive = true)
    {
        var product = await _stripeProductService.GetAsync(productId);

        var priceListOptions = new PriceListOptions
        {
            Product = product.Id,
            Active = true
        };

        var activePrice = _stripePriceService.ListAsync(priceListOptions).Result.First();

        if (price * 100 != activePrice.UnitAmountDecimal)
        {
            var priceUpdateOptions = new PriceUpdateOptions
            {
                Active = false
            };

            await _stripePriceService.UpdateAsync(activePrice.Id, priceUpdateOptions);

            var priceCreateOptions = new PriceCreateOptions
            {
                Currency = "BRL",
                UnitAmountDecimal = price * 100,
                Product = productId
            };

            var newPrice = await _stripePriceService.CreateAsync(priceCreateOptions);
        }

        var productUpdateOptions = new ProductUpdateOptions()
        {
            Name = name,
            Description = description,
            Active = isActive
        };

        return await _stripeProductService.UpdateAsync(productId, productUpdateOptions);
    }

    //Reservation
    public async Task<PaymentIntent> CreatePaymentIntentAsync(decimal expectedTotalAmount, string stripeCustomerId, IRoom room)
    {
        var amountInCents = (int)( expectedTotalAmount * 100 );

        var products = new List<ProductServiceInfo>
        {
            new (false, room.Id, room.StripeProductId, $"Hospedagem no cômodo {room.Name}", 1, expectedTotalAmount )
        };

        var metadata = JsonConvert.SerializeObject(products);

        var options = new PaymentIntentCreateOptions
        {
            Amount = amountInCents,
            Currency = "BRL",
            Customer = stripeCustomerId,
            CaptureMethod = "manual",
            PaymentMethodTypes = new List<string> { "card" },
            Metadata = new Dictionary<string, string>
            {
                { "products", metadata }
            }
        };

        return await _stripePaymentIntentService.CreateAsync(options);
    }

    public async Task<bool> CancelPaymentIntentAsync(string paymentIntentId)
    {
        return await _stripePaymentIntentService.CancelAsync(paymentIntentId) is null ? false : true;
    }

    public async Task<PaymentIntent> GetPaymentIntentAsync(string paymentIntentId)
    {
        return await _stripePaymentIntentService.GetAsync(paymentIntentId);
    }

    public async Task<PaymentIntent> UpdatePaymentIntentAsync(string paymentIntentId, decimal totalAmount)
    {
        var amountInCents = (int)( totalAmount * 100 );

        var options = new PaymentIntentUpdateOptions
        {
            Amount = amountInCents,
        };

        return await _stripePaymentIntentService.UpdateAsync(paymentIntentId, options);
    }

    public async Task<PaymentIntent> AddPaymentIntentProduct(string paymentIntentId, IService service)
    {
     
        var paymentIntent = await _stripePaymentIntentService.GetAsync(paymentIntentId);

        if (!paymentIntent.Metadata.TryGetValue("products", out var productMetadata))
            throw new ArgumentException("Os metadados 'products' do PaymentIntent não foram encontrados");

        var products = JsonConvert.DeserializeObject<List<ProductServiceInfo>>(productMetadata);

        var product = products.FirstOrDefault(x => x.Id == service.Id);
        if (product != null)
        {
            product.Quantity++;
        }
        else
        {
            var newProduct = new ProductServiceInfo(true, service.Id, service.StripeProductId, service.Name, 1, service.Price);
            products.Add(newProduct);
        }

        var metadata = JsonConvert.SerializeObject(products);
        var updateOptions = new PaymentIntentUpdateOptions
        {
            Metadata = new Dictionary<string, string>
            {
                { "products", metadata }
            }
        };

        return await _stripePaymentIntentService.UpdateAsync(paymentIntentId, updateOptions);
    }

    public async Task<PaymentIntent> RemovePaymentIntentProduct(string paymentIntentId, Guid serviceId)
    {
    
        var paymentIntent = await _stripePaymentIntentService.GetAsync(paymentIntentId);

        if (!paymentIntent.Metadata.TryGetValue("products", out var productMetadata))
            throw new ArgumentException("Os metadados 'products' do PaymentIntent não foram encontrados");

        var products = JsonConvert.DeserializeObject<List<ProductServiceInfo>>(productMetadata);

        var product = products.FirstOrDefault(x => x.Id == serviceId)
            ?? throw new ArgumentException("O serviço não foi encontrado nos metadados do PaymentIntent");

        if (product.Quantity > 1)
        {
            product.Quantity--;
        }
        else
        {
            products.Remove(product);
        }

        var metadata = JsonConvert.SerializeObject(products);
        var updateOptions = new PaymentIntentUpdateOptions
        {
            Metadata = new Dictionary<string, string>
            {
                { "products", metadata }
            }
        };

        return await _stripePaymentIntentService.UpdateAsync(paymentIntentId, updateOptions);
    }

    public async Task<PaymentIntent> ConfirmPaymentIntentAsync(string paymentIntentId)
    {
        var confirmOptions = new PaymentIntentConfirmOptions
        {
            ReturnUrl = $"{Configuration.BaseUrl}/success.html",
        };

        return await _stripePaymentIntentService.ConfirmAsync(paymentIntentId, confirmOptions);
    }

    public async Task<PaymentIntent> CapturePaymentIntentAsync(string paymentIntentId, IReservation reservation)
    {
        var amountToCapture = (long)( reservation.TotalAmount() * 100 );

        var captureOptions = new PaymentIntentCaptureOptions
        {
            AmountToCapture = amountToCapture
        };

        return await _stripePaymentIntentService.CaptureAsync(paymentIntentId, captureOptions);
    }


    public async Task<Price> GetFirstActivePriceByProductId(string productId)
    {
        var options = new PriceListOptions
        {
            Product = productId,
            Active = true
        };

        var prices = await _stripePriceService.ListAsync(options);

        return prices.First();
    }

    public async Task<PaymentIntent> CreatePaymentMethodAsync(string tokenId, string paymentIntentId)
    {
        var paymentMethodCreateOptions = new PaymentMethodCreateOptions
        {
            Type = "card",
            Card = new PaymentMethodCardOptions
            {
                Token = tokenId
            }
        };

        var paymentMethod = await _stripePaymentMethodService.CreateAsync(paymentMethodCreateOptions);

        var options = new PaymentIntentUpdateOptions
        {
            PaymentMethod = paymentMethod.Id,
        };

        return await _stripePaymentIntentService.UpdateAsync(paymentIntentId, options);
    }
}

public class ProductServiceInfo
{
    public bool IsService { get; set; }
    public Guid Id { get; set; }
    public string ProductId { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public ProductServiceInfo(bool isService, Guid id, string productId, string name, int quantity, decimal unitPrice)
    {
        IsService = isService;
        Id = id;
        ProductId = productId;
        Name = name;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }
}
