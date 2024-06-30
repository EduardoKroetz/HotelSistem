using Hotel.Domain.Data;
using Hotel.Domain.DTOs.InvoiceDTOs;
using Hotel.Domain.Repositories;
using Hotel.Tests.UnitTests.Repositories.InMemoryDatabase.Utils;
using Hotel.Tests.UnitTests.Repositories.InMemoryDatabase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Hotel.Domain.Entities.InvoiceEntity;
using Hotel.Domain.Entities.CategoryEntity;
using Hotel.Domain.Entities.ReservationEntity;
using Hotel.Domain.Entities.RoomEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.ValueObjects;
using Hotel.Domain.Entities.CustomerEntity;
using Hotel.Domain.Entities.ServiceEntity;

namespace Hotel.Tests.UnitTests.Repositories;

[TestClass]
public class InvoiceRepositoryTest
{
    private readonly InvoiceRepository _invoiceRepository;
    private readonly HotelDbContext _dbContext;
    private readonly RepositoryTestUtils _utils;
    private static readonly AsyncLocal<IDbContextTransaction?> _currentTransaction = new AsyncLocal<IDbContextTransaction?>();

    public InvoiceRepositoryTest()
    {
        var sqliteDatabase = new SqliteDatabase();
        _dbContext = sqliteDatabase.Context;
        _invoiceRepository = new InvoiceRepository(_dbContext);
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

    private async Task<Invoice> CreateInvoiceAsync()
    {
        var newCustomer = await _utils.CreateCustomerAsync(new Customer(new Name("Carlos", "Oliveira"), new Email("coliveira@example.com"), new Phone("+55 (21) 31345-6789"), "_customer123", EGender.Masculine, DateTime.Now.AddYears(-30), new Address("Brazil", "Brasília", "Quadra 123", 3)));
        var newCategory = await _utils.CreateCategoryAsync(new Category("Deluxe", "Deluxe", 190));
        var newRoom = await _utils.CreateRoomAsync(new Room("Deluxe Beach", 99, 170, 8, "Deluxe beach is a room", newCategory));
        var newReservation = await _utils.CreateReservationAsync(new Reservation(newRoom, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), newCustomer, 4));
        newReservation.ToCheckIn();
        var newInvoice = newReservation.Finish();
        return await _utils.CreateInvoiceAsync(newInvoice);
    }

    [TestMethod]
    public async Task GetByIdAsync_ReturnsWithCorrectParameters()
    {
        // Arrange
        var invoice = await CreateInvoiceAsync();

        // Act
        var retrievedInvoice = await _invoiceRepository.GetByIdAsync(invoice.Id);

        // Assert
        Assert.IsNotNull(retrievedInvoice);
        Assert.AreEqual(invoice.Id, retrievedInvoice.Id);
        Assert.AreEqual(invoice.PaymentMethod, retrievedInvoice.PaymentMethod);
        Assert.AreEqual(invoice.ReservationId, retrievedInvoice.ReservationId);
        Assert.AreEqual(Math.Round(invoice.TotalAmount), Math.Round(retrievedInvoice.TotalAmount));
    }

    [TestMethod]
    public async Task GetAsync_ReturnWithCorrectParameters()
    {
        // Arrange
        var invoice = await CreateInvoiceAsync();
        var parameters = new InvoiceQueryParameters(0, 100, invoice.PaymentMethod, invoice.TotalAmount, "eq", null, null, null);

        // Act
        var invoices = await _invoiceRepository.GetAsync(parameters);
        var retrievedInvoice = invoices.FirstOrDefault();

        // Assert
        Assert.IsNotNull(retrievedInvoice);
        Assert.AreEqual(invoice.Id, retrievedInvoice.Id);
        Assert.AreEqual(invoice.PaymentMethod, retrievedInvoice.PaymentMethod);
        Assert.AreEqual(invoice.ReservationId, retrievedInvoice.ReservationId);
        Assert.AreEqual(Math.Round(invoice.TotalAmount), Math.Round(retrievedInvoice.TotalAmount));
    }

    [TestMethod]
    public async Task GetAsync_WhereCardPaymentMethod_ReturnsInvoices()
    {
        // Arrange
        var invoice = await CreateInvoiceAsync();
        var parameters = new InvoiceQueryParameters(0, 100, "card", null, null, null, null, null);

        // Act
        var invoices = await _invoiceRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(invoices.Any());
        foreach (var inv in invoices)
        {
            Assert.AreEqual("card", inv.PaymentMethod);
        }
    }

