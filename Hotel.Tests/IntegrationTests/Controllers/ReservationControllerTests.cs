using Hotel.Domain.Data;
using Hotel.Domain.DTOs.ReservationDTOs;
using Hotel.Domain.Entities.CustomerEntity;
using Hotel.Domain.Entities.ReservationEntity;
using Hotel.Domain.Entities.CategoryEntity;
using Hotel.Domain.Entities.RoomEntity;
using Hotel.Domain.Entities.ServiceEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Services.TokenServices;
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
        //Arange
        var newCustomer = new CreateUser
        (
            "Maria", "Silva",
            "mariaSilva@gmail.com",
            "+55 (11) 98765-1234",
            "password123",
            EGender.Feminine,
            DateTime.Now.AddYears(-25),
            "Brazil", "São Paulo", "SP-101", 101
        );

        var verificationCode = new VerificationCode(new Email(newCustomer.Email));
        await _dbContext.VerificationCodes.AddAsync(verificationCode);
        await _dbContext.SaveChangesAsync();

        var createCustomerResponse = await _client.PostAsJsonAsync($"v1/register/customers?code={verificationCode.Code}", newCustomer);
        var createCustomerContent = JsonConvert.DeserializeObject<Response<DataStripeCustomerId>>(await createCustomerResponse.Content.ReadAsStringAsync())!;
        var customer = await _dbContext.Customers.FirstAsync(x => x.Id == createCustomerContent.Data.Id);

        _factory.Login(_client, customer);

        var room = new Room("Quarto 2",2, 50, 2, "Quarto padrão", _category);

        await _dbContext.Rooms.AddAsync(room);
        await _dbContext.SaveChangesAsync();

        var body = new CreateReservation(DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), room.Id, 1);

        //Act
        var response = await _client.PostAsJsonAsync(_baseUrl, body);

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Response<DataStripePaymentIntentId>>(await response.Content.ReadAsStringAsync())!;
        var createdReservation = await _dbContext.Reservations
            .Include(x => x.Customer)
            .Include(x => x.Room)
            .FirstAsync(x => x.Id == content!.Data!.Id);

        
        Assert.AreEqual("Reserva criada com sucesso!", content.Message);
        Assert.AreEqual(body.ExpectedCheckIn, createdReservation.ExpectedCheckIn);
        Assert.AreEqual(body.ExpectedCheckOut, createdReservation.ExpectedCheckOut);
        Assert.AreEqual(body.Capacity, createdReservation.Capacity);
        Assert.AreEqual(body.RoomId, createdReservation.RoomId);
        Assert.IsNull(createdReservation.Invoice);
        Assert.IsNull(createdReservation.CheckIn);
        Assert.IsNull(createdReservation.CheckOut);
        Assert.AreEqual(room.Price, createdReservation.DailyRate);
        Assert.AreEqual(1, Math.Round(createdReservation.ExpectedTimeHosted.TotalDays, 0));
        Assert.AreEqual(50, Math.Round(createdReservation.ExpectedTotalAmount(), 2));
        Assert.AreEqual(ERoomStatus.Reserved, createdReservation.Room!.Status);

        var paymentIntent = await _stripePaymentIntentService.GetAsync(content.Data.StripePaymentIntentId);
        Assert.IsNotNull(paymentIntent);
        Assert.AreEqual("requires_payment_method",paymentIntent.Status);
        Assert.AreEqual(Math.Round(createdReservation.ExpectedTotalAmount() * 100), paymentIntent.Amount);
        Assert.AreEqual(createdReservation.Customer!.StripeCustomerId, paymentIntent.CustomerId);
        Assert.AreEqual(createdReservation.RoomId.ToString(), paymentIntent.Metadata["room_id"]);
    }

    [TestMethod]
    public async Task CreateReservation_WithUnavailableRoom_ShouldReturn_BAD_REQUEST()
    {
        //Arange
        var customer = new Domain.Entities.CustomerEntity.Customer(
          new Name("Juliana", "Martins"),
          new Email("julianaMartins@gmail.com"),
          new Phone("+55 (48) 98765-1321"),
          "123",
          EGender.Feminine,
          DateTime.Now.AddYears(-24),
          new Domain.ValueObjects.Address("Brazil", "Florianópolis", "SC-909", 909)
        );
        var room = new Room("Quarto 3",3, 80, 2, "Quarto de luxo nível 1", _category);
        var reservation = new Reservation(room, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), customer, 1);

        await _dbContext.Customers.AddAsync(customer);
        await _dbContext.Rooms.AddAsync(room);
        await _dbContext.Reservations.AddAsync(reservation);
        await _dbContext.SaveChangesAsync();

        var body = new CreateReservation(DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), room.Id, 1);

        //Act
        var response = await _client.PostAsJsonAsync(_baseUrl, body);

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;
        var reservations = await _dbContext.Reservations.Where(x => x.RoomId == room.Id).ToListAsync();

        Assert.AreEqual("Não é possível realizar a reserva pois a hospedagem está indisponível.", content.Errors[0]);
        Assert.AreEqual(1, reservations.Count);
    }

    [TestMethod]
    public async Task CreateReservation_WithDisabledRoom_ShouldReturn_BAD_REQUEST()
    {
        //Arange
        var room = new Room("Quarto 4", 4, 110, 2, "Quarto de luxo nível 2", _category);
        room.Disable();

        await _dbContext.Rooms.AddAsync(room);
        await _dbContext.SaveChangesAsync();

        var body = new CreateReservation(DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), room.Id, 1);

        //Act
        var response = await _client.PostAsJsonAsync(_baseUrl, body);

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;
        var reservations = await _dbContext.Reservations.Where(x => x.RoomId == room.Id).ToListAsync();

       
        Assert.AreEqual("Não é possível realizar a reserva pois a hospedagem está inativo.", content.Errors[0]);
        Assert.AreEqual(0, reservations.Count);
    }

    [TestMethod]
    public async Task CreateReservation_WithRoomGuestsLimitExceeded_ShouldReturn_BAD_REQUEST()
    {
        //Arange
        var room = new Room("Quarto 5",5, 150, 2, "Quarto de luxo nível 3", _category);

        await _dbContext.Rooms.AddAsync(room);
        await _dbContext.SaveChangesAsync();

        var body = new CreateReservation(DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), room.Id, 3);

        //Act
        var response = await _client.PostAsJsonAsync(_baseUrl, body);

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;
        var reservations = await _dbContext.Reservations.Where(x => x.RoomId == room.Id).ToListAsync();

       
        Assert.AreEqual("Capacidade máxima de hospedades da hospedagem excedida.", content.Errors[0]);
        Assert.AreEqual(0, reservations.Count);
    }

    [TestMethod]
    public async Task CreateReservation_WithNonexistCustomer_ShouldReturn_NOT_FOUND()
    {
        //Arange
        var customer = new Domain.Entities.CustomerEntity.Customer(
          new Name("Pedro", "Silva"),
          new Email("pedroSilva@gmail.com"),
          new Phone("+55 (62) 99876-5432"),
          "password8",
          EGender.Masculine,
          DateTime.Now.AddYears(-31),
          new Domain.ValueObjects.Address("Brazil", "Goiânia", "GO-808", 808)
        );
        var room = new Room("Quarto 6",6, 190, 2, "Quarto de luxo nível 4", _category);

        await _dbContext.Rooms.AddAsync(room);
        await _dbContext.SaveChangesAsync();

        var token = _tokenService.GenerateToken(customer);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var body = new CreateReservation(DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), room.Id, 1);

        //Act
        var response = await _client.PostAsJsonAsync(_baseUrl, body);

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;
        var reservations = await _dbContext.Reservations.Where(x => x.RoomId == room.Id).ToListAsync();


        Assert.AreEqual("Usuário não encontrado", content.Errors[0]);
        Assert.AreEqual(0, reservations.Count);
    }

    [TestMethod]
    public async Task CreateReservation_WithNonexistRoom_ShouldReturn_NOT_FOUND()
    {
        //Arange
        var body = new CreateReservation(DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), Guid.NewGuid(), 1); //Random guid

        //Act
        var response = await _client.PostAsJsonAsync(_baseUrl, body);

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;


        Assert.AreEqual("Hospedagem não encontrada", content.Errors[0]);
    }

    [TestMethod]
    public async Task CreateReservation_WithInvalidStripeCustomerId_ShouldReturn_BAD_REQUEST_AND_MAKE_ROLLBACK()
    {
        //Arange
        var newCustomer = new Domain.Entities.CustomerEntity.Customer
        (
            new Name("Camila", "Barbosa"),
            new Email("camilaBarbosa@gmail.com"),
            new Phone("+55 (95) 98765-6543"),
            "password543",
            EGender.Feminine,
            DateTime.Now.AddYears(-32),
            new Domain.ValueObjects.Address("Brazil", "Manaus", "MA-1010", 1010)
        );
        var room = new Room("Quarto 931", 931, 50, 2, "Quarto padrão 931", _category);

        await _dbContext.Rooms.AddAsync(room);
        await _dbContext.Customers.AddAsync(newCustomer);
        await _dbContext.SaveChangesAsync();

        _factory.Login(_client, newCustomer);

        var body = new CreateReservation(DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), room.Id, 1);

        //Act
        var response = await _client.PostAsJsonAsync(_baseUrl, body);

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;

        Assert.AreEqual("Ocorreu um erro ao criar a intenção de pagamento no Stripe", content.Errors[0]);

        var wasCreated = await _dbContext.Reservations.AnyAsync(x => x.RoomId == room.Id);
        Assert.IsFalse(wasCreated);
    }

    [TestMethod]
    public async Task GetReservations_ShouldReturn_OK()
    {
        //Arange
        var customer = new Domain.Entities.CustomerEntity.Customer(
          new Name("Fernanda", "Ribeiro"),
          new Email("fernandaRibeiro@gmail.com"),
          new Phone("+55 (51) 91034-5678"),
          "password7",
          EGender.Feminine,
          DateTime.Now.AddYears(-26),
          new Domain.ValueObjects.Address("Brazil", "Porto Alegre", "RS-707", 707)
        );
        var room = new Room("Quarto 7",7, 240, 2, "Quarto de luxo nível 5", _category);
        var reservation = new Reservation(room, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), customer, 1);

        await _dbContext.Customers.AddAsync(customer);
        await _dbContext.Rooms.AddAsync(room);
        await _dbContext.Reservations.AddAsync(reservation);
        await _dbContext.SaveChangesAsync();

        //Act
        var response = await _client.GetAsync(_baseUrl);

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Response<List<GetReservation>>>(await response.Content.ReadAsStringAsync())!;

        
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
        //Arange
        var customer = new Domain.Entities.CustomerEntity.Customer(
          new Name("Lucas", "Ferreira"),
          new Email("lucasFerreira@gmail.com"),
          new Phone("+55 (61) 92744-6789"),
          "password6",
          EGender.Masculine,
          DateTime.Now.AddYears(-28),
          new Domain.ValueObjects.Address("Brazil", "Brasília", "DF-606", 606)
        );
        var room = new Room("Quarto 8",8, 70, 5, "Quarto de luxo básico", _category);
        var reservation = new Reservation(room, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), customer, 3);

        await _dbContext.Customers.AddAsync(customer);
        await _dbContext.Rooms.AddAsync(room);
        await _dbContext.Reservations.AddAsync(reservation);
        await _dbContext.SaveChangesAsync();

        //Act
        var response = await _client.GetAsync($"{_baseUrl}/{reservation.Id}");

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Response<GetReservation>>(await response.Content.ReadAsStringAsync())!;

        
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
        //Arange
        var newCustomer = new CreateUser
        (
            "Camila", "Costa",
            "camilaCosta@gmail.com",
            "+55 (71) 93136-7891",
            "password5",
            EGender.Feminine,
            DateTime.Now.AddYears(-29),
            "Brazil", "Salvador", "BA-505", 505
        );
        var verificationCode = new VerificationCode(new Email(newCustomer.Email));
        await _dbContext.VerificationCodes.AddAsync(verificationCode);
        await _dbContext.SaveChangesAsync();

        var createCustomerResponse = await _client.PostAsJsonAsync($"v1/register/customers?code={verificationCode.Code}", newCustomer);
        var createCustomerContent = JsonConvert.DeserializeObject<Response<DataStripeCustomerId>>(await createCustomerResponse.Content.ReadAsStringAsync())!;
        var customer = await _dbContext.Customers.FirstAsync(x => x.Id == createCustomerContent.Data.Id);

        var room = new Room("Quarto 9", 9, 70, 5, "Quarto 1", _category);

        await _dbContext.Rooms.AddAsync(room);
        await _dbContext.SaveChangesAsync();

        _factory.Login(_client, customer);

        var newReservation = new CreateReservation(DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), room.Id, 3);
        var createReservationResponse = await _client.PostAsJsonAsync(_baseUrl, newReservation);
        var createReservationContent = JsonConvert.DeserializeObject<Response<DataStripePaymentIntentId>>(await createReservationResponse.Content.ReadAsStringAsync())!;
        var reservation = await _dbContext.Reservations.FirstAsync(x => x.Id == createReservationContent.Data.Id);

        _factory.Login(_client, _rootAdminToken);

        //Act
        var response = await _client.DeleteAsync($"{_baseUrl}/{reservation.Id}");

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Response<DataStripePaymentIntentId>>(await response.Content.ReadAsStringAsync())!;

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

        var newCustomer = new CreateUser
        (
            "Rafael", "Ribeiro",
            "rafaelRibeiro@gmail.com",
            "+55 (84) 98765-3456",
            "password210",
            EGender.Masculine,
            DateTime.Now.AddYears(-40),
            "Brazil", "Natal", "NT-909", 909
        );
        var verificationCode = new VerificationCode(new Email(newCustomer.Email));
        await dbContext.VerificationCodes.AddAsync(verificationCode);
        await dbContext.SaveChangesAsync();

        var createCustomerResponse = await client.PostAsJsonAsync($"v1/register/customers?code={verificationCode.Code}", newCustomer);
        var createCustomerContent = JsonConvert.DeserializeObject<Response<DataStripeCustomerId>>(await createCustomerResponse.Content.ReadAsStringAsync())!;
        var customer = await dbContext.Customers.FirstAsync(x => x.Id == createCustomerContent.Data.Id);

        var room = new Room("Quarto 671", 671, 70, 5, "Quarto 671", _category);

        await dbContext.Rooms.AddAsync(room);
        await dbContext.SaveChangesAsync();

        factory.Login(client, customer);

        var newReservation = new CreateReservation(DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), room.Id, 3);
        var createReservationResponse = await client.PostAsJsonAsync(_baseUrl, newReservation);
        var createReservationContent = JsonConvert.DeserializeObject<Response<DataStripePaymentIntentId>>(await createReservationResponse.Content.ReadAsStringAsync())!;
        var reservation = await dbContext.Reservations.FirstAsync(x => x.Id == createReservationContent.Data.Id);

        factory.Login(client, _rootAdminToken);

        var apiKey = StripeConfiguration.ApiKey.ToString();
        StripeConfiguration.ApiKey = "";

        //Act
        var response = await client.DeleteAsync($"{_baseUrl}/{reservation.Id}");

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;

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

        var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;

        
        Assert.AreEqual("Você não tem acesso a esse serviço.", content.Errors[0]);
    }


    [TestMethod]
    public async Task DeleteReservation_WithNonexistReservation_ShouldReturn_NOT_FOUND()
    {
        //Arange
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _rootAdminToken);

        //Act
        var response = await _client.DeleteAsync($"{_baseUrl}/{Guid.NewGuid()}");

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;


    }

    [TestMethod]
    public async Task DeleteReservation_WithCheckedInReservationStatus_ShouldReturn_BAD_REQUEST()
    {
        //Arange
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _rootAdminToken);
        var customer = new Domain.Entities.CustomerEntity.Customer(
          new Name("Rafael", "Oliveira"),
          new Email("rafaelOliveira@gmail.com"),
          new Phone("+55 (41) 97604-1210"),
          "password4",
          EGender.Masculine,
          DateTime.Now.AddYears(-32),
          new Domain.ValueObjects.Address("Brazil", "Curitiba", "PR-404", 404)
        );
        var room = new Room("1Quarto 0",10, 70, 5, "Quarto 2", _category);
        var reservation = new Reservation(room, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), customer, 3);
        reservation.ToCheckIn(); // check in and change status

        await _dbContext.Customers.AddAsync(customer);
        await _dbContext.Rooms.AddAsync(room);
        await _dbContext.Reservations.AddAsync(reservation);
        await _dbContext.SaveChangesAsync();

        //Act
        var response = await _client.DeleteAsync($"{_baseUrl}/{reservation.Id}");

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;

       
        Assert.IsTrue(content!.Errors.Any(x => x.Contains("deletar a reserva sem antes finaliza-la.")));
    }

    [TestMethod]
    public async Task UpdateExpectedCheckOut_ShouldReturn_OK()
    {
        //Arange
        var customer = new Domain.Entities.CustomerEntity.Customer(
          new Name("Beatriz", "Santos"),
          new Email("beatrizSantos@gmail.com"),
          new Phone("+55 (31) 90176-5432"),
          "password3",
          EGender.Feminine,
          DateTime.Now.AddYears(-27),
          new Domain.ValueObjects.Address("Brazil", "Belo Horizonte", "MG-303", 303)
        );
        var room = new Room("1Quarto 1",11, 70, 5, "Quarto 3", _category);
        var reservation = new Reservation(room, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), customer, 3);

        await _dbContext.Customers.AddAsync(customer);
        await _dbContext.Rooms.AddAsync(room);
        await _dbContext.Reservations.AddAsync(reservation);
        await _dbContext.SaveChangesAsync();

        var newExpectedCheckOut = DateTime.Now.AddDays(3);
        var body = new UpdateCheckOut(newExpectedCheckOut);

        //Act
        var response = await _client.PatchAsJsonAsync($"{_baseUrl}/{reservation.Id}/check-out", body);

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;

        var updatedReservation = await _dbContext.Reservations.FirstAsync(x => x.Id == reservation.Id);

        
        Assert.AreEqual(0, content.Errors.Count);
        Assert.AreEqual("CheckOut esperado atualizado com sucesso!", content.Message);
        Assert.AreEqual(newExpectedCheckOut, updatedReservation.ExpectedCheckOut);
        Assert.AreEqual(reservation.Id, updatedReservation.Id);
        Assert.AreEqual(reservation.DailyRate, updatedReservation.DailyRate);
        Assert.AreEqual(reservation.Capacity, updatedReservation.Capacity);
        Assert.AreEqual(reservation.Status, updatedReservation.Status);
        Assert.AreEqual(reservation.CustomerId, updatedReservation.CustomerId);
        Assert.AreEqual(reservation.ExpectedCheckIn, updatedReservation.ExpectedCheckIn);
        Assert.AreEqual(reservation.ExpectedTimeHosted, updatedReservation.ExpectedTimeHosted);
        Assert.AreEqual(reservation.CheckIn, updatedReservation.CheckIn);
        Assert.AreEqual(reservation.CheckOut, updatedReservation.CheckOut);
        Assert.AreEqual(reservation.InvoiceId, updatedReservation.InvoiceId);
        Assert.AreEqual(reservation.RoomId, updatedReservation.RoomId);
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

        var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;


        Assert.AreEqual(1, content.Errors.Count);
        Assert.AreEqual("Reserva não encontrada.", content.Errors[0]);
    }

    [TestMethod]
    public async Task UpdateExpectedCheckOut_WithCheckedOutStatus_ShouldReturn_BAD_REQUEST()
    {
        //Arange
        var customer = new Domain.Entities.CustomerEntity.Customer(
          new Name("Eduardo", "Almeida"),
          new Email("eduardoAlmeida@gmail.com"),
          new Phone("+55 (19) 91123-4467"),
          "password11",
          EGender.Masculine,
          DateTime.Now.AddYears(-29),
          new Domain.ValueObjects.Address("Brazil", "Campinas", "SP-1111", 1111)
        );
        var room = new Room("1Quarto 2",12, 90, 5, "Quarto 12", _category);
        var reservation = new Reservation(room, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), customer, 3);

        reservation.ToCheckIn();
        reservation.Finish(EPaymentMethod.Pix);

        await _dbContext.Customers.AddAsync(customer);
        await _dbContext.Rooms.AddAsync(room);
        await _dbContext.Reservations.AddAsync(reservation);
        await _dbContext.SaveChangesAsync();

        var newExpectedCheckOut = DateTime.Now.AddDays(7);
        var body = new UpdateCheckOut(newExpectedCheckOut);

        //Act
        var response = await _client.PatchAsJsonAsync($"{_baseUrl}/{reservation.Id}/check-out", body);

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;

        var updatedReservation = await _dbContext.Reservations.FirstAsync(x => x.Id == reservation.Id);

       
        Assert.AreEqual(1, content.Errors.Count);
        Assert.AreEqual("Não é possível alterar o CheckOut esperado com o status da reserva CheckedOut.", content.Errors[0]);
        Assert.AreEqual(reservation.ExpectedCheckOut, updatedReservation.ExpectedCheckOut);
        Assert.AreEqual(reservation.Id, updatedReservation.Id);
        Assert.AreEqual(reservation.DailyRate, updatedReservation.DailyRate);
        Assert.AreEqual(reservation.Capacity, updatedReservation.Capacity);
        Assert.AreEqual(reservation.Status, updatedReservation.Status);
        Assert.AreEqual(reservation.CustomerId, updatedReservation.CustomerId);
        Assert.AreEqual(reservation.ExpectedCheckIn, updatedReservation.ExpectedCheckIn);
        Assert.AreEqual(reservation.ExpectedTimeHosted, updatedReservation.ExpectedTimeHosted);
        Assert.AreEqual(reservation.CheckIn, updatedReservation.CheckIn);
        Assert.AreEqual(reservation.CheckOut, updatedReservation.CheckOut);
        Assert.AreEqual(reservation.InvoiceId, updatedReservation.InvoiceId);
        Assert.AreEqual(reservation.RoomId, updatedReservation.RoomId);
    }

    [TestMethod]
    public async Task UpdateExpectedCheckOut_WithCancelledStatus_ShouldReturn_BAD_REQUEST()
    {
        //Arange
        var customer = new Domain.Entities.CustomerEntity.Customer(
          new Name("Larissa", "Rodrigues"),
          new Email("larissaRodrigues@gmail.com"),
          new Phone("+55 (85) 99886-6543"),
          "password12",
          EGender.Feminine,
          DateTime.Now.AddYears(-27),
          new Domain.ValueObjects.Address("Brazil", "Fortaleza", "CE-1212", 1212)
        );
        var room = new Room("1Quarto 3",13, 90, 5, "Quarto 13", _category);
        var reservation = new Reservation(room, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), customer, 3);

        reservation.ToCancelled();

        await _dbContext.Customers.AddAsync(customer);
        await _dbContext.Rooms.AddAsync(room);
        await _dbContext.Reservations.AddAsync(reservation);
        await _dbContext.SaveChangesAsync();

        var newExpectedCheckOut = DateTime.Now.AddDays(7);
        var body = new UpdateCheckOut(newExpectedCheckOut);

        //Act
        var response = await _client.PatchAsJsonAsync($"{_baseUrl}/{reservation.Id}/check-out", body);

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;

        var updatedReservation = await _dbContext.Reservations.FirstAsync(x => x.Id == reservation.Id);

       
        Assert.AreEqual(1, content.Errors.Count);
        Assert.AreEqual("Não é possível alterar o CheckOut esperado com o status da reserva Canceled.", content.Errors[0]);
        Assert.AreEqual(reservation.Id, updatedReservation.Id);
        Assert.AreEqual(reservation.DailyRate, updatedReservation.DailyRate);
        Assert.AreEqual(reservation.Capacity, updatedReservation.Capacity);
        Assert.AreEqual(reservation.Status, updatedReservation.Status);
        Assert.AreEqual(reservation.CustomerId, updatedReservation.CustomerId);
        Assert.AreEqual(reservation.ExpectedCheckIn, updatedReservation.ExpectedCheckIn);
        Assert.AreEqual(reservation.ExpectedCheckOut, updatedReservation.ExpectedCheckOut);
        Assert.AreEqual(reservation.ExpectedTimeHosted, updatedReservation.ExpectedTimeHosted);
        Assert.AreEqual(reservation.CheckIn, updatedReservation.CheckIn);
        Assert.AreEqual(reservation.CheckOut, updatedReservation.CheckOut);
        Assert.AreEqual(reservation.InvoiceId, updatedReservation.InvoiceId);
        Assert.AreEqual(reservation.RoomId, updatedReservation.RoomId);
    }

    [TestMethod]
    public async Task UpdateExpectedCheckIn_ShouldReturn_OK()
    {
        //Arange
        var customer = new Domain.Entities.CustomerEntity.Customer(
          new Name("Thiago", "Gomes"),
          new Email("thiagoGomes@gmail.com"),
          new Phone("+55 (13) 97354-3110"),
          "password13",
          EGender.Masculine,
          DateTime.Now.AddYears(-30),
          new Domain.ValueObjects.Address("Brazil", "Santos", "SP-1313", 1313)
        );
        var room = new Room("1Quarto 4",14, 90, 5, "Quarto 14", _category);
        var reservation = new Reservation(room, DateTime.Now.AddDays(1), DateTime.Now.AddDays(9), customer, 3);

        await _dbContext.Customers.AddAsync(customer);
        await _dbContext.Rooms.AddAsync(room);
        await _dbContext.Reservations.AddAsync(reservation);
        await _dbContext.SaveChangesAsync();

        var newExpectedCheckIn = DateTime.Now.AddDays(7);
        var body = new UpdateCheckIn(newExpectedCheckIn);

        //Act
        var response = await _client.PatchAsJsonAsync($"{_baseUrl}/{reservation.Id}/check-in", body);

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;

        var updatedReservation = await _dbContext.Reservations.FirstAsync(x => x.Id == reservation.Id);

        
        Assert.AreEqual(0, content.Errors.Count);
        Assert.AreEqual("CheckIn esperado atualizado com sucesso!", content.Message);
        Assert.AreEqual(newExpectedCheckIn, updatedReservation.ExpectedCheckIn);
        Assert.AreEqual(reservation.Id, updatedReservation.Id);
        Assert.AreEqual(reservation.DailyRate, updatedReservation.DailyRate);
        Assert.AreEqual(reservation.Capacity, updatedReservation.Capacity);
        Assert.AreEqual(reservation.Status, updatedReservation.Status);
        Assert.AreEqual(reservation.CustomerId, updatedReservation.CustomerId);
        Assert.AreEqual(reservation.ExpectedCheckOut, updatedReservation.ExpectedCheckOut);
        Assert.AreEqual(reservation.ExpectedTimeHosted, updatedReservation.ExpectedTimeHosted);
        Assert.AreEqual(reservation.CheckIn, updatedReservation.CheckIn);
        Assert.AreEqual(reservation.CheckOut, updatedReservation.CheckOut);
        Assert.AreEqual(reservation.InvoiceId, updatedReservation.InvoiceId);
        Assert.AreEqual(reservation.RoomId, updatedReservation.RoomId);
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

        var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;


        Assert.AreEqual(1, content.Errors.Count);
        Assert.AreEqual("Reserva não encontrada.", content.Errors[0]);
    }

    [TestMethod]
    public async Task UpdateExpectedCheckIn_WithCheckedOutStatus_ShouldReturn_BAD_REQUEST()
    {
        //Arange
        var customer = new Domain.Entities.CustomerEntity.Customer(
          new Name("Carolina", "Araujo"),
          new Email("carolinaAraujo@gmail.com"),
          new Phone("+55 (16) 93458-7890"),
          "password14",
          EGender.Feminine,
          DateTime.Now.AddYears(-26),
          new Domain.ValueObjects.Address("Brazil", "Ribeirão Preto", "SP-1414", 1414)
        );
        var room = new Room("1Quarto 5",15, 90, 5, "Quarto 15", _category);
        var reservation = new Reservation(room, DateTime.Now.AddDays(1), DateTime.Now.AddDays(6), customer, 3);

        reservation.ToCheckIn();
        reservation.Finish(EPaymentMethod.Pix);

        await _dbContext.Customers.AddAsync(customer);
        await _dbContext.Rooms.AddAsync(room);
        await _dbContext.Reservations.AddAsync(reservation);
        await _dbContext.SaveChangesAsync();

        var body = new UpdateCheckIn(DateTime.Now.AddDays(7));

        //Act
        var response = await _client.PatchAsJsonAsync($"{_baseUrl}/{reservation.Id}/check-in", body);

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;

        var updatedReservation = await _dbContext.Reservations.FirstAsync(x => x.Id == reservation.Id);

       
        Assert.AreEqual(1, content.Errors.Count);
        Assert.AreEqual("Só é possível alterar o CheckIn esperado se o status for 'Pending' ou 'NoShow'.", content.Errors[0]);
        Assert.AreEqual(reservation.Id, updatedReservation.Id);
        Assert.AreEqual(reservation.DailyRate, updatedReservation.DailyRate);
        Assert.AreEqual(reservation.Capacity, updatedReservation.Capacity);
        Assert.AreEqual(reservation.Status, updatedReservation.Status);
        Assert.AreEqual(reservation.CustomerId, updatedReservation.CustomerId);
        Assert.AreEqual(reservation.ExpectedCheckIn, updatedReservation.ExpectedCheckIn);
        Assert.AreEqual(reservation.ExpectedCheckOut, updatedReservation.ExpectedCheckOut);
        Assert.AreEqual(reservation.ExpectedTimeHosted, updatedReservation.ExpectedTimeHosted);
        Assert.AreEqual(reservation.CheckIn, updatedReservation.CheckIn);
        Assert.AreEqual(reservation.CheckOut, updatedReservation.CheckOut);
        Assert.AreEqual(reservation.InvoiceId, updatedReservation.InvoiceId);
        Assert.AreEqual(reservation.RoomId, updatedReservation.RoomId);
    }

    [TestMethod]
    public async Task UpdateExpectedCheckIn_WithCancelledStatus_ShouldReturn_BAD_REQUEST()
    {
        //Arange
        var customer = new Domain.Entities.CustomerEntity.Customer(
          new Name("Ricardo", "Melo"),
          new Email("ricardoMelo@gmail.com"),
          new Phone("+55 (82) 92310-6789"),
          "password15",
          EGender.Masculine,
          DateTime.Now.AddYears(-32),
          new Domain.ValueObjects.Address("Brazil", "Maceió", "AL-1515", 1515)
        );
        var room = new Room("1Quarto 6",16, 90, 5, "Quarto 16", _category);
        var reservation = new Reservation(room, DateTime.Now.AddDays(1), DateTime.Now.AddDays(6), customer, 3);

        reservation.ToCancelled();

        await _dbContext.Customers.AddAsync(customer);
        await _dbContext.Rooms.AddAsync(room);
        await _dbContext.Reservations.AddAsync(reservation);
        await _dbContext.SaveChangesAsync();

        var body = new UpdateCheckIn(DateTime.Now.AddDays(7));

        //Act
        var response = await _client.PatchAsJsonAsync($"{_baseUrl}/{reservation.Id}/check-in", body);

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;

        var updatedReservation = await _dbContext.Reservations.FirstAsync(x => x.Id == reservation.Id);

       
        Assert.AreEqual(1, content.Errors.Count);
        Assert.AreEqual("Só é possível alterar o CheckIn esperado se o status for 'Pending' ou 'NoShow'.", content.Errors[0]);
        Assert.AreEqual(reservation.Id, updatedReservation.Id);
        Assert.AreEqual(reservation.DailyRate, updatedReservation.DailyRate);
        Assert.AreEqual(reservation.Capacity, updatedReservation.Capacity);
        Assert.AreEqual(reservation.Status, updatedReservation.Status);
        Assert.AreEqual(reservation.CustomerId, updatedReservation.CustomerId);
        Assert.AreEqual(reservation.ExpectedCheckIn, updatedReservation.ExpectedCheckIn);
        Assert.AreEqual(reservation.ExpectedCheckOut, updatedReservation.ExpectedCheckOut);
        Assert.AreEqual(reservation.ExpectedTimeHosted, updatedReservation.ExpectedTimeHosted);
        Assert.AreEqual(reservation.CheckIn, updatedReservation.CheckIn);
        Assert.AreEqual(reservation.CheckOut, updatedReservation.CheckOut);
        Assert.AreEqual(reservation.InvoiceId, updatedReservation.InvoiceId);
        Assert.AreEqual(reservation.RoomId, updatedReservation.RoomId);
    }

    [TestMethod]
    public async Task UpdateExpectedCheckIn_WithCheckedInStatus_ShouldReturn_BAD_REQUEST()
    {
        //Arange
        var customer = new Domain.Entities.CustomerEntity.Customer(
          new Name("Patrícia", "Teixeira"),
          new Email("patriciaTeixeira@gmail.com"),
          new Phone("+55 (91) 91904-5678"),
          "password16",
          EGender.Feminine,
          DateTime.Now.AddYears(-28),
          new Domain.ValueObjects.Address("Brazil", "Belém", "PA-1616", 1616)
        );
        var room = new Room("1Quarto 7",17, 90, 5, "Quarto 17", _category);
        var reservation = new Reservation(room, DateTime.Now.AddDays(1), DateTime.Now.AddDays(6), customer, 3);

        reservation.ToCheckIn();

        await _dbContext.Customers.AddAsync(customer);
        await _dbContext.Rooms.AddAsync(room);
        await _dbContext.Reservations.AddAsync(reservation);
        await _dbContext.SaveChangesAsync();

        var body = new UpdateCheckIn(DateTime.Now.AddDays(7));

        //Act
        var response = await _client.PatchAsJsonAsync($"{_baseUrl}/{reservation.Id}/check-in", body);

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;

        var updatedReservation = await _dbContext.Reservations.FirstAsync(x => x.Id == reservation.Id);

       
        Assert.AreEqual(1, content.Errors.Count);
        Assert.AreEqual("Só é possível alterar o CheckIn esperado se o status for 'Pending' ou 'NoShow'.", content.Errors[0]);
        Assert.AreEqual(reservation.Id, updatedReservation.Id);
        Assert.AreEqual(reservation.DailyRate, updatedReservation.DailyRate);
        Assert.AreEqual(reservation.Capacity, updatedReservation.Capacity);
        Assert.AreEqual(reservation.Status, updatedReservation.Status);
        Assert.AreEqual(reservation.CustomerId, updatedReservation.CustomerId);
        Assert.AreEqual(reservation.ExpectedCheckIn, updatedReservation.ExpectedCheckIn);
        Assert.AreEqual(reservation.ExpectedCheckOut, updatedReservation.ExpectedCheckOut);
        Assert.AreEqual(reservation.ExpectedTimeHosted, updatedReservation.ExpectedTimeHosted);
        Assert.AreEqual(reservation.CheckIn, updatedReservation.CheckIn);
        Assert.AreEqual(reservation.CheckOut, updatedReservation.CheckOut);
        Assert.AreEqual(reservation.InvoiceId, updatedReservation.InvoiceId);
        Assert.AreEqual(reservation.RoomId, updatedReservation.RoomId);
    }

    [TestMethod]
    public async Task AddServiceToReservation_ShouldReturn_OK()
    {
        //Arange
        var customer = new Domain.Entities.CustomerEntity.Customer(
          new Name("Bruno", "Barbosa"),
          new Email("brunoBarbosa@gmail.com"),
          new Phone("+55 (51) 98790-5623"),
          "password17",
          EGender.Masculine,
          DateTime.Now.AddYears(-31),
          new Domain.ValueObjects.Address("Brazil", "Caxias do Sul", "RS-1717", 1717)
        );
        var room = new Room("1Quarto 8",18, 90, 5, "Quarto 18", _category);
        var reservation = new Reservation(room, DateTime.Now.AddDays(1), DateTime.Now.AddDays(6), customer, 3);
        var service = new Service("Room Cleaning", "Room Cleaning", 30.00m, EPriority.Medium, 60);

        room.AddService(service);

        await _dbContext.Customers.AddAsync(customer);
        await _dbContext.Rooms.AddAsync(room);
        await _dbContext.Reservations.AddAsync(reservation);
        await _dbContext.Services.AddAsync(service);
        await _dbContext.SaveChangesAsync();

        _factory.Login(_client, _rootAdminToken);

        //Act
        var response = await _client.PostAsJsonAsync($"{_baseUrl}/{reservation.Id}/services/{service.Id}", new { });

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;

        var reservationWithServices = await _dbContext.Reservations.Include(x => x.Services).FirstAsync(x => x.Id == reservation.Id);

        
        Assert.AreEqual(0, content.Errors.Count);
        Assert.AreEqual("Serviço adicionado com sucesso!", content.Message);

        Assert.IsTrue(reservationWithServices.Services.Any(x => x.Id == service.Id));

        Assert.AreEqual(reservation.Id, reservationWithServices.Id);
        Assert.AreEqual(reservation.DailyRate, reservationWithServices.DailyRate);
        Assert.AreEqual(reservation.Capacity, reservationWithServices.Capacity);
        Assert.AreEqual(reservation.Status, reservationWithServices.Status);
        Assert.AreEqual(reservation.CustomerId, reservationWithServices.CustomerId);
        Assert.AreEqual(reservation.ExpectedCheckIn, reservationWithServices.ExpectedCheckIn);
        Assert.AreEqual(reservation.ExpectedCheckOut, reservationWithServices.ExpectedCheckOut);
        Assert.AreEqual(reservation.ExpectedTimeHosted, reservationWithServices.ExpectedTimeHosted);
        Assert.AreEqual(reservation.CheckIn, reservationWithServices.CheckIn);
        Assert.AreEqual(reservation.CheckOut, reservationWithServices.CheckOut);
        Assert.AreEqual(reservation.InvoiceId, reservationWithServices.InvoiceId);
        Assert.AreEqual(reservation.RoomId, reservationWithServices.RoomId);
    }

    [TestMethod]
    public async Task AddServiceToReservation_WithNonexistReservation_ShouldReturn_NOT_FOUND()
    {
        //Arange
        var ramdomId = Guid.NewGuid();
        var service = new Service("Breakfast Delivery", "Breakfast Delivery", 20.00m, EPriority.High, 30);

        await _dbContext.Services.AddAsync(service);
        await _dbContext.SaveChangesAsync();

        _factory.Login(_client, _rootAdminToken);

        //Act
        var response = await _client.PostAsJsonAsync($"{_baseUrl}/{ramdomId}/services/{service.Id}", new { });

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;


        Assert.AreEqual(1, content.Errors.Count);
        Assert.AreEqual("Reserva não encontrada.", content.Errors[0]);
    }

    [TestMethod]
    public async Task AddServiceToReservation_WithNonexistService_ShouldReturn_NOT_FOUND()
    {
        //Arange
        var customer = new Domain.Entities.CustomerEntity.Customer(
          new Name("Isabela", "Freitas"),
          new Email("isabelaFreitas@gmail.com"),
          new Phone("+55 (11) 97931-3210"),
          "password18",
          EGender.Feminine,
          DateTime.Now.AddYears(-25),
          new Domain.ValueObjects.Address("Brazil", "São Bernardo do Campo", "SP-1818", 1818)
        );
        var room = new Room("1Quarto 9",19, 90, 5, "Quarto 19", _category);
        var reservation = new Reservation(room, DateTime.Now.AddDays(1), DateTime.Now.AddDays(6), customer, 3);

        await _dbContext.Customers.AddAsync(customer);
        await _dbContext.Rooms.AddAsync(room);
        await _dbContext.Reservations.AddAsync(reservation);
        await _dbContext.SaveChangesAsync();

        _factory.Login(_client, _rootAdminToken);

        //Act
        var response = await _client.PostAsJsonAsync($"{_baseUrl}/{reservation.Id}/services/{Guid.NewGuid()}", new { });

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;


        Assert.AreEqual(1, content.Errors.Count);
        Assert.AreEqual("Serviço não encontrado.", content.Errors[0]);
    }


    [TestMethod]
    public async Task AddServiceToReservation_WithUnavailableServiceAtTheRoom_ShouldReturn_BAD_REQUEST()
    {
        //Arange
        var customer = new Domain.Entities.CustomerEntity.Customer(
          new Name("Marcelo", "Duarte"),
          new Email("marceloDuarte@gmail.com"),
          new Phone("+55 (21) 99656-7890"),
          "password19",
          EGender.Masculine,
          DateTime.Now.AddYears(-34),
          new Domain.ValueObjects.Address("Brazil", "Niterói", "RJ-1919", 1919)
        );
        var room = new Room("2Quarto 0",20, 90, 5, "Quarto 20", _category);
        var reservation = new Reservation(room, DateTime.Now.AddDays(1), DateTime.Now.AddDays(6), customer, 3);
        var service = new Service("Spa Treatment", "Spa Treatment", 50.00m, EPriority.Low, 90);

        await _dbContext.Customers.AddAsync(customer);
        await _dbContext.Rooms.AddAsync(room);
        await _dbContext.Reservations.AddAsync(reservation);
        await _dbContext.Services.AddAsync(service);
        await _dbContext.SaveChangesAsync();

        _factory.Login(_client, _rootAdminToken);

        //Act
        var response = await _client.PostAsJsonAsync($"{_baseUrl}/{reservation.Id}/services/{service.Id}", new { });

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;

        var reservationWithServices = await _dbContext.Reservations.Include(x => x.Services).FirstAsync(x => x.Id == reservation.Id);

       
        Assert.AreEqual(1, content.Errors.Count);
        Assert.AreEqual("Esse serviço não está dísponível nessa hospedagem.", content.Errors[0]);

        Assert.AreEqual(0, reservationWithServices.Services.Count);
    }

    [TestMethod]
    public async Task AddServiceToReservation_WithDisabledService_ShouldReturn_BAD_REQUEST()
    {
        //Arange
        var customer = new Domain.Entities.CustomerEntity.Customer(
          new Name("Gabriela", "Moreira"),
          new Email("gabrielaMoreira@gmail.com"),
          new Phone("+55 (61) 92315-6389"),
          "password20",
          EGender.Feminine,
          DateTime.Now.AddYears(-24),
          new Domain.ValueObjects.Address("Brazil", "Taguatinga", "DF-2020", 2020)
        );
        var room = new Room("2Quarto 1",21, 90, 5, "Quarto 21", _category);
        var reservation = new Reservation(room, DateTime.Now.AddDays(1), DateTime.Now.AddDays(6), customer, 3);
        var service = new Service("Wake-Up Call", "Wake-Up Call", 1.00m, EPriority.High, 5);

        service.Disable();
        room.AddService(service);

        await _dbContext.Customers.AddAsync(customer);
        await _dbContext.Rooms.AddAsync(room);
        await _dbContext.Reservations.AddAsync(reservation);
        await _dbContext.Services.AddAsync(service);
        await _dbContext.SaveChangesAsync();

        _factory.Login(_client, _rootAdminToken);

        //Act
        var response = await _client.PostAsJsonAsync($"{_baseUrl}/{reservation.Id}/services/{service.Id}", new { });

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;

        var reservationWithServices = await _dbContext.Reservations.Include(x => x.Services).FirstAsync(x => x.Id == reservation.Id);

       
        Assert.AreEqual(1, content.Errors.Count);
        Assert.IsTrue(content!.Errors.Any(x => x.Contains("desativado")));

        Assert.AreEqual(0, reservationWithServices.Services.Count);
    }

    [TestMethod]
    public async Task RemoveServiceFromReservation_ShouldReturn_OK()
    {
        //Arange
        var customer = new Domain.Entities.CustomerEntity.Customer(
          new Name("Felipe", "Costa"),
          new Email("felipeCosta@gmail.com"),
          new Phone("+55 (83) 99931-5678"),
          "password21",
          EGender.Masculine,
          DateTime.Now.AddYears(-29),
          new Domain.ValueObjects.Address("Brazil", "João Pessoa", "PB-2121", 2121)
        );
        var room = new Room("2Quarto 2",22, 90, 5, "Quarto 22", _category);
        var reservation = new Reservation(room, DateTime.Now.AddDays(1), DateTime.Now.AddDays(6), customer, 3);
        var service = new Service("Minibar Restock", "Minibar Restock", 15.00m, EPriority.Low, 20);

        room.AddService(service);
        reservation.AddService(service);

        await _dbContext.Services.AddAsync(service);
        await _dbContext.Customers.AddAsync(customer);
        await _dbContext.Rooms.AddAsync(room);
        await _dbContext.Reservations.AddAsync(reservation);
        await _dbContext.SaveChangesAsync();

        _factory.Login(_client, _rootAdminToken);

        //Act
        var response = await _client.DeleteAsync($"{_baseUrl}/{reservation.Id}/services/{service.Id}");

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;

        var reservationWithServices = await _dbContext.Reservations.Include(x => x.Services).FirstAsync(x => x.Id == reservation.Id);

        
        Assert.AreEqual(0, content.Errors.Count);
        Assert.AreEqual("Serviço removido com sucesso!", content.Message);

        Assert.AreEqual(0, reservationWithServices.Services.Count);

        Assert.AreEqual(reservation.Id, reservationWithServices.Id);
        Assert.AreEqual(reservation.DailyRate, reservationWithServices.DailyRate);
        Assert.AreEqual(reservation.Capacity, reservationWithServices.Capacity);
        Assert.AreEqual(reservation.Status, reservationWithServices.Status);
        Assert.AreEqual(reservation.CustomerId, reservationWithServices.CustomerId);
        Assert.AreEqual(reservation.ExpectedCheckIn, reservationWithServices.ExpectedCheckIn);
        Assert.AreEqual(reservation.ExpectedCheckOut, reservationWithServices.ExpectedCheckOut);
        Assert.AreEqual(reservation.ExpectedTimeHosted, reservationWithServices.ExpectedTimeHosted);
        Assert.AreEqual(reservation.CheckIn, reservationWithServices.CheckIn);
        Assert.AreEqual(reservation.CheckOut, reservationWithServices.CheckOut);
        Assert.AreEqual(reservation.InvoiceId, reservationWithServices.InvoiceId);
        Assert.AreEqual(reservation.RoomId, reservationWithServices.RoomId);
    }

    [TestMethod]
    public async Task RemoveServiceFromReservation_WithNonexistReservation_ShouldReturn_NOT_FOUND()
    {
        //Arange
        var customer = new Domain.Entities.CustomerEntity.Customer(
          new Name("Laura", "Nascimento"),
          new Email("lauraNascimento@gmail.com"),
          new Phone("+55 (62) 99887-6543"),
          "password22",
          EGender.Feminine,
          DateTime.Now.AddYears(-28),
          new Domain.ValueObjects.Address("Brazil", "Anápolis", "GO-2222", 2222)
        );
        var room = new Room("2Quarto 3",23, 90, 5, "Quarto 23", _category);
        var service = new Service("Airport Shuttle", "Airport Shuttle", 60.00m, EPriority.High, 45);

        room.AddService(service);

        await _dbContext.Services.AddAsync(service);
        await _dbContext.Customers.AddAsync(customer);
        await _dbContext.Rooms.AddAsync(room);
        await _dbContext.SaveChangesAsync();

        _factory.Login(_client, _rootAdminToken);

        //Act
        var response = await _client.DeleteAsync($"{_baseUrl}/{Guid.NewGuid()}/services/{service.Id}");

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;


        Assert.AreEqual(1, content.Errors.Count);
        Assert.AreEqual("Reserva não encontrada.", content.Errors[0]);
    }

    [TestMethod]
    public async Task RemoveServiceFromReservation_WithNonexistService_ShouldReturn_NOT_FOUND()
    {
        //Arange
        var customer = new Domain.Entities.CustomerEntity.Customer(
          new Name("Vinícius", "Cardoso"),
          new Email("viniciusCardoso@gmail.com"),
          new Phone("+55 (47) 98739-4311"),
          "password23",
          EGender.Masculine,
          DateTime.Now.AddYears(-31),
          new Domain.ValueObjects.Address("Brazil", "Joinville", "SC-2323", 2323)
        );
        var room = new Room("2Quarto 4",24, 90, 5, "Quarto 24", _category);
        var reservation = new Reservation(room, DateTime.Now.AddDays(1), DateTime.Now.AddDays(6), customer, 3);

        await _dbContext.Customers.AddAsync(customer);
        await _dbContext.Rooms.AddAsync(room);
        await _dbContext.Reservations.AddAsync(reservation);
        await _dbContext.SaveChangesAsync();

        _factory.Login(_client, _rootAdminToken);

        //Act
        var response = await _client.DeleteAsync($"{_baseUrl}/{reservation.Id}/services/{Guid.NewGuid()}");

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;


        Assert.AreEqual(1, content.Errors.Count);
        Assert.AreEqual("Serviço não encontrado.", content.Errors[0]);
    }

    [TestMethod]
    public async Task RemoveServiceFromReservation_WithoutContainsService_ShouldReturn_BAD_REQUEST()
    {
        //Arange
        var customer = new Domain.Entities.CustomerEntity.Customer(
          new Name("André", "Pinto"),
          new Email("andrePinto@gmail.com"),
          new Phone("+55 (95) 93456-7890"),
          "password25",
          EGender.Masculine,
          DateTime.Now.AddYears(-33),
          new Domain.ValueObjects.Address("Brazil", "Boa Vista", "RR-2525", 2525)
        );
        var room = new Room("2Quarto 5",25, 90, 5, "Quarto 25", _category);
        var reservation = new Reservation(room, DateTime.Now.AddDays(1), DateTime.Now.AddDays(6), customer, 3);
        var service = new Service("Jet Ski Rental", "Jet Ski Rental", 90.00m, EPriority.High, 60);

        room.AddService(service);

        await _dbContext.Services.AddAsync(service);
        await _dbContext.Customers.AddAsync(customer);
        await _dbContext.Rooms.AddAsync(room);
        await _dbContext.Reservations.AddAsync(reservation);
        await _dbContext.SaveChangesAsync();

        _factory.Login(_client, _rootAdminToken);

        //Act
        var response = await _client.DeleteAsync($"{_baseUrl}/{reservation.Id}/services/{service.Id}");

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;

       
        Assert.AreEqual(1, content.Errors.Count);
        Assert.IsTrue(content!.Errors.Any(x => x.Contains("atribuido a essa reserva.")));
    }

    [TestMethod]
    public async Task FinishReservation_ShouldReturn_OK()
    {
        //Arange
        var newCustomer = new CreateUser
        (
            "Eduardo", "Wimp",
            "eduardoowk1@gmail.com",
            "+55 (92) 92345-6789",
            "password26",
            EGender.Feminine,
            DateTime.Now.AddYears(-27),
            "Brazil", "Manaus", "AM-2626", 2626
        );

        var verificationCode = new VerificationCode(new Email(newCustomer.Email));
        await _dbContext.VerificationCodes.AddAsync(verificationCode);
        await _dbContext.SaveChangesAsync();

        var createCustomerResponse = await _client.PostAsJsonAsync($"v1/register/customers?code={verificationCode.Code}", newCustomer);
        var createCustomerContent = JsonConvert.DeserializeObject<Response<DataStripeCustomerId>>(await createCustomerResponse.Content.ReadAsStringAsync())!;
        var customer = await _dbContext.Customers.FirstAsync(x => x.Id == createCustomerContent.Data.Id);

        _factory.Login(_client, _rootAdminToken);

        var newRoom = new EditorRoom("2Quarto 6", 26, 90, 5, "Quarto 26", _category.Id);
        var createRoomResponse = await _client.PostAsJsonAsync("v1/rooms", newRoom);
        var roomId = JsonConvert.DeserializeObject<Response<DataId>>(await createRoomResponse.Content.ReadAsStringAsync())!.Data.Id;

        _factory.Login(_client, customer);

        var newReservation = new CreateReservation(DateTime.Now.AddDays(1), DateTime.Now.AddDays(6), roomId, 3);
        var createReservationResponse = await _client.PostAsJsonAsync("v1/reservations", newReservation);
        var reservationId = JsonConvert.DeserializeObject<Response<DataStripePaymentIntentId>>(await createReservationResponse.Content.ReadAsStringAsync())!.Data.Id;

        var reservation = await _dbContext.Reservations.FirstAsync(x => x.Id == reservationId);
        reservation.ToCheckIn();

        await _dbContext.SaveChangesAsync();
        //Act
        var response = await _client.PatchAsJsonAsync($"{_baseUrl}/finish/{reservation.Id}", new { });

        response.EnsureSuccessStatusCode();

        //Assert
        var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;


        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var updatedReservation = await _dbContext.Reservations.FirstAsync(x => x.Id == reservation.Id);

        
        Assert.AreEqual(0, content.Errors.Count);
        Assert.AreEqual("Reserva finalizada com sucesso!", content.Message);

        Assert.AreEqual(EReservationStatus.CheckedOut, updatedReservation.Status);

        Assert.AreEqual(reservation.Id, updatedReservation.Id);
        Assert.AreEqual(reservation.DailyRate, updatedReservation.DailyRate);
        Assert.AreEqual(reservation.Capacity, updatedReservation.Capacity);
        Assert.AreEqual(reservation.CustomerId, updatedReservation.CustomerId);
        Assert.AreEqual(reservation.ExpectedCheckIn, updatedReservation.ExpectedCheckIn);
        Assert.AreEqual(reservation.ExpectedCheckOut, updatedReservation.ExpectedCheckOut);
        Assert.AreEqual(reservation.ExpectedTimeHosted, updatedReservation.ExpectedTimeHosted);
        Assert.AreEqual(reservation.CheckIn, updatedReservation.CheckIn);
        Assert.AreEqual(reservation.CheckOut, updatedReservation.CheckOut);
        Assert.AreEqual(reservation.InvoiceId, updatedReservation.InvoiceId);
        Assert.AreEqual(reservation.RoomId, updatedReservation.RoomId);
    }

    [TestMethod]
    public async Task FinishReservation_WithNonexistReservation_ShouldReturn_NOT_FOUND()
    {
        //Act
        var response = await _client.PatchAsJsonAsync($"{_baseUrl}/finish/{Guid.NewGuid()}", new { });

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;


        Assert.AreEqual(1, content.Errors.Count);
        Assert.AreEqual("Reserva não encontrada.", content.Errors[0]);
    }

    [TestMethod]
    public async Task FinishReservation_WithDifferentCustomer_ShouldReturn_UNAUTHORIZED()
    {
        //Arange
        var customer = new Domain.Entities.CustomerEntity.Customer(
          new Name("Fernando", "Dias"),
          new Email("fernandoDias@gmail.com"),
          new Phone("+55 (98) 91234-5678"),
          "password27",
          EGender.Masculine,
          DateTime.Now.AddYears(-32),
          new Domain.ValueObjects.Address("Brazil", "São Luís", "MA-2727", 2727)
        );
        var room = new Room("2Quarto 7",27, 90, 5, "Quarto 27", _category);
        var reservation = new Reservation(room, DateTime.Now.AddDays(1), DateTime.Now.AddDays(6), customer, 3);

        reservation.ToCheckIn();

        await _dbContext.Customers.AddAsync(customer);
        await _dbContext.Rooms.AddAsync(room);
        await _dbContext.Reservations.AddAsync(reservation);
        await _dbContext.SaveChangesAsync();

        //Act
        var response = await _client.PatchAsJsonAsync($"{_baseUrl}/finish/{reservation.Id}", new { });

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;

        var updatedReservation = await _dbContext.Reservations.FirstAsync(x => x.Id == reservation.Id);

        
        Assert.AreEqual(1, content.Errors.Count);
        Assert.AreEqual("Você não tem permissão para finalizar reserva alheia.", content.Errors[0]);

        Assert.AreEqual(EReservationStatus.CheckedIn, updatedReservation.Status);
    }

    [TestMethod]
    public async Task CancelReservation_ShouldReturn_OK()
    {
        //Arange
        var newCustomer = new CreateUser
        (
            "Aline", "Fernandes",
            "alineFernandes@gmail.com",
            "+55 (75) 99715-4321",
            "password28",
            EGender.Feminine,
            DateTime.Now.AddYears(-25),
            "Brazil", "Feira de Santana", "BA-2828", 2828
        );

        var verificationCode = new VerificationCode(new Email(newCustomer.Email));
        await _dbContext.VerificationCodes.AddAsync(verificationCode);
        await _dbContext.SaveChangesAsync();

        var createCustomerResponse = await _client.PostAsJsonAsync($"v1/register/customers?code={verificationCode.Code}", newCustomer);
        var createCustomerContent = JsonConvert.DeserializeObject<Response<DataStripeCustomerId>>(await createCustomerResponse.Content.ReadAsStringAsync())!;
        var customer = await _dbContext.Customers.FirstAsync(x => x.Id == createCustomerContent.Data.Id);

        var room = new Room("Quarto 28",49, 90, 5, "Quarto 28", _category);

        await _dbContext.Rooms.AddAsync(room);
        await _dbContext.SaveChangesAsync();

        _factory.Login(_client, customer);

        var newReservation = new CreateReservation(DateTime.Now.AddDays(1), DateTime.Now.AddDays(6), room.Id, 3);
        var createReservationResponse = await _client.PostAsJsonAsync(_baseUrl, newReservation);
        var createReservationContent = JsonConvert.DeserializeObject<Response<DataStripePaymentIntentId>>(await createReservationResponse.Content.ReadAsStringAsync())!;
        var reservation = await _dbContext.Reservations.FirstAsync(x => x.Id == createReservationContent.Data.Id);

        //Act
        var response = await _client.PatchAsJsonAsync($"{_baseUrl}/cancel/{reservation.Id}", new { });

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;

        var updatedReservation = await _dbContext.Reservations.FirstAsync(x => x.Id == reservation.Id);

        
        Assert.AreEqual(0, content.Errors.Count);
        Assert.AreEqual("Reserva cancelada com sucesso!", content.Message);

        Assert.AreEqual(EReservationStatus.Canceled, updatedReservation.Status);

        Assert.AreEqual(reservation.Id, updatedReservation.Id);
        Assert.AreEqual(reservation.DailyRate, updatedReservation.DailyRate);
        Assert.AreEqual(reservation.Capacity, updatedReservation.Capacity);
        Assert.AreEqual(reservation.CustomerId, updatedReservation.CustomerId);
        Assert.AreEqual(reservation.ExpectedCheckIn, updatedReservation.ExpectedCheckIn);
        Assert.AreEqual(reservation.ExpectedCheckOut, updatedReservation.ExpectedCheckOut);
        Assert.AreEqual(reservation.ExpectedTimeHosted, updatedReservation.ExpectedTimeHosted);
        Assert.AreEqual(reservation.CheckIn, updatedReservation.CheckIn);
        Assert.AreEqual(reservation.CheckOut, updatedReservation.CheckOut);
        Assert.AreEqual(reservation.InvoiceId, updatedReservation.InvoiceId);
        Assert.AreEqual(reservation.RoomId, updatedReservation.RoomId);

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

        var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;


        Assert.AreEqual(1, content.Errors.Count);
        Assert.AreEqual("Reserva não encontrada.", content.Errors[0]);
    }

    [TestMethod]
    public async Task CancelReservation_WithDifferentCustomer_ShouldReturn_UNAUTHORIZED()
    {
        //Arange
        var customer = new Domain.Entities.CustomerEntity.Customer(
          new Name("Paulo", "Moura"),
          new Email("pauloMoura@gmail.com"),
          new Phone("+55 (88) 97614-3310"),
          "password29",
          EGender.Masculine,
          DateTime.Now.AddYears(-34),
          new Domain.ValueObjects.Address("Brazil", "Sobral", "CE-2929", 2929)
        );
        var room = new Room("Quarto 821",821, 90, 5, "Quarto 821", _category);
        var reservation = new Reservation(room, DateTime.Now.AddDays(1), DateTime.Now.AddDays(3), customer, 2);

        await _dbContext.Customers.AddAsync(customer);
        await _dbContext.Rooms.AddAsync(room);
        await _dbContext.Reservations.AddAsync(reservation);
        await _dbContext.SaveChangesAsync();

        //Act
        var response = await _client.PatchAsJsonAsync($"{_baseUrl}/cancel/{reservation.Id}", new { });

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;

        var updatedReservation = await _dbContext.Reservations.FirstAsync(x => x.Id == reservation.Id);

        
        Assert.AreEqual(1, content.Errors.Count);
        Assert.AreEqual("Você não tem permissão para cancelar reserva alheia.", content.Errors[0]);

        Assert.AreEqual(EReservationStatus.Pending, updatedReservation.Status);
    }

    [TestMethod]
    public async Task CancelReservation_WithStripeError_ShouldReturn_BAD_REQUEST_AND_MAKE_ROLLBACK()
    {
        //Arange
        var factory = new HotelWebApplicationFactory();
        var client = factory.CreateClient();
        var dbContext = factory.Services.GetRequiredService<HotelDbContext>();

        var newCustomer = new CreateUser
        (
            "Ana", "Oliveira",
            "anaOliveira@gmail.com",
            "+55 (19) 98765-8765",
            "password789",
            EGender.Feminine,
            DateTime.Now.AddYears(-22),
            "Brazil", "Campinas", "CP-404", 404
        );

        var verificationCode = new VerificationCode(new Email(newCustomer.Email));
        await dbContext.VerificationCodes.AddAsync(verificationCode);
        await dbContext.SaveChangesAsync();

        var createCustomerResponse = await client.PostAsJsonAsync($"v1/register/customers?code={verificationCode.Code}", newCustomer);
        var createCustomerContent = JsonConvert.DeserializeObject<Response<DataStripeCustomerId>>(await createCustomerResponse.Content.ReadAsStringAsync())!;
        var customer = await dbContext.Customers.FirstAsync(x => x.Id == createCustomerContent.Data.Id);

        var room = new Room("Quarto 132", 132, 70, 5, "Quarto 132", _category);

        await dbContext.Rooms.AddAsync(room);
        await dbContext.SaveChangesAsync();

        factory.Login(client, customer);

        var newReservation = new CreateReservation(DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), room.Id, 3);
        var createReservationResponse = await client.PostAsJsonAsync(_baseUrl, newReservation);
        var createReservationContent = JsonConvert.DeserializeObject<Response<DataStripePaymentIntentId>>(await createReservationResponse.Content.ReadAsStringAsync())!;
        var reservation = await dbContext.Reservations.FirstAsync(x => x.Id == createReservationContent.Data.Id);

        var apiKey = StripeConfiguration.ApiKey.ToString();
        StripeConfiguration.ApiKey = "";

        //Act
        var response = await client.PatchAsJsonAsync($"{_baseUrl}/cancel/{reservation.Id}", new { });

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;

        var exists = await dbContext.Reservations.AnyAsync(x => x.Id == reservation.Id);

        Assert.AreEqual("Ocorreu um erro ao cancelar o PaymentIntent no Stripe", content.Errors[0]);
        Assert.IsTrue(exists);

        StripeConfiguration.ApiKey = apiKey;
        var paymentIntent = await _stripePaymentIntentService.GetAsync(reservation.StripePaymentIntentId);
        Assert.AreEqual("requires_payment_method", paymentIntent.Status);
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
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

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

        var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;


        Assert.AreEqual(expectedError, content.Errors[0]);
    }
}

internal record GetReservation(Guid Id, decimal DailyRate, TimeSpan ExpectedTimeHosted, DateTime ExpectedCheckIn, DateTime ExpectedCheckOut, TimeSpan? TimeHosted, DateTime? CheckIn, DateTime? CheckOut, EReservationStatus Status, int Capacity, Guid RoomId, Guid CustomerId, Guid? InvoiceId);
internal record TotalAmountResponse(decimal TotalAmount);