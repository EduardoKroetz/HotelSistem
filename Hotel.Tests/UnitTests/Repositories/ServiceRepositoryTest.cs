using Hotel.Domain.Data;
using Hotel.Domain.DTOs.ServiceDTOs;
using Hotel.Domain.Entities.CategoryEntity;
using Hotel.Domain.Entities.CustomerEntity;
using Hotel.Domain.Entities.ReservationEntity;
using Hotel.Domain.Entities.ResponsibilityEntity;
using Hotel.Domain.Entities.RoomEntity;
using Hotel.Domain.Entities.ServiceEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Repositories;
using Hotel.Domain.ValueObjects;
using Hotel.Tests.UnitTests.Repositories.InMemoryDatabase;
using Hotel.Tests.UnitTests.Repositories.InMemoryDatabase.Utils;
using Hotel.Tests.UnitTests.Repositories.Mock;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel.Tests.UnitTests.Repositories
{
    [TestClass]
    public class ServiceRepositoryTest
    {
        private readonly ServiceRepository _serviceRepository;
        private readonly HotelDbContext _dbContext;
        private readonly RepositoryTestUtils _utils;
        private static readonly AsyncLocal<IDbContextTransaction?> _currentTransaction = new AsyncLocal<IDbContextTransaction?>();

        public ServiceRepositoryTest()
        {
            var sqliteDatabase = new SqliteDatabase();
            _dbContext = sqliteDatabase.Context;
            _serviceRepository = new ServiceRepository(_dbContext);
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
            var newService = await _utils.CreateServiceAsync(new Service("Spa", "Spa", 80, EPriority.Low, 15));

            // Act
            var service = await _serviceRepository.GetByIdAsync(newService.Id);

            // Assert
            Assert.IsNotNull(service);
            Assert.AreEqual(newService.Id, service.Id);
            Assert.AreEqual(newService.Name, service.Name);
            Assert.AreEqual(newService.Price, service.Price);
            Assert.AreEqual(newService.Priority, service.Priority);
            Assert.AreEqual(newService.IsActive, service.IsActive);
            Assert.AreEqual(newService.TimeInMinutes, service.TimeInMinutes);
        }

        [TestMethod]
        public async Task GetAsync_ReturnWithCorrectParameters()
        {
            // Arrange
            var newService = await _utils.CreateServiceAsync(new Service("Spa", "Spa", 80, EPriority.Low, 15));
            var parameters = new ServiceQueryParameters(0, 100, newService.Name, newService.Price, null, null, null, null, null, null, null, null, null, null, null);

            // Act
            var services = await _serviceRepository.GetAsync(parameters);

            // Assert
            Assert.IsNotNull(services);
            var service = services.FirstOrDefault();
            Assert.IsNotNull(service);
            Assert.AreEqual(newService.Id, service.Id);
            Assert.AreEqual(newService.Name, service.Name);
            Assert.AreEqual(newService.Price, service.Price);
            Assert.AreEqual(newService.Priority, service.Priority);
            Assert.AreEqual(newService.IsActive, service.IsActive);
            Assert.AreEqual(newService.TimeInMinutes, service.TimeInMinutes);
        }

        [TestMethod]
        public async Task GetAsync_WhereNameEquals_ReturnServices()
        {
            // Arrange
            var newService = await _utils.CreateServiceAsync(new Service("Spa", "Spa", 80, EPriority.Low, 15));
            var parameters = new ServiceQueryParameters(0, 100, newService.Name, null, null, null, null, null, null, null, null, null, null, null, null);

            // Act
            var services = await _serviceRepository.GetAsync(parameters);

            // Assert
            Assert.IsTrue(services.Any());
            foreach (var service in services)
                Assert.AreEqual(newService.Name, service.Name);
        }

        [TestMethod]
        public async Task GetAsync_WherePriceGratherThan50_ReturnServices()
        {
            // Arrange
            var newService = await _utils.CreateServiceAsync(new Service("Spa", "Spa", 80, EPriority.Low, 15));
            var parameters = new ServiceQueryParameters(0, 100, null, 50m, "gt", null, null, null, null, null, null, null, null, null, null);

            // Act
            var services = await _serviceRepository.GetAsync(parameters);

            // Assert
            Assert.IsTrue(services.Any());
            foreach (var service in services)
                Assert.IsTrue(50 < service.Price);
        }

        [TestMethod]
        public async Task GetAsync_WherePriceLessThan70_ReturnServices()
        {
            // Arrange
            var newService = await _utils.CreateServiceAsync(new Service("Spa", "Spa", 60, EPriority.Low, 15));
            var parameters = new ServiceQueryParameters(0, 100, null, 70m, "lt", null, null, null, null, null, null, null, null, null, null);

            // Act
            var services = await _serviceRepository.GetAsync(parameters);

            // Assert
            Assert.IsTrue(services.Any());
            foreach (var service in services)
                Assert.IsTrue(70 > service.Price);
        }

        [TestMethod]
        public async Task GetAsync_WherePriceEquals70_ReturnServices()
        {
            // Arrange
            var newService = await _utils.CreateServiceAsync(new Service("Spa", "Spa", 70, EPriority.Low, 15));
            var parameters = new ServiceQueryParameters(0, 100, null, 70m, "eq", null, null, null, null, null, null, null, null, null, null);

            // Act
            var services = await _serviceRepository.GetAsync(parameters);

            // Assert
            Assert.IsTrue(services.Any());
            foreach (var service in services)
                Assert.AreEqual(70, service.Price);
        }

        [TestMethod]
        public async Task GetAsync_WherePriorityEqualsMedium_ReturnServices()
        {
            // Arrange
            var newService = await _utils.CreateServiceAsync(new Service("Spa", "Spa", 80, EPriority.Medium, 15));
            var parameters = new ServiceQueryParameters(0, 100, null, null, null, EPriority.Medium, null, null, null, null, null, null, null, null, null);

            // Act
            var services = await _serviceRepository.GetAsync(parameters);

            // Assert
            Assert.IsTrue(services.Any());
            foreach (var service in services)
                Assert.AreEqual(EPriority.Medium, service.Priority);
        }

        [TestMethod]
        public async Task GetAsync_WhereIsActiveEqualsTrue_ReturnServices()
        {
            // Arrange
            var newService = await _utils.CreateServiceAsync(new Service("Spa", "Spa", 80, EPriority.Low, 15));
            var parameters = new ServiceQueryParameters(0, 100, null, null, null, null, true, null, null, null, null, null, null, null, null);

            // Act
            var services = await _serviceRepository.GetAsync(parameters);

            // Assert
            Assert.IsTrue(services.Any());
            foreach (var service in services)
                Assert.IsTrue(service.IsActive);
        }

        [TestMethod]
        public async Task GetAsync_WhereIsActiveEqualsFalse_ReturnServices()
        {
            // Arrange
            var newService = await _utils.CreateServiceAsync(new Service("Spa", "Spa", 80, EPriority.Low, 15));
            newService.Disable();
            await _dbContext.SaveChangesAsync();
            var parameters = new ServiceQueryParameters(0, 100, null, null, null, null, false, null, null, null, null, null, null, null, null);

            // Act
            var services = await _serviceRepository.GetAsync(parameters);

            // Assert
            Assert.IsTrue(services.Any());
            foreach (var service in services)
                Assert.IsFalse(service.IsActive);
        }

        [TestMethod]
        public async Task GetAsync_WhereTimeInMinutesGratherThan30_ReturnServices()
        {
            // Arrange
            var newService = await _utils.CreateServiceAsync(new Service("Spa", "Spa", 80, EPriority.Low, 45));
            var parameters = new ServiceQueryParameters(0, 100, null, null, null, null, null, 30, "gt", null, null, null, null, null, null);

            // Act
            var services = await _serviceRepository.GetAsync(parameters);

            // Assert
            Assert.IsTrue(services.Any());
            foreach (var service in services)
                Assert.IsTrue(30 < service.TimeInMinutes);
        }

        [TestMethod]
        public async Task GetAsync_WhereTimeInMinutesLessThan30_ReturnServices()
        {
            // Arrange
            var newService = await _utils.CreateServiceAsync(new Service("Spa", "Spa", 80, EPriority.Low, 15));
            var parameters = new ServiceQueryParameters(0, 100, null, null, null, null, null, 30, "lt", null, null, null, null, null, null);

            // Act
            var services = await _serviceRepository.GetAsync(parameters);

            // Assert
            Assert.IsTrue(services.Any());
            foreach (var service in services)
                Assert.IsTrue(30 > service.TimeInMinutes);
        }

        [TestMethod]
        public async Task GetAsync_WhereTimeInMinutesEquals30_ReturnServices()
        {
            // Arrange
            var newService = await _utils.CreateServiceAsync(new Service("Spa", "Spa", 80, EPriority.Low, 30));
            var parameters = new ServiceQueryParameters(0, 100, null, null, null, null, null, 30, "eq", null, null, null, null, null, null);

            // Act
            var services = await _serviceRepository.GetAsync(parameters);

            // Assert
            Assert.IsTrue(services.Any());
            foreach (var service in services)
                Assert.AreEqual(30, service.TimeInMinutes);
        }

        [TestMethod]
        public async Task GetAsync_WhereResponsibilityId_ReturnServices()
        {
            // Arrange
            var newService = await _utils.CreateServiceAsync(new Service("Spa", "Spa", 80, EPriority.Low, 15));
            var newResponsibility = await _utils.CreateResponsibilityAsync(new Responsibility("Auxiliar Spa", "Auxiliar spa", EPriority.Low));
            newService.AddResponsibility(newResponsibility);
            await _dbContext.SaveChangesAsync();
            var parameters = new ServiceQueryParameters(0, 100, null, null, null, null, null, null, null, newResponsibility.Id, null, null, null, null, null);

            // Act
            var services = await _serviceRepository.GetAsync(parameters);

            // Assert
            Assert.IsTrue(services.Any());
            foreach (var service in services)
            {
                var hasResponsibility = await _dbContext.Services
                    .Where(s => s.Id == service.Id)
                    .SelectMany(s => s.Responsibilities)
                    .AnyAsync(r => r.Id == newResponsibility.Id);

                Assert.IsTrue(hasResponsibility);
            }
        }

        [TestMethod]
        public async Task GetAsync_WhereReservationId_ReturnServices()
        {
            // Arrange
            var newCustomer = await _utils.CreateCustomerAsync(new Customer(new Name("Carlos", "Oliveira"), new Email("coliveira@example.com"), new Phone("+55 (21) 31345-6789"), "_customer123", EGender.Masculine, DateTime.Now.AddYears(-30), new Address("Brazil", "Brasília", "Quadra 123", 3)));
            var newCategory = await _utils.CreateCategoryAsync(new Category("Deluxe", "Deluxe", 190));
            var newRoom = await _utils.CreateRoomAsync(new Room("Deluxe Beach", 99, 119, 8, "Deluxe beach is a room", newCategory));
            var newReservation = await _utils.CreateReservationAsync(new Reservation(newRoom, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), newCustomer, 4));
            var newService = await _utils.CreateServiceAsync(new Service("Spa", "Spa", 80, EPriority.Low, 15));
            newReservation.ToCheckIn();
            newReservation.AddService(newService);
            await _dbContext.SaveChangesAsync();
            var parameters = new ServiceQueryParameters(0, 100, null, null, null, null, null, null, null, null, newReservation.Id, null, null, null, null);

            // Act
            var services = await _serviceRepository.GetAsync(parameters);

            // Assert
            Assert.IsTrue(services.Any());
            foreach (var service in services)
            {
                var hasReservation = await _dbContext.Services
                    .Where(s => s.Id == service.Id)
                    .SelectMany(s => s.Reservations)
                    .AnyAsync(r => r.Id == newReservation.Id);

                Assert.IsTrue(hasReservation);
            }
        }

        [TestMethod]
        public async Task GetAsync_WhereInvoiceId_ReturnServices()
        {
            // Arrange
            var newCustomer = await _utils.CreateCustomerAsync(new Customer(new Name("Carlos", "Oliveira"), new Email("coliveira@example.com"), new Phone("+55 (21) 31345-6789"), "_customer123", EGender.Masculine, DateTime.Now.AddYears(-30), new Address("Brazil", "Brasília", "Quadra 123", 3)));
            var newCategory = await _utils.CreateCategoryAsync(new Category("Deluxe", "Deluxe", 190));
            var newRoom = await _utils.CreateRoomAsync(new Room("Deluxe Beach", 99, 119, 8, "Deluxe beach is a room", newCategory));
            var newReservation = await _utils.CreateReservationAsync(new Reservation(newRoom, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), newCustomer, 4));
            var newService = await _utils.CreateServiceAsync(new Service("Spa", "Spa", 80, EPriority.Low, 15));
            newReservation.ToCheckIn();
            newReservation.AddService(newService);
            var newInvoice = newReservation.Finish();
            await _dbContext.SaveChangesAsync();
            var parameters = new ServiceQueryParameters(0, 100, null, null, null, null, null, null, null, null, null, newInvoice.Id, null, null, null);

            // Act
            var services = await _serviceRepository.GetAsync(parameters);

            // Assert
            Assert.IsTrue(services.Any());
            foreach (var service in services)
            {
                var hasInvoice = await _dbContext.Services
                    .Where(s => s.Id == service.Id)
                    .SelectMany(s => s.Invoices)
                    .AnyAsync(i => i.Id == newInvoice.Id);

                Assert.IsTrue(hasInvoice);
            }
        }

        [TestMethod]
        public async Task GetAsync_WhereRoomId_ReturnServices()
        {
            // Arrange
            var newCategory = await _utils.CreateCategoryAsync(new Category("Deluxe", "Deluxe", 190));
            var newRoom = await _utils.CreateRoomAsync(new Room("Deluxe Beach", 99, 119, 8, "Deluxe beach is a room", newCategory));
            var newService = await _utils.CreateServiceAsync(new Service("Spa", "Spa", 80, EPriority.Low, 15));
            newRoom.AddService(newService);
            await _dbContext.SaveChangesAsync();
            var parameters = new ServiceQueryParameters(0, 100, null, null, null, null, null, null, null, null, null, null, newRoom.Id, null, null);

            // Act
            var services = await _serviceRepository.GetAsync(parameters);

            // Assert
            Assert.IsTrue(services.Any());
            foreach (var service in services)
            {
                var hasRoom = await _dbContext.Services
                    .Where(s => s.Id == service.Id)
                    .SelectMany(s => s.Rooms)
                    .AnyAsync(r => r.Id == newRoom.Id);

                Assert.IsTrue(hasRoom);
            }
        }
    }
}
