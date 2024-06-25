using Hotel.Domain.Data;
using Hotel.Domain.DTOs.ReservationDTOs;
using Hotel.Domain.Entities.ReservationEntity;
using Hotel.Domain.Entities.CategoryEntity;
using Hotel.Domain.Entities.RoomEntity;
using Hotel.Domain.Entities.ServiceEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.ValueObjects;
using Hotel.Tests.IntegrationTests.Factories;
using Hotel.Tests.IntegrationTests.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Hotel.Domain.DTOs.RoomDTOs;
using Stripe;
using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.Entities.VerificationCodeEntity;
using Hotel.Domain.Services;
using Hotel.Domain.DTOs.ServiceDTOs;
using Hotel.Domain.DTOs.CategoryDTOs;
using Hotel.Domain.DTOs.PaymentDTOs;

namespace Hotel.Tests.IntegrationTests.Controllers;

[TestClass]
public class ReservationControllerTests
{
    private static HotelWebApplicationFactory _factory = null!;
    private static HttpClient _client = null!;
    private static HotelDbContext _dbContext = null!;
    private static string _rootAdminToken = null!; //Allows access all endpoints
    private static string _customerToken = null!;
    private const string _baseUrl = "v1/reservations";
    private static Domain.Services.TokenServices.TokenService _tokenService = null!;
    private static Category _category = null!;
    private static PaymentIntentService _stripePaymentIntentService = new PaymentIntentService();
    private static TestService _testService = null!;

    [ClassInitialize]
    public static void ClassInitialize(TestContext? context)
    {
        _factory = new HotelWebApplicationFactory();
        _client = _factory.CreateClient();
        _dbContext = _factory.Services.GetRequiredService<HotelDbContext>();
        _tokenService = _factory.Services.GetRequiredService<Domain.Services.TokenServices.TokenService>();

        _rootAdminToken = _factory.LoginFullAccess().Result;
        _customerToken = _factory.LoginCustomer().Result;

        _category = new Category("Categoria de luxo", "Abrange os diversos tipos de quartos com avaliação média", 45);
        _dbContext.Categories.AddAsync(_category).AsTask().Wait();
        _dbContext.SaveChangesAsync().Wait();

        _testService = new TestService(_dbContext, _factory, _client, _rootAdminToken);
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        _factory.Dispose();
    }

    [TestInitialize]
    public void TestInitiatlize()
    {
        _factory.Login(_client, _customerToken);
    }

    [TestMethod]
    public async Task CreateReservation_ShouldReturn_OK()
    {
        // Arrange
        var newCustomer = new CreateUser("Maria", "Silva", "mariaSilva@gmail.com", "+55 (11) 98765-1234", "password123", EGender.Feminine, DateTime.Now.AddYears(-25), "Brazil", "São Paulo", "SP-101", 101);
        var customer = await _testService.CreateCustomerAsync(newCustomer);

        var room = await _testService.CreateRoomAsync(new EditorRoom("Quarto 2", 2, 50, 2, "Quarto padrão", _category.Id));

        // Act
        var body = new CreateReservation(DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), room.Id, 1);
        var createdReservation = await _testService.CreateReservationAsync(customer, body);

        // Assert
        Assert.IsNotNull(createdReservation);
        Assert.AreEqual(ERoomStatus.Reserved, createdReservation.Room!.Status);

        var reservation = await _testService.GetReservation(createdReservation.Id);

        _testService.CompareReservation(reservation, createdReservation);

        var paymentIntent = await _testService.GetAndVerifyPaymentIntent(_stripePaymentIntentService, createdReservation);
        var products = _testService.GetMetadataProductsFromPaymentIntent(paymentIntent);

        var roomProduct = products.First(x => x.Id == room.Id);
        Assert.IsFalse(roomProduct.IsService);
        Assert.AreEqual(1, roomProduct.Quantity);
        Assert.AreEqual(room.StripeProductId, roomProduct.ProductId);

