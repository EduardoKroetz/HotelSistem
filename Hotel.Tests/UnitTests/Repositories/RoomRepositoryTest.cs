using Hotel.Domain.DTOs.RoomDTOs;
using Hotel.Domain.Entities.CategoryEntity;
using Hotel.Domain.Entities.RoomEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Repositories;
using Hotel.Tests.UnitTests.Repositories.Mock;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Tests.UnitTests.Repositories;

[TestClass]
public class RoomRepositoryTest
{
    private static RoomRepository RoomRepository { get; set; }

    static RoomRepositoryTest()
    => RoomRepository = new RoomRepository(BaseRepositoryTest.MockConnection.Context);

    [TestMethod]
    public async Task GetByIdAsync_ReturnsWithCorrectParameters()
    {
        var rooms = await RoomRepository.GetByIdAsync(BaseRepositoryTest.Rooms[0].Id);

        Assert.IsNotNull(rooms);
        Assert.AreEqual(BaseRepositoryTest.Rooms[0].Id, rooms.Id);
        Assert.AreEqual(BaseRepositoryTest.Rooms[0].Number, rooms.Number);
        Assert.AreEqual(BaseRepositoryTest.Rooms[0].Description, rooms.Description);
        Assert.AreEqual(BaseRepositoryTest.Rooms[0].Price, rooms.Price);
        Assert.AreEqual(BaseRepositoryTest.Rooms[0].Status, rooms.Status);
        Assert.AreEqual(BaseRepositoryTest.Rooms[0].Capacity, rooms.Capacity);
        Assert.AreEqual(BaseRepositoryTest.Rooms[0].Description, rooms.Description);
    }

    [TestMethod]
    public async Task GetAsync_ReturnWithCorrectParameters()
    {
        var anyRoom = BaseRepositoryTest.Rooms[0];

        var parameters = new RoomQueryParameters(0, 100, null, anyRoom.Number, "eq", anyRoom.Price, "eq", anyRoom.Status, null, null, null, null, null, null);
        var rooms = await RoomRepository.GetAsync(parameters);

        var room = rooms.ToList()[0];

        Assert.IsNotNull(room);
        Assert.AreEqual(anyRoom.Id, room.Id);
        Assert.AreEqual(anyRoom.Name, room.Name);
        Assert.AreEqual(anyRoom.Number, room.Number);
        Assert.AreEqual(anyRoom.Description, room.Description);
        Assert.AreEqual(anyRoom.Price, room.Price);
        Assert.AreEqual(anyRoom.Status, room.Status);
        Assert.AreEqual(anyRoom.Capacity, room.Capacity);
        Assert.AreEqual(anyRoom.Description, room.Description);
    }



    [TestMethod]
    public async Task GetAsync_WhereName_ReturnRooms()
    {
        var category = new Category("Deluxe beach", "Quartos de luxo com vista para praia", 190);
        var room = new Room("Quarto de luxo a beira mar", 999, 230m, 6, "Um quarto de luxo com vista luxuosa", category, "");

        await BaseRepositoryTest.MockConnection.Context.Rooms.AddAsync(room);
        await BaseRepositoryTest.MockConnection.Context.Rooms.AddAsync(room);
        await BaseRepositoryTest.MockConnection.Context.SaveChangesAsync();

        var parameters = new RoomQueryParameters(0, 100, room.Name, null, null, null, null, null, null, null, null, null, null, null);
        var rooms = await RoomRepository.GetAsync(parameters);
        var returnedRoom = rooms.FirstOrDefault()!;

        Assert.IsTrue(rooms.Any());

        Assert.AreEqual(room.Id, returnedRoom.Id);
        Assert.AreEqual(room.Name, returnedRoom.Name);
        Assert.AreEqual(room.Number, returnedRoom.Number);
        Assert.AreEqual(room.Description, returnedRoom.Description);
        Assert.AreEqual(room.Price, returnedRoom.Price);
        Assert.AreEqual(room.Status, returnedRoom.Status);
        Assert.AreEqual(room.Capacity, returnedRoom.Capacity);
        Assert.AreEqual(room.Description, returnedRoom.Description);
    }

