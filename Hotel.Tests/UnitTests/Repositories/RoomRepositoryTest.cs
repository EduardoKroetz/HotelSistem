using Hotel.Domain.Data;
using Hotel.Domain.DTOs.RoomDTOs;
using Hotel.Domain.Entities.CategoryEntity;
using Hotel.Domain.Entities.RoomEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Repositories;
using Hotel.Tests.UnitTests.Repositories.InMemoryDatabase.Utils;
using Hotel.Tests.UnitTests.Repositories.InMemoryDatabase;
using Microsoft.EntityFrameworkCore.Storage;
using Hotel.Domain.Entities.ServiceEntity;
using Microsoft.EntityFrameworkCore;
using Hotel.Domain.Entities.ReservationEntity;
using Hotel.Domain.ValueObjects;
using Hotel.Domain.Entities.CustomerEntity;

namespace Hotel.Tests.UnitTests.Repositories;

[TestClass]
public class RoomRepositoryTest
{
    private readonly RoomRepository _roomRepository;
    private readonly HotelDbContext _dbContext;
    private readonly RepositoryTestUtils _utils;
    private static readonly AsyncLocal<IDbContextTransaction?> _currentTransaction = new AsyncLocal<IDbContextTransaction?>();

    public RoomRepositoryTest()
    {
        var sqliteDatabase = new SqliteDatabase();
        _dbContext = sqliteDatabase.Context;
        _roomRepository = new RoomRepository(_dbContext);
        _utils = new RepositoryTestUtils(_dbContext);
    }

    [TestInitialize]
    public async Task Initialize()
    {
        _currentTransaction.Value = await _dbContext.Database.BeginTransactionAsync();
    }

    [TestCleanup]
    public async Task Cleanup()
    {
        if (_currentTransaction.Value != null)
        {
            await _currentTransaction.Value.RollbackAsync();
            await _currentTransaction.Value.DisposeAsync();
            _currentTransaction.Value = null;
        }
    }

    [TestMethod]
    public async Task GetByIdAsync_ReturnsWithCorrectParameters()
    {
        // Arrange
        var newCategory = await _utils.CreateCategoryAsync(new Category("Deluxe", "Deluxe", 190));
        var newRoom = await _utils.CreateRoomAsync(new Room("Deluxe Room", 99, 178.30m, 9, "Deluxe room is a...", newCategory));

        // Act
        var room = await _roomRepository.GetByIdAsync(newRoom.Id);

        // Assert
        Assert.IsNotNull(room);
        Assert.AreEqual(newRoom.Id, room.Id);
        Assert.AreEqual(newRoom.Number, room.Number);
        Assert.AreEqual(newRoom.Description, room.Description);
        Assert.AreEqual(newRoom.Price, room.Price);
        Assert.AreEqual(newRoom.Status, room.Status);
        Assert.AreEqual(newRoom.Capacity, room.Capacity);
        Assert.AreEqual(newRoom.Description, room.Description);
    }

    [TestMethod]
    public async Task GetAsync_ReturnWithCorrectParameters()
    {
        // Arrange
        var newCategory = await _utils.CreateCategoryAsync(new Category("Deluxe", "Deluxe", 190));
        var newRoom = await _utils.CreateRoomAsync(new Room("Deluxe Room", 99, 178.30m, 9, "Deluxe room is a...", newCategory));

        var parameters = new RoomQueryParameters(0, 100, null, newRoom.Number, "eq", newRoom.Price, "eq", newRoom.Status, null, null, null, null, null, null);

        // Act
        var rooms = await _roomRepository.GetAsync(parameters);
        var room = rooms.ToList()[0];

        // Assert
        Assert.IsNotNull(room);
        Assert.AreEqual(newRoom.Id, room.Id);
        Assert.AreEqual(newRoom.Name, room.Name);
        Assert.AreEqual(newRoom.Number, room.Number);
        Assert.AreEqual(newRoom.Description, room.Description);
        Assert.AreEqual(newRoom.Price, room.Price);
        Assert.AreEqual(newRoom.Status, room.Status);
        Assert.AreEqual(newRoom.Capacity, room.Capacity);
        Assert.AreEqual(newRoom.Description, room.Description);
    }

