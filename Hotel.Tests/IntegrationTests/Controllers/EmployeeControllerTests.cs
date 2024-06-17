using Hotel.Domain.Data;
using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.DTOs.EmployeeContext.EmployeeDTOs;
using Hotel.Domain.Entities.AdminContext.AdminEntity;
using Hotel.Domain.Entities.AdminContext.PermissionEntity;
using Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;
using Hotel.Domain.Entities.EmployeeContext.ResponsibilityEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Services.Permissions;
using Hotel.Domain.Services.TokenServices;
using Hotel.Domain.ValueObjects;
using Hotel.Tests.IntegrationTests.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Hotel.Tests.IntegrationTests.Controllers;

[TestClass]
public class EmployeeControllerTests
{
  private static HotelWebApplicationFactory _factory = null!;
  private static HttpClient _client = null!;
  private static TokenService _tokenService = null!;
  private static HotelDbContext _dbContext = null!;
  private const string _baseUrl = "v1/employees";
  private static string _rootAdminToken = null!;
  private static Permission _defaultEmployeePermission = null!;
  private static List<Permission> _defaultEmployeePermissions = null!;
  private static List<Permission> _permissions = null!;

  [ClassInitialize]
  public static void ClassInitialize(TestContext? context)
  {
    _factory = new HotelWebApplicationFactory();
    _client = _factory.CreateClient();
    _dbContext = _factory.Services.GetRequiredService<HotelDbContext>();
    _tokenService = _factory.Services.GetRequiredService<TokenService>();

    _defaultEmployeePermission = _dbContext.Permissions.AsTracking().First(x => x.Name.Contains("DefaultEmployeePermission"));
    _permissions = _dbContext.Permissions.ToListAsync().Result;
    _defaultEmployeePermissions = _permissions.Where(x => DefaultEmployeePermissions.PermissionsName.Any(y => y.ToString() == x.Name)).ToList();
    _rootAdminToken = _factory.LoginFullAccess().Result;
  }