    [TestMethod]
    public async Task GetAsync_WhereNumberGratherThan20_ReturnRooms()
    {
        var parameters = new RoomQueryParameters(0, 100, null, 20, "gt", null, null, null, null, null, null, null, null, null);
        var rooms = await RoomRepository.GetAsync(parameters);

        Assert.IsTrue(rooms.Any());

        foreach (var room in rooms)
            Assert.IsTrue(20 < room.Number);
    }

    [TestMethod]
    public async Task GetAsync_WhereNumberLessThan20_ReturnRooms()
    {
        var parameters = new RoomQueryParameters(0, 100, null, 20, "lt", null, null, null, null, null, null, null, null, null);
        var rooms = await RoomRepository.GetAsync(parameters);

        Assert.IsTrue(rooms.Any());

        foreach (var room in rooms)
            Assert.IsTrue(20 > room.Number);
    }


    [TestMethod]
    public async Task GetAsync_WhereNumberEquals9_ReturnRooms()
    {
        var parameters = new RoomQueryParameters(0, 100, null, 9, "eq", null, null, null, null, null, null, null, null, null);
        var rooms = await RoomRepository.GetAsync(parameters);

        Assert.IsTrue(rooms.Any());

        foreach (var room in rooms)
            Assert.AreEqual(9, room.Number);
    }

    [TestMethod]
    public async Task GetAsync_WherePriceGratherThan50_ReturnRooms()
    {
        var parameters = new RoomQueryParameters(0, 100, null, null, null, 50, "gt", null, null, null, null, null, null, null);
        var rooms = await RoomRepository.GetAsync(parameters);

        Assert.IsTrue(rooms.Any());

        foreach (var room in rooms)
            Assert.IsTrue(50 < room.Price);
    }

    [TestMethod]
    public async Task GetAsync_WherePriceLessThan60_ReturnRooms()
    {
        var parameters = new RoomQueryParameters(0, 100, null, null, null, 60, "lt", null, null, null, null, null, null, null);
        var rooms = await RoomRepository.GetAsync(parameters);

        Assert.IsTrue(rooms.Any());

        foreach (var room in rooms)
            Assert.IsTrue(60 > room.Price);
    }

    [TestMethod]
    public async Task GetAsync_WherePriceEquals50_ReturnRooms()
    {
        var parameters = new RoomQueryParameters(0, 100, null, null, null, 50, "eq", null, null, null, null, null, null, null);
        var rooms = await RoomRepository.GetAsync(parameters);

        Assert.IsTrue(rooms.Any());

        foreach (var room in rooms)
            Assert.AreEqual(50, room.Price);
    }

    [TestMethod]
    public async Task GetAsync_WhereStatusEqualsOutOfService_ReturnRooms()
    {
        var parameters = new RoomQueryParameters(0, 100, null, null, null, null, null, ERoomStatus.OutOfService, null, null, null, null, null, null);
        var rooms = await RoomRepository.GetAsync(parameters);

        Assert.IsTrue(rooms.Any());

        foreach (var room in rooms)
            Assert.AreEqual(ERoomStatus.OutOfService, room.Status);
    }

    [TestMethod]
    public async Task GetAsync_WhereStatusEqualsReserved_ReturnRooms()
    {
        var parameters = new RoomQueryParameters(0, 100, null, null, null, null, null, ERoomStatus.Reserved, null, null, null, null, null, null);
        var rooms = await RoomRepository.GetAsync(parameters);

        Assert.IsTrue(rooms.Any());

        foreach (var room in rooms)
            Assert.AreEqual(ERoomStatus.Reserved, room.Status);
    }

    [TestMethod]
    public async Task GetAsync_WhereCapacityGratherThan2_ReturnRooms()
    {
        var parameters = new RoomQueryParameters(0, 100, null, null, null, null, null, null, 2, "gt", null, null, null, null);
        var rooms = await RoomRepository.GetAsync(parameters);

        Assert.IsTrue(rooms.Any());

        foreach (var room in rooms)
            Assert.IsTrue(2 < room.Capacity);
    }


