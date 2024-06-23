using Hotel.Domain.Entities.Interfaces;
using Hotel.Domain.Services.Interfaces;
using Stripe;

namespace Hotel.Domain.Services;

public class StripeService : IStripeService
{
    private readonly CustomerService _stripeCustomerService;
    private readonly ProductService _stripeProductService;
    private readonly PriceService _stripePriceService;
    private readonly PaymentIntentService _stripePaymentIntentService;

    public StripeService()
    {
        _stripeCustomerService = new CustomerService();
        _stripeProductService = new ProductService();
        _stripePriceService = new PriceService();
        _stripePaymentIntentService = new PaymentIntentService();
    }

    //Customer
    public async Task<Customer> CreateCustomerAsync(ICustomer customer)
    {
        var options = new CustomerCreateOptions
        {
            Name = customer.Name.GetFullName(),
            Email = customer.Email.Address,
            Address = new AddressOptions()
            {
                City = customer.Address?.City ?? "",
                Country = customer.Address?.Country ?? "",
            },
            Phone = customer.Phone.Number
        };

        return await _stripeCustomerService.CreateAsync(options);  
    }

    public async Task<bool> DeleteCustomerAsync(string customerId)
    {
        return await _stripeCustomerService.DeleteAsync(customerId) is null ? false : true;
    }

    public async Task<Customer> GetCustomerAsync(string customerId)
    {
        return await _stripeCustomerService.GetAsync(customerId);
    }

    public async Task<Customer> UpdateCustomerAsync(string customerId, ICustomer customer)
    {
        var options = new CustomerUpdateOptions
        {
            Name = customer.Name.GetFullName(),
            Email = customer.Email.Address,
            Address = new AddressOptions()
            {
                City = customer.Address?.City ?? "",
                Country = customer.Address?.Country ?? "",
            },
            Phone = customer.Phone.Number
        };
        
        return await _stripeCustomerService.UpdateAsync(customerId, options);
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

        var priceCreateOptions = new PriceCreateOptions
        {
            Currency = "BRL",
            UnitAmountDecimal = price * 100,
            Product = product.Id
        };

        await _stripePriceService.CreateAsync(priceCreateOptions);

        return product;
    }

    public async Task<Product> DisableProductAsync(string productId)
    {
        var product = await _stripeProductService.GetAsync(productId);

        var productUpdateOptions = new ProductUpdateOptions
        {
            Active = false,
        };

       return await _stripeProductService.UpdateAsync(productId, productUpdateOptions);   
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

        var activePrice  = _stripePriceService.ListAsync(priceListOptions).Result.First();

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
    public async Task<PaymentIntent> CreatePaymentIntentAsync(decimal expectedTotalAmount, string stripeCustomerId, Guid roomId)
    {
        var amountInCents = (int) (expectedTotalAmount * 100);

        var options = new PaymentIntentCreateOptions
        {
            Amount = amountInCents,
            Currency = "BRL",
            Customer = stripeCustomerId,
            Metadata = new Dictionary<string, string>
            {
                { "room_id", roomId.ToString() }
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
            Amount = amountInCents
        };

        return await _stripePaymentIntentService.UpdateAsync(paymentIntentId, options);
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
}
