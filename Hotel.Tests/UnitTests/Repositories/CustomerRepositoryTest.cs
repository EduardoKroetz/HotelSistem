using Hotel.Domain.Data;
using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.Enums;
using Hotel.Domain.Repositories;
using Hotel.Tests.UnitTests.Repositories.InMemoryDatabase.Utils;
using Hotel.Tests.UnitTests.Repositories.InMemoryDatabase;
using Microsoft.EntityFrameworkCore.Storage;
using Hotel.Domain.Entities.CustomerEntity;
using Hotel.Domain.ValueObjects;

namespace Hotel.Tests.UnitTests.Repositories;

[TestClass]
public class CustomerRepositoryTest
{
    private readonly CustomerRepository _customerRepository;
    private readonly HotelDbContext _dbContext;
    private readonly RepositoryTestUtils _utils;
    private static readonly AsyncLocal<IDbContextTransaction?> _currentTransaction = new AsyncLocal<IDbContextTransaction?>();

    public CustomerRepositoryTest()
    {
        var sqliteDatabase = new SqliteDatabase();
        _dbContext = sqliteDatabase.Context;
        _customerRepository = new CustomerRepository(_dbContext);
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
        var newCustomer = await _utils.CreateCustomerAsync(new Customer(new Name("Carlos", "Oliveira"), new Email("coliveira@example.com"), new Phone("+55 (21) 31345-6789"), "_customer123", EGender.Masculine, DateTime.Now.AddYears(-30), new Address("Brazil", "Brasília", "Quadra 123", 3)));

        // Act
        var customer = await _customerRepository.GetByIdAsync(newCustomer.Id);

        // Assert
        Assert.IsNotNull(customer);
        Assert.AreEqual(newCustomer.Name.FirstName, customer.FirstName);
        Assert.AreEqual(newCustomer.Name.LastName, customer.LastName);
        Assert.AreEqual(newCustomer.Email.Address, customer.Email);
        Assert.AreEqual(newCustomer.Phone.Number, customer.Phone);
        Assert.AreEqual(newCustomer.Id, customer.Id);
    }

    [TestMethod]
    public async Task GetAsync_ReturnWithCorrectParameters()
    {
        // Arrange
        var newCustomer = await _utils.CreateCustomerAsync(new Customer(new Name("Carlos", "Oliveira"), new Email("coliveira@example.com"), new Phone("+55 (21) 31345-6789"), "_customer123", EGender.Masculine, DateTime.Now.AddYears(-30), new Address("Brazil", "Brasília", "Quadra 123", 3)));
        var parameters = new UserQueryParameters { Name = newCustomer.Name.FirstName };

        // Act
        var customers = await _customerRepository.GetAsync(parameters);
        var customer = customers.ToList()[0];

        // Assert
        Assert.IsNotNull(customer);
        Assert.AreEqual(newCustomer.Name.FirstName, customer.FirstName);
        Assert.AreEqual(newCustomer.Name.LastName, customer.LastName);
        Assert.AreEqual(newCustomer.Email.Address, customer.Email);
        Assert.AreEqual(newCustomer.Phone.Number, customer.Phone);
        Assert.AreEqual(newCustomer.Id, customer.Id);
    }

    [TestMethod]
    public async Task GetAsync_WhereNameIncludesJoao_ReturnsCustomers()
    {
        // Arrange
        var newCustomer = await _utils.CreateCustomerAsync(new Customer(new Name("João", "Silva"), new Email("jsilva@example.com"), new Phone("+55 (21) 31345-6789"), "_customer123", EGender.Masculine, DateTime.Now.AddYears(-25), new Address("Brazil", "Rio de Janeiro", "Rua 456", 7)));
        var parameters = new UserQueryParameters { Name = "João" };

        // Act
        var customers = await _customerRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(customers.Any());
        foreach (var customer in customers)
            Assert.IsTrue(customer.FirstName.Contains("João"));
    }

    [TestMethod]
    public async Task GetAsync_WhereEmailContainsCom_ReturnsCustomers()
    {
        // Arrange
        var newCustomer = await _utils.CreateCustomerAsync(new Customer(new Name("Carlos", "Oliveira"), new Email("coliveira@example.com"), new Phone("+55 (21) 31345-6789"), "_customer123", EGender.Masculine, DateTime.Now.AddYears(-30), new Address("Brazil", "Brasília", "Quadra 123", 3)));
        var parameters = new UserQueryParameters { Email = ".com", };

        // Act
        var customers = await _customerRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(customers.Any());
        foreach (var customer in customers)
            Assert.IsTrue(customer.Email.Contains(".com"));
    }

    [TestMethod]
    public async Task GetAsync_WherePhoneContains55_ReturnsCustomers()
    {
        // Arrange
        var newCustomer = await _utils.CreateCustomerAsync(new Customer(new Name("Carlos", "Oliveira"), new Email("coliveira@example.com"), new Phone("+55 (21) 31345-6789"), "_customer123", EGender.Masculine, DateTime.Now.AddYears(-30), new Address("Brazil", "Brasília", "Quadra 123", 3)));
        var parameters = new UserQueryParameters { Phone = "55" };

        // Act
        var customers = await _customerRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(customers.Any());
        foreach (var customer in customers)
            Assert.IsTrue(customer.Phone.Contains("55"));
    }

    [TestMethod]
    public async Task GetAsync_WhereGenderEqualsMasculine_ReturnsCustomers()
    {
        // Arrange
        var newCustomer = await _utils.CreateCustomerAsync(new Customer(new Name("Carlos", "Oliveira"), new Email("coliveira@example.com"), new Phone("+55 (21) 31345-6789"), "_customer123", EGender.Masculine, DateTime.Now.AddYears(-30), new Address("Brazil", "Brasília", "Quadra 123", 3)));
        var parameters = new UserQueryParameters { Gender =  EGender.Masculine };

        // Act
        var customers = await _customerRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(customers.Any());
        foreach (var customer in customers)
            Assert.AreEqual(EGender.Masculine, customer.Gender);
    }

