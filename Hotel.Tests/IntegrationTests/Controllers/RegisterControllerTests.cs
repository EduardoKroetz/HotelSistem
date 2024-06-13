using Hotel.Domain.Data;
using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.DTOs.EmployeeContext.EmployeeDTOs;
using Hotel.Domain.Entities;
using Hotel.Domain.Entities.AdminContext.AdminEntity;
using Hotel.Domain.Entities.CustomerContext;
using Hotel.Domain.Enums;
using Hotel.Domain.ValueObjects;
using Hotel.Tests.IntegrationTests.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Hotel.Tests.IntegrationTests.Controllers;

[TestClass]
public class RegisterControllerTests
{
  private static HotelWebApplicationFactory _factory = null!;
  private static HttpClient _client = null!;
  private static HotelDbContext _dbContext = null!;
  private const string _baseUrl = "v1/register";
  private static string _rootAdminToken = null!;

  [ClassInitialize]
  public static void ClassInitialize(TestContext context)
  {
    _factory = new HotelWebApplicationFactory();
    _client = _factory.CreateClient();
    _dbContext = _factory.Services.GetRequiredService<HotelDbContext>();

    _rootAdminToken = _factory.LoginFullAccess().Result;
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _rootAdminToken);
  }

  [ClassCleanup]
  public static void ClassCleanup()
  {
    _factory.Dispose();
  }


  //Customers
  [TestMethod]
  public async Task CustomerRegister_ShouldReturn_OK()
  {
    //Arrange
    var body = new CreateUser("Jennifer", "Lawrence", "jenniferLawrenceOfficial@gmail.com", "+44 (20) 97890-1234", "123" ,EGender.Feminine,DateTime.Now.AddYears(-20), "United Kingdom", "London", "UK-123", 456);

    var toEmail = new Email(body.Email);
    var code = new VerificationCode(toEmail);

    await _dbContext.VerificationCodes.AddAsync(code);
    await _dbContext.SaveChangesAsync();

    //Act
    var response = await _client.PostAsJsonAsync($"{_baseUrl}/customers?code={code.Code}", body);

    //Assert

    var customer = await _dbContext.Customers.FirstAsync(x => x.Email.Address == body.Email);
    var existsCode = await _dbContext.VerificationCodes.AnyAsync(x => x.Code == code.Code);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    Assert.AreEqual(body.FirstName, customer.Name.FirstName);
    Assert.AreEqual(body.LastName, customer.Name.LastName);
    Assert.AreEqual(body.Email, customer.Email.Address);
    Assert.AreEqual(body.Phone, customer.Phone.Number);
    Assert.AreEqual(body.Gender, customer.Gender);
    Assert.AreEqual(body.DateOfBirth, customer.DateOfBirth);
    Assert.AreEqual(body.Country, customer.Address!.Country);
    Assert.AreEqual(body.City, customer.Address.City);
    Assert.AreEqual(body.Number, customer.Address.Number);
    Assert.AreEqual(body.Street, customer.Address.Street);

    Assert.IsFalse(existsCode);
  }

  [TestMethod]
  public async Task CustomerRegister_NoValidationCode_ShouldReturn_BAD_REQUEST()
  {
    //Arrange
    var body = new CreateUser("Robert", "Downey", "robertDowneyJr@gmail.com", "+1 (310) 123-4567", "234", EGender.Masculine, DateTime.Now.AddYears(-35), "United States", "New York", "US-456", 789);

    //Act
    var response = await _client.PostAsJsonAsync($"{_baseUrl}/customers", body);

    //Assert
    var customer = await _dbContext.Customers.FirstOrDefaultAsync(x => x.Email.Address == body.Email);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    Assert.IsNull(customer);

  }

  //Admins

  [TestMethod]
  public async Task AdminRegister_ShouldReturn_OK()
  {
    //Arrange
    var body = new CreateUser("Tom", "Hiddleston", "tomHiddlesto@gmail.com", "+44 (20) 98135-4321", "345", EGender.Masculine, DateTime.Now.AddYears(-39), "United Kingdom", "London", "UK-567", 203);

    var toEmail = new Email(body.Email);
    var code = new VerificationCode(toEmail);

    await _dbContext.VerificationCodes.AddAsync(code);
    await _dbContext.SaveChangesAsync();

    //Act
    var response = await _client.PostAsJsonAsync($"{_baseUrl}/admins?code={code.Code}", body);

    //Assert
    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

    var admin = await _dbContext.Admins.FirstAsync(x => x.Email.Address == body.Email);
    var existsCode = await _dbContext.VerificationCodes.AnyAsync(x => x.Code == code.Code);

    Assert.AreEqual(body.FirstName, admin.Name.FirstName);
    Assert.AreEqual(body.LastName, admin.Name.LastName);
    Assert.AreEqual(body.Email, admin.Email.Address);
    Assert.AreEqual(body.Phone, admin.Phone.Number);
    Assert.AreEqual(body.Gender, admin.Gender);
    Assert.AreEqual(body.DateOfBirth, admin.DateOfBirth);
    Assert.AreEqual(body.Country, admin.Address!.Country);
    Assert.AreEqual(body.City, admin.Address.City);
    Assert.AreEqual(body.Number, admin.Address.Number);
    Assert.AreEqual(body.Street, admin.Address.Street);

    Assert.IsFalse(existsCode);
  }

  [TestMethod]
  public async Task AdminRegister_NoValidationCode_ShouldReturn_BAD_REQUEST()
  {
    //Arrange
    var body = new CreateUser("Gal", "Gadot", "galGad@gmail.com", "+972 (31) 99123-4567", "456", EGender.Feminine, DateTime.Now.AddYears(-35), "Israel", "Tel Aviv", "IL-123", 204);

    //Act
    var response = await _client.PostAsJsonAsync($"{_baseUrl}/admins", body);

    //Assert
    var admin = await _dbContext.Admins.FirstOrDefaultAsync(x => x.Email.Address == body.Email);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    Assert.IsNull(admin);

  }

  //Employees

  [TestMethod]
  public async Task EmployeeRegister_ShouldReturn_OK()
  {
    //Arrange
    var body = new CreateEmployee("John", "Doe", "johndoe@example.com", "+99 (555) 99123-4567", "1234", EGender.Masculine, DateTime.Now.AddYears(-25), "United States", "New York", "US-ABC", 101, 3000);

    var toEmail = new Email(body.Email);
    var code = new VerificationCode(toEmail);

    await _dbContext.VerificationCodes.AddAsync(code);
    await _dbContext.SaveChangesAsync();

    //Act
    var response = await _client.PostAsJsonAsync($"{_baseUrl}/employees?code={code.Code}", body);

    //Assert
    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

    var employee = await _dbContext.Employees.FirstAsync(x => x.Email.Address == body.Email);
    var existsCode = await _dbContext.VerificationCodes.AnyAsync(x => x.Code == code.Code);

    Assert.AreEqual(body.FirstName, employee.Name.FirstName);
    Assert.AreEqual(body.LastName, employee.Name.LastName);
    Assert.AreEqual(body.Email, employee.Email.Address);
    Assert.AreEqual(body.Phone, employee.Phone.Number);
    Assert.AreEqual(body.Gender, employee.Gender);
    Assert.AreEqual(body.DateOfBirth, employee.DateOfBirth);
    Assert.AreEqual(body.Country, employee.Address!.Country);
    Assert.AreEqual(body.City, employee.Address.City);
    Assert.AreEqual(body.Number, employee.Address.Number);
    Assert.AreEqual(body.Street, employee.Address.Street);

    Assert.IsFalse(existsCode);
  }


  [TestMethod]
  public async Task EmployeeRegister_NoValidationCode_ShouldReturn_BAD_REQUEST()
  {
    //Arrange
    var body = new CreateEmployee("Jane", "Smith", "janesmith@example.com", "+44 (20) 7123-4567", "5678", EGender.Feminine, DateTime.Now.AddYears(-30), "United Kingdom", "Manchester", "UK-XYZ", 202, 2500);

    //Act
    var response = await _client.PostAsJsonAsync($"{_baseUrl}/employees", body);

    //Assert
    var employee = await _dbContext.Employees.FirstOrDefaultAsync(x => x.Email.Address == body.Email);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    Assert.IsNull(employee);
  }


}


