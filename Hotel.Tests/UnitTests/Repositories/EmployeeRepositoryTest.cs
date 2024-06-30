using Hotel.Domain.Data;
using Hotel.Domain.DTOs.EmployeeDTOs;
using Hotel.Domain.Enums;
using Hotel.Domain.Repositories;
using Hotel.Tests.UnitTests.Repositories.InMemoryDatabase.Utils;
using Hotel.Tests.UnitTests.Repositories.InMemoryDatabase;
using Microsoft.EntityFrameworkCore.Storage;
using Hotel.Domain.Entities.EmployeeEntity;
using Hotel.Domain.ValueObjects;

namespace Hotel.Tests.UnitTests.Repositories;

[TestClass]
public class EmployeeRepositoryTest
{
    private readonly EmployeeRepository _employeeRepository;
    private readonly HotelDbContext _dbContext;
    private readonly RepositoryTestUtils _utils;
    private static readonly AsyncLocal<IDbContextTransaction?> _currentTransaction = new AsyncLocal<IDbContextTransaction?>();

    public EmployeeRepositoryTest()
    {
        var sqliteDatabase = new SqliteDatabase();
        _dbContext = sqliteDatabase.Context;
        _employeeRepository = new EmployeeRepository(_dbContext);
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
        var newEmployee = await _utils.CreateEmployeeAsync(new Employee(new Name("Ana", "Santos"), new Email("anasantos@example.com"), new Phone("+55 (31) 98765-9121"), "ana456", EGender.Feminine, DateTime.Now.AddYears(-28), new Address("Brazil", "Belo Horizonte", "Avenida Afonso Pena", 456), 400));

        // Act
        var employee = await _employeeRepository.GetByIdAsync(newEmployee.Id);

        // Assert
        Assert.IsNotNull(employee);
        Assert.AreEqual(newEmployee.Id, employee.Id);
        Assert.AreEqual(newEmployee.Name.FirstName, employee.FirstName);
        Assert.AreEqual(newEmployee.Name.LastName, employee.LastName);
        Assert.AreEqual(newEmployee.Email.Address, employee.Email);
        Assert.AreEqual(newEmployee.Phone.Number, employee.Phone);
        Assert.AreEqual(newEmployee.Salary, employee.Salary);
    }

    [TestMethod]
    public async Task GetAsync_ReturnWithCorrectParameters()
    {
        // Arrange
        var newEmployee = await _utils.CreateEmployeeAsync(new Employee(new Name("Lucas", "Silva"), new Email("lucassilva@example.com"), new Phone("+55 (21) 99876-5432"), "lucas123", EGender.Masculine, DateTime.Now.AddYears(-35), new Address("Brazil", "Rio de Janeiro", "Rua das Laranjeiras", 123)));
        var parameters = new EmployeeQueryParameters(0, 100, newEmployee.Name.FirstName, null, null, null, null, null, null, null, null, null);

        // Act
        var employees = await _employeeRepository.GetAsync(parameters);
        var employee = employees.FirstOrDefault();

        // Assert
        Assert.IsNotNull(employee);
        Assert.AreEqual(newEmployee.Name.FirstName, employee.FirstName);
        Assert.AreEqual(newEmployee.Name.LastName, employee.LastName);
        Assert.AreEqual(newEmployee.Email.Address, employee.Email);
        Assert.AreEqual(newEmployee.Phone.Number, employee.Phone);
        Assert.AreEqual(newEmployee.Id, employee.Id);
    }

    [TestMethod]
    public async Task GetAsync_WhereNameIncludesLucas_ReturnsEmployees()
    {
        // Arrange
        await _utils.CreateEmployeeAsync(new Employee(new Name("Lucas", "Souza"), new Email("lucassouza@example.com"), new Phone("+55 (11) 98765-4321"), "lucas789", EGender.Masculine, DateTime.Now.AddYears(-25), new Address("Brazil", "São Paulo", "Avenida Paulista", 789)));
        var parameters = new EmployeeQueryParameters(0, 100, "Lucas", null, null, null, null, null, null, null, null, null);

        // Act
        var employees = await _employeeRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(employees.Any());
        foreach (var employee in employees)
            Assert.IsTrue(employee.FirstName.Contains("Lucas"));
    }

    [TestMethod]
    public async Task GetAsync_WhereEmailContainsCom_ReturnsEmployees()
    {
        // Arrange
        await _utils.CreateEmployeeAsync(new Employee(new Name("Maria", "Fernandes"), new Email("maria.fernandes@example.com"), new Phone("+55 (21) 97654-3210"), "maria123", EGender.Feminine, DateTime.Now.AddYears(-30), new Address("Brazil", "Rio de Janeiro", "Rua das Laranjeiras", 321)));
        var parameters = new EmployeeQueryParameters(0, 100, null, ".com", null, null, null, null, null, null, null, null);

        // Act
        var employees = await _employeeRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(employees.Any());
        foreach (var employee in employees)
            Assert.IsTrue(employee.Email.Contains(".com"));
    }