    [TestMethod]
    public async Task GetAsync_WhereDateOfBirthGratherThan2000_ReturnsCustomers()
    {
        // Arrange
        var newCustomer = await _utils.CreateCustomerAsync(new Customer(new Name("Carlos", "Oliveira"), new Email("coliveira@example.com"), new Phone("+55 (21) 31345-6789"), "_customer123", EGender.Masculine, DateTime.Now.AddYears(-20), new Address("Brazil", "Brasília", "Quadra 123", 3)));
        var parameters = new UserQueryParameters {  DateOfBirth = DateTime.Now.AddYears(-24), DateOfBirthOperator = "gt", };

        // Act
        var customers = await _customerRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(customers.Any());
        foreach (var customer in customers)
            Assert.IsTrue(DateTime.Now.AddYears(-24) < customer.DateOfBirth);
    }

    [TestMethod]
    public async Task GetAsync_WhereCreatedAtGratherThanYesterday_ReturnsCustomers()
    {
        // Arrange
        var newCustomer = await _utils.CreateCustomerAsync(new Customer(new Name("Carlos", "Oliveira"), new Email("coliveira@example.com"), new Phone("+55 (21) 31345-6789"), "_customer123", EGender.Masculine, DateTime.Now.AddYears(-30), new Address("Brazil", "Brasília", "Quadra 123", 3)));
        var parameters = new UserQueryParameters { CreatedAt = DateTime.Now.AddDays(-1), CreatedAtOperator = "gt" };

        // Act
        var customers = await _customerRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(customers.Any());
        foreach (var customer in customers)
            Assert.IsTrue(DateTime.Now.AddDays(-1) < customer.CreatedAt);
    }

    [TestMethod]
    public async Task GetAsync_WhereCreatedAtLessThanToday_ReturnsCustomers()
    {
        // Arrange
        var newCustomer = await _utils.CreateCustomerAsync(new Customer(new Name("Carlos", "Oliveira"), new Email("coliveira@example.com"), new Phone("+55 (21) 31345-6789"), "_customer123", EGender.Masculine, DateTime.Now.AddYears(-30), new Address("Brazil", "Brasília", "Quadra 123", 3)));
        var parameters = new UserQueryParameters { CreatedAt = DateTime.Now.AddDays(1), CreatedAtOperator = "lt" };

        // Act
        var customers = await _customerRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(customers.Any());
        foreach (var customer in customers)
            Assert.IsTrue(DateTime.Now.AddDays(1) > customer.CreatedAt);
    }

    [TestMethod]
    public async Task GetAsync_WhereCreatedAtEquals_ReturnsCustomers()
    {
        // Arrange
        var newCustomer = await _utils.CreateCustomerAsync(new Customer(new Name("Carlos", "Oliveira"), new Email("coliveira@example.com"), new Phone("+55 (21) 31345-6789"), "_customer123", EGender.Masculine, DateTime.Now.AddYears(-30), new Address("Brazil", "Brasília", "Quadra 123", 3)));
        var parameters = new UserQueryParameters { CreatedAt = newCustomer.CreatedAt, CreatedAtOperator = "eq" };

        // Act
        var customers = await _customerRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(customers.Any());
        foreach (var customer in customers)
            Assert.AreEqual(newCustomer.CreatedAt, customer.CreatedAt);
    }

    [TestMethod]
    public async Task GetAsync_WhereEmailIncludesExample_And_PhoneIncludes55_And_GenderEqualsMasculine_ReturnsCustomers()
    {
        // Arrange
        var newCustomer = await _utils.CreateCustomerAsync(new Customer(new Name("Carlos", "Oliveira"), new Email("coliveira@example.com"), new Phone("+55 (21) 31345-6789"), "_customer123", EGender.Masculine, DateTime.Now.AddYears(-30), new Address("Brazil", "Brasília", "Quadra 123", 3)));
        var parameters = new UserQueryParameters { Email = "example", Phone = "55", Gender = EGender.Masculine };

        // Act
        var customers = await _customerRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(customers.Any());
        foreach (var customer in customers)
        {
            Assert.IsTrue(customer.Email.Contains("example"));
            Assert.IsTrue(customer.Phone.Contains("55"));
            Assert.AreEqual(EGender.Masculine, customer.Gender);
        }
    }

    [TestMethod]
    public async Task GetAsync_WhereNameIncludesR_And_DateOfBirthLessThan31Years_And_CreatedAtLessThanTomorrow_ReturnsCustomers()
    {
        // Arrange
        var newCustomer = await _utils.CreateCustomerAsync(new Customer(new Name("Roberto", "Oliveira"), new Email("coliveira@example.com"), new Phone("+55 (21) 31345-6789"), "_customer123", EGender.Masculine, DateTime.Now.AddYears(-33), new Address("Brazil", "Brasília", "Quadra 123", 3)));
        var parameters = new UserQueryParameters { Name = "R", DateOfBirth = DateTime.Now.AddYears(-31), DateOfBirthOperator = "lt", CreatedAt = DateTime.Now.AddDays(1), CreatedAtOperator = "lt" };

        // Act
        var customers = await _customerRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(customers.Any());
        foreach (var customer in customers)
        {
            Assert.IsTrue(customer.FirstName.Contains('R'));
            Assert.IsTrue(DateTime.Now.AddYears(-31) > customer.DateOfBirth);
            Assert.IsTrue(DateTime.Now.AddDays(1) > customer.CreatedAt);
        }
    }
}
