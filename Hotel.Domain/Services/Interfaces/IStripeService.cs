using Hotel.Domain.ValueObjects;
using Stripe;

namespace Hotel.Domain.Services.Interfaces;

public interface IStripeService
{
    Task<Customer> CreateCustomerAsync(Name name, Email email, Phone phone, ValueObjects.Address? address);
    Task<bool> DeleteCustomerAsync(string customerId);
    Task<Customer> GetCustomerAsync(string customerId);
    Task<Customer> UpdateCustomerAsync(string customerId, Name name, Email email, Phone phone, ValueObjects.Address? address);

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