    [TestMethod]
    public async Task GetAsync_WherePhoneContains55_ReturnsEmployees()
    {
        // Arrange
        await _utils.CreateEmployeeAsync(new Employee(new Name("João", "Pereira"), new Email("joao.pereira@example.com"), new Phone("+55 (41) 98765-4321"), "joao123", EGender.Masculine, DateTime.Now.AddYears(-27), new Address("Brazil", "Curitiba", "Rua das Flores", 123)));
        var parameters = new EmployeeQueryParameters(0, 100, null, null, "55", null, null, null, null, null, null, null);

        // Act
        var employees = await _employeeRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(employees.Any());
        foreach (var employee in employees)
            Assert.IsTrue(employee.Phone.Contains("55"));
    }

    [TestMethod]
    public async Task GetAsync_WhereGenderEqualsMasculine_ReturnsEmployees()
    {
        // Arrange
        await _utils.CreateEmployeeAsync(new Employee(new Name("Pedro", "Silva"), new Email("pedro.silva@example.com"), new Phone("+55 (21) 99876-5432"), "pedro123", EGender.Masculine, DateTime.Now.AddYears(-40), new Address("Brazil", "Rio de Janeiro", "Rua das Laranjeiras", 123)));
        var parameters = new EmployeeQueryParameters(0, 100, null, null, null, EGender.Masculine, null, null, null, null, null, null);

        // Act
        var employees = await _employeeRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(employees.Any());
        foreach (var employee in employees)
            Assert.AreEqual(EGender.Masculine, employee.Gender);
    }

    [TestMethod]
    public async Task GetAsync_WhereDateOfBirthGreaterThan2000_ReturnsEmployees()
    {
        // Arrange
        await _utils.CreateEmployeeAsync(new Employee(new Name("Mariana", "Gomes"), new Email("mariana.gomes@example.com"), new Phone("+55 (21) 98765-4321"), "mariana123", EGender.Feminine, DateTime.Now.AddYears(-23), new Address("Brazil", "Rio de Janeiro", "Rua das Laranjeiras", 123)));
        var parameters = new EmployeeQueryParameters(0, 100, null, null, null, null, DateTime.Now.AddYears(-24), "gt", null, null, null, null);

        // Act
        var employees = await _employeeRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(employees.Any());
        foreach (var employee in employees)
            Assert.IsTrue(DateTime.Now.AddYears(-24) < employee.DateOfBirth);
    }

    [TestMethod]
    public async Task GetAsync_WhereCreatedAtGreaterThanYesterday_ReturnsEmployees()
    {
        // Arrange
        var newEmployee = await _utils.CreateEmployeeAsync(new Employee(new Name("Fernanda", "Souza"), new Email("fernanda.souza@example.com"), new Phone("+55 (21) 98765-4321"), "fernanda123", EGender.Feminine, DateTime.Now.AddYears(-30), new Address("Brazil", "Rio de Janeiro", "Rua das Laranjeiras", 123)));
        var parameters = new EmployeeQueryParameters(0, 100, null, null, null, null, null, null, DateTime.Now.AddDays(-1), "gt", null, null);

        // Act
        var employees = await _employeeRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(employees.Any());
        foreach (var employee in employees)
            Assert.IsTrue(DateTime.Now.AddDays(-1) < employee.CreatedAt);
    }

    [TestMethod]
    public async Task GetAsync_WhereCreatedAtLessThanToday_ReturnsEmployees()
    {
        // Arrange
        var newEmployee = await _utils.CreateEmployeeAsync(new Employee(new Name("Rafael", "Mendes"), new Email("rafael.mendes@example.com"), new Phone("+55 (21) 98765-4321"), "rafael123", EGender.Masculine, DateTime.Now.AddYears(-29), new Address("Brazil", "Rio de Janeiro", "Rua das Laranjeiras", 123)));
        var parameters = new EmployeeQueryParameters(0, 100, null, null, null, null, null, null, DateTime.Now.AddDays(1), "lt", null, null);

        // Act
        var employees = await _employeeRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(employees.Any());
        foreach (var employee in employees)
            Assert.IsTrue(DateTime.Now.AddDays(1) > employee.CreatedAt);
    }

    [TestMethod]
    public async Task GetAsync_WhereCreatedAtEquals_ReturnsEmployees()
    {
        // Arrange
        var newEmployee = await _utils.CreateEmployeeAsync(new Employee(new Name("Roberto", "Lima"), new Email("roberto.lima@example.com"), new Phone("+55 (21) 98765-4321"), "roberto123", EGender.Masculine, DateTime.Now.AddYears(-33), new Address("Brazil", "Rio de Janeiro", "Rua das Laranjeiras", 123)));
        var parameters = new EmployeeQueryParameters(0, 100, null, null, null, null, null, null, newEmployee.CreatedAt, "eq", null, null);

        // Act
        var employees = await _employeeRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(employees.Any());
        foreach (var employee in employees)
            Assert.AreEqual(newEmployee.CreatedAt, employee.CreatedAt);
    }

