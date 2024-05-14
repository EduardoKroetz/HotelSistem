using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.DTOs.EmployeeContext.EmployeeDTOs;
using Hotel.Domain.Entities.CustomerContext;
using Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Repositories.EmployeeContext;
using Hotel.Domain.ValueObjects;

namespace Hotel.Tests.Repositories.CustomerContext;

[TestClass]
public class EmployeeRepositoryTest : BaseRepositoryTest
{
  private List<Employee> _employees { get; set; } = [];
  private Employee _defaultEmployee { get; set; } = null!;
  private EmployeeRepository _employeeRepository { get; set; } = null!;

  [TestInitialize]
  public async Task StartupTest()
  {
    await Startup(null);
    _employeeRepository = new EmployeeRepository(mockConnection.Context);

    _defaultEmployee = new Employee(new Name("João", "Pedro"), new Email("joaopedro@gmail.com"), new Phone("+55 (55) 33112-3456"), "1234", EGender.Masculine, DateTime.Now.AddYears(-20), new Address("Brazil", "São Paulo", "Avenida Paulista", 999), 1750m);

    _employees.AddRange(
    [
      _defaultEmployee,
      new Employee(new Name("Maria", "Silva"), new Email("maria.silva@example.com"), new Phone("+55 (11) 98765-4321"), "senha123", EGender.Feminine, DateTime.Now.AddYears(-25), new Address("Brazil", "Rio de Janeiro", "Rua Copacabana", 123),800m),
      new Employee(new Name("Carlos", "Oliveira"), new Email("coliveira@example.com"), new Phone("+55 (21) 12345-6789"), "_customer123", EGender.Masculine, DateTime.Now.AddYears(-30), new Address("Brazil", "Brasília", "Quadra 123", 3),3000m),
      new Employee(new Name("Ana", "Santos"), new Email("anasantos@example.com"), new Phone("+55 (31) 98765-4321"), "ana456", EGender.Feminine, DateTime.Now.AddYears(-28), new Address("Brazil", "Belo Horizonte", "Avenida Afonso Pena", 456),1400m),
      new Employee(new Name("Rafael", "Silveira"), new Email("rafaelsilveira@example.com"), new Phone("+55 (19) 98765-4321"), "rafa789", EGender.Masculine, DateTime.Now.AddYears(-32), new Address("Brazil", "Campinas", "Rua Barão de Jaguara", 789),2400m)
    ]);

    await mockConnection.Context.Employees.AddRangeAsync(_employees);
    await mockConnection.Context.SaveChangesAsync();
  }

  [TestCleanup]
  public async Task CleanupTest()
  {
    mockConnection.Context.Employees.RemoveRange(_employees);
    await mockConnection.Context.SaveChangesAsync();
    _employees.Clear();
  }

  [TestMethod]
  public async Task GetByIdAsync_ReturnsWithCorrectParameters()
  {
    var employee = await _employeeRepository.GetByIdAsync(_defaultEmployee.Id);

    Assert.IsNotNull(employee);
    Assert.AreEqual(_defaultEmployee.Id, employee.Id);
    Assert.AreEqual(_defaultEmployee.Name.FirstName, employee.FirstName);
    Assert.AreEqual(_defaultEmployee.Name.LastName, employee.LastName);
    Assert.AreEqual(_defaultEmployee.Email.Address, employee.Email);
    Assert.AreEqual(_defaultEmployee.Phone.Number, employee.Phone);
    Assert.AreEqual(_defaultEmployee.Salary, employee.Salary);

  }

  [TestMethod]
  public async Task GetAsync_ReturnWithCorrectParameters()
  {
    var parameters = new EmployeeQueryParameters(0, 100, "João", null, null, null, null, null, null, null, null, null);
    var employees = await _employeeRepository.GetAsync(parameters);

    var employee = employees.ToList()[0];

    Assert.IsNotNull(employee);
    Assert.AreEqual(_defaultEmployee.Name.FirstName, employee.FirstName);
    Assert.AreEqual(_defaultEmployee.Name.LastName, employee.LastName);
    Assert.AreEqual(_defaultEmployee.Email.Address, employee.Email);
    Assert.AreEqual(_defaultEmployee.Phone.Number, employee.Phone);
    Assert.AreEqual(_defaultEmployee.Id, employee.Id);
  }

