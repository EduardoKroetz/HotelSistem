﻿using Hotel.Domain.Data;
using Hotel.Domain.DTOs.RoomContext.RoomDTOs;
using Hotel.Domain.Entities.CustomerContext;
using Hotel.Domain.Entities.ReservationContext.ReservationEntity;
using Hotel.Domain.Entities.RoomContext.CategoryEntity;
using Hotel.Domain.Entities.RoomContext.RoomEntity;
using Hotel.Domain.Entities.RoomContext.ServiceEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.ValueObjects;
using Hotel.Tests.IntegrationTests.Factories;
using Hotel.Tests.IntegrationTests.Utilities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using static PdfSharp.Capabilities.Features;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Hotel.Tests.IntegrationTests.Controllers;

[TestClass]
public class RoomReportControllerTests
{
  private static HotelWebApplicationFactory _factory = null!;
  private static HttpClient _client = null!;
  private static HotelDbContext _dbContext = null!;
  private static string _rootAdminToken = null!;
  private const string _baseUrl = "v1/rooms";
  private static Category _basicCategory = null!;
  private static Category _standardCategory = null!;
  private static Category _deluxeCategory = null!;


  [ClassInitialize]
  public static void ClassInitialize(TestContext context)
  {
    _factory = new HotelWebApplicationFactory();
    _client = _factory.CreateClient();
    _dbContext = _factory.Services.GetRequiredService<HotelDbContext>();

    _rootAdminToken = _factory.LoginFullAccess().Result;
    _factory.Login(_client, _rootAdminToken);

    _basicCategory = new Category("Basic", "Mais variados quartos básicos", 35);
    _standardCategory = new Category("Standard", "Mais variados quartos padrões", 50);
    _deluxeCategory = new Category("Deluxe", "Mais variados quartos de luxo", 110);

    _dbContext.Categories.AddRangeAsync([_basicCategory, _standardCategory, _deluxeCategory]).Wait();
    _dbContext.SaveChangesAsync().Wait();
  }

  [ClassCleanup]
  public static void ClassCleanup()
  {
    _factory.Dispose();
  }

  [TestInitialize]
  public void TestInitialize()
  {
    _factory.Login(_client, _rootAdminToken);
  }

  [TestMethod]
  public async Task CreateRoom_ShouldReturn_OK()
  {
    //Arange
    var body = new EditorRoom(1, 35, 2, "Quarto básico 1", _basicCategory.Id);

    //Act
    var response = await _client.PostAsJsonAsync(_baseUrl, body);

    //Assert
    response.EnsureSuccessStatusCode();

    var content = JsonConvert.DeserializeObject<Response<DataId>>(await response.Content.ReadAsStringAsync())!;
    var room = await _dbContext.Rooms.FirstAsync(x => x.Id == content.Data.Id);

    Assert.AreEqual(200, content.Status);
    Assert.AreEqual("Cômodo criado com sucesso!", content.Message);
    Assert.AreEqual(0, content.Errors.Count);

    Assert.AreEqual(body.Number, room.Number);
    Assert.AreEqual(body.Price, room.Price);
    Assert.AreEqual(body.Capacity, room.Capacity);
    Assert.AreEqual(body.Description, room.Description);
    Assert.AreEqual(body.CategoryId, room.CategoryId);
  }

  //[TestMethod]
  //public async Task CreateRoom_WithNumberAlreadyRegistered_ShouldReturn_BAD_REQUEST()
  //{
  //  //Arange
  //  var room = new Room(2, 35, 2, "Quarto básico 2", _basicCategory.Id);
  //  await _dbContext.Rooms.AddAsync(room);
  //  await _dbContext.SaveChangesAsync();

  //  var body = new EditorRoom(2, 40, 3, "Quarto básico 2", _basicCategory.Id);

  //  //Act
  //  var response = await _client.PostAsJsonAsync(_baseUrl, body);

  //  //Assert
  //  Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

  //  var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;

  //  Assert.AreEqual(400, content.Status);
  //  Assert.IsTrue(content.Errors.Any(x => x.Equals("Esse número do cômodo já foi cadastrado.")));
  //}