    [TestMethod]
    public async Task GetAsync_WhereTotalAmountGratherThan9_ReturnsInvoices()
    {
        // Arrange
        var newCustomer = await _utils.CreateCustomerAsync(new Customer(new Name("Carlos", "Oliveira"), new Email("coliveira@example.com"), new Phone("+55 (21) 31345-6789"), "_customer123", EGender.Masculine, DateTime.Now.AddYears(-30), new Address("Brazil", "Brasília", "Quadra 123", 3)));
        var newCategory = await _utils.CreateCategoryAsync(new Category("Deluxe", "Deluxe", 190));
        var newRoom = await _utils.CreateRoomAsync(new Room("Deluxe Beach", 99, 170, 8, "Deluxe beach is a room", newCategory));
        var newReservation = await _utils.CreateReservationAsync(new Reservation(newRoom, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), newCustomer, 4));
        var newService = await _utils.CreateServiceAsync(new Service("Spa", "Spa", 50, EPriority.Medium, 30));
        newReservation.ToCheckIn();
        newReservation.AddService(newService);
        var newInvoice = newReservation.Finish();
        await _utils.CreateInvoiceAsync(newInvoice);

        var parameters = new InvoiceQueryParameters(0, 100, null, 9m, "gt", null, null, null);

        // Act
        var invoices = await _invoiceRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(invoices.Any());
        foreach (var inv in invoices)
        {
            Assert.IsTrue(9 < inv.TotalAmount);
        }
    }

    [TestMethod]
    public async Task GetAsync_WhereTotalAmountLessThan100_ReturnsInvoices()
    {
        // Arrange
        var invoice = await CreateInvoiceAsync();
        await _dbContext.SaveChangesAsync();
        var parameters = new InvoiceQueryParameters(0, 100, null, 100m, "lt", null, null, null);

        // Act
        var invoices = await _invoiceRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(invoices.Any());
        foreach (var inv in invoices)
        {
            Assert.IsTrue(100 > inv.TotalAmount);
        }
    }

    [TestMethod]
    public async Task GetAsync_WhereTotalAmountEquals_ReturnsInvoices()
    {
        // Arrange
        var invoice = await CreateInvoiceAsync();
        var totalAmount = invoice.TotalAmount;
        var parameters = new InvoiceQueryParameters(0, 100, null, totalAmount, "eq", null, null, null);

        // Act
        var invoices = await _invoiceRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(invoices.Any());
        foreach (var inv in invoices)
        {
            Assert.AreEqual(Math.Round(totalAmount), Math.Round(inv.TotalAmount));
        }
    }

    [TestMethod]
    public async Task GetAsync_WhereCustomerId_ReturnsInvoices()
    {
        // Arrange
        var invoice = await CreateInvoiceAsync();
        var parameters = new InvoiceQueryParameters(0, 100, null, null, null, invoice.CustomerId, null, null);

        // Act
        var invoices = await _invoiceRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(invoices.Any());
        foreach (var inv in invoices)
        {
            Assert.AreEqual(invoice.CustomerId, inv.CustomerId);
        }
    }

    [TestMethod]
    public async Task GetAsync_WhereReservationId_ReturnsInvoices()
    {
        // Arrange
        var invoice = await CreateInvoiceAsync();
        var reservationId = invoice.ReservationId;
        var parameters = new InvoiceQueryParameters(0, 100, null, null, null, null, reservationId, null);

        // Act
        var invoices = await _invoiceRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(invoices.Any());
        foreach (var inv in invoices)
        {
            Assert.AreEqual(reservationId, inv.ReservationId);
        }
    }

    [TestMethod]
    public async Task GetAsync_WhereServiceId_ReturnsInvoices()
    {
        // Arrange
        var newCustomer = await _utils.CreateCustomerAsync(new Customer(new Name("Carlos", "Oliveira"), new Email("coliveira@example.com"), new Phone("+55 (21) 31345-6789"), "_customer123", EGender.Masculine, DateTime.Now.AddYears(-30), new Address("Brazil", "Brasília", "Quadra 123", 3)));
        var newCategory = await _utils.CreateCategoryAsync(new Category("Deluxe", "Deluxe", 190));
        var newRoom = await _utils.CreateRoomAsync(new Room("Deluxe Beach", 99, 170, 8, "Deluxe beach is a room", newCategory));
        var newReservation = await _utils.CreateReservationAsync(new Reservation(newRoom, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), newCustomer, 4));
        var newService = await _utils.CreateServiceAsync(new Service("Spa", "Spa", 50, EPriority.Medium, 30));
        newReservation.ToCheckIn();
        newReservation.AddService(newService);
        var newInvoice = newReservation.Finish();
        await _utils.CreateInvoiceAsync(newInvoice);
        var parameters = new InvoiceQueryParameters(0, 100, null, null, null, null, null, newService.Id);

        // Act
        var invoices = await _invoiceRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(invoices.Any());
        foreach (var inv in invoices)
        {
            var hasService = inv.Services.Any(s => s.Id == newService.Id);
            Assert.IsTrue(hasService);
        }
    }
}