    [TestMethod]
    public async Task GetAsync_WhereEmailIncludesExample_And_PhoneIncludes55_And_GenderEqualsMasculine_ReturnsEmployees()
    {
        // Arrange
        await _utils.CreateEmployeeAsync(new Employee(new Name("Carlos", "Almeida"), new Email("carlos.almeida@example.com"), new Phone("+55 (21) 98765-4321"), "carlos123", EGender.Masculine, DateTime.Now.AddYears(-40), new Address("Brazil", "Rio de Janeiro", "Rua das Laranjeiras", 123)));
        var parameters = new EmployeeQueryParameters(0, 100, null, "example", "55", EGender.Masculine, null, null, null, null, null, null);

        // Act
        var employees = await _employeeRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(employees.Any());

        foreach (var employee in employees)
        {
            Assert.IsTrue(employee.Email.Contains("example"));
            Assert.IsTrue(employee.Phone.Contains("55"));
            Assert.AreEqual(EGender.Masculine, employee.Gender);
        }
    }

    [TestMethod]
    public async Task GetAsync_WhereNameIncludesR_And_DateOfBirthLessThan31Years_And_CreatedAtLessThanTomorrow_ReturnsEmployees()
    {
        // Arrange
        await _utils.CreateEmployeeAsync(new Employee(new Name("Roberto", "Oliveira"), new Email("coliveira@example.com"), new Phone("+55 (21) 31345-6789"), "_customer123", EGender.Masculine, DateTime.Now.AddYears(-33), new Address("Brazil", "Brasília", "Quadra 123", 3)));
        var parameters = new EmployeeQueryParameters(0, 100, "R", null, null, null, DateTime.Now.AddYears(-31), "lt", DateTime.Now.AddDays(1), "lt", null, null);

        // Act
        var employees = await _employeeRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(employees.Any());

        foreach (var employee in employees)
        {
            Assert.IsTrue(employee.FirstName.Contains('R'));
            Assert.IsTrue(DateTime.Now.AddYears(-31) > employee.DateOfBirth);
            Assert.IsTrue(DateTime.Now.AddDays(1) > employee.CreatedAt);
        }
    }

    [TestMethod]
    public async Task GetAsync_WhereSalaryGreaterThan2000_ReturnsEmployees()
    {
        // Arrange
        await _utils.CreateEmployeeAsync(new Employee(new Name("Paulo", "Moreira"), new Email("paulo.moreira@example.com"), new Phone("+55 (21) 98765-4321"), "paulo123", EGender.Masculine, DateTime.Now.AddYears(-35), new Address("Brazil", "Rio de Janeiro", "Rua das Laranjeiras", 123), 2500m));
        var parameters = new EmployeeQueryParameters(0, 100, null, null, null, null, null, null, null, null, 2000m, "gt");

        // Act
        var employees = await _employeeRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(employees.Any());

        foreach (var employee in employees)
            Assert.IsTrue(2000m < employee.Salary);
    }

    [TestMethod]
    public async Task GetAsync_WhereSalaryLessThan1500_ReturnsEmployees()
    {
        // Arrange
        await _utils.CreateEmployeeAsync(new Employee(new Name("Julia", "Martins"), new Email("julia.martins@example.com"), new Phone("+55 (21) 98765-4321"), "julia123", EGender.Feminine, DateTime.Now.AddYears(-28), new Address("Brazil", "Rio de Janeiro", "Rua das Laranjeiras", 123), 1400m));
        var parameters = new EmployeeQueryParameters(0, 100, null, null, null, null, null, null, null, null, 1500m, "lt");

        // Act
        var employees = await _employeeRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(employees.Any());

        foreach (var employee in employees)
            Assert.IsTrue(1500m > employee.Salary);
    }

    [TestMethod]
    public async Task GetAsync_WhereSalaryEquals1400_ReturnsEmployees()
    {
        // Arrange
        await _utils.CreateEmployeeAsync(new Employee(new Name("Bruna", "Oliveira"), new Email("bruna.oliveira@example.com"), new Phone("+55 (21) 98765-4321"), "bruna123", EGender.Feminine, DateTime.Now.AddYears(-32), new Address("Brazil", "Rio de Janeiro", "Rua das Laranjeiras", 123), 1400m));
        var parameters = new EmployeeQueryParameters(0, 100, null, null, null, null, null, null, null, null, 1400m, "eq");

        // Act
        var employees = await _employeeRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(employees.Any());
        foreach (var employee in employees)
            Assert.AreEqual(1400m, employee.Salary);
    }
}