  [TestInitialize]
  public void TestInitialize() 
  {
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _rootAdminToken);
  }

  [ClassCleanup]
  public static void ClassCleanup()
  {
    _factory.Dispose();
  }

  [TestMethod]
  public async Task GetEmployees_ShouldReturn_OK()
  {
    //Arrange
    var employee = new Employee
    (
      new Name("Jennifer", "Lawrence"),
      new Email("jenniferLawrenceOfficial@gmail.com"),
      new Phone("+44 (20) 97899-1234"),
      "789",
      EGender.Feminine,
      DateTime.Now.AddYears(-30),
      new Address("United States", "Los Angeles", "US-456", 789)
    );

    await _dbContext.Employees.AddAsync(employee);
    await _dbContext.SaveChangesAsync();

    //Act
    var response = await _client.GetAsync($"{_baseUrl}?take=1");

    //Assert
    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
  }

  [TestMethod]
  public async Task GetEmployeeById_ShouldReturn_OK()
  {
    //Arrange
    var employee = new Employee
    (
      new Name("Ana", "Souza"),
      new Email("anaSouzaOfficial@gmail.com"),
      new Phone("+55 (31) 91234-8688"),
      "789",
      EGender.Feminine,
      DateTime.Now.AddYears(-28),
      new Address("Brazil", "Belo Horizonte", "BR-123", 789)
    );

    await _dbContext.Employees.AddAsync(employee);
    await _dbContext.SaveChangesAsync();

    //Act
    var response = await _client.GetAsync($"{_baseUrl}/{employee.Id}");

    //Assert
    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
  }

  [TestMethod]
  public async Task DeleteEmployee_ShouldReturn_OK()
  {
    //Arrange
    var employee = new Employee
    (
      new Name("João", "Pereira"),
      new Email("joaoPereira@gmail.com"),
      new Phone("+55 (21) 98765-1221"),
      "password2",
      EGender.Masculine,
      DateTime.Now.AddYears(-30),
      new Address("Brazil", "Rio de Janeiro", "RJ-202", 202)
    );

    await _dbContext.Employees.AddAsync(employee);
    await _dbContext.SaveChangesAsync();

    //Act
    var response = await _client.DeleteAsync($"{_baseUrl}/{employee.Id}");

    //Assert
    var wasNotDeleted = await _dbContext.Employees.AnyAsync(x => x.Id == employee.Id);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    Assert.IsFalse(wasNotDeleted);
  }

  [TestMethod]
  public async Task DeleteLoggedEmployee_ShouldReturn_OK()
  {
    //Arrange
    var employee = new Employee
    (
      new Name("Emma", "Watson"),
      new Email("emmaWatson@gmail.com"),
      new Phone("+44 (20) 99346-1912"),
      "123",
      EGender.Feminine,
      DateTime.Now.AddYears(-31),
      new Address("United Kingdom", "London", "UK-123", 456)
    );

    var token = _tokenService.GenerateToken(employee);
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    await _dbContext.Employees.AddAsync(employee);
    await _dbContext.SaveChangesAsync();

    //Act
    var response = await _client.DeleteAsync(_baseUrl);

    //Assert
    var wasNotDeleted = await _dbContext.Employees.AnyAsync(x => x.Id == employee.Id);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    Assert.IsFalse(wasNotDeleted);
  }

  [TestMethod]
  public async Task AddEmployeePermission_ShouldReturn_OK()
  {
    var permission = await _dbContext.Permissions.FirstOrDefaultAsync(x => x.Name.Contains("CreateEmployee"));

    //Arrange
    var employee = new Employee
    (
      new Name("Carlos", "Silva"),
      new Email("carlosSilvl@gmail.com"),
      new Phone("+55 (21) 99916-5432"),
      "456",
      EGender.Masculine,
      DateTime.Now.AddYears(-35),
      new Address("Brazil", "Rio de Janeiro", "BR-789", 123),
      null,
      [_defaultEmployeePermission]
    );

    await _dbContext.Employees.AddAsync(employee);
    await _dbContext.SaveChangesAsync();

    //Act
    var response = await _client.PostAsync($"{_baseUrl}/{employee.Id}/permissions/{permission!.Id}", null);

    //Assert
    var employeeWithPermissions = await _dbContext.Employees
      .Include(x => x.Permissions)
      .FirstOrDefaultAsync(x => x.Id == employee.Id);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    Assert.AreEqual(employee.Id, employeeWithPermissions!.Id);
    Assert.AreEqual(2,employeeWithPermissions.Permissions.Count);
    Assert.IsTrue(employeeWithPermissions.Permissions.Any(x => x.Id == permission.Id));
    Assert.IsTrue(employeeWithPermissions.Permissions.Any(x => x.Id == _defaultEmployeePermission.Id));
  }

  [TestMethod]
  public async Task RemoveEmployeePermission_ShouldReturn_OK()
  {
    var permission = await _dbContext.Permissions.FirstOrDefaultAsync(x => x.Name.Contains("GetEmployee"));

    //Arrange
    var employee = new Employee
    (
      new Name("Mariana", "Lima"),
      new Email("marianaLima@gmail.com"),
      new Phone("+55 (11) 92264-4678"),
      "password1",
      EGender.Feminine,
      DateTime.Now.AddYears(-25),
      new Address("Brazil", "São Paulo", "SP-101", 101),
      null,
      [_defaultEmployeePermission]
    );

    await _dbContext.Employees.AddAsync(employee);
    await _dbContext.SaveChangesAsync();

    //Act
    var response = await _client.DeleteAsync($"{_baseUrl}/{employee.Id}/permissions/{permission!.Id}");

    //Assert
    var employeeWithPermissions = await _dbContext.Employees
      .Where(x => x.Id == employee.Id)
      .Include(x => x.Permissions)
      .FirstOrDefaultAsync();

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    Assert.AreEqual(employee.Id,employeeWithPermissions!.Id);
    Assert.IsFalse(employeeWithPermissions.Permissions.Any(x => x.Id == permission.Id));
  }

  [TestMethod]
  public async Task UpdateEmployee_ShouldReturn_OK()
  {
    //Arrange
    var employee = new Employee
    (
      new Name("Rafael", "Oliveira"),
      new Email("rafaelOliveira@gmail.com"),
      new Phone("+55 (41) 97654-3210"),
      "password4",
      EGender.Masculine,
      DateTime.Now.AddYears(-32),
      new Address("Brazil", "Curitiba", "PR-404", 404)
    );

    await _dbContext.Employees.AddAsync(employee);
    await _dbContext.SaveChangesAsync();

    var body = new UpdateEmployee("Luan", "Queiroz", "+55 (41) 93651-3210", EGender.Feminine, DateTime.Now.AddYears(-20), "Brazil", "Curitiba", "PR-404", 404, null);

    //Act
    var response = await _client.PutAsJsonAsync($"{_baseUrl}/{employee.Id}", body);

    //Assert
    var updatedEmployee = await _dbContext.Employees.FirstOrDefaultAsync(x => x.Id == employee.Id);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    Assert.AreEqual(updatedEmployee!.Id, employee.Id);
    Assert.AreEqual(updatedEmployee.Name.FirstName, body.FirstName);
    Assert.AreEqual(updatedEmployee.Name.LastName, body.LastName);
    Assert.AreEqual(updatedEmployee.Phone.Number, body.Phone);
    Assert.AreEqual(updatedEmployee.Gender, body.Gender);
    Assert.AreEqual(updatedEmployee.DateOfBirth, body.DateOfBirth);
    Assert.AreEqual(updatedEmployee!.Address!.Country, body.Country);
    Assert.AreEqual(updatedEmployee!.Address.City, body.City);
    Assert.AreEqual(updatedEmployee!.Address!.Number, body.Number);
    Assert.AreEqual(updatedEmployee!.Address.Street, body.Street);
  }


  [TestMethod]
  public async Task UpdateLoggedEmployee_ShouldReturn_OK()
  {
    //Arrange
    var employee = new Employee
    (
      new Name("Camila", "Costa"),
      new Email("camilaCosta@gmail.com"),
      new Phone("+55 (71) 93456-7890"),
      "password5",
      EGender.Feminine,
      DateTime.Now.AddYears(-29),
      new Address("Brazil", "Salvador", "BA-505", 505)
    );

    await _dbContext.Employees.AddAsync(employee);
    await _dbContext.SaveChangesAsync();

    var token = _tokenService.GenerateToken(employee);
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    var body = new UpdateEmployee("Jão", "Pedro", "+55 (41) 96652-3210", EGender.Feminine, DateTime.Now.AddYears(-20), "Brazil", "Curitiba", "PR-404", 404, null);

    //Act
    var response = await _client.PutAsJsonAsync(_baseUrl, body);

    //Assert
    var updatedEmployee = await _dbContext.Employees.FirstOrDefaultAsync(x => x.Id == employee.Id);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    Assert.AreEqual(updatedEmployee!.Id, employee.Id);
    Assert.AreEqual(updatedEmployee.Name.FirstName, body.FirstName);
    Assert.AreEqual(updatedEmployee.Name.LastName, body.LastName);
    Assert.AreEqual(updatedEmployee.Phone.Number, body.Phone);
    Assert.AreEqual(updatedEmployee.Gender, body.Gender);
    Assert.AreEqual(updatedEmployee.DateOfBirth, body.DateOfBirth);
    Assert.AreEqual(updatedEmployee!.Address!.Country, body.Country);
    Assert.AreEqual(updatedEmployee!.Address.City, body.City);
    Assert.AreEqual(updatedEmployee!.Address!.Number, body.Number);
    Assert.AreEqual(updatedEmployee!.Address.Street, body.Street);
  }

  [TestMethod]
  public async Task UpdateEmployeeName_ShouldReturn_OK()
  {
    //Arrange
    var employee = new Employee
    (
      new Name("Lucas", "Ferreira"),
      new Email("lucasFerreira@gmail.com"),
      new Phone("+55 (61) 92345-6789"),
      "password6",
      EGender.Masculine,
      DateTime.Now.AddYears(-28),
      new Address("Brazil", "Brasília", "DF-606", 606)
    );

    await _dbContext.Employees.AddAsync(employee);
    await _dbContext.SaveChangesAsync();

    var token = _tokenService.GenerateToken(employee);
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    var body = new Name("John", "Wick");

    //Act
    var response = await _client.PatchAsJsonAsync($"{_baseUrl}/name", body);

    //Assert
    var updatedEmployee = await _dbContext.Employees.FirstOrDefaultAsync(x => x.Id == employee.Id);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    Assert.AreEqual(updatedEmployee!.Name.FirstName, body.FirstName);
    Assert.AreEqual(updatedEmployee.Name.LastName, body.LastName);
  }

  [TestMethod]
  public async Task UpdateEmployeeEmail_ShouldReturn_OK()
  {
    //Arrange
    var employee = new Employee
    (
      new Name("Fernanda", "River"),
      new Email("fernandaRiver@gmail.com"),
      new Phone("+55 (51) 91239-5678"),
      "password7",
      EGender.Feminine,
      DateTime.Now.AddYears(-26),
      new Address("Brazil", "Porto Alegre", "RS-707", 707)
    );

    await _dbContext.Employees.AddAsync(employee);
    await _dbContext.SaveChangesAsync();

    var token = _tokenService.GenerateToken(employee);
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    var body = new Email("feeRriber@gmail.com");

    //Act
    var response = await _client.PatchAsJsonAsync($"{_baseUrl}/email", body);

    //Assert
    var updatedEmployee = await _dbContext.Employees.FirstOrDefaultAsync(x => x.Id == employee.Id);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    Assert.AreEqual(updatedEmployee!.Email.Address, body.Address);
  }

  [TestMethod]
  public async Task UpdateEmployeePhone_ShouldReturn_OK()
  {
    //Arrange
    var employee = new Employee
    (
      new Name("Michele", "Silva"),
      new Email("micheleSilvaa100@gmail.com"),
      new Phone("+55 (62) 99846-1432"),
      "password8",
      EGender.Masculine,
      DateTime.Now.AddYears(-31),
      new Address("Brazil", "Goiânia", "GO-808", 808)
    );

    await _dbContext.Employees.AddAsync(employee);
    await _dbContext.SaveChangesAsync();

    var token = _tokenService.GenerateToken(employee);
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    var body = new Phone("+55 (62) 99156-3449");

    //Act
    var response = await _client.PatchAsJsonAsync($"{_baseUrl}/phone", body);

    //Assert
    var updatedEmployee = await _dbContext.Employees.FirstOrDefaultAsync(x => x.Id == employee.Id);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    Assert.AreEqual(updatedEmployee!.Phone.Number, body.Number);
  }

  [TestMethod]
  public async Task UpdateEmployeeAddress_ShouldReturn_OK()
  {
    //Arrange
    var employee = new Employee
    (
      new Name("Vinicius", "Silva"),
      new Email("viniSilva@gmail.com"),
      new Phone("+55 (62) 91876-3432"),
      "password8",
      EGender.Masculine,
      DateTime.Now.AddYears(-31),
      new Address("Brazil", "Goiânia", "GO-808", 808)
    );

    await _dbContext.Employees.AddAsync(employee);
    await _dbContext.SaveChangesAsync();

    var token = _tokenService.GenerateToken(employee);
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    var body = new Address("Brazil", "Florianópolis", "SC-909", 909);

    //Act
    var response = await _client.PatchAsJsonAsync($"{_baseUrl}/address", body);

    //Assert
    var updatedEmployee = await _dbContext.Employees.FirstOrDefaultAsync(x => x.Id == employee.Id);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    Assert.AreEqual(updatedEmployee!.Address!.Country, body.Country);
    Assert.AreEqual(updatedEmployee!.Address.City, body.City);
    Assert.AreEqual(updatedEmployee!.Address!.Number, body.Number);
    Assert.AreEqual(updatedEmployee!.Address.Street, body.Street);
  }

  [TestMethod]
  public async Task UpdateEmployeeGender_ShouldReturn_OK()
  {
    //Arrange
    var employee = new Employee
    (
      new Name("Gustavo", "Souza"),
      new Email("gustavoSouza@gmail.com"),
      new Phone("+55 (27) 93456-7890"),
      "password10",
      EGender.Masculine,
      DateTime.Now.AddYears(-33),
      new Address("Brazil", "Vitória", "ES-1010", 1010)
    );

    await _dbContext.Employees.AddAsync(employee);
    await _dbContext.SaveChangesAsync();

    var token = _tokenService.GenerateToken(employee);
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    //Act
    var response = await _client.PatchAsJsonAsync($"{_baseUrl}/gender/2", new { });

    //Assert
    var updatedEmployee = await _dbContext.Employees.FirstOrDefaultAsync(x => x.Id == employee.Id);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    Assert.AreEqual(EGender.Feminine, updatedEmployee!.Gender);
  }

  [TestMethod]
  public async Task UpdateEmployeeDateOfBirth_ShouldReturn_OK()
  {
    //Arrange
    var employee = new Employee
    (
      new Name("Geovane", "Silva"),
      new Email("geoSilv@gmail.com"),
      new Phone("+55 (27) 93113-7859"),
      "password10",
      EGender.Masculine,
      DateTime.Now.AddYears(-33),
      new Address("Brazil", "Vitória", "ES-1011", 1011)
    );

    await _dbContext.Employees.AddAsync(employee);
    await _dbContext.SaveChangesAsync();

    var token = _tokenService.GenerateToken(employee);
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    var body = new UpdateDateOfBirth(DateTime.Now.AddYears(-35));

    //Act
    var response = await _client.PatchAsJsonAsync($"{_baseUrl}/date-of-birth", body);

    //Assert
    var updatedEmployee = await _dbContext.Employees.FirstOrDefaultAsync(x => x.Id == employee.Id);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    Assert.AreEqual(body.DateOfBirth, updatedEmployee!.DateOfBirth);
  }

  [TestMethod]
  public async Task AddEmployeeResponsability_ShouldReturn_OK()
  {
    var responsibility = new Responsibility("Auxiliar", "Auxiliar em serviços gerais", EPriority.Low);

    //Arrange
    var employee = new Employee
    (
      new Name("Maria", "Silva"),
      new Email("mariaSilvl@gmail.com"),
      new Phone("+55 (21) 99918-1332"),
      "456",
      EGender.Masculine,
      DateTime.Now.AddYears(-35),
      new Address("Brazil", "Rio de Janeiro", "BR-789", 123),
      null,
      [_defaultEmployeePermission]
    );

    await _dbContext.Responsibilities.AddAsync(responsibility);
    await _dbContext.Employees.AddAsync(employee);
    await _dbContext.SaveChangesAsync();

    //Act
    var response = await _client.PostAsync($"{_baseUrl}/{employee.Id}/responsibilities/{responsibility.Id}", null);

    //Assert
    var employeeWithResponsibilities = await _dbContext.Employees
      .Include(x => x.Responsibilities)
      .FirstOrDefaultAsync(x => x.Id == employee.Id);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    Assert.AreEqual(employee.Id, employeeWithResponsibilities!.Id);
    Assert.AreEqual(1, employeeWithResponsibilities.Responsibilities.Count);
    Assert.IsTrue(employeeWithResponsibilities.Responsibilities.Any(x => x.Id == responsibility.Id));
  }

  [TestMethod]
  public async Task RemoveEmployeeResponsability_ShouldReturn_OK()
  {
    var responsibility = new Responsibility("Limpeza", "Limpeza geral", EPriority.Low);

    //Arrange
    var employee = new Employee
    (
      new Name("Brenda", "Carvalho"),
      new Email("brenddaaCarvas@gmail.com"),
      new Phone("+55 (11) 99921-5499"),
      "456",
      EGender.Masculine,
      DateTime.Now.AddYears(-35),
      new Address("Brazil", "Rio de Janeiro", "BR-789", 123),
      null,
      [_defaultEmployeePermission]
    );

    employee.AddResponsibility(responsibility);

    await _dbContext.Employees.AddAsync(employee);
    await _dbContext.SaveChangesAsync();

    //Act
    var response = await _client.DeleteAsync($"{_baseUrl}/{employee.Id}/responsibilities/{responsibility.Id}");

    //Assert
    var employeeWithResponsibilities = await _dbContext.Employees
      .Include(x => x.Responsibilities)
      .FirstOrDefaultAsync(x => x.Id == employee.Id);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    Assert.AreEqual(employee.Id, employeeWithResponsibilities!.Id);
    Assert.AreEqual(0, employeeWithResponsibilities.Responsibilities.Count);
    Assert.IsFalse(employeeWithResponsibilities.Responsibilities.Any(x => x.Id == responsibility.Id));
  }

  //
  //TESTES DE PERMISSÂO
  //

  [TestMethod]
  [DataRow("GetAdmins", "v1/admins", "GET")]
  [DataRow("GetAdmin", "v1/admins/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "GET")]
  [DataRow("EditAdmin", "v1/admins/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "PUT")]
  [DataRow("DeleteAdmin", "v1/admins/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "DELETE")]
  [DataRow("AdminAssignPermission", "v1/admins/f6c5e02b-a0ae-429e-beb3-d433d51ad414/permissions/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "POST")]
  [DataRow("AdminUnassignPermission", "v1/admins/f6c5e02b-a0ae-429e-beb3-d433d51ad414/permissions/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "DELETE")]
  [DataRow("EditCustomer", "v1/customers/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "PUT")]
  [DataRow("DeleteCustomer", "v1/customers/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "DELETE")]
  [DataRow("CreateAdmin", "v1/register/admins", "POST")]
  [DataRow("GetEmployee", "v1/employees/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "GET")]
  [DataRow("GetEmployees", "v1/employees", "GET")]
  [DataRow("DeleteEmployee", "v1/employees/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "DELETE")]
  [DataRow("EditEmployee", "v1/employees/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "PUT")]
  [DataRow("CreateEmployee", "v1/register/employees", "POST")]
  [DataRow("AssignEmployeeResponsibility", "v1/employees/f6c5e02b-a0ae-429e-beb3-d433d51ad414/responsibilities/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "POST")]
  [DataRow("UnassignEmployeeResponsibility", "v1/employees/f6c5e02b-a0ae-429e-beb3-d433d51ad414/responsibilities/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "DELETE")]
  [DataRow("AssignEmployeePermission", "v1/employees/f6c5e02b-a0ae-429e-beb3-d433d51ad414/permissions/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "POST")]
  [DataRow("UnassignEmployeePermission", "v1/employees/f6c5e02b-a0ae-429e-beb3-d433d51ad414/permissions/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "DELETE")]
  [DataRow("GetResponsibilities", "v1/responsibilities", "GET")]
  [DataRow("GetResponsibility", "v1/responsibilities/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "GET")]
  [DataRow("CreateResponsibility", "v1/responsibilities", "POST")]
  [DataRow("EditResponsibility", "v1/responsibilities/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "PUT")]
  [DataRow("DeleteResponsibility", "v1/responsibilities/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "DELETE")]
  [DataRow("DeleteRoomInvoice", "v1/room-invoices/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "DELETE")]
  [DataRow("GetRoomInvoices", "v1/room-invoices", "GET")]
  [DataRow("GetRoomInvoice", "v1/room-invoices/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "GET")]
  [DataRow("DeleteReservation", "v1/reservations/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "DELETE")]
  [DataRow("UpdateReservationCheckout", "v1/reservations/f6c5e02b-a0ae-429e-beb3-d433d51ad414/check-out", "PATCH")]
  [DataRow("UpdateReservationCheckIn", "v1/reservations/f6c5e02b-a0ae-429e-beb3-d433d51ad414/check-in", "PATCH")]
  [DataRow("AddServiceToReservation", "v1/reservations/f6c5e02b-a0ae-429e-beb3-d433d51ad414/services/e3347565-8ec7-4a3b-be3a-951317bb53dc", "POST")]
  [DataRow("RemoveServiceFromReservation", "v1/reservations/f6c5e02b-a0ae-429e-beb3-d433d51ad414/services/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "DELETE")]
  [DataRow("CreateCategory", "v1/categories", "POST")]
  [DataRow("EditCategory", "v1/categories/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "PUT")]
  [DataRow("DeleteCategory", "v1/categories/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "DELETE")]
  [DataRow("GetReports", "v1/reports", "GET")]
  [DataRow("GetReport", "v1/reports/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "GET")]
  [DataRow("EditReport", "v1/reports/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "PUT")]
  [DataRow("CreateReport", "v1/reports", "POST")]
  [DataRow("FinishReport", "v1/reports/finish/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "PATCH")]
  [DataRow("CreateRoom", "v1/rooms", "POST")]
  [DataRow("EditRoom", "v1/rooms/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "PUT")]
  [DataRow("DeleteRoom", "v1/rooms/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "DELETE")]
  [DataRow("AddRoomService", "v1/rooms/f6c5e02b-a0ae-429e-beb3-d433d51ad414/services/e3347565-8ec7-4a3b-be3a-951317bb53dc", "POST")]
  [DataRow("RemoveRoomService", "v1/rooms/f6c5e02b-a0ae-429e-beb3-d433d51ad414/services/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "DELETE")]
  [DataRow("UpdateRoomNumber", "v1/rooms/f6c5e02b-a0ae-429e-beb3-d433d51ad414/number/1", "PATCH")]
  [DataRow("UpdateRoomCapacity", "v1/rooms/f6c5e02b-a0ae-429e-beb3-d433d51ad414/capacity/2", "PATCH")]
  [DataRow("UpdateRoomCategory", "v1/rooms/f6c5e02b-a0ae-429e-beb3-d433d51ad414/category/62eb01d1-a7ba-4c09-ae5b-5ec6b5071577", "PATCH")]
  [DataRow("UpdateRoomPrice", "v1/rooms/f6c5e02b-a0ae-429e-beb3-d433d51ad414/price", "PATCH")]
  [DataRow("EnableRoom", "v1/rooms/enable/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "PATCH")]
  [DataRow("DisableRoom", "v1/rooms/disable/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "PATCH")]
  [DataRow("GetServices", "v1/services", "GET")]
  [DataRow("GetService", "v1/services/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "GET")]
  [DataRow("UpdateService", "v1/services/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "PUT")]
  [DataRow("CreateService", "v1/services", "POST")]
  [DataRow("DeleteService", "v1/services/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "DELETE")]
  [DataRow("AssignServiceResponsibility", "v1/services/f6c5e02b-a0ae-429e-beb3-d433d51ad414/responsibilities/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "POST")]
  [DataRow("UnassignServiceResponsibility", "v1/services/f6c5e02b-a0ae-429e-beb3-d433d51ad414/responsibilities/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "DELETE")]
  [DataRow("AvailableRoomStatus", "v1/rooms/available/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "PATCH")]
  public void AccessEndpointsWithOutPermission_ShouldReturn_FORBIDDEN(string permissionName, string endpoint, string method)
  {
    var permissions = _permissions.Where(x => x.Name != "DefaultAdminPermission" && x.Name != "DefaultEmployeePermission").ToList();

    //Arrange
    var employee = new Employee
    (
      new Name("Gabriel", "Souz"),
      new Email("gabriSouz@gmail.com"),
      new Phone("+55 (27) 93123-7810"),
      "password10",
      EGender.Masculine,
      DateTime.Now.AddYears(-33),
      new Address("Brazil", "Vitória", "ES-1011", 1011),
      null,
      permissions
    );
    try
    {
      _dbContext.Employees.Add(employee);
      _dbContext.SaveChanges();

      var permission = _dbContext.Permissions.FirstOrDefault(x => x.Name.Equals(permissionName));
      employee.UnassignPermission(permission!);

      var token = _tokenService.GenerateToken(employee);
      _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

      //Act
      var response = method switch
      {
        "POST" => _client.PostAsJsonAsync(endpoint, new { }).Result,
        "PUT" => _client.PutAsJsonAsync(endpoint, new { }).Result,
        "GET" => _client.GetAsync(endpoint).Result,
        "DELETE" => _client.DeleteAsync(endpoint).Result,
        "PATCH" => _client.PatchAsync(endpoint, null).Result,
        _ => null!
      };

      //Assert
      Assert.AreEqual(HttpStatusCode.Forbidden, response!.StatusCode);
    }
    finally
    {
      _dbContext.Employees.Remove(employee!);
      _dbContext.SaveChanges();
    }
  }

}



