using Hotel.Domain.Data;
using Hotel.Domain.DTOs.ReservationContext.ReservationDTOs;
using Hotel.Domain.Entities.CustomerContext;
using Hotel.Domain.Entities.Interfaces;
using Hotel.Domain.Entities.ReservationContext.ReservationEntity;
using Hotel.Domain.Entities.RoomContext.CategoryEntity;
using Hotel.Domain.Entities.RoomContext.RoomEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Services.TokenServices;
using Hotel.Domain.ValueObjects;
using Hotel.Tests.IntegrationTests.Factories;
using Hotel.Tests.IntegrationTests.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

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
  private static TokenService _tokenService = null!;
  private static Category _category = null!;

  [ClassInitialize]
  public static void ClassInitialize(TestContext context)
  {
    _factory = new HotelWebApplicationFactory();
    _client = _factory.CreateClient();
    _dbContext = _factory.Services.GetRequiredService<HotelDbContext>();
    _tokenService = _factory.Services.GetRequiredService<TokenService>();

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
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _customerToken);
  }

  [TestMethod]
  public async Task CreateReservation_ShouldReturn_OK()
  {
    //Arange
    var room = new Room(2, 50, 2, "Quarto padrão",_category.Id);

    await _dbContext.Rooms.AddAsync(room);
    await _dbContext.SaveChangesAsync();

    var body = new CreateReservation(DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), room.Id, 1);

    //Act
    var response = await _client.PostAsJsonAsync(_baseUrl, body);

    //Assert
    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

    var content = JsonConvert.DeserializeObject<Response<DataId>>(await response.Content.ReadAsStringAsync());
    var createdReservation = await _dbContext.Reservations.Include(x => x.Room).FirstAsync(x => x.Id == content!.Data!.Id);

    Assert.AreEqual(200, content!.Status);
    Assert.AreEqual("Reserva criada com sucesso!", content.Message);
    Assert.AreEqual(body.ExpectedCheckIn, createdReservation.ExpectedCheckIn);
    Assert.AreEqual(body.ExpectedCheckOut, createdReservation.ExpectedCheckOut);
    Assert.AreEqual(body.Capacity, createdReservation.Capacity);
    Assert.AreEqual(body.RoomId, createdReservation.RoomId);
    Assert.IsNull(createdReservation.Invoice);
    Assert.IsNull(createdReservation.CheckIn);
    Assert.IsNull(createdReservation.CheckOut);
    Assert.AreEqual(room.Price,createdReservation.DailyRate);
    Assert.AreEqual(1, Math.Round(createdReservation.ExpectedTimeHosted.TotalDays, 0));
    Assert.AreEqual(50, Math.Round(createdReservation.ExpectedTotalAmount(), 2));
    Assert.AreEqual(ERoomStatus.Reserved, createdReservation.Room!.Status);
  }

  [TestMethod]
  public async Task CreateReservation_WithUnavailableRoom_ShouldReturn_BAD_REQUEST()
  {
    //Arange
    var customer = new Customer(
      new Name("Juliana", "Martins"),
      new Email("julianaMartins@gmail.com"),
      new Phone("+55 (48) 98765-4321"),
      "123",
      EGender.Feminine,
      DateTime.Now.AddYears(-24),
      new Address("Brazil", "Florianópolis", "SC-909", 909)
    );
    var room = new Room(3, 80, 2, "Quarto de luxo nível 1", _category.Id);
    var reservation = new Reservation(room,DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), customer, 1);

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

    var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync());
    var reservations = await _dbContext.Reservations.Where(x => x.RoomId == room.Id).ToListAsync();

    Assert.AreEqual(400, content!.Status);
    Assert.IsTrue(content.Errors.Any(x => x.Equals("Não é possível realizar a reserva pois o cômodo está indisponível.")));
    Assert.AreEqual(1, reservations.Count);
  }

  [TestMethod]
  public async Task CreateReservation_WithDisabledRoom_ShouldReturn_BAD_REQUEST()
  {
    //Arange
    var room = new Room(4, 110, 2, "Quarto de luxo nível 2", _category.Id);
    room.Disable();

    await _dbContext.Rooms.AddAsync(room);
    await _dbContext.SaveChangesAsync();

    var body = new CreateReservation(DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), room.Id, 1);

    //Act
    var response = await _client.PostAsJsonAsync(_baseUrl, body);

    //Assert
    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

    var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync());
    var reservations = await _dbContext.Reservations.Where(x => x.RoomId == room.Id).ToListAsync();

    Assert.AreEqual(400, content!.Status);
    Assert.IsTrue(content.Errors.Any(x => x.Equals("Não é possível realizar a reserva pois o cômodo está inativo.")));
    Assert.AreEqual(0, reservations.Count);
  }

  [TestMethod]
  public async Task CreateReservation_WithRoomGuestsLimitExceeded_ShouldReturn_BAD_REQUEST()
  {
    //Arange
    var room = new Room(5, 150, 2, "Quarto de luxo nível 3", _category.Id);

    await _dbContext.Rooms.AddAsync(room);
    await _dbContext.SaveChangesAsync();

    var body = new CreateReservation(DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), room.Id, 3);

    //Act
    var response = await _client.PostAsJsonAsync(_baseUrl, body);

    //Assert
    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

    var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync());
    var reservations = await _dbContext.Reservations.Where(x => x.RoomId == room.Id).ToListAsync();

    Assert.AreEqual(400, content!.Status);
    Assert.IsTrue(content.Errors.Any(x => x.Equals("Capacidade máxima de hospedades do cômodo excedida.")));
    Assert.AreEqual(0, reservations.Count);
  }

  [TestMethod]
  public async Task CreateReservation_WithNonexistCustomer_ShouldReturn_NOT_FOUND()
  {
    //Arange
    var customer = new Customer(
      new Name("Pedro", "Silva"),
      new Email("pedroSilva@gmail.com"),
      new Phone("+55 (62) 99876-5432"),
      "password8",
      EGender.Masculine,
      DateTime.Now.AddYears(-31),
      new Address("Brazil", "Goiânia", "GO-808", 808)
    );
    var room = new Room(6, 190, 2, "Quarto de luxo nível 4", _category.Id);

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

    var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync());
    var reservations = await _dbContext.Reservations.Where(x => x.RoomId == room.Id).ToListAsync();

    Assert.AreEqual(404, content!.Status);
    Assert.IsTrue(content.Errors.Any(x => x.Equals("Usuário não encontrado.")));
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

    var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync());

    Assert.AreEqual(404, content!.Status);
    Assert.IsTrue(content.Errors.Any(x => x.Equals("Hospedagem não encontrada.")));
  }

  [TestMethod]
  public async Task GetReservations_ShouldReturn_OK()
  {
    //Arange
    var customer = new Customer(
      new Name("Fernanda", "Ribeiro"),
      new Email("fernandaRibeiro@gmail.com"),
      new Phone("+55 (51) 91234-5678"),
      "password7",
      EGender.Feminine,
      DateTime.Now.AddYears(-26),
      new Address("Brazil", "Porto Alegre", "RS-707", 707)
    );
    var room = new Room(7, 240, 2, "Quarto de luxo nível 5", _category.Id);
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

    var content = JsonConvert.DeserializeObject<Response<List<GetReservation>>>(await response.Content.ReadAsStringAsync());

    Assert.AreEqual(200, content!.Status);
    Assert.AreEqual("Sucesso!", content.Message);
    foreach(var getReservation in content.Data)
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
    var customer = new Customer(
      new Name("Lucas", "Ferreira"),
      new Email("lucasFerreira@gmail.com"),
      new Phone("+55 (61) 92345-6789"),
      "password6",
      EGender.Masculine,
      DateTime.Now.AddYears(-28),
      new Address("Brazil", "Brasília", "DF-606", 606)
    );
    var room = new Room(8, 70, 5, "Quarto de luxo básico", _category.Id);
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

    var content = JsonConvert.DeserializeObject<Response<GetReservation>>(await response.Content.ReadAsStringAsync());

    Assert.AreEqual(200, content!.Status);
    Assert.AreEqual("Sucesso!", content.Message);

    Assert.AreEqual(reservation.Id, content.Data.Id);
    Assert.AreEqual(reservation.DailyRate ,content.Data.DailyRate);
    Assert.AreEqual(reservation.Capacity ,content.Data.Capacity);
    Assert.AreEqual(reservation.Status ,content.Data.Status);
    Assert.AreEqual(reservation.CustomerId ,content.Data.CustomerId);
    Assert.AreEqual(reservation.ExpectedCheckIn ,content.Data.ExpectedCheckIn);
    Assert.AreEqual(reservation.ExpectedCheckOut ,content.Data.ExpectedCheckOut);
    Assert.AreEqual(reservation.ExpectedTimeHosted ,content.Data.ExpectedTimeHosted);
    Assert.AreEqual(reservation.CheckIn, content.Data.CheckIn);
    Assert.AreEqual(reservation.CheckOut, content.Data.CheckOut);
    Assert.AreEqual(reservation.InvoiceId, content.Data.InvoiceId);
    Assert.AreEqual(reservation.RoomId, content.Data.RoomId);
  }


  [TestMethod]
  public async Task DeleteReservation_ShouldReturn_OK()
  {
    //Arange
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _rootAdminToken);
    var customer = new Customer(
      new Name("Camila", "Costa"),
      new Email("camilaCosta@gmail.com"),
      new Phone("+55 (71) 93456-7890"),
      "password5",
      EGender.Feminine,
      DateTime.Now.AddYears(-29),
      new Address("Brazil", "Salvador", "BA-505", 505)
    );
    var room = new Room(9, 70, 5, "Quarto 1", _category.Id);
    var reservation = new Reservation(room, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), customer, 3);

    await _dbContext.Customers.AddAsync(customer);
    await _dbContext.Rooms.AddAsync(room);
    await _dbContext.Reservations.AddAsync(reservation);
    await _dbContext.SaveChangesAsync();

    //Act
    var response = await _client.DeleteAsync($"{_baseUrl}/{reservation.Id}");

    //Assert
    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

    var content = JsonConvert.DeserializeObject<Response<DataId>>(await response.Content.ReadAsStringAsync());

    var hasDeleted = ! await _dbContext.Reservations.AnyAsync(x => x.Id == reservation.Id);

    Assert.AreEqual(200, content!.Status);
    Assert.AreEqual("Reserva deletada com sucesso!", content.Message);
    Assert.AreEqual(reservation.Id, content.Data.Id);
    Assert.IsTrue(hasDeleted);
  }

  [TestMethod]
  public async Task DeleteReservation_WithoutPermission_ShouldReturn_UNAUTHORIZED()
  {
    Assert.Fail();
  }


  [TestMethod]
  public async Task DeleteReservation_WithNonexistReservation_ShouldReturn_NOT_FOUND()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task DeleteReservation_WithCheckedInReservationStatus_ShouldReturn_BAD_REQUEST()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task DeleteReservation_WithDifferentCustomerId_ShouldReturn_UNAUTHORIZED()
  {
    Assert.Fail();
  }


  [TestMethod]
  public async Task UpdateExpectedCheckOut_ShouldReturn_OK()
  {
    //Verificar se expectedTimeHosted também foi atualizado
    Assert.Fail();
  }

  [TestMethod]
  public async Task UpdateExpectedCheckOut_WithNonexistReservation_ShouldReturn_NOT_FOUND()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task UpdateExpectedCheckOut_WithCheckedOutStatus_ShouldReturn_BAD_REQUEST()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task UpdateExpectedCheckOut_WithCancelledStatus_ShouldReturn_BAD_REQUEST()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task UpdateExpectedCheckIn_ShouldReturn_OK()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task UpdateExpectedCheckIn_WithNonexistReservation_ShouldReturn_NOT_FOUND()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task UpdateExpectedCheckIn_WithCheckedOutStatus_ShouldReturn_BAD_REQUEST()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task UpdateExpectedCheckIn_WithCancelledStatus_ShouldReturn_BAD_REQUEST()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task AddServiceToReservation_ShouldReturn_OK()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task AddServiceToReservation_WithNonexistReservation_ShouldReturn_NOT_FOUND()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task AddServiceToReservation_WithNonexistService_ShouldReturn_NOT_FOUND()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task AddServiceToReservation_WithNonexistRoom_ShouldReturn_NOT_FOUND()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task AddServiceToReservation_WithUnavailableServiceAtTheRoom_ShouldReturn_BAD_REQUEST()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task AddServiceToReservation_WithDisabledService_ShouldReturn_BAD_REQUEST()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task RemoveServiceFromReservation_ShouldReturn_OK()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task RemoveServiceFromReservation_WithNonexistReservation_ShouldReturn_NOT_FOUND()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task RemoveServiceFromReservation_WithNonexistService_ShouldReturn_NOT_FOUND()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task RemoveServiceFromReservation_WithoutContainsService_ShouldReturn_BAD_REQUEST()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task FinishReservation_ShouldReturn_OK()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task FinishReservation_WithNonexistReservation_ShouldReturn_NOT_FOUND()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task FinishReservation_WithDifferentCustomer_ShouldReturn_UNAUTHORIZED()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task CancelReservation_ShouldReturn_OK()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task CancelReservation_WithNonexistReservation_ShouldReturn_NOT_FOUND()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task CancelReservation_WithDifferentCustomer_ShouldReturn_UNAUTHORIZED()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task GetTotalAmount_ShouldReturn_OK()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task GetTotalAmount_WithInvalidParameters_ShouldReturn_BAD_REQUEST()
  {
    Assert.Fail();
  }
}

internal record GetReservation(Guid Id, decimal DailyRate, TimeSpan ExpectedTimeHosted, DateTime ExpectedCheckIn, DateTime ExpectedCheckOut, TimeSpan? TimeHosted, DateTime? CheckIn, DateTime? CheckOut, EReservationStatus Status, int Capacity, Guid RoomId, Guid CustomerId, Guid? InvoiceId);