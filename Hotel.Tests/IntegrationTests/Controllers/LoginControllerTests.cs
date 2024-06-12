using Hotel.Domain.Data;
using Hotel.Domain.DTOs.AuthenticationContext;
using Hotel.Domain.Entities.AdminContext.AdminEntity;
using Hotel.Domain.Entities.CustomerContext;
using Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.ValueObjects;
using Hotel.Tests.IntegrationTests.Factories;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Hotel.Tests.IntegrationTests.Controllers;

[TestClass]
public class LoginControllerTests
{
  private static HotelWebApplicationFactory _factory = null!;
  private static HttpClient _client = null!;
  private static HotelDbContext _dbContext = null!;
  private const string _baseUrl = "v1/login";
  private static Admin _admin = null!;
  private static Customer _customer = null!;
  private static Employee _employee = null!;

  [ClassInitialize]
  public static void ClassInitialize(TestContext context)
  {
    _factory = new HotelWebApplicationFactory();
    _client = _factory.CreateClient();
    _dbContext = _factory.Services.GetRequiredService<HotelDbContext>();

    _customer = new Customer
    (
      new Name("Jennifer", "Lawrence"),
      new Email("jenniferLawrenceOfficial@gmail.com"),
      new Phone("+44 (20) 97890-1234"),
      "123",
      EGender.Feminine,
      DateTime.Now.AddYears(-30),
      new Address("United States", "Los Angeles", "US-456", 789)
    );

    _admin = new Admin
    (
      new Name("João", "Pereira"),
      new Email("joaoPereira@gmail.com"),
      new Phone("+55 (21) 98765-4321"),
      "123",
      EGender.Masculine,
      DateTime.Now.AddYears(-30),
      new Address("Brazil", "Rio de Janeiro", "RJ-202", 202)
    );

    _employee = new Employee
    (
      new Name("Emma", "Watson"),
      new Email("emmaWatson@gmail.com"),
      new Phone("+44 (20) 99346-1912"),
      "123",
      EGender.Feminine,
      DateTime.Now.AddYears(-31),
      new Address("United Kingdom", "London", "UK-123", 456)
    );

    _dbContext.Admins.AddAsync(_admin);
    _dbContext.Customers.AddAsync(_customer);
    _dbContext.Employees.AddAsync(_employee);
    _dbContext.SaveChangesAsync();
  }

  [ClassCleanup]
  public static void ClassCleanup()
  {
    _factory.Dispose();
  }

  [TestMethod]
  public async Task CustomerLogin_ShouldReturn_OK()
  {
    //Arrange
    var body = new LoginDTO(_customer.Email.Address, "123");

    //Act
    var response = await _client.PostAsJsonAsync(_baseUrl, body);

    //Assert
    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

    var content = JsonConvert.DeserializeObject<LoginResponse>(await response.Content.ReadAsStringAsync());

    Assert.IsNotNull(content);
    var token = content.data.token;

    //Test the returned token
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    var responseGetRooms = await _client.GetAsync("v1/rooms");

    Assert.IsNotNull(responseGetRooms);
    Assert.AreEqual(HttpStatusCode.OK, responseGetRooms.StatusCode);
  }

  [TestMethod]
  public async Task CustomerLogin_WithInvalidPassword_ShouldReturn_BAD_REQUEST()
  {
    //Arrange

    var body = new LoginDTO(_customer.Email.Address, "ADasLKX#4s12");

    //Act
    var response = await _client.PostAsJsonAsync(_baseUrl, body);

    //Assert
    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

    var content = JsonConvert.DeserializeObject<LoginResponse>(await response.Content.ReadAsStringAsync());

    Assert.IsNotNull(content);
    Assert.AreEqual(1, content.errors.Count);
    Assert.IsTrue(content.errors.Any(x => x.Equals("Email ou senha inválidos.")));
  }

  [TestMethod]
  public async Task CustomerLogin_WithInvalidEmail_ShouldReturn_BAD_REQUEST()
  {
    //Arrange

    var body = new LoginDTO("aa@gmail.com", "123");

    //Act
    var response = await _client.PostAsJsonAsync(_baseUrl, body);

    //Assert
    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

    var content = JsonConvert.DeserializeObject<LoginResponse>(await response.Content.ReadAsStringAsync());

    Assert.IsNotNull(content);
    Assert.AreEqual(1, content.errors.Count);
    Assert.IsTrue(content.errors.Any(x => x.Equals("Email ou senha inválidos.")));
  }

