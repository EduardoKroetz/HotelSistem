using Hotel.Domain.Data;
using Hotel.Domain.DTOs.ReservationDTOs;
using Hotel.Domain.Repositories;
using Hotel.Tests.UnitTests.Repositories.InMemoryDatabase.Utils;
using Hotel.Tests.UnitTests.Repositories.InMemoryDatabase;
using Microsoft.EntityFrameworkCore.Storage;
using Hotel.Domain.Entities.ReservationEntity;
using Hotel.Domain.Entities.CategoryEntity;
using Hotel.Domain.Entities.RoomEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.ValueObjects;
using Hotel.Domain.Entities.CustomerEntity;


namespace Hotel.Tests.UnitTests.Repositories;

[TestClass]
public class ReservationRepositoryTest
{
    private readonly ReservationRepository _reservationRepository;
    private readonly HotelDbContext _dbContext;
    private readonly RepositoryTestUtils _utils;
    private static readonly AsyncLocal<IDbContextTransaction?> _currentTransaction = new AsyncLocal<IDbContextTransaction?>();

    public ReservationRepositoryTest()
    {
        var sqliteDatabase = new SqliteDatabase();
        _dbContext = sqliteDatabase.Context;
        _reservationRepository = new ReservationRepository(_dbContext);
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

    public async Task<Reservation> CreateReservationAsync()
    {
        var newCustomer = await _utils.CreateCustomerAsync(new Customer(new Name("Carlos", "Oliveira"), new Email("coliveira@example.com"), new Phone("+55 (21) 31345-6789"), "_customer123", EGender.Masculine, DateTime.Now.AddYears(-30), new Address("Brazil", "Brasília", "Quadra 123", 3)));
        var newCategory = await _utils.CreateCategoryAsync(new Category("Deluxe", "Deluxe", 190));
        var newRoom = await _utils.CreateRoomAsync(new Room("Deluxe Beach", 99, 170, 8, "Deluxe beach is a room", newCategory));
        return await _utils.CreateReservationAsync(new Reservation(newRoom, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), newCustomer, 4));
    }


    [TestMethod]
    public async Task GetByIdAsync_ReturnsWithCorrectParameters()
    {
        //Arrange
        var newReservation = await CreateReservationAsync();

        //Act
        var reservation = await _reservationRepository.GetByIdAsync(newReservation.Id);

        //Assert
        Assert.IsNotNull(reservation);
        Assert.AreEqual(newReservation.Id, reservation.Id);
        Assert.AreEqual(newReservation.DailyRate, reservation.DailyRate);
        Assert.AreEqual(newReservation.TimeHosted, reservation.TimeHosted);
        Assert.AreEqual(newReservation.CheckIn, reservation.CheckIn);
        Assert.AreEqual(newReservation.CheckOut, reservation.CheckOut);
        Assert.AreEqual(newReservation.Status, reservation.Status);
        Assert.AreEqual(newReservation.Capacity, reservation.Capacity);
        Assert.AreEqual(newReservation.RoomId, reservation.RoomId);
        Assert.AreEqual(newReservation.InvoiceId, reservation.InvoiceId);
    }

    [TestMethod]
    public async Task GetAsync_ReturnWithCorrectParameters()
    {
        //Arrange
        var newReservation = await CreateReservationAsync();

        //Act
        var parameters = new ReservationQueryParameters{ TimeHosted = newReservation.TimeHosted, TimeHostedOperator = "eq", DailyRate = newReservation.DailyRate, DailyRateOperator = "eq", CheckIn = newReservation.CheckIn, CheckInOperator = "eq", CheckOut = newReservation.CheckOut, CheckOutOperator = "eq", Status = newReservation.Status, Capacity = newReservation.Capacity, CapacityOperator = "eq", RoomId = newReservation.RoomId , InvoiceId = newReservation.InvoiceId };
        var reservations = await _reservationRepository.GetAsync(parameters);

        var reservation = reservations.ToList()[0];

        //Assert
        Assert.IsNotNull(reservation);
        Assert.AreEqual(newReservation.Id, reservation.Id);
        Assert.AreEqual(newReservation.DailyRate, reservation.DailyRate);
        Assert.AreEqual(newReservation.TimeHosted, reservation.TimeHosted);
        Assert.AreEqual(newReservation.CheckIn, reservation.CheckIn);
        Assert.AreEqual(newReservation.CheckOut, reservation.CheckOut);
        Assert.AreEqual(newReservation.Status, reservation.Status);
        Assert.AreEqual(newReservation.Capacity, reservation.Capacity);
        Assert.AreEqual(newReservation.RoomId, reservation.RoomId);
        Assert.AreEqual(newReservation.InvoiceId, reservation.InvoiceId);
    }

