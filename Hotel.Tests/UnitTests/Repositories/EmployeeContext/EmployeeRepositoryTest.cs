using Hotel.Domain.DTOs.EmployeeContext.EmployeeDTOs;
using Hotel.Domain.Enums;
using Hotel.Domain.Repositories.EmployeeContext;
using Hotel.Tests.UnitTests.Repositories.Mock;

namespace Hotel.Tests.UnitTests.Repositories.EmployeeContext;

[TestClass]
public class EmployeeRepositoryTest
{
    private static EmployeeRepository EmployeeRepository { get; set; }

    static EmployeeRepositoryTest()
    => EmployeeRepository = new EmployeeRepository(BaseRepositoryTest.MockConnection.Context);


    [TestMethod]
    public async Task GetByIdAsync_ReturnsWithCorrectParameters()
    {
        var employee = await EmployeeRepository.GetByIdAsync(BaseRepositoryTest.Employees[0].Id);

        Assert.IsNotNull(employee);
        Assert.AreEqual(BaseRepositoryTest.Employees[0].Id, employee.Id);
        Assert.AreEqual(BaseRepositoryTest.Employees[0].Name.FirstName, employee.FirstName);
        Assert.AreEqual(BaseRepositoryTest.Employees[0].Name.LastName, employee.LastName);
        Assert.AreEqual(BaseRepositoryTest.Employees[0].Email.Address, employee.Email);
        Assert.AreEqual(BaseRepositoryTest.Employees[0].Phone.Number, employee.Phone);
        Assert.AreEqual(BaseRepositoryTest.Employees[0].Salary, employee.Salary);

    }

    [TestMethod]
    public async Task GetAsync_ReturnWithCorrectParameters()
    {
        var parameters = new EmployeeQueryParameters(0, 100, BaseRepositoryTest.Employees[0].Name.FirstName, null, null, null, null, null, null, null, null, null);
        var employees = await EmployeeRepository.GetAsync(parameters);

        var employee = employees.ToList()[0];

        Assert.IsNotNull(employee);
        Assert.AreEqual(BaseRepositoryTest.Employees[0].Name.FirstName, employee.FirstName);
        Assert.AreEqual(BaseRepositoryTest.Employees[0].Name.LastName, employee.LastName);
        Assert.AreEqual(BaseRepositoryTest.Employees[0].Email.Address, employee.Email);
        Assert.AreEqual(BaseRepositoryTest.Employees[0].Phone.Number, employee.Phone);
        Assert.AreEqual(BaseRepositoryTest.Employees[0].Id, employee.Id);
    }

    [TestMethod]
    public async Task GetAsync_WhereNameIncludesLucas_ReturnsEmployees()
    {
        var parameters = new EmployeeQueryParameters(0, 100, "Lucas", null, null, null, null, null, null, null, null, null);
        var employees = await EmployeeRepository.GetAsync(parameters);

        Assert.IsTrue(employees.Any());
        foreach (var employee in employees)
            Assert.IsTrue(employee.FirstName.Contains("Lucas"));

    }

    [TestMethod]
    public async Task GetAsync_WhereEmailContainsCom_ReturnsEmployees()
    {
        var parameters = new EmployeeQueryParameters(0, 100, null, ".com", null, null, null, null, null, null, null, null);
        var employees = await EmployeeRepository.GetAsync(parameters);

        Assert.IsTrue(employees.Any());
        foreach (var employee in employees)
            Assert.IsTrue(employee.Email.Contains(".com"));

    }

    [TestMethod]
    public async Task GetAsync_WherePhoneContains55_ReturnsEmployees()
    {
        var parameters = new EmployeeQueryParameters(0, 100, null, null, "55", null, null, null, null, null, null, null);
        var employees = await EmployeeRepository.GetAsync(parameters);

        Assert.IsTrue(employees.Any());
        foreach (var employee in employees)
            Assert.IsTrue(employee.Phone.Contains("55"));

    }

    [TestMethod]
    public async Task GetAsync_WhereGenderEqualsMasculine_ReturnsEmployees()
    {
        var parameters = new EmployeeQueryParameters(0, 100, null, null, null, EGender.Masculine, null, null, null, null, null, null);
        var employees = await EmployeeRepository.GetAsync(parameters);

        Assert.IsTrue(employees.Any());
        foreach (var employee in employees)
            Assert.AreEqual(EGender.Masculine, employee.Gender);

    }

