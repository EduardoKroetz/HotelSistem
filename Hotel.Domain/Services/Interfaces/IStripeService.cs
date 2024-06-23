using Hotel.Domain.Entities.Interfaces;
using Stripe;

namespace Hotel.Domain.Services.Interfaces;

public interface IStripeService
{
    Task<Customer> CreateCustomerAsync(ICustomer customer);
    Task<bool> DeleteCustomerAsync(string customerId);
    Task<Customer> GetCustomerAsync(string customerId);
    Task<Customer> UpdateCustomerAsync(string customerId, ICustomer customer);

    Task<Product> CreateProductAsync(string name, string description, decimal price);
    Task<Product> DisableProductAsync(string productId);
    Task<Product> GetProductAsync(string productId);
    Task<Product> UpdateProductAsync(string productId, string name, string description, decimal price, bool isActive = true);

    Task<PaymentIntent> CreatePaymentIntentAsync(decimal expectedTotalAmount, string stripeCustomerId, Guid roomId);
    Task<bool> CancelPaymentIntentAsync(string paymentIntentId);
    Task<PaymentIntent> GetPaymentIntentAsync(string paymentIntentId);
    Task<PaymentIntent> UpdatePaymentIntentAsync(string paymentIntentId, decimal totalAmount);

    Task<Price> GetFirstActivePriceByProductId(string productId);
}
