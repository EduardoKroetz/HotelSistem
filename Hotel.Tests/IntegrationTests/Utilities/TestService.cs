

using Hotel.Domain.Data;
using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.DTOs.CategoryDTOs;
using Hotel.Domain.DTOs.ReservationDTOs;
using Hotel.Domain.DTOs.RoomDTOs;
using Hotel.Domain.DTOs.ServiceDTOs;
using Hotel.Domain.Entities.CategoryEntity;
using Hotel.Domain.Entities.CustomerEntity;
using Hotel.Domain.Entities.ReservationEntity;
using Hotel.Domain.Entities.RoomEntity;
using Hotel.Domain.Entities.ServiceEntity;
using Hotel.Domain.Entities.VerificationCodeEntity;
using Hotel.Domain.Services;
using Hotel.Domain.ValueObjects;
using Hotel.Tests.IntegrationTests.Factories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Stripe;
using System.Net.Http.Json;

namespace Hotel.Tests.IntegrationTests.Utilities;

internal class TestService
{
    private readonly HotelDbContext _dbContext;
    private readonly HotelWebApplicationFactory _factory;
    private readonly string _rootAdminToken;
    private readonly HttpClient _client;

    public TestService(HotelDbContext dbContext, HotelWebApplicationFactory factory, HttpClient client, string rootAdminToken)
    {
        _dbContext = dbContext;
        _factory = factory;
        _rootAdminToken = rootAdminToken;
        _client = client;
    }

    public async Task<Reservation> CreateReservationAsync(Domain.Entities.CustomerEntity.Customer customer, CreateReservation newReservation)
    {
        _factory.Login(_client, customer);
        var response = await _client.PostAsJsonAsync("v1/reservations", newReservation);
        var content = JsonConvert.DeserializeObject<Response<DataStripePaymentIntentId>>(await response.Content.ReadAsStringAsync())!;
        var reservation = await _dbContext.Reservations.FirstAsync(x => x.Id == content.Data.Id);

        return reservation;
    }

    public async Task<Service> CreateServiceAsync(EditorService newService)
    {
        _factory.Login(_client, _rootAdminToken);
        var response = await _client.PostAsJsonAsync("v1/services", newService);
        var content = JsonConvert.DeserializeObject<Response<DataStripeProductId>>(await response.Content.ReadAsStringAsync())!;
        var service = await _dbContext.Services.FirstAsync(x => x.Id == content.Data.Id);

        return service;
    }

    public async Task<Room> CreateRoomAsync(EditorRoom newRoom)
    {
        _factory.Login(_client, _rootAdminToken);
        var response = await _client.PostAsJsonAsync("v1/rooms", newRoom);
        var content = JsonConvert.DeserializeObject<Response<DataStripeProductId>>(await response.Content.ReadAsStringAsync())!;
        var room = await _dbContext.Rooms.FirstAsync(x => x.Id == content.Data.Id);

        return room;
    }

    public async Task<Domain.Entities.CustomerEntity.Customer> CreateCustomerAsync(CreateUser newCustomer)
    {
        var verificationCode = new VerificationCode(new Email(newCustomer.Email));
        await _dbContext.VerificationCodes.AddAsync(verificationCode);
        await _dbContext.SaveChangesAsync();

        var response = await _client.PostAsJsonAsync($"v1/register/customers?code={verificationCode.Code}", newCustomer);
        var content = await DeserializeResponse<DataStripeCustomerId>(response);
        var customer = await _dbContext.Customers.FirstAsync(x => x.Id == content.Data.Id);

        return customer;
    }

    public async Task<Response<T>> DeserializeResponse<T>(HttpResponseMessage response)
    {
        return JsonConvert.DeserializeObject<Response<T>>(await response.Content.ReadAsStringAsync())!;
    }

    public void CompareReservation(Reservation expected, Reservation current)
    {
        Assert.AreEqual(expected.Id, current.Id);
        Assert.AreEqual(expected.DailyRate, current.DailyRate);
        Assert.AreEqual(expected.Capacity, current.Capacity);
        Assert.AreEqual(expected.Status, current.Status);
        Assert.AreEqual(expected.CustomerId, current.CustomerId);
        Assert.AreEqual(expected.ExpectedCheckIn, current.ExpectedCheckIn);
        Assert.AreEqual(expected.ExpectedCheckOut, current.ExpectedCheckOut);
        Assert.AreEqual(expected.ExpectedTimeHosted, current.ExpectedTimeHosted);
        Assert.AreEqual(expected.CheckIn, current.CheckIn);
        Assert.AreEqual(expected.CheckOut, current.CheckOut);
        Assert.AreEqual(expected.InvoiceId, current.InvoiceId);
        Assert.AreEqual(expected.RoomId, current.RoomId);
    }

    public async Task<PaymentIntent> GetAndVerifyPaymentIntent(PaymentIntentService paymentIntentService, Reservation reservation)
    {
        var paymentIntent = await paymentIntentService.GetAsync(reservation.StripePaymentIntentId);
        Assert.IsNotNull(paymentIntent);
        Assert.AreEqual("requires_payment_method", paymentIntent.Status);
        Assert.AreEqual((long)( reservation.ExpectedTotalAmount() * 100 ), paymentIntent.Amount);
        Assert.AreEqual(reservation.Customer!.StripeCustomerId, paymentIntent.CustomerId);

        return paymentIntent;
    }

    public List<ProductServiceInfo> GetMetadataProductsFromPaymentIntent(PaymentIntent paymentIntent)
    {
        var metadata = paymentIntent.Metadata["products"];
        var products = JsonConvert.DeserializeObject<List<ProductServiceInfo>>(metadata)!;

        return products;
    }

    public async Task AddServiceToReservation(Guid reservationId, Guid serviceId)
    {
        _factory.Login(_client, _rootAdminToken);
        await _client.PostAsJsonAsync($"v1/reservations/{reservationId}/services/{serviceId}", new { });
    }

    public async Task AddServiceToRoom(Guid roomId, Guid serviceId)
    {
        _factory.Login(_client, _rootAdminToken);
        await _client.PostAsJsonAsync($"v1/rooms/{roomId}/services/{serviceId}", new { });
    }

    public async Task<Reservation> GetReservation(Guid reservationId)
    {
        return await _dbContext.Reservations
            .Include(x => x.Services)
            .Include(x => x.Room)
            .Include(x => x.Customer)
            .Include(x => x.Invoice)
            .FirstAsync(x => x.Id == reservationId);
    }

    public async Task<HttpResponseMessage> RemoveServiceFromReservationAsync(Guid reservationId, Guid serviceId)
    {
        return await _client.DeleteAsync($"v1/reservations/{reservationId}/services/{serviceId}");
    }

    public async Task DisableRoomAsync(Guid roomId)
    {
        _factory.Login(_client, _rootAdminToken);
        await _client.PatchAsJsonAsync($"v1/rooms/disable/{roomId}", new { });
    }

    public async Task<Category> CreateCategoryAsync(EditorCategory newCategory) 
    {
        _factory.Login(_client, _rootAdminToken);

        var response = await _client.PostAsJsonAsync("v1/categories", newCategory);
        var content = await DeserializeResponse<DataId>(response);
        var category = await _dbContext.Categories.FirstAsync(x => x.Id == content.Data.Id);

        return category;
    }

}
