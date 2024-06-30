using Hotel.Domain.Data;
using Hotel.Domain.DTOs.ResponsibilityDTOs;
using Hotel.Domain.Enums;
using Hotel.Domain.Repositories;
using Hotel.Tests.UnitTests.Repositories.InMemoryDatabase.Utils;
using Hotel.Tests.UnitTests.Repositories.InMemoryDatabase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Hotel.Domain.Entities.ResponsibilityEntity;
using Hotel.Domain.Entities.EmployeeEntity;
using Hotel.Domain.ValueObjects;
using Hotel.Domain.Entities.ServiceEntity;

namespace Hotel.Tests.UnitTests.Repositories;

[TestClass]
public class ResponsibilityRepositoryTest
{
    private readonly ResponsibilityRepository _responsibilityRepository;
    private readonly HotelDbContext _dbContext;
    private readonly RepositoryTestUtils _utils;
    private static readonly AsyncLocal<IDbContextTransaction?> _currentTransaction = new AsyncLocal<IDbContextTransaction?>();

    public ResponsibilityRepositoryTest()
    {
        var sqliteDatabase = new SqliteDatabase();
        _dbContext = sqliteDatabase.Context;
        _responsibilityRepository = new ResponsibilityRepository(_dbContext);
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
        //Arrange
        var newResponsibility = await _utils.CreateResponsibilityAsync(new Responsibility("Limpeza", "Limpeza", EPriority.Medium));

        //Act
        var responsibility = await _responsibilityRepository.GetByIdAsync(newResponsibility.Id);

        //Assert
        Assert.IsNotNull(responsibility);
        Assert.AreEqual(newResponsibility.Id, responsibility.Id);
        Assert.AreEqual(newResponsibility.Name, responsibility.Name);
        Assert.AreEqual(newResponsibility.Priority, responsibility.Priority);
        Assert.AreEqual(newResponsibility.Description, responsibility.Description);
    }

    [TestMethod]
    public async Task GetAsync_ReturnWithCorrectParameters()
    {
        //Arrange
        var newResponsibility = await _utils.CreateResponsibilityAsync(new Responsibility("Limpeza", "Limpeza", EPriority.Medium));

        //Act
        var parameters = new ResponsibilityQueryParameters { Name = newResponsibility.Name };
        var responsibilities = await _responsibilityRepository.GetAsync(parameters);

        var responsibility = responsibilities.ToList()[0];

        //Assert
        Assert.IsNotNull(responsibility);
        Assert.AreEqual(newResponsibility.Id, responsibility.Id);
        Assert.AreEqual(newResponsibility.Name, responsibility.Name);
        Assert.AreEqual(newResponsibility.Priority, responsibility.Priority);
        Assert.AreEqual(newResponsibility.Description, responsibility.Description);
    }

    [TestMethod]
    public async Task GetAsync_WhereNameIncludesPlanejar_ReturnsResponsibilities()
    {
        //Arrange
        await _utils.CreateResponsibilityAsync(new Responsibility("Planejar Limpeza", "Planejar a limpeza dos quartos", EPriority.Medium));

        //Act
        var parameters = new ResponsibilityQueryParameters { Name = "Planejar" };
        var responsibilities = await _responsibilityRepository.GetAsync(parameters);

        //Assert
        Assert.IsTrue(responsibilities.Any());
        foreach (var responsibility in responsibilities)
            Assert.IsTrue(responsibility.Name.Contains("Planejar"));
    }

    [TestMethod]
    public async Task GetAsync_WherePriorityEqualsLow_ReturnsResponsibilities()
    {
        //Arrange
        await _utils.CreateResponsibilityAsync(new Responsibility("Limpeza Básica", "Limpeza básica dos corredores", EPriority.Low));

        //Act
        var parameters = new ResponsibilityQueryParameters { Priority = EPriority.Low };
        var responsibilities = await _responsibilityRepository.GetAsync(parameters);

        //Assert
        Assert.IsTrue(responsibilities.Any());
        foreach (var responsibility in responsibilities)
            Assert.AreEqual(EPriority.Low, responsibility.Priority);
    }