    [TestMethod]
    public async Task GetAsync_WhereCapacityLessThan3_ReturnRooms()
    {
        var parameters = new RoomQueryParameters(0, 100, null, null, null, null, null, null, 3, "lt", null, null, null, null);
        var rooms = await RoomRepository.GetAsync(parameters);

        Assert.IsTrue(rooms.Any());

        foreach (var room in rooms)
            Assert.IsTrue(3 > room.Capacity);
    }


    [TestMethod]
    public async Task GetAsync_WhereCapacityEquals2_ReturnRooms()
    {
        var parameters = new RoomQueryParameters(0, 100, null, null, null, null, null, null, 2, "eq", null, null, null, null);
        var rooms = await RoomRepository.GetAsync(parameters);

        Assert.IsTrue(rooms.Any());

        foreach (var room in rooms)
            Assert.AreEqual(2, room.Capacity);
    }

    [TestMethod]
    public async Task GetAsync_WhereServiceId_ReturnRooms()
    {
        var parameters = new RoomQueryParameters(0, 100, null, null, null, null, null, null, null, null, BaseRepositoryTest.Services[0].Id, null, null, null);
        var rooms = await RoomRepository.GetAsync(parameters);

        Assert.IsTrue(rooms.Any());

        foreach (var room in rooms)
        {
            var hasService = await BaseRepositoryTest.MockConnection.Context.Rooms
             .Where(x => x.Id == room.Id)
             .SelectMany(x => x.Services)
             .AnyAsync(x => x.Id == BaseRepositoryTest.Services[0].Id);

            Assert.IsTrue(hasService);
        }
    }

    [TestMethod]
    public async Task GetAsync_WhereCategoryId_ReturnRooms()
    {
        var parameters = new RoomQueryParameters(0, 100, null, null, null, null, null, null, null, null, null, BaseRepositoryTest.Categories[0].Id, null, null);
        var rooms = await RoomRepository.GetAsync(parameters);

        Assert.IsTrue(rooms.Any());

        foreach (var room in rooms)
            Assert.AreEqual(BaseRepositoryTest.Categories[0].Id, room.CategoryId);

    }

    [TestMethod]
    public async Task GetAsync_WhereCreatedAtGratherThanYesterday_ReturnRooms()
    {
        var parameters = new RoomQueryParameters(0, 100, null, null, null, null, null, null, null, null, null, null, DateTime.Now.AddDays(-1), "gt");
        var rooms = await RoomRepository.GetAsync(parameters);

        Assert.IsTrue(rooms.Any());
        foreach (var room in rooms)
            Assert.IsTrue(DateTime.Now.AddDays(-1) < room.CreatedAt);

    }

    [TestMethod]
    public async Task GetAsync_WhereCreatedAtLessThanToday_ReturnRooms()
    {
        var parameters = new RoomQueryParameters(0, 100, null, null, null, null, null, null, null, null, null, null, DateTime.Now.AddDays(1), "lt");
        var rooms = await RoomRepository.GetAsync(parameters);

        Assert.IsTrue(rooms.Any());
        foreach (var room in rooms)
            Assert.IsTrue(DateTime.Now.AddDays(1) > room.CreatedAt);

    }

    [TestMethod]
    public async Task GetAsync_WhereCreatedAtEquals_ReturnRooms()
    {
        var parameters = new RoomQueryParameters(0, 100, null, null, null, null, null, null, null, null, null, null, BaseRepositoryTest.Rooms[0].CreatedAt, "eq");
        var rooms = await RoomRepository.GetAsync(parameters);

        Assert.IsTrue(rooms.Any());
        foreach (var room in rooms)
            Assert.AreEqual(BaseRepositoryTest.Employees[0].CreatedAt.Date, room.CreatedAt.Date);
    }


    [TestMethod]
    public async Task GetAsync_WhereNumberGratherThan5_And_PriceLessThan60_And_CapacityGratherThan2_ReturnRooms()
    {

        var parameters = new RoomQueryParameters(0, 100, null, 5, "gt", 60, "lt", null, 2, "gt", null, null, null, null);
        var rooms = await RoomRepository.GetAsync(parameters);

        Assert.IsTrue(rooms.Any());
        foreach (var room in rooms)
        {
            Assert.IsTrue(5 < room.Number);
            Assert.IsTrue(60 > room.Price);
            Assert.IsTrue(2 < room.Capacity);

        }

    }


}