  [TestMethod]
  public async Task AdminLogin_ShouldReturn_OK()
  {

    var body = new LoginDTO(_admin.Email.Address, "123");

    //Act
    var response = await _client.PostAsJsonAsync(_baseUrl, body);

    //Assert
    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

    var content = JsonConvert.DeserializeObject<LoginResponse>(await response.Content.ReadAsStringAsync());

    Assert.IsNotNull(content);
    var token = content.data.token;

    //Test the returned token
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    var responseGetRooms = await _client.GetAsync("v1/rooms");

    Assert.IsNotNull(responseGetRooms);
    Assert.AreEqual(HttpStatusCode.OK, responseGetRooms.StatusCode);
  }

  [TestMethod]
  public async Task AdminLogin_WithInvalidPassword_ShouldReturn_BAD_REQUEST()
  {
    //Arrange
    var body = new LoginDTO(_admin.Email.Address, "123756");

    //Act
    var response = await _client.PostAsJsonAsync(_baseUrl, body);

    //Assert
    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

    var content = JsonConvert.DeserializeObject<LoginResponse>(await response.Content.ReadAsStringAsync());

    Assert.IsNotNull(content);
    Assert.AreEqual(1, content.errors.Count);
    Assert.IsTrue(content.errors.Any(x => x.Equals("Email ou senha inválidos.")));
  }

  [TestMethod]
  public async Task AdminLogin_WithInvalidEmail_ShouldReturn_BAD_REQUEST()
  {
    //Arrange

    var body = new LoginDTO("admin@gmail.com", "123");

    //Act
    var response = await _client.PostAsJsonAsync(_baseUrl, body);

    //Assert
    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

    var content = JsonConvert.DeserializeObject<LoginResponse>(await response.Content.ReadAsStringAsync());

    Assert.IsNotNull(content);
    Assert.AreEqual(1, content.errors.Count);
    Assert.IsTrue(content.errors.Any(x => x.Equals("Email ou senha inválidos.")));
  }

  [TestMethod]
  public async Task EmployeeLogin_ShouldReturn_OK()
  {

    var body = new LoginDTO(_employee.Email.Address, "123");

    //Act
    var response = await _client.PostAsJsonAsync(_baseUrl, body);

    //Assert
    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

    var content = JsonConvert.DeserializeObject<LoginResponse>(await response.Content.ReadAsStringAsync());

    Assert.IsNotNull(content);
    var token = content.data.token;

    //Test the returned token
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    var responseGetRooms = await _client.GetAsync("v1/rooms");

    Assert.IsNotNull(responseGetRooms);
    Assert.AreEqual(HttpStatusCode.OK, responseGetRooms.StatusCode);
  }

  [TestMethod]
  public async Task EmployeeLogin_WithInvalidPassword_ShouldReturn_BAD_REQUEST()
  {
    //Arrange
    var body = new LoginDTO(_employee.Email.Address, "123756");

    //Act
    var response = await _client.PostAsJsonAsync(_baseUrl, body);

    //Assert
    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

    var content = JsonConvert.DeserializeObject<LoginResponse>(await response.Content.ReadAsStringAsync());

    Assert.IsNotNull(content);
    Assert.AreEqual(1, content.errors.Count);
    Assert.IsTrue(content.errors.Any(x => x.Equals("Email ou senha inválidos.")));
  }

  [TestMethod]
  public async Task EmployeeLogin_WithInvalidEmail_ShouldReturn_BAD_REQUEST()
  {
    //Arrange

    var body = new LoginDTO("admin@gmail.com", "123");

    //Act
    var response = await _client.PostAsJsonAsync(_baseUrl, body);

    //Assert
    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

    var content = JsonConvert.DeserializeObject<LoginResponse>(await response.Content.ReadAsStringAsync());

    Assert.IsNotNull(content);
    Assert.AreEqual(1, content.errors.Count);
    Assert.IsTrue(content.errors.Any(x => x.Equals("Email ou senha inválidos.")));
  }
}



public record LoginData(string token);
public record LoginResponse(int status, string message, LoginData data, List<string> errors);
