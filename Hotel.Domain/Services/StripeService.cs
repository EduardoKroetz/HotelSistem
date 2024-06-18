using Hotel.Domain.Services.Interfaces;
using Stripe;

namespace Hotel.Domain.Services;

public class StripeService : IStripeService
{
    //Customer
    public async Task<Customer> CreateCustomerAsync(string name, string email)
    {
        var options = new CustomerCreateOptions 
        { 
            Name = name, 
            Email = email 
        };

        var service = new CustomerService();
        return await service.CreateAsync(options);  
    }

    public Task<bool> DeleteCustomerAsync(string customerId)
    {
        throw new NotImplementedException();
    }

    public Task<Customer> GetCustomerAsync(string customerId)
    {
        throw new NotImplementedException();
    }

    public Task<Customer> UpdateCustomerAsync(string customerId, string name, string email)
    {
        throw new NotImplementedException();
    }


    //Room
    public Task<Product> CreateRoomAsync(string name, string description)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteRoomAsync(string productId)
    {
        throw new NotImplementedException();
    }

    public Task<Product> GetRoomAsync(string productId)
    {
        throw new NotImplementedException();
    }

    public Task<Product> UpdateRoomAsync(string productId, string name, string description)
    {
        throw new NotImplementedException();
    }

    //Service
    public Task<Product> CreateServiceAsync(string name, string description)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteServiceAsync(string productId)
    {
        throw new NotImplementedException();
    }

    public Task<Product> GetServiceAsync(string productId)
    {
        throw new NotImplementedException();
    }

    public Task<Product> UpdateServiceAsync(string productId, string name, string description)
    {
        throw new NotImplementedException();
    }


    //Reservation
    public Task<PaymentIntent> CreateReservationAsync(string customerId, string roomId, int amount, string currency)
    {
        throw new NotImplementedException();
    }

    public Task<bool> CancelReservationAsync(string paymentIntentId)
    {
        throw new NotImplementedException();
    }

    public Task<PaymentIntent> GetReservationAsync(string paymentIntentId)
    {
        throw new NotImplementedException();
    }

    public Task<PaymentIntent> UpdateReservationAsync(string paymentIntentId, int amount, string currency)
    {
        throw new NotImplementedException();
    }


}