    [TestMethod]
    public async Task GetAsync_WhereName_ReturnRooms()
    {
        // Arrange
        var newCategory = await _utils.CreateCategoryAsync(new Category("Deluxe beach", "Quartos de luxo com vista para praia", 190));
        var newRoom = await _utils.CreateRoomAsync(new Room("Quarto de luxo a beira mar", 999, 230m, 6, "Um quarto de luxo com vista luxuosa", newCategory));

        var parameters = new RoomQueryParameters(0, 100, newRoom.Name, null, null, null, null, null, null, null, null, null, null, null);

        // Act
        var rooms = await _roomRepository.GetAsync(parameters);
        var returnedRoom = rooms.FirstOrDefault();

        // Assert
        Assert.IsTrue(rooms.Any());
        Assert.AreEqual(newRoom.Id, returnedRoom.Id);
        Assert.AreEqual(newRoom.Name, returnedRoom.Name);
        Assert.AreEqual(newRoom.Number, returnedRoom.Number);
        Assert.AreEqual(newRoom.Description, returnedRoom.Description);
        Assert.AreEqual(newRoom.Price, returnedRoom.Price);
        Assert.AreEqual(newRoom.Status, returnedRoom.Status);
        Assert.AreEqual(newRoom.Capacity, returnedRoom.Capacity);
        Assert.AreEqual(newRoom.Description, returnedRoom.Description);
    }

    [TestMethod]
    public async Task GetAsync_WhereNumberGratherThan20_ReturnRooms()
    {
        // Arrange
        var newCategory = await _utils.CreateCategoryAsync(new Category("Deluxe", "Deluxe", 190));
        var newRoom1 = await _utils.CreateRoomAsync(new Room("Room 21", 21, 200m, 2, "Room with number greater than 20", newCategory));
        var newRoom2 = await _utils.CreateRoomAsync(new Room("Room 22", 22, 220m, 3, "Room with number greater than 20", newCategory));

        var parameters = new RoomQueryParameters(0, 100, null, 20, "gt", null, null, null, null, null, null, null, null, null);

        // Act
        var rooms = await _roomRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(rooms.Any());
        foreach (var room in rooms)
            Assert.IsTrue(20 < room.Number);
    }

    [TestMethod]
    public async Task GetAsync_WhereNumberLessThan20_ReturnRooms()
    {
        // Arrange
        var newCategory = await _utils.CreateCategoryAsync(new Category("Deluxe", "Deluxe", 190));
        var newRoom1 = await _utils.CreateRoomAsync(new Room("Room 19", 19, 200m, 2, "Room with number less than 20", newCategory));
        var newRoom2 = await _utils.CreateRoomAsync(new Room("Room 18", 18, 220m, 3, "Room with number less than 20", newCategory));

        var parameters = new RoomQueryParameters(0, 100, null, 20, "lt", null, null, null, null, null, null, null, null, null);

        // Act
        var rooms = await _roomRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(rooms.Any());
        foreach (var room in rooms)
            Assert.IsTrue(20 > room.Number);
    }

    [TestMethod]
    public async Task GetAsync_WhereNumberEquals9_ReturnRooms()
    {
        // Arrange
        var newCategory = await _utils.CreateCategoryAsync(new Category("Deluxe", "Deluxe", 190));
        var newRoom = await _utils.CreateRoomAsync(new Room("Room 9", 9, 200m, 2, "Room with number 9", newCategory));

        var parameters = new RoomQueryParameters(0, 100, null, 9, "eq", null, null, null, null, null, null, null, null, null);

        // Act
        var rooms = await _roomRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(rooms.Any());
        foreach (var room in rooms)
            Assert.AreEqual(9, room.Number);
    }

    [TestMethod]
    public async Task GetAsync_WherePriceGratherThan50_ReturnRooms()
    {
        // Arrange
        var newCategory = await _utils.CreateCategoryAsync(new Category("Deluxe", "Deluxe", 190));
        var newRoom = await _utils.CreateRoomAsync(new Room("Room 1", 1, 60m, 2, "Room with price greater than 50", newCategory));

        var parameters = new RoomQueryParameters(0, 100, null, null, null, 50, "gt", null, null, null, null, null, null, null);

        // Act
        var rooms = await _roomRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(rooms.Any());
        foreach (var room in rooms)
            Assert.IsTrue(50 < room.Price);
    }