        Assert.AreEqual(1, products.Count);
    }


    [TestMethod]
    public async Task CreateReservation_WithUnavailableRoom_ShouldReturn_BAD_REQUEST()
    {
        // Arrange
        var customer = await _testService.CreateCustomerAsync(new CreateUser("Juliana", "Martins", "julianaMartins@gmail.com", "+55 (48) 98765-1321", "123", EGender.Feminine, DateTime.Now.AddYears(-24), "Brazil", "Florianópolis", "SC-909", 909));
        var room = await _testService.CreateRoomAsync(new EditorRoom("Quarto 3", 3, 80, 2, "Quarto de luxo nível 1", _category.Id));
        var reservation = await _testService.CreateReservationAsync(customer, new CreateReservation(DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), room.Id, 1));

        var body = new CreateReservation(DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), room.Id, 1);

        _factory.Login(_client, customer);
        // Act
        var response = await _client.PostAsJsonAsync(_baseUrl, body);

        // Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        var content = await _testService.DeserializeResponse<object>(response);
        var reservations = await _dbContext.Reservations.Where(x => x.RoomId == room.Id).ToListAsync();

        Assert.AreEqual("Não é possível realizar a reserva pois a hospedagem está indisponível.", content.Errors[0]);
        Assert.AreEqual(1, reservations.Count);
    }

    [TestMethod]
    public async Task CreateReservation_WithDisabledRoom_ShouldReturn_BAD_REQUEST()
    {
        // Arrange
        var customer = await _testService.CreateCustomerAsync(new CreateUser("Marina", "Oliveira", "marina.oliveira@example.com", "+55 (47) 91234-5678", "987654", EGender.Feminine, new DateTime(1998, 7, 15), "Brasil", "São Paulo", "04578-123", 123));
        var room = await _testService.CreateRoomAsync(new EditorRoom("Quarto 4", 4, 110, 2, "Quarto de luxo nível 2", _category.Id));
        await _testService.DisableRoomAsync(room.Id);

        var body = new CreateReservation(DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), room.Id, 1);

        _factory.Login(_client, customer);
        // Act
        var response = await _client.PostAsJsonAsync(_baseUrl, body);

        // Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        var content = await _testService.DeserializeResponse<object>(response);
        var reservations = await _dbContext.Reservations.Where(x => x.RoomId == room.Id).ToListAsync();

        Assert.AreEqual("Não é possível realizar a reserva pois a hospedagem está inativo.", content.Errors[0]);
        Assert.AreEqual(0, reservations.Count);
    }


    [TestMethod]
    public async Task CreateReservation_WithRoomGuestsLimitExceeded_ShouldReturn_BAD_REQUEST()
    {
        // Arrange
        var room = await _testService.CreateRoomAsync(new EditorRoom("Quarto 5", 5, 150, 2, "Quarto de luxo nível 3", _category.Id));

        var body = new CreateReservation(DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), room.Id, 3);

        _factory.Login(_client, _customerToken);
        // Act
        var response = await _client.PostAsJsonAsync(_baseUrl, body);

        // Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        var content = await _testService.DeserializeResponse<object>(response);
        var reservations = await _dbContext.Reservations.Where(x => x.RoomId == room.Id).ToListAsync();

        Assert.AreEqual("Capacidade máxima de hospedes da hospedagem excedida.", content.Errors[0]);
        Assert.AreEqual(0, reservations.Count);
    }


    [TestMethod]
    public async Task CreateReservation_WithNonexistCustomer_ShouldReturn_NOT_FOUND()
    {
        // Arrange
        var customer = new Domain.Entities.CustomerEntity.Customer(new Name("Rafael", "Oliveira"),new Email("rafaelOliveira@gmail.com"),new Phone("+55 (41) 97604-1210"),"password4",EGender.Masculine,DateTime.Now.AddYears(-32),new Domain.ValueObjects.Address("Brazil", "Curitiba", "PR-404", 404));
        var room = await _testService.CreateRoomAsync(new EditorRoom("Quarto 6", 6, 190, 2, "Quarto de luxo nível 4", _category.Id));

        _factory.Login(_client, customer);

        var body = new CreateReservation(DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), room.Id, 1);

        // Act
        var response = await _client.PostAsJsonAsync(_baseUrl, body);

        // Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

        var content = await _testService.DeserializeResponse<object>(response);
        var reservations = await _dbContext.Reservations.Where(x => x.RoomId == room.Id).ToListAsync();

        Assert.AreEqual("Usuário não encontrado", content.Errors[0]);
        Assert.AreEqual(0, reservations.Count);
    }


    [TestMethod]
    public async Task CreateReservation_WithNonexistRoom_ShouldReturn_NOT_FOUND()
    {
        // Arrange
        var body = new CreateReservation(DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), Guid.NewGuid(), 1); // Gerando um GUID aleatório

        // Act
        var response = await _client.PostAsJsonAsync(_baseUrl, body);

        // Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

        var content = await _testService.DeserializeResponse<object>(response);

        Assert.AreEqual("Hospedagem não encontrada", content.Errors[0]);
    }

    [TestMethod]
    public async Task CreateReservation_WithInvalidStripeCustomerId_ShouldReturn_BAD_REQUEST_AND_MAKE_ROLLBACK()
    {
        // Arrange
        var newCustomer = new Domain.Entities.CustomerEntity.Customer(new Name("Camila", "Barbosa"),new Email("camilaBarbosa@gmail.com"),new Phone("+55 (95) 98765-6543"),"password543",EGender.Feminine,DateTime.Now.AddYears(-32),new Domain.ValueObjects.Address("Brazil", "Manaus", "MA-1010", 1010));
        await _dbContext.Customers.AddAsync(newCustomer);
        await _dbContext.SaveChangesAsync();
        var room = await _testService.CreateRoomAsync(new EditorRoom("Quarto 931", 931, 50, 2, "Quarto padrão 931", _category.Id));

        _factory.Login(_client, newCustomer);

        var body = new CreateReservation(DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), room.Id, 1);

        // Act
        var response = await _client.PostAsJsonAsync(_baseUrl, body);

        // Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        var content = await _testService.DeserializeResponse<object>(response);

        Assert.AreEqual("Ocorreu um erro ao criar a intenção de pagamento no Stripe", content.Errors[0]);

        var wasCreated = await _dbContext.Reservations.AnyAsync(x => x.RoomId == room.Id);
        Assert.IsFalse(wasCreated);
    }

    [TestMethod]
    public async Task GetReservations_ShouldReturn_OK()
    {
        // Arrange
        var customer = await _testService.CreateCustomerAsync(new CreateUser("Fernanda", "Ribeiro", "fernandaRibeiro@gmail.com", "+55 (51) 91034-5678", "password7", EGender.Feminine, DateTime.Now.AddYears(-26), "Brazil", "Porto Alegre", "RS-707", 707));
        var room = await _testService.CreateRoomAsync(new EditorRoom("Quarto 7", 7, 240, 2, "Quarto de luxo nível 5", _category.Id));
        var reservation = await _testService.CreateReservationAsync(customer, new CreateReservation(DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), room.Id, 1));

        // Act
        var response = await _client.GetAsync(_baseUrl);

        // Assert
        Assert.IsNotNull(response);
        response.EnsureSuccessStatusCode();

        var content = await _testService.DeserializeResponse<List<GetReservation>>(response);

        Assert.AreEqual("Sucesso!", content.Message);
        foreach (var getReservation in content.Data)
        {
            Assert.IsNotNull(getReservation.DailyRate);
            Assert.IsNotNull(getReservation.Capacity);
            Assert.IsNotNull(getReservation.Status);
            Assert.IsNotNull(getReservation.CustomerId);
            Assert.IsNotNull(getReservation.ExpectedCheckIn);
            Assert.IsNotNull(getReservation.ExpectedCheckOut);
            Assert.IsNotNull(getReservation.ExpectedTimeHosted);
        }
    }


    [TestMethod]
    public async Task GetReservationById_ShouldReturn_OK()
    {
        // Arrange
        var customer = await _testService.CreateCustomerAsync(new CreateUser("Lucas", "Ferreira", "lucasFerreira@gmail.com", "+55 (61) 92744-6789", "password6", EGender.Masculine, DateTime.Now.AddYears(-28), "Brazil", "Brasília", "DF-606", 606));
        var room = await _testService.CreateRoomAsync(new EditorRoom("Quarto 8", 8, 70, 5, "Quarto de luxo básico", _category.Id));
        var reservation = await _testService.CreateReservationAsync(customer, new CreateReservation(DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), room.Id, 3));

        // Act
        var response = await _client.GetAsync($"{_baseUrl}/{reservation.Id}");

        // Assert
        Assert.IsNotNull(response);
        response.EnsureSuccessStatusCode();

        var content = await _testService.DeserializeResponse<GetReservation>(response);

        Assert.AreEqual("Sucesso!", content.Message);

        Assert.AreEqual(reservation.Id, content.Data.Id);
        Assert.AreEqual(reservation.DailyRate, content.Data.DailyRate);
        Assert.AreEqual(reservation.Capacity, content.Data.Capacity);
        Assert.AreEqual(reservation.Status, content.Data.Status);
        Assert.AreEqual(reservation.CustomerId, content.Data.CustomerId);
        Assert.AreEqual(reservation.ExpectedCheckIn, content.Data.ExpectedCheckIn);
        Assert.AreEqual(reservation.ExpectedCheckOut, content.Data.ExpectedCheckOut);
        Assert.AreEqual(reservation.ExpectedTimeHosted, content.Data.ExpectedTimeHosted);
        Assert.AreEqual(reservation.CheckIn, content.Data.CheckIn);
        Assert.AreEqual(reservation.CheckOut, content.Data.CheckOut);
        Assert.AreEqual(reservation.InvoiceId, content.Data.InvoiceId);
        Assert.AreEqual(reservation.RoomId, content.Data.RoomId);
    }



    [TestMethod]
    public async Task DeleteReservation_ShouldReturn_OK()
    {
        // Arrange
        var newCustomer = await _testService.CreateCustomerAsync(new CreateUser("Camila", "Costa", "camilaCosta@gmail.com", "+55 (71) 93136-7891", "password5", EGender.Feminine, DateTime.Now.AddYears(-29), "Brazil", "Salvador", "BA-505", 505));
        var room = await _testService.CreateRoomAsync(new EditorRoom("Quarto 9", 9, 70, 5, "Quarto 1", _category.Id));
        var reservation = await _testService.CreateReservationAsync(newCustomer, new CreateReservation(DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), room.Id, 3));

        _factory.Login(_client, _rootAdminToken);

        // Act
        var response = await _client.DeleteAsync($"{_baseUrl}/{reservation.Id}");

        // Assert
        Assert.IsNotNull(response);
        response.EnsureSuccessStatusCode();

        var content = await _testService.DeserializeResponse<DataStripePaymentIntentId>(response);

        var exists = await _dbContext.Reservations.AnyAsync(x => x.Id == reservation.Id);

        Assert.AreEqual("Reserva deletada com sucesso!", content.Message);
        Assert.AreEqual(reservation.Id, content.Data.Id);
        Assert.IsFalse(exists);

        var paymentIntent = await _stripePaymentIntentService.GetAsync(reservation.StripePaymentIntentId);
        Assert.AreEqual("canceled", paymentIntent.Status);
    }


    [TestMethod]
    public async Task DeleteReservation_WithStripeError_ShouldReturn_BAD_REQUEST_AND_MAKE_ROLLBACK()
    {
        //Arange
        var factory = new HotelWebApplicationFactory();
        var client = factory.CreateClient();
        var dbContext = factory.Services.GetRequiredService<HotelDbContext>();
        var testService = new TestService(dbContext, factory, client, _rootAdminToken);

        var category = await testService.CreateCategoryAsync(new EditorCategory("Basic", "Basic", 50));
        var customer = await testService.CreateCustomerAsync(new CreateUser("Rafael", "Ribeiro", "rafaelRibeiro@gmail.com", "+55 (84) 98765-3456", "password210", EGender.Masculine, DateTime.Now.AddYears(-40), "Brazil", "Natal", "NT-909", 909));
        var room = await testService.CreateRoomAsync(new EditorRoom("Quarto 671", 671, 70, 5, "Quarto 671", category.Id));
        var reservation = await testService.CreateReservationAsync(customer, new CreateReservation(DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), room.Id, 3));

        factory.Login(client, _rootAdminToken);

        var apiKey = StripeConfiguration.ApiKey.ToString();
        StripeConfiguration.ApiKey = "";

        //Act
        var response = await client.DeleteAsync($"{_baseUrl}/{reservation.Id}");

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        var content = await _testService.DeserializeResponse<object>(response);

        var exists = await dbContext.Reservations.AnyAsync(x => x.Id == reservation.Id);

        Assert.AreEqual("Ocorreu um erro ao cancelar o PaymentIntent no Stripe", content.Errors[0]);
        Assert.IsTrue(exists);

        StripeConfiguration.ApiKey = apiKey;
        var paymentIntent = await _stripePaymentIntentService.GetAsync(reservation.StripePaymentIntentId);
        Assert.AreEqual("requires_payment_method", paymentIntent.Status);
    }


    [TestMethod]
    public async Task DeleteReservation_WithoutPermission_ShouldReturn_FORBIDDEN()
    {
        //Act
        var response = await _client.DeleteAsync($"{_baseUrl}/{Guid.NewGuid()}");

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);

        var content = await _testService.DeserializeResponse<object>(response);

        
        Assert.AreEqual("Você não tem acesso a esse serviço.", content.Errors[0]);
    }


    [TestMethod]
    public async Task DeleteReservation_WithNonexistReservation_ShouldReturn_NOT_FOUND()
    {
        //Arange
        _factory.Login(_client, _rootAdminToken);

        //Act
        var response = await _client.DeleteAsync($"{_baseUrl}/{Guid.NewGuid()}");

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

        var content = await _testService.DeserializeResponse<object>(response);
    }

    [TestMethod]
    public async Task DeleteReservation_WithCheckedInReservationStatus_ShouldReturn_BAD_REQUEST()
    {
        //Arange
        var customer = await _testService.CreateCustomerAsync(new CreateUser("Rafael", "Oliveira", "rafaelOliveira@gmail.com", "+55 (41) 97604-1210", "password4", EGender.Masculine, DateTime.Now.AddYears(-32), "Brazil", "Curitiba", "PR-404", 404));
        var room = await _testService.CreateRoomAsync(new EditorRoom("1Quarto 0", 10, 70, 5, "Quarto 2", _category.Id));
        var reservation = await _testService.CreateReservationAsync(customer ,new CreateReservation(DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), room.Id, 3));
        reservation.ToCheckIn(); // check in and change status

        await _dbContext.SaveChangesAsync();

        _factory.Login(_client, _rootAdminToken);
        //Act
        var response = await _client.DeleteAsync($"{_baseUrl}/{reservation.Id}");

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        var content = await _testService.DeserializeResponse<object>(response);

        Assert.IsTrue(content!.Errors.Any(x => x.Contains("deletar a reserva sem antes finaliza-la.")));
    }

    [TestMethod]
    public async Task UpdateExpectedCheckOut_ShouldReturn_OK()
    { 
        // Arrange
        var newCustomer = await _testService.CreateCustomerAsync(new CreateUser("Beatriz", "Santos","beatrizSantos@gmail.com", "+55 (31) 90176-5432", "password3",EGender.Feminine,DateTime.Now.AddYears(-27),"Brazil", "Belo Horizonte", "MG-303", 303));
        var room = await _testService.CreateRoomAsync(new EditorRoom("Quarto 11", 11, 70, 5, "Quarto 11", _category.Id));
        var reservation = await _testService.CreateReservationAsync(newCustomer, new CreateReservation(DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), room.Id, 3));

        var newExpectedCheckOut = DateTime.Now.AddDays(3);

        // Act
        var response = await _client.PatchAsJsonAsync($"{_baseUrl}/{reservation.Id}/check-out", new UpdateCheckOut(newExpectedCheckOut));

        // Assert
        Assert.IsNotNull(response);
        response.EnsureSuccessStatusCode();

        var content = await _testService.DeserializeResponse<object>(response);

        var updatedReservation = await _testService.GetReservation(reservation.Id);

        Assert.AreEqual(0, content.Errors.Count);
        Assert.AreEqual("CheckOut esperado atualizado com sucesso!", content.Message);

        Assert.AreEqual(newExpectedCheckOut, updatedReservation.ExpectedCheckOut);

        await _testService.GetAndVerifyPaymentIntent(_stripePaymentIntentService, reservation);
    }


    [TestMethod]
    public async Task UpdateExpectedCheckOut_WithNonexistReservation_ShouldReturn_NOT_FOUND()
    {
        var body = new UpdateCheckOut(DateTime.Now.AddDays(3));

        //Act
        var response = await _client.PatchAsJsonAsync($"{_baseUrl}/{Guid.NewGuid()}/check-out", body);

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

        var content = await _testService.DeserializeResponse<object>(response);


        Assert.AreEqual(1, content.Errors.Count);
        Assert.AreEqual("Reserva não encontrada.", content.Errors[0]);
    }

    [TestMethod]
    public async Task UpdateExpectedCheckOut_WithCheckedOutStatus_ShouldReturn_BAD_REQUEST()
    {
        // Arrange
        var reservation = await _testService.CreateReservationAsync(
            await _testService.CreateCustomerAsync(new CreateUser("Eduardo", "Almeida", "eduardoAlmeida@gmail.com", "+55 (19) 91123-4467", "password11", EGender.Masculine, DateTime.Now.AddYears(-29), "Brazil", "Campinas", "SP-1111", 1111)),
            new CreateReservation(DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), ( await _testService.CreateRoomAsync(new EditorRoom("1Quarto 2", 12, 90, 5, "Quarto 12", _category.Id)) ).Id, 3)
        );

        reservation.ToCheckIn();
        reservation.Finish(EPaymentMethod.Pix);

        await _dbContext.SaveChangesAsync();

        // Act
        var newExpectedCheckOut = DateTime.Now.AddDays(7);
        var response = await _client.PatchAsJsonAsync($"{_baseUrl}/{reservation.Id}/check-out", new UpdateCheckOut(newExpectedCheckOut));

        // Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        var content = await _testService.DeserializeResponse<object>(response);

        var updatedReservation = await _testService.GetReservation(reservation.Id);

        Assert.AreEqual(1, content.Errors.Count);
        Assert.AreEqual("Não é possível alterar o CheckOut esperado com o status da reserva CheckedOut.", content.Errors[0]);
        Assert.AreNotEqual(newExpectedCheckOut, updatedReservation.ExpectedCheckOut);
    }


    [TestMethod]
    public async Task UpdateExpectedCheckOut_WithCancelledStatus_ShouldReturn_BAD_REQUEST()
    {
        // Arrange
        var customer = await _testService.CreateCustomerAsync(new CreateUser("Larissa", "Rodrigues", "larissaRodrigues@gmail.com", "+55 (85) 99886-6543", "password12", EGender.Feminine, DateTime.Now.AddYears(-27), "Brazil", "Fortaleza", "CE-1212", 1212));
        var room = await _testService.CreateRoomAsync(new EditorRoom("Quarto 13", 13, 90, 5, "Quarto 13", _category.Id));
        var reservation = await _testService.CreateReservationAsync(customer, new CreateReservation(DateTime.Now.AddDays(1),DateTime.Now.AddDays(2),room.Id, 5));
        reservation.ToCancelled();

        await _dbContext.SaveChangesAsync();

        var newExpectedCheckOut = DateTime.Now.AddDays(7);
        var body = new UpdateCheckOut(newExpectedCheckOut);

        // Act
        var response = await _client.PatchAsJsonAsync($"{_baseUrl}/{reservation.Id}/check-out", body);

        // Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        var content = await _testService.DeserializeResponse<object>(response);

        Assert.AreEqual(1, content.Errors.Count);
        Assert.AreEqual("Não é possível alterar o CheckOut esperado com o status da reserva Canceled.", content.Errors[0]);
    }


    [TestMethod]
    public async Task UpdateExpectedCheckIn_ShouldReturn_OK()
    {
        // Arrange
        var customer = await _testService.CreateCustomerAsync(new CreateUser("Thiago", "Gomes", "thiagoGomes@gmail.com", "+55 (13) 97354-3110", "password13", EGender.Masculine, DateTime.Now.AddYears(-30), "Brazil", "Santos", "SP-1313", 1313));
        var room = await _testService.CreateRoomAsync(new EditorRoom("Quarto 853", 853, 70, 5, "Quarto 853", _category.Id));

        _factory.Login(_client, customer);

        var reservation = await _testService.CreateReservationAsync(customer, new CreateReservation(DateTime.Now.AddDays(1), DateTime.Now.AddDays(9), room.Id, 3));

        var newExpectedCheckIn = DateTime.Now.AddDays(7);
        var body = new UpdateCheckIn(newExpectedCheckIn);

        // Act
        var response = await _client.PatchAsJsonAsync($"{_baseUrl}/{reservation.Id}/check-in", body);

        // Assert
        Assert.IsNotNull(response);
        response.EnsureSuccessStatusCode();

        var content = await _testService.DeserializeResponse<object>(response);

        var updatedReservation = await _dbContext.Reservations.FirstAsync(x => x.Id == reservation.Id);

        Assert.AreEqual(0, content.Errors.Count);
        Assert.AreEqual("CheckIn esperado atualizado com sucesso!", content.Message);
        Assert.AreEqual(newExpectedCheckIn, updatedReservation.ExpectedCheckIn);

        await _testService.GetAndVerifyPaymentIntent(_stripePaymentIntentService, reservation);
    }


    [TestMethod]
    public async Task UpdateExpectedCheckIn_WithNonexistReservation_ShouldReturn_NOT_FOUND()
    {
        //Arange
        var reservationId = Guid.NewGuid();
        var body = new UpdateCheckIn(DateTime.Now.AddDays(5));

        //Act
        var response = await _client.PatchAsJsonAsync($"{_baseUrl}/{reservationId}/check-in", body);

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

        var content = await _testService.DeserializeResponse<object>(response);


        Assert.AreEqual(1, content.Errors.Count);
        Assert.AreEqual("Reserva não encontrada.", content.Errors[0]);
    }

    [TestMethod]
    public async Task UpdateExpectedCheckIn_WithCheckedOutStatus_ShouldReturn_BAD_REQUEST()
    {
        // Arrange
        var customer = await _testService.CreateCustomerAsync(new CreateUser("Carolina", "Araujo", "carolinaAraujo@gmail.com", "+55 (16) 93458-7890", "password14", EGender.Feminine, DateTime.Now.AddYears(-26), "Brazil", "Ribeirão Preto", "SP-1414", 1414));

        var room = await _testService.CreateRoomAsync(new EditorRoom("1Quarto 5", 15, 90, 5, "Quarto 15", _category.Id));

        var reservation = await _testService.CreateReservationAsync(customer, new CreateReservation(DateTime.Now.AddDays(1), DateTime.Now.AddDays(6), room.Id, 4));

        reservation.ToCheckIn();
        reservation.Finish(EPaymentMethod.Pix);

        var body = new UpdateCheckIn(DateTime.Now.AddDays(7));

        // Act
        var response = await _client.PatchAsJsonAsync($"{_baseUrl}/{reservation.Id}/check-in", body);

        // Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        var content = await _testService.DeserializeResponse<object>(response);

        Assert.AreEqual(1, content.Errors.Count);
        Assert.AreEqual("Só é possível alterar o CheckIn esperado se o status for 'Pending' ou 'NoShow'.", content.Errors[0]);
    }

    [TestMethod]
    public async Task UpdateExpectedCheckIn_WithCancelledStatus_ShouldReturn_BAD_REQUEST()
    {
        // Arrange
        var customer = await _testService.CreateCustomerAsync(new CreateUser("Ricardo", "Melo", "ricardoMelo@gmail.com", "+55 (82) 92310-6789", "password15", EGender.Masculine, DateTime.Now.AddYears(-32), "Brazil", "Maceió", "AL-1515", 1515));

        var room = await _testService.CreateRoomAsync(new EditorRoom("1Quarto 6", 16, 90, 5, "Quarto 16", _category.Id));

        var reservation = await _testService.CreateReservationAsync(customer, new CreateReservation(DateTime.Now.AddDays(1), DateTime.Now.AddDays(6), room.Id, 4));
        reservation.ToCancelled();

        var body = new UpdateCheckIn(DateTime.Now.AddDays(7));

        // Act
        var response = await _client.PatchAsJsonAsync($"{_baseUrl}/{reservation.Id}/check-in", body);

        // Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        var content = await _testService.DeserializeResponse<object>(response);

        Assert.AreEqual(1, content.Errors.Count);
        Assert.AreEqual("Só é possível alterar o CheckIn esperado se o status for 'Pending' ou 'NoShow'.", content.Errors[0]);
    }

    [TestMethod]
    public async Task UpdateExpectedCheckIn_WithCheckedInStatus_ShouldReturn_BAD_REQUEST()
    {
        // Arrange
        var customer = await _testService.CreateCustomerAsync(new CreateUser("Patrícia", "Teixeira", "patriciaTeixeira@gmail.com", "+55 (91) 91904-5678", "password16", EGender.Feminine, DateTime.Now.AddYears(-28), "Brazil", "Belém", "PA-1616", 1616));
        var room = await _testService.CreateRoomAsync(new EditorRoom("1Quarto 7", 17, 90, 5, "Quarto 17", _category.Id));
        var reservation = await _testService.CreateReservationAsync(customer, new CreateReservation(DateTime.Now.AddDays(1), DateTime.Now.AddDays(6), room.Id, 3));
        reservation.ToCheckIn();

        // Act
        var body = new UpdateCheckIn(DateTime.Now.AddDays(7));
        var response = await _client.PatchAsJsonAsync($"{_baseUrl}/{reservation.Id}/check-in", body);

        // Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        var content = await _testService.DeserializeResponse<object>(response);

        Assert.AreEqual(1, content.Errors.Count);
        Assert.AreEqual("Só é possível alterar o CheckIn esperado se o status for 'Pending' ou 'NoShow'.", content.Errors[0]);
    }

    [TestMethod]
    public async Task AddServiceToReservation_ShouldReturn_OK()
    {
        // Arrange
        var customer = await _testService.CreateCustomerAsync(new CreateUser("Bruno", "Barbosa", "brunoBarbosa@gmail.com", "+55 (51) 98790-5623", "password17", EGender.Masculine, DateTime.Now.AddYears(-31), "Brazil", "Caxias do Sul", "RS-1717", 1717));
        var room = await _testService.CreateRoomAsync(new EditorRoom("Quarto 18", 18, 90, 5, "Quarto 18", _category.Id));
        var reservation = await _testService.CreateReservationAsync(customer, new CreateReservation(DateTime.Now.AddDays(1), DateTime.Now.AddDays(6), room.Id, 3));
        var service = await _testService.CreateServiceAsync(new EditorService("Room Cleaning", "Room Cleaning", 30.00m, EPriority.Medium, 60));
        await _testService.AddServiceToRoom(room.Id, service.Id);

        // Act
        var response = await _client.PostAsJsonAsync($"{_baseUrl}/{reservation.Id}/services/{service.Id}", new { });

        // Assert
        Assert.IsNotNull(response);
        response.EnsureSuccessStatusCode();

        var content = await _testService.DeserializeResponse<object>(response);

        var reservationWithServices = await _dbContext.Reservations.Include(x => x.Services).FirstAsync(x => x.Id == reservation.Id);

        Assert.AreEqual(0, content.Errors.Count);
        Assert.AreEqual("Serviço adicionado com sucesso!", content.Message);

        Assert.IsTrue(reservationWithServices.Services.Any(x => x.Id == service.Id));

        var paymentIntent = await _testService.GetAndVerifyPaymentIntent(_stripePaymentIntentService, reservation);
        var products = _testService.GetMetadataProductsFromPaymentIntent(paymentIntent);
       
        var roomProduct = products.First(x => x.Id == room.Id);
        var serviceProduct = products.First(x => x.Id == service.Id);

        Assert.AreEqual(false, roomProduct.IsService);
        Assert.AreEqual(1, roomProduct.Quantity);
        Assert.AreEqual(room.StripeProductId, roomProduct.ProductId);

        Assert.IsTrue(serviceProduct.IsService);
        Assert.AreEqual(service.Name, serviceProduct.Name);
        Assert.AreEqual(service.Price, serviceProduct.UnitPrice);
        Assert.AreEqual(1, serviceProduct.Quantity);
        Assert.AreEqual(service.StripeProductId, serviceProduct.ProductId);

        Assert.AreEqual(2, products.Count);
    }


    [TestMethod]
    public async Task AddSameServiceReservation_ShouldReturn_OK()
    {
        // Arrange
        var customer = await _testService.CreateCustomerAsync(new CreateUser("Gabriel", "Martins", "gabrielMartins@gmail.com", "+55 (31) 98765-2345", "password765", EGender.Masculine, DateTime.Now.AddYears(-26), "Brazil", "Belo Horizonte", "MG-111", 111));
        var room = await _testService.CreateRoomAsync(new EditorRoom("Quarto 541", 541, 90, 5, "Quarto 541", _category.Id));
        var reservation = await _testService.CreateReservationAsync(customer, new CreateReservation(DateTime.Now.AddDays(1), DateTime.Now.AddDays(6), room.Id, 3));
        var service = await _testService.CreateServiceAsync(new EditorService("Drink Service", "Drink Service", 15.00m, EPriority.Medium, 7));
        await _testService.AddServiceToRoom(room.Id, service.Id);

        // Act
        var response1 = await _client.PostAsJsonAsync($"{_baseUrl}/{reservation.Id}/services/{service.Id}", new { });
        var response2 = await _client.PostAsJsonAsync($"{_baseUrl}/{reservation.Id}/services/{service.Id}", new { });

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response1.StatusCode);
        Assert.AreEqual(HttpStatusCode.OK, response2.StatusCode);

        var paymentIntent = await _testService.GetAndVerifyPaymentIntent(_stripePaymentIntentService, reservation);
        var products = _testService.GetMetadataProductsFromPaymentIntent(paymentIntent);
        var roomProduct = products.First(x => x.Id == room.Id);
        var serviceProduct = products.First(x => x.Id == service.Id);

        Assert.AreEqual(false, roomProduct.IsService);
        Assert.AreEqual(1, roomProduct.Quantity);
        Assert.AreEqual(room.StripeProductId, roomProduct.ProductId);

        Assert.IsTrue(serviceProduct.IsService);
        Assert.AreEqual(service.Name, serviceProduct.Name);
        Assert.AreEqual(service.Price, serviceProduct.UnitPrice);
        Assert.AreEqual(2, serviceProduct.Quantity); 
        Assert.AreEqual(service.StripeProductId, serviceProduct.ProductId);

        Assert.AreEqual(2, products.Count); // Verifica se há dois itens (quarto e serviço) no carrinho
    }


    [TestMethod]
    public async Task AddServiceToReservation_WithNonexistReservation_ShouldReturn_NOT_FOUND()
    {
        // Arrange
        var randomId = Guid.NewGuid();
        var service = await _testService.CreateServiceAsync(new EditorService("Breakfast Delivery", "Breakfast Delivery", 20.00m, EPriority.High, 30));

        // Act
        var response = await _client.PostAsJsonAsync($"{_baseUrl}/{randomId}/services/{service.Id}", new { });

        // Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

        var content = await _testService.DeserializeResponse<object>(response);

        Assert.AreEqual(1, content.Errors.Count);
        Assert.AreEqual("Reserva não encontrada.", content.Errors[0]);
    }

    [TestMethod]
    public async Task AddServiceToReservation_WithNonexistService_ShouldReturn_NOT_FOUND()
    {
        // Arrange
        var customer = await _testService.CreateCustomerAsync(
            new CreateUser("Isabela", "Freitas", "isabelaFreitas@gmail.com", "+55 (11) 97931-3210", "password18", EGender.Feminine, DateTime.Now.AddYears(-25), "Brazil", "São Bernardo do Campo", "SP-1818", 1818)
        );

        var room = await _testService.CreateRoomAsync(new EditorRoom("1Quarto 9", 19, 90, 5, "Quarto 19", _category.Id));
        var reservation = await _testService.CreateReservationAsync(customer, new CreateReservation(DateTime.Now.AddDays(1), DateTime.Now.AddDays(6), room.Id, 3));

        _factory.Login(_client, _rootAdminToken);

        // Act
        var response = await _client.PostAsJsonAsync($"{_baseUrl}/{reservation.Id}/services/{Guid.NewGuid()}", new { });

        // Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

        var content = await _testService.DeserializeResponse<object>(response);

        Assert.AreEqual(1, content.Errors.Count);
        Assert.AreEqual("Serviço não encontrado.", content.Errors[0]);
    }

    [TestMethod]
    public async Task AddServiceToReservation_WithDisabledService_ShouldReturn_BAD_REQUEST()
    {
        // Arrange
        var customer = await _testService.CreateCustomerAsync(
            new CreateUser("Gabriela", "Moreira", "gabrielaMoreira@gmail.com", "+55 (61) 92315-6389", "password20", EGender.Feminine, DateTime.Now.AddYears(-24), "Brazil", "Taguatinga", "DF-2020", 2020)
        );

        var room = await _testService.CreateRoomAsync(new EditorRoom("2Quarto 1", 21, 90, 5, "Quarto 21", _category.Id));
        var reservation = await _testService.CreateReservationAsync(customer, new CreateReservation(DateTime.Now.AddDays(1), DateTime.Now.AddDays(6), room.Id, 3));

        var service = await _testService.CreateServiceAsync(new EditorService("Wake-Up Call", "Wake-Up Call", 1.00m, EPriority.High, 5));
        service.Disable();
        await _dbContext.SaveChangesAsync();

        await _testService.AddServiceToRoom(room.Id, service.Id); 

        _factory.Login(_client, _rootAdminToken);

        // Act
        var response = await _client.PostAsJsonAsync($"{_baseUrl}/{reservation.Id}/services/{service.Id}", new { });

        // Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        var content = await _testService.DeserializeResponse<object>(response);

        var reservationWithServices = await _dbContext.Reservations.Include(x => x.Services).FirstAsync(x => x.Id == reservation.Id);

        Assert.AreEqual(1, content.Errors.Count);
        Assert.IsTrue(content.Errors.Any(x => x.Contains("desativado")));

        Assert.AreEqual(0, reservationWithServices.Services.Count);
    }


    [TestMethod]
    public async Task RemoveServiceFromReservation_ShouldReturn_OK()
    {
        // Arrange
        var customer = await _testService.CreateCustomerAsync(
            new CreateUser("Felipe", "Costa", "felipeCosta@gmail.com", "+55 (83) 99931-5678", "password21", EGender.Masculine, DateTime.Now.AddYears(-29), "Brazil", "João Pessoa", "PB-2121", 2121)
        );

        var service = await _testService.CreateServiceAsync(new EditorService("Minibar Restock", "Minibar Restock", 15.00m, EPriority.Low, 20));

        var room = await _testService.CreateRoomAsync(new EditorRoom("Quarto 22", 22, 90, 5, "Quarto 22", _category.Id));
        await _testService.AddServiceToRoom(room.Id, service.Id);

        var reservation = await _testService.CreateReservationAsync(customer, new CreateReservation(DateTime.Now.AddDays(1), DateTime.Now.AddDays(6), room.Id, 3));
        await _testService.AddServiceToReservation(reservation.Id, service.Id);

        _factory.Login(_client, _rootAdminToken);

        // Act
        var response = await _client.DeleteAsync($"{_baseUrl}/{reservation.Id}/services/{service.Id}");

        // Assert
        Assert.IsNotNull(response);
        response.EnsureSuccessStatusCode();

        var content = await _testService.DeserializeResponse<object>(response);
        var reservationWithServices = await _dbContext.Reservations.Include(x => x.Services).FirstAsync(x => x.Id == reservation.Id);

        Assert.AreEqual(0, content.Errors.Count);
        Assert.AreEqual("Serviço removido com sucesso!", content.Message);

        Assert.AreEqual(0, reservationWithServices.Services.Count);

        var paymentIntent = await _testService.GetAndVerifyPaymentIntent(_stripePaymentIntentService, reservation);
        var products = _testService.GetMetadataProductsFromPaymentIntent(paymentIntent);
       
        var roomProduct = products.First(x => x.Id == room.Id);
        var serviceProduct = products.FirstOrDefault(x => x.Id == service.Id);

        Assert.IsFalse(roomProduct.IsService);
        Assert.AreEqual(1, roomProduct.Quantity);
        Assert.AreEqual(room.StripeProductId, roomProduct.ProductId);

        Assert.IsNull(serviceProduct);

        Assert.AreEqual(1, products.Count);
    }


    [TestMethod]
    public async Task RemoveServiceFromReservation_WithServiceQtdBeyond1_ShouldReturn_OK()
    {
        //Arange
        var customer = await _testService.CreateCustomerAsync(new CreateUser("Vanessa", "Costa", "vanessaCosta@gmail.com", "+55 (21) 98765-5432", "password987", EGender.Feminine, DateTime.Now.AddYears(-31), "Brazil", "Rio de Janeiro", "RJ-666", 666));
        var service = await _testService.CreateServiceAsync(new EditorService("KitNet Service", "KitNet Service", 30.00m, EPriority.Low, 20));
        var room = await _testService.CreateRoomAsync(new EditorRoom("Quarto 87", 87, 90, 5, "Quarto 87", _category.Id));
        await _testService.AddServiceToRoom(room.Id, service.Id);

        var reservation = await _testService.CreateReservationAsync(customer, new CreateReservation(DateTime.Now.AddDays(1), DateTime.Now.AddDays(6), room.Id, 3));       

        await _testService.AddServiceToReservation(reservation.Id, service.Id);
        await _testService.AddServiceToReservation(reservation.Id, service.Id);

        //Act
        var response = await _client.DeleteAsync($"{_baseUrl}/{reservation.Id}/services/{service.Id}");

        //Assert
        Assert.IsNotNull(response);
        response.EnsureSuccessStatusCode();

        var content = await _testService.DeserializeResponse<object>(response);

        var updatedReservation = await _testService.GetReservation(reservation.Id);

        _testService.CompareReservation(reservation, updatedReservation);

        Assert.AreEqual(0, content.Errors.Count);
        Assert.AreEqual("Serviço removido com sucesso!", content.Message);

        Assert.AreEqual(1, updatedReservation.Services.Count);

        var paymentIntent = await _testService.GetAndVerifyPaymentIntent(_stripePaymentIntentService, reservation);

        var products = _testService.GetMetadataProductsFromPaymentIntent(paymentIntent);

        var roomProduct = products.First(x => x.Id == room.Id);
        var serviceProduct = products.First(x => x.Id == service.Id);

        Assert.IsFalse(roomProduct.IsService);
        Assert.AreEqual(1, roomProduct.Quantity);
        Assert.AreEqual(room.StripeProductId, roomProduct.ProductId);

        Assert.IsTrue(serviceProduct.IsService);
        Assert.AreEqual(service.Name, serviceProduct.Name);
        Assert.AreEqual(service.Price, serviceProduct.UnitPrice);
        Assert.AreEqual(1, serviceProduct.Quantity);
        Assert.AreEqual(service.StripeProductId, serviceProduct.ProductId);

        Assert.AreEqual(2, products.Count);
    }

    [TestMethod]
    public async Task RemoveServiceFromReservation_WithNonexistReservation_ShouldReturn_NOT_FOUND()
    {
        // Arrange
        var customer = await _testService.CreateCustomerAsync(new CreateUser("Laura", "Nascimento", "lauraNascimento@gmail.com", "+55 (62) 99887-6543", "password22", EGender.Feminine, DateTime.Now.AddYears(-28), "Brazil", "Anápolis", "GO-2222", 2222));

        var room = await _testService.CreateRoomAsync(new EditorRoom("2Quarto 3", 23, 90, 5, "Quarto 23", _category.Id));
        var service = await _testService.CreateServiceAsync(new EditorService("Airport Shuttle", "Airport Shuttle", 60.00m, EPriority.High, 45));
        room.AddService(service);

        // Act
        var response = await _client.DeleteAsync($"{_baseUrl}/{Guid.NewGuid()}/services/{service.Id}");

        // Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

        var content = await _testService.DeserializeResponse<object>(response);

        Assert.AreEqual(1, content.Errors.Count);
        Assert.AreEqual("Reserva não encontrada.", content.Errors[0]);
    }


    [TestMethod]
    public async Task RemoveServiceFromReservation_WithNonexistService_ShouldReturn_NOT_FOUND()
    {
        // Arrange
        var customer = await _testService.CreateCustomerAsync(new CreateUser("Vinícius", "Cardoso", "viniciusCardoso@gmail.com", "+55 (47) 98739-4311", "password23", EGender.Masculine, DateTime.Now.AddYears(-31), "Brazil", "Joinville", "SC-2323", 2323));
        var room = await _testService.CreateRoomAsync(new EditorRoom("2Quarto 4", 24, 90, 5, "Quarto 24", _category.Id));
        var reservation = await _testService.CreateReservationAsync(customer, new CreateReservation(DateTime.Now.AddDays(1), DateTime.Now.AddDays(6), room.Id, 3));

        _factory.Login(_client, _rootAdminToken);
        // Act
        var response = await _client.DeleteAsync($"{_baseUrl}/{reservation.Id}/services/{Guid.NewGuid()}");

        // Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

        var content = await _testService.DeserializeResponse<object>(response);

        Assert.AreEqual(1, content.Errors.Count);
        Assert.AreEqual("Serviço não encontrado.", content.Errors[0]);
    }


    [TestMethod]
    public async Task RemoveServiceFromReservation_WithoutContainsService_ShouldReturn_BAD_REQUEST()
    {
        // Arrange
        var customer = await _testService.CreateCustomerAsync(new CreateUser("André", "Pinto", "andrePinto@gmail.com", "+55 (95) 93456-7890", "password25", EGender.Masculine, DateTime.Now.AddYears(-33), "Brazil", "Boa Vista", "RR-2525", 2525));
        var room = await _testService.CreateRoomAsync(new EditorRoom("2Quarto 5", 25, 90, 5, "Quarto 25", _category.Id));
        var reservation = await _testService.CreateReservationAsync(customer, new CreateReservation(DateTime.Now.AddDays(1), DateTime.Now.AddDays(6), room.Id, 3));
        var service = await _testService.CreateServiceAsync(new EditorService("Jet Ski Rental", "Jet Ski Rental", 90.00m, EPriority.High, 60));

        _factory.Login(_client, _rootAdminToken);
        // Act
        var response = await _client.DeleteAsync($"{_baseUrl}/{reservation.Id}/services/{service.Id}");

        // Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        var content = await _testService.DeserializeResponse<object>(response);

        Assert.AreEqual(1, content.Errors.Count);
        Assert.IsTrue(content!.Errors.Any(x => x.Contains("atribuido a essa reserva")));
    }

    [TestMethod]
    public async Task FinishReservation_ShouldReturn_OK()
    {
        // Arrange
        var customer = await _testService.CreateCustomerAsync(new CreateUser("Eduardo", "Wimp", "eduardoowk1@gmail.com", "+55 (92) 92345-6789", "password26", EGender.Feminine, DateTime.Now.AddYears(-27), "Brazil", "Manaus", "AM-2626", 2626));
        var room = await _testService.CreateRoomAsync(new EditorRoom("2Quarto 6", 26, 90, 5, "Quarto 26", _category.Id));
        var reservation = await _testService.CreateReservationAsync(customer, new CreateReservation(DateTime.Now.AddDays(1), DateTime.Now.AddDays(6), room.Id, 3));

        reservation.ToCheckIn(); // Marca a reserva como check-in
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await _client.PatchAsJsonAsync($"{_baseUrl}/finish/{reservation.Id}", new { });

        // Assert
        var content = await _testService.DeserializeResponse<object>(response);

        response.EnsureSuccessStatusCode();

        var updatedReservation = await _dbContext.Reservations.FirstAsync(x => x.Id == reservation.Id);

        Assert.AreEqual(0, content.Errors.Count);
        Assert.AreEqual("Reserva finalizada com sucesso!", content.Message);
        Assert.AreEqual(EReservationStatus.CheckedOut, updatedReservation.Status);
    }

    [TestMethod]
    public async Task FinishReservation_WithNonexistReservation_ShouldReturn_NOT_FOUND()
    {
        //Act
        var response = await _client.PatchAsJsonAsync($"{_baseUrl}/finish/{Guid.NewGuid()}", new { });

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

        var content = await _testService.DeserializeResponse<object>(response);


        Assert.AreEqual(1, content.Errors.Count);
        Assert.AreEqual("Reserva não encontrada.", content.Errors[0]);
    }

    [TestMethod]
    public async Task FinishReservation_WithDifferentCustomer_ShouldReturn_UNAUTHORIZED()
    {
        // Arrange
        var customer1 = await _testService.CreateCustomerAsync(new CreateUser(
            "Fernando", "Dias", "fernandoDias@gmail.com", "+55 (98) 91234-5678", "password27",
            EGender.Masculine, DateTime.Now.AddYears(-32), "Brazil", "São Luís", "MA-2727", 2727));

        var room = await _testService.CreateRoomAsync(new EditorRoom("2Quarto 7", 27, 90, 5, "Quarto 27", _category.Id));

        var reservation = await _testService.CreateReservationAsync(customer1, new CreateReservation(
            DateTime.Now.AddDays(1), DateTime.Now.AddDays(6), room.Id, 3));

        reservation.ToCheckIn();
        await _dbContext.SaveChangesAsync();

        // Act
        _factory.Login(_client, _customerToken); //Login com outro cliente que não criou a reserva

        var response = await _client.PatchAsJsonAsync($"{_baseUrl}/finish/{reservation.Id}", new { });

        // Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);

        var content = await _testService.DeserializeResponse<object>(response);

        var updatedReservation = await _testService.GetReservation(reservation.Id);

        Assert.AreEqual(1, content.Errors.Count);
        Assert.AreEqual("Você não tem permissão para finalizar reserva alheia.", content.Errors[0]);

        Assert.AreEqual(EReservationStatus.CheckedIn, updatedReservation.Status);
    }


    [TestMethod]
    public async Task CancelReservation_ShouldReturn_OK()
    {
        // Arrange
        var newCustomer = await _testService.CreateCustomerAsync(new CreateUser(
            "Aline", "Fernandes", "alineFernandes@gmail.com", "+55 (75) 99715-4321", "password28",
            EGender.Feminine, DateTime.Now.AddYears(-25), "Brazil", "Feira de Santana", "BA-2828", 2828));

        var room = await _testService.CreateRoomAsync(new EditorRoom("Quarto 28", 49, 90, 5, "Quarto 28", _category.Id));

        var reservation = await _testService.CreateReservationAsync(newCustomer, new CreateReservation(
            DateTime.Now.AddDays(1), DateTime.Now.AddDays(6), room.Id, 3));

        // Act
        var response = await _client.PatchAsJsonAsync($"{_baseUrl}/cancel/{reservation.Id}", new { });

        // Assert
        Assert.IsNotNull(response);
        response.EnsureSuccessStatusCode();

        var content = await _testService.DeserializeResponse<object>(response);

        var updatedReservation = await _testService.GetReservation(reservation.Id);

        Assert.AreEqual(0, content.Errors.Count);
        Assert.AreEqual("Reserva cancelada com sucesso!", content.Message);

        Assert.AreEqual(EReservationStatus.Canceled, updatedReservation.Status);

        var paymentIntent = await _stripePaymentIntentService.GetAsync(reservation.StripePaymentIntentId);
        Assert.AreEqual("canceled", paymentIntent.Status);
    }


    [TestMethod]
    public async Task CancelReservation_WithNonexistReservation_ShouldReturn_NOT_FOUND()
    {
        //Act
        var response = await _client.PatchAsJsonAsync($"{_baseUrl}/cancel/{Guid.NewGuid()}", new { });

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

        var content = await _testService.DeserializeResponse<object>(response);


        Assert.AreEqual(1, content.Errors.Count);
        Assert.AreEqual("Reserva não encontrada.", content.Errors[0]);
    }

    [TestMethod]
    public async Task CancelReservation_WithDifferentCustomer_ShouldReturn_UNAUTHORIZED()
    {
        // Arrange
        var customer = await _testService.CreateCustomerAsync(new CreateUser("Paulo", "Moura", "pauloMoura@gmail.com", "+55 (88) 97614-3310", "password29",EGender.Masculine, DateTime.Now.AddYears(-34), "Brazil", "Sobral", "CE-2929", 2929));
        var room = await _testService.CreateRoomAsync(new EditorRoom("Quarto 821", 821, 90, 5, "Quarto 821", _category.Id));
        var reservation = await _testService.CreateReservationAsync(customer, new CreateReservation(DateTime.Now.AddDays(1), DateTime.Now.AddDays(3), room.Id, 2));

        _factory.Login(_client, _customerToken);
        // Act
        var response = await _client.PatchAsJsonAsync($"{_baseUrl}/cancel/{reservation.Id}", new { });

        // Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);

        var content = await _testService.DeserializeResponse<object>(response);

        var updatedReservation = await _testService.GetReservation(reservation.Id);

        Assert.AreEqual(1, content.Errors.Count);
        Assert.AreEqual("Você não tem permissão para cancelar reserva alheia.", content.Errors[0]);

        Assert.AreEqual(EReservationStatus.Pending, updatedReservation.Status);
    }


    [TestMethod]
    public async Task CancelReservation_WithStripeError_ShouldReturn_BAD_REQUEST_AND_MAKE_ROLLBACK()
    {
        // Arrange
        var customer = await _testService.CreateCustomerAsync(new CreateUser("Ana", "Oliveira", "anaOliveira@gmail.com", "+55 (19) 98765-8765", "password789",EGender.Feminine, DateTime.Now.AddYears(-22), "Brazil", "Campinas", "CP-404", 404));
        var room = await _testService.CreateRoomAsync(new EditorRoom("Quarto 132", 132, 70, 5, "Quarto 132", _category.Id));
        var reservation = await _testService.CreateReservationAsync(customer, new CreateReservation(
            DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), room.Id, 3));

        // Simulate Stripe API key error
        var originalApiKey = StripeConfiguration.ApiKey;
        StripeConfiguration.ApiKey = "";

        try
        {
            // Act
            var response = await _client.PatchAsJsonAsync($"{_baseUrl}/cancel/{reservation.Id}", new { });

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            var content = await _testService.DeserializeResponse<object>(response);

            var existsInDb = await _dbContext.Reservations.AnyAsync(x => x.Id == reservation.Id);

            Assert.AreEqual("Ocorreu um erro ao cancelar o PaymentIntent no Stripe", content.Errors[0]);
            Assert.IsTrue(existsInDb);

            StripeConfiguration.ApiKey = originalApiKey;
            // Check that the PaymentIntent is still in requires_payment_method state
            var paymentIntent = await _stripePaymentIntentService.GetAsync(reservation.StripePaymentIntentId);
            Assert.AreEqual("requires_payment_method", paymentIntent.Status);
        }
        finally
        {
            // Ensure to reset the Stripe API key
            StripeConfiguration.ApiKey = originalApiKey;
        }
    }

    [TestMethod]
    public async Task GetTotalAmount_ShouldReturn_OK()
    {
        //Arange
        var services = new List<Service>()
        {
            new Service("Tennis Lesson","Tennis Lesson", 50.00m, EPriority.Medium, 60),
            new Service("Personal Shopping","Personal Shopping", 60.00m, EPriority.Medium, 90)
        };

        await _dbContext.Services.AddRangeAsync(services);
        await _dbContext.SaveChangesAsync();

        var format = "yyyy-MM-ddTHH:mm:ss";
        //Act

        var response = await _client.GetAsync($"{_baseUrl}/total-amount?checkIn={DateTime.Now.AddDays(1).ToString(format)}&checkOut={DateTime.Now.AddDays(4).ToString(format)}&dailyRate=50&servicesIds={services[0].Id},{services[1].Id}");

        //Assert
        Assert.IsNotNull(response);
        response.EnsureSuccessStatusCode();

        var content = JsonConvert.DeserializeObject<Response<TotalAmountResponse>>(await response.Content.ReadAsStringAsync())!;

        Assert.AreEqual("Sucesso!", content.Message);
        Assert.AreEqual(260, Math.Ceiling(content.Data.TotalAmount));
    }

    [TestMethod]
    [DataRow("2024-06-15T00:00:00", "2024-05-14T00:00:00", 30.00, "A data de check-out deve ser maior que a data de check-in.")]
    public async Task GetTotalAmount_WithInvalidParameters_ShouldReturn_BAD_REQUEST(string checkIn, string checkOut, double dailyRate, string expectedError)
    {
        //Act
        var response = await _client.GetAsync($"{_baseUrl}/total-amount?checkIn={checkIn}&checkOut={checkOut}&dailyRate={dailyRate}");

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        var content = await _testService.DeserializeResponse<object>(response);


        Assert.AreEqual(expectedError, content.Errors[0]);
    }
}

internal record GetReservation(Guid Id, decimal DailyRate, TimeSpan ExpectedTimeHosted, DateTime ExpectedCheckIn, DateTime ExpectedCheckOut, TimeSpan? TimeHosted, DateTime? CheckIn, DateTime? CheckOut, EReservationStatus Status, int Capacity, Guid RoomId, Guid CustomerId, Guid? InvoiceId);
internal record TotalAmountResponse(decimal TotalAmount);