    [TestMethod]
    public async Task GetAsync_WhereEmployeeId_ReturnsResponsibilities()
    {
        //Arrange
        var newResponsibility = await _utils.CreateResponsibilityAsync(new Responsibility("Limpeza", "Limpeza", EPriority.Medium));
        var newEmployee = await _utils.CreateEmployeeAsync(new Employee(new Name("Ana", "Santos"), new Email("anasantos@example.com"), new Phone("+55 (31) 98765-9121"), "ana456", EGender.Feminine, DateTime.Now.AddYears(-28), new Address("Brazil", "Belo Horizonte", "Avenida Afonso Pena", 456), 400));
        var res = await _responsibilityRepository.GetEntityByIdAsync(newResponsibility.Id);
        res?.Employees.Add(newEmployee);
        await _dbContext.SaveChangesAsync();

        //Act
        var parameters = new ResponsibilityQueryParameters { EmployeeId = newEmployee.Id };
        var responsibilities = await _responsibilityRepository.GetAsync(parameters);

        //Assert
        Assert.IsTrue(responsibilities.Any());
        foreach (var responsibility in responsibilities)
        {
            var hasEmployee = await _dbContext.Responsibilities
                .Where(x => x.Id == responsibility.Id)
                .SelectMany(x => x.Employees)
                .AnyAsync(x => x.Id == newEmployee.Id);

            Assert.IsTrue(hasEmployee);
        }
    }

    [TestMethod]
    public async Task GetAsync_WhereServiceId_ReturnsResponsibilities()
    {
        //Arrange
        var newResponsibility = await _utils.CreateResponsibilityAsync(new Responsibility("Limpeza", "Limpeza", EPriority.Medium));
        var newService = await _utils.CreateServiceAsync(new Service("Spa", "Spa", 50, EPriority.Medium, 30));
        newResponsibility.Services.Add(newService);
        await _dbContext.SaveChangesAsync();

        //Act
        var parameters = new ResponsibilityQueryParameters { ServiceId = newService.Id };
        var responsibilities = await _responsibilityRepository.GetAsync(parameters);

        //Assert
        Assert.IsTrue(responsibilities.Any());
        foreach (var responsibility in responsibilities)
        {
            var hasService = await _dbContext.Responsibilities
                .Where(x => x.Id == responsibility.Id)
                .SelectMany(x => x.Services)
                .AnyAsync(x => x.Id == newService.Id);

            Assert.IsTrue(hasService);
        }
    }

    [TestMethod]
    public async Task GetAsync_WhereCreatedAtGratherThanYesterday_ReturnsResponsibilities()
    {
        //Arrange
        await _utils.CreateResponsibilityAsync(new Responsibility("Limpeza", "Limpeza", EPriority.Medium));

        //Act
        var parameters = new ResponsibilityQueryParameters { CreatedAt = DateTime.Now.AddDays(-1), CreatedAtOperator = "gt" };
        var responsibilities = await _responsibilityRepository.GetAsync(parameters);

        //Assert
        Assert.IsTrue(responsibilities.Any());
        foreach (var responsibility in responsibilities)
            Assert.IsTrue(DateTime.Now.AddDays(-1) < responsibility.CreatedAt);
    }

    [TestMethod]
    public async Task GetAsync_WhereCreatedAtLessThanToday_ReturnsResponsibilities()
    {
        //Arrange
        await _utils.CreateResponsibilityAsync(new Responsibility("Limpeza", "Limpeza", EPriority.Medium));

        //Act
        var parameters = new ResponsibilityQueryParameters { CreatedAt = DateTime.Now, CreatedAtOperator = "lt" };
        var responsibilities = await _responsibilityRepository.GetAsync(parameters);

        //Assert
        Assert.IsTrue(responsibilities.Any());
        foreach (var responsibility in responsibilities)
            Assert.IsTrue(DateTime.Now > responsibility.CreatedAt);
    }

    [TestMethod]
    public async Task GetAsync_WhereNameIncludesOrganizar_And_PriorityEqualsMedium_ReturnsResponsibilities()
    {
        //Arrange
        await _utils.CreateResponsibilityAsync(new Responsibility("Organizar Limpeza", "Organizar a limpeza dos quartos", EPriority.Medium));

        //Act
        var parameters = new ResponsibilityQueryParameters { Name = "Organizar", Priority = EPriority.Medium };
        var responsibilities = await _responsibilityRepository.GetAsync(parameters);

        //Assert
        Assert.IsTrue(responsibilities.Any());

        foreach (var responsibility in responsibilities)
        {
            Assert.IsNotNull(responsibility);
            Assert.IsTrue(responsibility.Name.Contains("Organizar"));
            Assert.AreEqual(EPriority.Medium, responsibility.Priority);
        }
    }
}