    [TestMethod]
    public async Task GetAsync_WherePriceLessThan60_ReturnRooms()
    {
        // Arrange
        var newCategory = await _utils.CreateCategoryAsync(new Category("Deluxe", "Deluxe", 190));
        var newRoom = await _utils.CreateRoomAsync(new Room("Room 2", 2, 50m, 2, "Room with price less than 60", newCategory));

        var parameters = new RoomQueryParameters(0, 100, null, null, null, 60, "lt", null, null, null, null, null, null, null);

        // Act
        var rooms = await _roomRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(rooms.Any());
        foreach (var room in rooms)
            Assert.IsTrue(60 > room.Price);
    }

    [TestMethod]
    public async Task GetAsync_WherePriceEquals50_ReturnRooms()
    {
        // Arrange
        var newCategory = await _utils.CreateCategoryAsync(new Category("Deluxe", "Deluxe", 190));
        var newRoom = await _utils.CreateRoomAsync(new Room("Room 3", 3, 50m, 2, "Room with price equals 50", newCategory));

        var parameters = new RoomQueryParameters(0, 100, null, null, null, 50, "eq", null, null, null, null, null, null, null);

        // Act
        var rooms = await _roomRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(rooms.Any());
        foreach (var room in rooms)
            Assert.AreEqual(50, room.Price);
    }

    [TestMethod]
    public async Task GetAsync_WhereStatusEqualsOutOfService_ReturnRooms()
    {
        // Arrange
        var newCustomer = await _utils.CreateCustomerAsync(new Customer(new Name("Carlos", "Oliveira"), new Email("coliveira@example.com"), new Phone("+55 (21) 31345-6789"), "_customer123", EGender.Masculine, DateTime.Now.AddYears(-30), new Address("Brazil", "Brasília", "Quadra 123", 3)));
        var newCategory = await _utils.CreateCategoryAsync(new Category("Deluxe", "Deluxe", 190));
        var newRoom = await _utils.CreateRoomAsync(new Room("Room 5", 5, 50m, 2, "Room with status reserved", newCategory));
        var newReservation = await _utils.CreateReservationAsync(new Reservation(newRoom, DateTime.Now.AddDays(3), DateTime.Now.AddDays(4), newCustomer, 2));
        newReservation.ToCheckIn();
        newReservation.Finish();
        await _dbContext.SaveChangesAsync();

        var parameters = new RoomQueryParameters(0, 100, null, null, null, null, null, ERoomStatus.OutOfService, null, null, null, null, null, null);

        // Act
        var rooms = await _roomRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(rooms.Any());
        foreach (var room in rooms)
            Assert.AreEqual(ERoomStatus.OutOfService, room.Status);
    }

    [TestMethod]
    public async Task GetAsync_WhereStatusEqualsReserved_ReturnRooms()
    {
        // Arrange
        var newCustomer = await _utils.CreateCustomerAsync(new Customer(new Name("Carlos", "Oliveira"), new Email("coliveira@example.com"), new Phone("+55 (21) 31345-6789"), "_customer123", EGender.Masculine, DateTime.Now.AddYears(-30), new Address("Brazil", "Brasília", "Quadra 123", 3)));
        var newCategory = await _utils.CreateCategoryAsync(new Category("Deluxe", "Deluxe", 190));
        var newRoom = await _utils.CreateRoomAsync(new Room("Room 5", 5, 50m, 2, "Room with status reserved", newCategory));
        await _utils.CreateReservationAsync(new Reservation(newRoom, DateTime.Now.AddDays(3) , DateTime.Now.AddDays(4), newCustomer, 2 ));

        var parameters = new RoomQueryParameters(0, 100, null, null, null, null, null, ERoomStatus.Reserved, null, null, null, null, null, null);

        // Act
        var rooms = await _roomRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(rooms.Any());
        foreach (var room in rooms)
            Assert.AreEqual(ERoomStatus.Reserved, room.Status);
    }

    [TestMethod]
    public async Task GetAsync_WhereCapacityGratherThan2_ReturnRooms()
    {
        // Arrange
        var newCategory = await _utils.CreateCategoryAsync(new Category("Deluxe", "Deluxe", 190));
        var newRoom = await _utils.CreateRoomAsync(new Room("Room 6", 6, 50m, 3, "Room with capacity greater than 2", newCategory));

        var parameters = new RoomQueryParameters(0, 100, null, null, null, null, null, null, 2, "gt", null, null, null, null);

        // Act
        var rooms = await _roomRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(rooms.Any());
        foreach (var room in rooms)
            Assert.IsTrue(2 < room.Capacity);
    }

