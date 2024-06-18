using Stripe;

namespace Hotel.Domain.Services.Interfaces;

public interface IStripeService
{
    // CRUD de Customer
    Task<Customer> CreateCustomerAsync(string name, string email);
    Task<Customer> GetCustomerAsync(string customerId);
    Task<Customer> UpdateCustomerAsync(string customerId, string name, string email);
    Task<bool> DeleteCustomerAsync(string customerId);

    // CRUD de Room (produto no Stripe)
    Task<Product> CreateRoomAsync(string name, string description);
    Task<Product> GetRoomAsync(string productId);
    Task<Product> UpdateRoomAsync(string productId, string name, string description);
    Task<bool> DeleteRoomAsync(string productId);

    // CRUD de Service (produto no Stripe)
    Task<Product> CreateServiceAsync(string name, string description);
    Task<Product> GetServiceAsync(string productId);
    Task<Product> UpdateServiceAsync(string productId, string name, string description);
    Task<bool> DeleteServiceAsync(string productId);

    // CRUD de Reservation (PaymentIntent no Stripe)
    Task<PaymentIntent> CreateReservationAsync(string customerId, string roomId, int amount, string currency);
    Task<PaymentIntent> GetReservationAsync(string paymentIntentId);
    Task<PaymentIntent> UpdateReservationAsync(string paymentIntentId, int amount, string currency);
    Task<bool> CancelReservationAsync(string paymentIntentId);
}