  [TestMethod]
  public async Task UpdateRoom_ShouldReturn_OK()
  {
    //Arange
    var room = new Room(3, 35, 2, "Quarto básico 2", _basicCategory.Id);
    await _dbContext.Rooms.AddAsync(room);
    await _dbContext.SaveChangesAsync();

    var body = new EditorRoom(3, 40, 4, "Quarto básico 3", _basicCategory.Id);

    //Act
    var response = await _client.PutAsJsonAsync($"{_baseUrl}/{room.Id}", body);

    //Assert
    response.EnsureSuccessStatusCode();

    var content = JsonConvert.DeserializeObject<Response<DataId>>(await response.Content.ReadAsStringAsync())!;
    var updatedRoom = await _dbContext.Rooms.FirstAsync(x => x.Id == content.Data.Id);

    Assert.AreEqual(200, content.Status);
    Assert.AreEqual("Cômodo atualizado com sucesso!", content.Message);
    Assert.AreEqual(0, content.Errors.Count);

    Assert.AreEqual(body.Number, updatedRoom.Number);
    Assert.AreEqual(body.Price, updatedRoom.Price);
    Assert.AreEqual(body.Capacity, updatedRoom.Capacity);
    Assert.AreEqual(body.Description, updatedRoom.Description);
    Assert.AreEqual(body.CategoryId, updatedRoom.CategoryId);
  }

  //[TestMethod]
  //public async Task UpdateRoom_WithNumberAlreadyRegistered_ShouldReturn_BAD_REQUEST()
  //{
  //  //Arange
  //  var room = new Room(4, 35, 2, "Quarto básico 2", _basicCategory.Id);
  //  await _dbContext.Rooms.AddRangeAsync(
  //  [
  //    new Room(5, 43, 2, "Quarto básico 4", _basicCategory.Id),
  //    room
  //  ]);
  //  await _dbContext.SaveChangesAsync();

  //  var body = new EditorRoom(5, 40, 4, "Quarto básico 4", _basicCategory.Id);

  //  //Act
  //  var response = await _client.PutAsJsonAsync($"{_baseUrl}/{room.Id}", body);

  //  //Assert
  //  Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

  //  var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;

  //  Assert.AreEqual(400, content.Status);
  //  Assert.IsTrue(content.Errors.Any(x => x.Equals("Esse número do cômodo já foi cadastrado.")));
  //}

  [TestMethod]
  public async Task UpdateRoom_WithUpdatedPriceAndAssociatedPendingReservations_ShouldReturn_BAD_REQUEST()
  {
    //Arange
    var customer = new Customer
    (
      new Name("Renata", "Oliveira"),
      new Email("renataOliveira@gmail.com"),
      new Phone("+55 (67) 97654-3210"),
      "password24",
      EGender.Feminine,
      DateTime.Now.AddYears(-26),
      new Address("Brazil", "Campo Grande", "MS-2424", 2424)
    );

    var room = new Room(6, 45, 2, "Quarto padrão", _standardCategory.Id);
    var reservation = new Reservation(room, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), customer, 1);
    await _dbContext.Rooms.AddAsync(room);
    await _dbContext.Reservations.AddAsync(reservation);
    await _dbContext.SaveChangesAsync();
   
    var body = new EditorRoom(6, 53, 4, "Quarto padrão", _standardCategory.Id);

    //Act
    var response = await _client.PutAsJsonAsync($"{_baseUrl}/{room.Id}", body);

    //Assert
    Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

    var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;