    [TestMethod]
    public async Task GetAsync_WhereTimeHostedLessThan24Hours_ReturnsReservations()
    {
        //Arrange
        var newCustomer = await _utils.CreateCustomerAsync(new Customer(new Name("Carlos", "Oliveira"), new Email("coliveira@example.com"), new Phone("+55 (21) 31345-6789"), "_customer123", EGender.Masculine, DateTime.Now.AddYears(-30), new Address("Brazil", "Brasília", "Quadra 123", 3)));
        var newCategory = await _utils.CreateCategoryAsync(new Category("Deluxe", "Deluxe", 190));
        var newRoom = await _utils.CreateRoomAsync(new Room("Deluxe Beach", 99, 119, 8, "Deluxe beach is a room", newCategory));
        var newReservation = await _utils.CreateReservationAsync(new Reservation(newRoom, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), newCustomer, 4));
        newReservation.ToCheckIn();
        newReservation.Finish();
        await _dbContext.SaveChangesAsync();

        //Act
        var parameters = new ReservationQueryParameters{ TimeHosted = TimeSpan.FromDays(1), TimeHostedOperator = "lt" };
        var reservations = await _reservationRepository.GetAsync(parameters);

        //Assert
        Assert.IsTrue(reservations.Any());
        foreach (var reservation in reservations)
        {
            Assert.IsTrue(TimeSpan.FromDays(1) > reservation.TimeHosted);
        }
    }

    [TestMethod]
    public async Task GetAsync_WhereTimeHostedEquals_ReturnsReservations()
    {
        //Arrange
        var newReservation = await CreateReservationAsync();

        //Act
        var parameters = new ReservationQueryParameters{ TimeHosted = newReservation.TimeHosted, TimeHostedOperator = "eq" };
        var reservations = await _reservationRepository.GetAsync(parameters);

        //Assert
        Assert.IsTrue(reservations.Any());
        foreach (var reservation in reservations)
        {
            Assert.AreEqual(newReservation.TimeHosted, reservation.TimeHosted);
        }
    }

    [TestMethod]
    public async Task GetAsync_WhereDailyRateGratherThan70_ReturnsReservations()
    {
        //Arrange
        var newReservation = await CreateReservationAsync();

        //Act
        var parameters = new ReservationQueryParameters{ DailyRate = 70, DailyRateOperator = "gt" };
        var reservations = await _reservationRepository.GetAsync(parameters);

        //Assert
        Assert.IsTrue(reservations.Any());
        foreach (var reservation in reservations)
        {
            Assert.IsTrue(70 < reservation.DailyRate);
        }
    }

    [TestMethod]
    public async Task GetAsync_WhereDailyRateLessThan120_ReturnsReservations()
    {
        //Arrange
        var newCustomer = await _utils.CreateCustomerAsync(new Customer(new Name("Carlos", "Oliveira"), new Email("coliveira@example.com"), new Phone("+55 (21) 31345-6789"), "_customer123", EGender.Masculine, DateTime.Now.AddYears(-30), new Address("Brazil", "Brasília", "Quadra 123", 3)));
        var newCategory = await _utils.CreateCategoryAsync(new Category("Deluxe", "Deluxe", 190));
        var newRoom = await _utils.CreateRoomAsync(new Room("Deluxe Beach", 99, 119, 8, "Deluxe beach is a room", newCategory));
        var newReservation = await _utils.CreateReservationAsync(new Reservation(newRoom, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), newCustomer, 4));

        //Act
        var parameters = new ReservationQueryParameters { DailyRate = 120, DailyRateOperator = "lt" };
        var reservations = await _reservationRepository.GetAsync(parameters);

        //Assert
        Assert.IsTrue(reservations.Any());
        foreach (var reservation in reservations)
        {
            Assert.IsTrue(120 > reservation.DailyRate);
        }
    }

    [TestMethod]
    public async Task GetAsync_WhereDailyRateEquals_ReturnsReservations()
    {
        //Arrange
        var newReservation = await CreateReservationAsync();

        //Act
        var parameters = new ReservationQueryParameters{ DailyRate = newReservation.DailyRate, DailyRateOperator = "eq" };
        var reservations = await _reservationRepository.GetAsync(parameters);

        //Assert
        Assert.IsTrue(reservations.Any());
        foreach (var reservation in reservations)
        {
            Assert.AreEqual(newReservation.DailyRate, reservation.DailyRate);
        }
    }


}