  [TestMethod]
  public async Task GetAsync_WhereNameIncludesJoao_ReturnsEmployees()
  {
    var parameters = new EmployeeQueryParameters(0, 100, "João", null, null, null, null, null, null, null, null, null);
    var employees = await _employeeRepository.GetAsync(parameters);

    Assert.IsTrue(employees.Any());
    foreach (var employee in employees)
    {
      Assert.IsNotNull(employee);
      Assert.IsTrue(employee.FirstName.Contains("João"));
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereEmailContainsCom_ReturnsEmployees()
  {
    var parameters = new EmployeeQueryParameters(0, 100, null, ".com", null, null, null, null, null, null, null, null);
    var employees = await _employeeRepository.GetAsync(parameters);

    Assert.IsTrue(employees.Any());
    foreach (var employee in employees)
    {
      Assert.IsNotNull(employee);
      Assert.IsTrue(employee.Email.Contains(".com"));
    }
  }

  [TestMethod]
  public async Task GetAsync_WherePhoneContains55_ReturnsEmployees()
  {
    var parameters = new EmployeeQueryParameters(0, 100, null, null, "55", null, null, null, null, null, null, null);
    var employees = await _employeeRepository.GetAsync(parameters);

    Assert.IsTrue(employees.Any());
    foreach (var employee in employees)
    {
      Assert.IsNotNull(employee);
      Assert.IsTrue(employee.Phone.Contains("55"));
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereGenderEqualsMasculine_ReturnsEmployees()
  {
    var parameters = new EmployeeQueryParameters(0, 100, null, null, null, EGender.Masculine, null, null, null, null, null, null);
    var employees = await _employeeRepository.GetAsync(parameters);

    Assert.IsTrue(employees.Any());
    foreach (var employee in employees)
    {
      Assert.IsNotNull(employee);
      Assert.AreEqual(EGender.Masculine, employee.Gender);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereDateOfBirthGratherThan2000_ReturnsEmployees()
  {
    var parameters = new EmployeeQueryParameters(0, 100, null, null, null, null, DateTime.Now.AddYears(-24), "gt", null, null, null, null);
    var employees = await _employeeRepository.GetAsync(parameters);

    Assert.IsTrue(employees.Any());
    foreach (var employee in employees)
    {
      Assert.IsNotNull(employee);
      Assert.IsTrue(DateTime.Now.AddYears(-24) < employee.DateOfBirth);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereCreatedAtGratherThanYesterday_ReturnsEmployees()
  {
    var parameters = new EmployeeQueryParameters(0, 100, null, null, null, null, null, null, DateTime.Now.AddDays(-1), "gt", null, null);
    var employees = await _employeeRepository.GetAsync(parameters);

    Assert.IsTrue(employees.Any());
    foreach (var employee in employees)
    {
      Assert.IsNotNull(employee);
      Assert.IsTrue(DateTime.Now.AddDays(-1) < employee.CreatedAt);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereCreatedAtLessThanToday_ReturnsEmployees()
  {
    var parameters = new EmployeeQueryParameters(0, 100, null, null, null, null, null, null, DateTime.Now.AddDays(1), "lt", null, null);
    var employees = await _employeeRepository.GetAsync(parameters);

    Assert.IsTrue(employees.Any());
    foreach (var employee in employees)
    {
      Assert.IsNotNull(employee);
      Assert.IsTrue(DateTime.Now.AddDays(1) > employee.CreatedAt);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereCreatedAtEquals_ReturnsEmployees()
  {
    var parameters = new EmployeeQueryParameters(0, 100, null, null, null, null, null, null, _defaultEmployee.CreatedAt, "eq", null, null);
    var employees = await _employeeRepository.GetAsync(parameters);

    Assert.IsTrue(employees.Any());
    foreach (var employee in employees)
    {
      Assert.IsNotNull(employee);
      Assert.AreEqual(_defaultEmployee.CreatedAt, employee.CreatedAt);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereEmailIncludesExample_And_PhoneIncludes55_And_GenderEqualsMasculine_ReturnsEmployees()
  {
    var parameters = new EmployeeQueryParameters(0, 100, null, "example", "55", EGender.Masculine, null, null, null, null, null, null);
    var employees = await _employeeRepository.GetAsync(parameters);

    Assert.IsTrue(employees.Any());

    foreach (var employee in employees)
    {
      Assert.IsNotNull(employee);
      Assert.IsTrue(employee.Email.Contains("example"));
      Assert.IsTrue(employee.Phone.Contains("55"));
      Assert.AreEqual(EGender.Masculine, employee.Gender);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereNameIncludesR_And_DateOfBirthLessThan31Years_And_CreatedAtLessThanTomorrow_ReturnsEmployees()
  {
    var parameters = new EmployeeQueryParameters(0, 100, "R", null, null, null, DateTime.Now.AddYears(-31), "lt", DateTime.Now.AddDays(1), "lt", null, null);

    var employees = await _employeeRepository.GetAsync(parameters);

    Assert.IsTrue(employees.Any());

    foreach (var employee in employees)
    {
      Assert.IsNotNull(employee);
      Assert.IsTrue(employee.FirstName.Contains('R'));
      Assert.IsTrue(DateTime.Now.AddYears(-31) > employee.DateOfBirth);
      Assert.IsTrue(DateTime.Now.AddDays(1) > employee.CreatedAt);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereSalaryGratherThan2000_ReturnsEmployees()
  {
    var parameters = new EmployeeQueryParameters(0, 100, null, null, null, null, null, null,null, null, 2000m, "gt");
    var employees = await _employeeRepository.GetAsync(parameters);

    Assert.IsTrue(employees.Any());
    foreach (var employee in employees)
    {
      Assert.IsNotNull(employee);
      Assert.IsTrue(2000m < employee.Salary);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereSalaryLessThan1500_ReturnsEmployees()
  {
    var parameters = new EmployeeQueryParameters(0, 100, null, null, null, null, null, null, null, null, 1500m, "lt");
    var employees = await _employeeRepository.GetAsync(parameters);

    Assert.IsTrue(employees.Any());
    foreach (var employee in employees)
    {
      Assert.IsNotNull(employee);
      Assert.IsTrue(1500m > employee.Salary);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereSalaryEquals1400_ReturnsEmployees()
  {
    var parameters = new EmployeeQueryParameters(0, 100, null, null, null, null, null, null, null, null, 1400m, "eq");
    var employees = await _employeeRepository.GetAsync(parameters);

    Assert.IsTrue(employees.Any());
    foreach (var employee in employees)
    {
      Assert.IsNotNull(employee);
      Assert.AreEqual(1400m, employee.Salary);
    }
  }

}