    Assert.AreEqual(400, content.Status);
    Assert.IsTrue(content.Errors.Any(x => x.Equals("Não foi possível atualizar o preço pois possuem reservas pendentes relacionadas ao cômodo.")));
  }


  [TestMethod]
  public async Task DeleteRoom_ShouldReturn_OK()
  {
    //Arange
    var room = new Room(7, 45, 2, "Quarto padrão", _standardCategory.Id);
    await _dbContext.Rooms.AddAsync(room);
    await _dbContext.SaveChangesAsync();

    //Act
    var response = await _client.DeleteAsync($"{_baseUrl}/{room.Id}");

    //Assert
    response.EnsureSuccessStatusCode();

    var content = JsonConvert.DeserializeObject<Response<DataId>>(await response.Content.ReadAsStringAsync())!;
    var exists = await _dbContext.Rooms.AnyAsync(x => x.Id == content.Data.Id);

    Assert.AreEqual(200, content.Status);
    Assert.AreEqual("Cômodo deletado com sucesso!", content.Message);
    Assert.AreEqual(0, content.Errors.Count);

    Assert.IsFalse(exists);
  }

  [TestMethod]
  public async Task DeleteRoom_WithAssociatedReservations_ShouldReturn_BAD_REQUEST()
  {
    //Arange
    var customer = new Customer
    (
      new Name("Juliana", "Silva"),
      new Email("julianaSilva@gmail.com"),
      new Phone("+55 (92) 92345-6789"),
      "password26",
      EGender.Feminine,
      DateTime.Now.AddYears(-27),
      new Address("Brazil", "Manaus", "AM-2626", 2626)
    );

    var room = new Room(8, 45, 2, "Quarto padrão", _standardCategory.Id);
    var reservation = new Reservation(room, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), customer, 1);
    await _dbContext.Rooms.AddAsync(room);
    await _dbContext.Reservations.AddAsync(reservation);
    await _dbContext.SaveChangesAsync();

    //Act
    var response = await _client.DeleteAsync($"{_baseUrl}/{room.Id}");

    //Assert
    Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

    var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;

    Assert.AreEqual(400, content.Status);
    Assert.IsTrue(content.Errors.Any(x => x.Equals("Não foi possível deletar o cômodo pois tem reservas associadas a ele. Sugiro que desative o cômodo.")));
  }

  [TestMethod]
  public async Task GetRooms_ShouldReturn_OK()
  {
    //Arange
    var createdRoom = new Room(9, 55, 4, "Quarto padrão", _standardCategory.Id);
    await _dbContext.Rooms.AddAsync(createdRoom);
    await _dbContext.SaveChangesAsync();

    //Act
    var response = await _client.GetAsync(_baseUrl);

    //Assert
    response.EnsureSuccessStatusCode();

    var content = JsonConvert.DeserializeObject<Response<List<GetRoomCollection>>>(await response.Content.ReadAsStringAsync())!;

    Assert.AreEqual(200, content.Status);
    Assert.AreEqual("Sucesso!", content.Message);
    Assert.AreEqual(0, content.Errors.Count);

    foreach (var room in content.Data)
    {
      Assert.IsNotNull(room.Id);
      Assert.IsNotNull(room.Number);
      Assert.IsNotNull(room.Price);
      Assert.IsNotNull(room.Capacity);
      Assert.IsNotNull(room.Description);
      Assert.IsNotNull(room.CategoryId);
      Assert.IsNotNull(room.CreatedAt);
      Assert.IsNotNull(room.Status);

    }
  }


  [TestMethod]
  public async Task GetRoomById_ShouldReturn_OK()
  {
    //Arange
    var room = new Room(10, 55, 4, "Quarto padrão", _standardCategory.Id);
    await _dbContext.Rooms.AddAsync(room);
    await _dbContext.SaveChangesAsync();

    //Act
    var response = await _client.GetAsync($"{_baseUrl}/{room.Id}");

    //Assert
    response.EnsureSuccessStatusCode();

    var content = JsonConvert.DeserializeObject<Response<GetRoom>>(await response.Content.ReadAsStringAsync())!;

    Assert.AreEqual(200, content.Status);
    Assert.AreEqual("Sucesso!", content.Message);
    Assert.AreEqual(0, content.Errors.Count);

    Assert.AreEqual(room.Id, content.Data.Id);
    Assert.AreEqual(room.Number, content.Data.Number);
    Assert.AreEqual(room.Price, content.Data.Price);
    Assert.AreEqual(room.Capacity, content.Data.Capacity);
    Assert.AreEqual(room.Description, content.Data.Description);
    Assert.AreEqual(room.CategoryId, content.Data.CategoryId);
    Assert.AreEqual(room.Images.Count, content.Data.Images.Count);
    Assert.AreEqual(room.Status, content.Data.Status);
    Assert.AreEqual(room.CreatedAt, content.Data.CreatedAt);
  }

  [TestMethod]
  public async Task AddService_ShouldReturn_OK()
  {
    //Arange
    var room = new Room(11, 53, 4, "Quarto padrão", _standardCategory.Id);
    var service = new Service("Breakfast Delivery", 20.00m, EPriority.High, 30);
    await _dbContext.Rooms.AddAsync(room);
    await _dbContext.Services.AddAsync(service);
    await _dbContext.SaveChangesAsync();

    //Act
    var response = await _client.PostAsJsonAsync($"{_baseUrl}/{room.Id}/services/{service.Id}", new { });

    //Assert
    response.EnsureSuccessStatusCode();

    var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;
    var updatedRoom = await _dbContext.Rooms.Include(x => x.Services).FirstAsync(x => x.Id == room.Id);

    Assert.AreEqual(200, content.Status);
    Assert.AreEqual("Serviço adicinado com sucesso!", content.Message);
    Assert.AreEqual(0, content.Errors.Count);

    Assert.AreEqual(room.Id, updatedRoom.Id);
    Assert.AreEqual(room.Number, updatedRoom.Number);
    Assert.AreEqual(room.Price, updatedRoom.Price);
    Assert.AreEqual(room.Capacity, updatedRoom.Capacity);
    Assert.AreEqual(room.Description, updatedRoom.Description);
    Assert.AreEqual(room.CategoryId, updatedRoom.CategoryId);
    Assert.AreEqual(room.Images.Count, updatedRoom.Images.Count);
    Assert.AreEqual(room.Status, updatedRoom.Status);
    Assert.AreEqual(room.CreatedAt, updatedRoom.CreatedAt);

    Assert.IsTrue(updatedRoom.Services.Any(x => x.Id == service.Id));
  }

  [TestMethod]
  public async Task AddNonexistService_ShouldReturn_NOT_FOUND()
  {
    //Arange
    var room = new Room(12, 53, 4, "Quarto padrão", _standardCategory.Id);
    await _dbContext.Rooms.AddAsync(room);
    await _dbContext.SaveChangesAsync();

    //Act
    var response = await _client.PostAsJsonAsync($"{_baseUrl}/{room.Id}/services/{Guid.NewGuid()}", new { });

    //Assert
    Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

    var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;

    Assert.AreEqual(404, content.Status);
    Assert.IsTrue(content.Errors.Any(x => x.Equals("Serviço não encontrado.")));
  }

  [TestMethod]
  public async Task RemoveService_ShouldReturn_OK()
  {
    //Arange
    var room = new Room(13, 53, 4, "Quarto padrão", _standardCategory.Id);
    var service = new Service("Breakfast Delivery", 20.00m, EPriority.High, 30);
    room.AddService(service);
    await _dbContext.Rooms.AddAsync(room);
    await _dbContext.Services.AddAsync(service);
    await _dbContext.SaveChangesAsync();

    //Act
    var response = await _client.DeleteAsync($"{_baseUrl}/{room.Id}/services/{service.Id}");

    //Assert
    response.EnsureSuccessStatusCode();

    var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;
    var updatedRoom = await _dbContext.Rooms.Include(x => x.Services).FirstAsync(x => x.Id == room.Id);

    Assert.AreEqual(200, content.Status);
    Assert.AreEqual("Serviço removido com sucesso!", content.Message);
    Assert.AreEqual(0, content.Errors.Count);

    Assert.AreEqual(room.Id, updatedRoom.Id);
    Assert.AreEqual(room.Number, updatedRoom.Number);
    Assert.AreEqual(room.Price, updatedRoom.Price);
    Assert.AreEqual(room.Capacity, updatedRoom.Capacity);
    Assert.AreEqual(room.Description, updatedRoom.Description);
    Assert.AreEqual(room.CategoryId, updatedRoom.CategoryId);
    Assert.AreEqual(room.Images.Count, updatedRoom.Images.Count);
    Assert.AreEqual(room.Status, updatedRoom.Status);
    Assert.AreEqual(room.CreatedAt, updatedRoom.CreatedAt);

    Assert.IsFalse(updatedRoom.Services.Any(x => x.Id == service.Id));
  }

  [TestMethod]
  public async Task RemoveNonexistService_ShouldReturn_NOT_FOUND()
  {
    //Arange
    var room = new Room(14, 53, 4, "Quarto padrão", _standardCategory.Id);
    await _dbContext.Rooms.AddAsync(room);
    await _dbContext.SaveChangesAsync();

    //Act
    var response = await _client.DeleteAsync($"{_baseUrl}/{room.Id}/services/{Guid.NewGuid()}");

    //Assert
    Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

    var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;

    Assert.AreEqual(404, content.Status);
    Assert.IsTrue(content.Errors.Any(x => x.Equals("Serviço não encontrado.")));
  }

  [TestMethod]
  public async Task UpdateRoomNumber_ShouldReturn_OK()
  {
    //Arange
    var room = new Room(15, 53, 4, "Quarto padrão", _standardCategory.Id);
    await _dbContext.Rooms.AddAsync(room);
    await _dbContext.SaveChangesAsync();

    var newNumber = 16;
    //Act
    var response = await _client.PatchAsJsonAsync($"{_baseUrl}/{room.Id}/number/{newNumber}", new { });

    //Assert
    response.EnsureSuccessStatusCode();

    var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;
    var updatedRoom = await _dbContext.Rooms.FirstAsync(x => x.Id == room.Id);

    Assert.AreEqual(200, content.Status);
    Assert.AreEqual("Número atualizado com sucesso!", content.Message);
    Assert.AreEqual(0, content.Errors.Count);

    Assert.AreEqual(room.Id, updatedRoom.Id);
    Assert.AreEqual(newNumber, updatedRoom.Number);
    Assert.AreEqual(room.Price, updatedRoom.Price);
    Assert.AreEqual(room.Capacity, updatedRoom.Capacity);
    Assert.AreEqual(room.Description, updatedRoom.Description);
    Assert.AreEqual(room.CategoryId, updatedRoom.CategoryId);
    Assert.AreEqual(room.Images.Count, updatedRoom.Images.Count);
    Assert.AreEqual(room.Status, updatedRoom.Status);
    Assert.AreEqual(room.CreatedAt, updatedRoom.CreatedAt);
  }

  [TestMethod]
  public async Task UpdateRoomCategory_ShouldReturn_OK()
  {
    //Arange
    var room = new Room(17, 199, 10, "Quarto deluxe", _standardCategory.Id);
    await _dbContext.Rooms.AddAsync(room);
    await _dbContext.SaveChangesAsync();

    var newCategoryId = _deluxeCategory.Id;
    //Act
    var response = await _client.PatchAsJsonAsync($"{_baseUrl}/{room.Id}/category/{newCategoryId}", new { });

    //Assert
    response.EnsureSuccessStatusCode();

    var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;
    var updatedRoom = await _dbContext.Rooms.FirstAsync(x => x.Id == room.Id);

    Assert.AreEqual(200, content.Status);
    Assert.AreEqual("Categoria atualizada com sucesso!", content.Message);
    Assert.AreEqual(0, content.Errors.Count);

    Assert.AreEqual(room.Id, updatedRoom.Id);
    Assert.AreEqual(room.Number, updatedRoom.Number);
    Assert.AreEqual(room.Price, updatedRoom.Price);
    Assert.AreEqual(room.Capacity, updatedRoom.Capacity);
    Assert.AreEqual(room.Description, updatedRoom.Description);
    Assert.AreEqual(newCategoryId, updatedRoom.CategoryId);
    Assert.AreEqual(room.Images.Count, updatedRoom.Images.Count);
    Assert.AreEqual(room.Status, updatedRoom.Status);
    Assert.AreEqual(room.CreatedAt, updatedRoom.CreatedAt);
  }

  [TestMethod]
  public async Task UpdateRoomCategory_WithNonexistCategory_ShouldReturn_NOT_FOUND()
  {
    //Arange
    var room = new Room(18, 199, 10, "Quarto deluxe", _standardCategory.Id);
    await _dbContext.Rooms.AddAsync(room);
    await _dbContext.SaveChangesAsync();

    //Act
    var response = await _client.PatchAsJsonAsync($"{_baseUrl}/{room.Id}/category/{Guid.NewGuid()}", new { });

    //Assert
    Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

    var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;

    Assert.AreEqual(404, content.Status);
    Assert.IsTrue(content.Errors.Any(x => x.Equals("Categoria não encontrada.")));
  }

  [TestMethod]
  public async Task UpdateRoomPrice_ShouldReturn_OK()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task UpdateRoomPrice_WithPendingReservationAssociated_ShouldReturn_BAD_REQUEST()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task UpdateRoomPrice_WithAssociatedNonPendingReservation_ShouldReturn_OK()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task UpdateRoomCapacity_ShouldReturn_OK()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task EnableRoom_ShouldReturn_OK()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task DisableRoom_ShouldReturn_OK()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task DisableRoom_WithAssociatedPendingReservations_ShouldReturn_BAD_REQUEST()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task UpdateToAvailableStatus_ShouldReturn_OK()
  {
    Assert.Fail();
  }
}