    [TestMethod]
    public async Task GetAsync_WhereDateOfBirthGratherThan2000_ReturnsEmployees()
    {
        var parameters = new EmployeeQueryParameters(0, 100, null, null, null, null, DateTime.Now.AddYears(-24), "gt", null, null, null, null);
        var employees = await EmployeeRepository.GetAsync(parameters);

        Assert.IsTrue(employees.Any());
        foreach (var employee in employees)
            Assert.IsTrue(DateTime.Now.AddYears(-24) < employee.DateOfBirth);

    }

    [TestMethod]
    public async Task GetAsync_WhereCreatedAtGratherThanYesterday_ReturnsEmployees()
    {
        var parameters = new EmployeeQueryParameters(0, 100, null, null, null, null, null, null, DateTime.Now.AddDays(-1), "gt", null, null);
        var employees = await EmployeeRepository.GetAsync(parameters);

        Assert.IsTrue(employees.Any());
        foreach (var employee in employees)
            Assert.IsTrue(DateTime.Now.AddDays(-1) < employee.CreatedAt);

    }

    [TestMethod]
    public async Task GetAsync_WhereCreatedAtLessThanToday_ReturnsEmployees()
    {
        var parameters = new EmployeeQueryParameters(0, 100, null, null, null, null, null, null, DateTime.Now.AddDays(1), "lt", null, null);
        var employees = await EmployeeRepository.GetAsync(parameters);

        Assert.IsTrue(employees.Any());
        foreach (var employee in employees)
            Assert.IsTrue(DateTime.Now.AddDays(1) > employee.CreatedAt);

    }

    [TestMethod]
    public async Task GetAsync_WhereCreatedAtEquals_ReturnsEmployees()
    {
        var parameters = new EmployeeQueryParameters(0, 100, null, null, null, null, null, null, BaseRepositoryTest.Employees[0].CreatedAt, "eq", null, null);
        var employees = await EmployeeRepository.GetAsync(parameters);

        Assert.IsTrue(employees.Any());
        foreach (var employee in employees)
            Assert.AreEqual(BaseRepositoryTest.Employees[0].CreatedAt, employee.CreatedAt);

    }

    [TestMethod]
    public async Task GetAsync_WhereEmailIncludesExample_And_PhoneIncludes55_And_GenderEqualsMasculine_ReturnsEmployees()
    {
        var parameters = new EmployeeQueryParameters(0, 100, null, "example", "55", EGender.Masculine, null, null, null, null, null, null);
        var employees = await EmployeeRepository.GetAsync(parameters);

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
        var parameters = new EmployeeQueryParameters(0, 100, "R", null, null, null, DateTime.Now.AddYears(-31), "lt", DateTime.Now.AddDays(1), "lt", null, null);

        var employees = await EmployeeRepository.GetAsync(parameters);

        Assert.IsTrue(employees.Any());

        foreach (var employee in employees)
        {
            Assert.IsTrue(employee.FirstName.Contains('R'));
            Assert.IsTrue(DateTime.Now.AddYears(-31) > employee.DateOfBirth);
            Assert.IsTrue(DateTime.Now.AddDays(1) > employee.CreatedAt);
        }
    }

    [TestMethod]
    public async Task GetAsync_WhereSalaryGratherThan2000_ReturnsEmployees()
    {
        var parameters = new EmployeeQueryParameters(0, 100, null, null, null, null, null, null, null, null, 2000m, "gt");
        var employees = await EmployeeRepository.GetAsync(parameters);

        Assert.IsTrue(employees.Any());

        foreach (var employee in employees)
            Assert.IsTrue(2000m < employee.Salary);

    }

    [TestMethod]
    public async Task GetAsync_WhereSalaryLessThan1500_ReturnsEmployees()
    {
        var parameters = new EmployeeQueryParameters(0, 100, null, null, null, null, null, null, null, null, 1500m, "lt");
        var employees = await EmployeeRepository.GetAsync(parameters);

        Assert.IsTrue(employees.Any());

        foreach (var employee in employees)
            Assert.IsTrue(1500m > employee.Salary);

    }

    [TestMethod]
    public async Task GetAsync_WhereSalaryEquals1400_ReturnsEmployees()
    {
        var parameters = new EmployeeQueryParameters(0, 100, null, null, null, null, null, null, null, null, 1400m, "eq");
        var employees = await EmployeeRepository.GetAsync(parameters);

        Assert.IsTrue(employees.Any());
        foreach (var employee in employees)
            Assert.AreEqual(1400m, employee.Salary);

    }

}