    [TestMethod]
    public async Task GetAsync_WhereCapacityLessThan3_ReturnRooms()
    {
        // Arrange
        var newCategory = await _utils.CreateCategoryAsync(new Category("Deluxe", "Deluxe", 190));
        var newRoom = await _utils.CreateRoomAsync(new Room("Room 7", 7, 50m, 2, "Room with capacity less than 3", newCategory));

        var parameters = new RoomQueryParameters(0, 100, null, null, null, null, null, null, 3, "lt", null, null, null, null);

        // Act
        var rooms = await _roomRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(rooms.Any());
        foreach (var room in rooms)
            Assert.IsTrue(3 > room.Capacity);
    }

    [TestMethod]
    public async Task GetAsync_WhereCapacityEquals2_ReturnRooms()
    {
        // Arrange
        var newCategory = await _utils.CreateCategoryAsync(new Category("Deluxe", "Deluxe", 190));
        var newRoom = await _utils.CreateRoomAsync(new Room("Room 8", 8, 50m, 2, "Room with capacity equals 2", newCategory));

        var parameters = new RoomQueryParameters(0, 100, null, null, null, null, null, null, 2, "eq", null, null, null, null);

        // Act
        var rooms = await _roomRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(rooms.Any());
        foreach (var room in rooms)
            Assert.AreEqual(2, room.Capacity);
    }

    [TestMethod]
    public async Task GetAsync_WhereServiceId_ReturnRooms()
    {
        // Arrange
        var newService = await _utils.CreateServiceAsync(new Service("Room Service", "Room service", 50m, EPriority.Medium, 15));
        var newCategory = await _utils.CreateCategoryAsync(new Category("Deluxe", "Deluxe", 190));
        var newRoom = await _utils.CreateRoomAsync(new Room("Room 9", 9, 50m, 2, "Room with specific service", newCategory));
        newRoom.Services.Add(newService);
        await _dbContext.SaveChangesAsync();

        var parameters = new RoomQueryParameters(0, 100, null, null, null, null, null, null, null, null, newService.Id, null, null, null);

        // Act
        var rooms = await _roomRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(rooms.Any());
        foreach (var room in rooms)
        {
            var hasService = await _dbContext.Rooms
                .Where(x => x.Id == room.Id)
                .SelectMany(x => x.Services)
                .AnyAsync(x => x.Id == newService.Id);

            Assert.IsTrue(hasService);
        }
    }

    [TestMethod]
    public async Task GetAsync_WhereCategoryId_ReturnRooms()
    {
        // Arrange
        var newCategory = await _utils.CreateCategoryAsync(new Category("Category Test", "Description", 190));
        var newRoom = await _utils.CreateRoomAsync(new Room("Room 10", 10, 50m, 2, "Room with specific category", newCategory));

        var parameters = new RoomQueryParameters(0, 100, null, null, null, null, null, null, null, null, null, newCategory.Id, null, null);

        // Act
        var rooms = await _roomRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(rooms.Any());
        foreach (var room in rooms)
            Assert.AreEqual(newCategory.Id, room.CategoryId);
    }

    [TestMethod]
    public async Task GetAsync_WhereNumberGratherThan5_And_PriceLessThan60_And_CapacityGratherThan2_ReturnRooms()
    {
        // Arrange
        var newCategory = await _utils.CreateCategoryAsync(new Category("Deluxe", "Deluxe", 190));
        var newRoom = await _utils.CreateRoomAsync(new Room("Room 14", 6, 50m, 3, "Room matching multiple criteria", newCategory));

        var parameters = new RoomQueryParameters(0, 100, null, 5, "gt", 60, "lt", null, 2, "gt", null, null, null, null);

        // Act
        var rooms = await _roomRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(rooms.Any());
        foreach (var room in rooms)
        {
            Assert.IsTrue(5 < room.Number);
            Assert.IsTrue(60 > room.Price);
            Assert.IsTrue(2 < room.Capacity);
        }
    }
}
