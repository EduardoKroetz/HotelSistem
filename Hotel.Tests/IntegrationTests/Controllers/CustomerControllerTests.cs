using Hotel.Domain.Data;
using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.Entities.CustomerContext;
using Hotel.Domain.Enums;
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
public class CustomerControllerTests
{
  private static HotelWebApplicationFactory _factory = null!;
  private static HttpClient _client = null!;
  private static HotelDbContext _dbContext = null!;
  private static string _rootAdminToken = null!;
  private const string _baseUrl = "v1/customers";
  private static TokenService _tokenService = null!;

  [ClassInitialize]
  public static void ClassInitialize(TestContext? context)
  {
    _factory = new HotelWebApplicationFactory();
    _client = _factory.CreateClient();
    _dbContext = _factory.Services.GetRequiredService<HotelDbContext>();
    _tokenService = _factory.Services.GetRequiredService<TokenService>();

    _rootAdminToken = _factory.LoginFullAccess().Result;
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _rootAdminToken);
  }

  [ClassCleanup]
  public static void ClassCleanup()
  {
    _factory.Dispose();
  }

  [TestInitialize]
  public void TestInitialize()
  {
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _rootAdminToken);
  }

  [TestMethod]
  public async Task GetCustomer_ShouldReturn_OK()
  {
    //Arrange
    var customer = new Customer
    (
      new Name("Jennifer", "Lawrence"),
      new Email("jenniferLawrenceOfficial@gmail.com"),
      new Phone("+44 (20) 97890-1234"),
      "789",
      EGender.Feminine,
      DateTime.Now.AddYears(-30),
      new Address("United States", "Los Angeles", "US-456", 789)
    );

    await _dbContext.Customers.AddAsync(customer);
    await _dbContext.SaveChangesAsync();

    //Act
    var response = await _client.GetAsync($"{_baseUrl}?take=1");

    //Assert
    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
  }

  [TestMethod]
  public async Task GetCustomerById_ShouldReturn_OK()
  {
    //Arrange
    var customer = new Customer
    (
      new Name("Ana", "Souza"),
      new Email("anaSouzaOfficial@gmail.com"),
      new Phone("+55 (31) 91234-5678"),
      "789",
      EGender.Feminine,
      DateTime.Now.AddYears(-28),
      new Address("Brazil", "Belo Horizonte", "BR-123", 789)
    );

    await _dbContext.Customers.AddAsync(customer);
    await _dbContext.SaveChangesAsync();

    //Act
    var response = await _client.GetAsync($"{_baseUrl}/{customer.Id}");

    //Assert
    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
  }

  [TestMethod]
  public async Task DeleteCustomer_ShouldReturn_OK()
  {
    //Arrange
    var customer = new Customer
    (
      new Name("João", "Pereira"),
      new Email("joaoPereira@gmail.com"),
      new Phone("+55 (21) 98765-4321"),
      "password2",
      EGender.Masculine,
      DateTime.Now.AddYears(-30),
      new Address("Brazil", "Rio de Janeiro", "RJ-202", 202)
    );

    await _dbContext.Customers.AddAsync(customer);
    await _dbContext.SaveChangesAsync();

    //Act
    var response = await _client.DeleteAsync($"{_baseUrl}/{customer.Id}");

    //Assert
    var wasNotDeleted = await _dbContext.Customers.AnyAsync(x => x.Id == customer.Id);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    Assert.IsFalse(wasNotDeleted);
  }

  [TestMethod]
  public async Task DeleteLoggedCustomer_ShouldReturn_OK()
  {
    //Arrange
    var customer = new Customer
    (
      new Name("Emma", "Watson"),
      new Email("emmaWatson@gmail.com"),
    new Phone("+44 (20) 99346-1912"),
      "123",
      EGender.Feminine,
      DateTime.Now.AddYears(-31),
      new Address("United Kingdom", "London", "UK-123", 456)
    );

    var token = _tokenService.GenerateToken(customer);
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    var dbContext = _factory.Services.GetRequiredService<HotelDbContext>();
    await dbContext.Customers.AddAsync(customer);
    await dbContext.SaveChangesAsync();

    //Act
    var response = await _client.DeleteAsync(_baseUrl);

    //Assert
    var wasNotDeleted = await dbContext.Customers.AnyAsync(x => x.Id == customer.Id);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    Assert.IsFalse(wasNotDeleted);
  }

  [TestMethod]
  public async Task UpdateCustomer_ShouldReturn_OK()
  {
    //Arrange
    var customer = new Customer
    (
      new Name("Rafael", "Oliveira"),
      new Email("rafaelOliveira@gmail.com"),
      new Phone("+55 (41) 97654-3210"),
      "password4",
      EGender.Masculine,
      DateTime.Now.AddYears(-32),
      new Address("Brazil", "Curitiba", "PR-404", 404)
    );

    await _dbContext.Customers.AddAsync(customer);
    await _dbContext.SaveChangesAsync();

    var body = new UpdateUser("Jão", "Pedro", "+55 (41) 97654-3210", EGender.Feminine, DateTime.Now.AddYears(-20), "Brazil", "Curitiba", "PR-404", 404);

    //Act
    var response = await _client.PutAsJsonAsync($"{_baseUrl}/{customer.Id}", body);

    //Assert
    var updatedCustomer = await _dbContext.Customers.FirstOrDefaultAsync(x => x.Id == customer.Id);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    Assert.AreEqual(updatedCustomer!.Id, customer.Id);
    Assert.AreEqual(updatedCustomer.Name.FirstName, body.FirstName);
    Assert.AreEqual(updatedCustomer.Name.LastName, body.LastName);
    Assert.AreEqual(updatedCustomer.Phone.Number, body.Phone);
    Assert.AreEqual(updatedCustomer.Gender, body.Gender);
    Assert.AreEqual(updatedCustomer.DateOfBirth, body.DateOfBirth);
    Assert.AreEqual(updatedCustomer!.Address!.Country, body.Country);
    Assert.AreEqual(updatedCustomer!.Address.City, body.City);
    Assert.AreEqual(updatedCustomer!.Address!.Number, body.Number);
    Assert.AreEqual(updatedCustomer!.Address.Street, body.Street);
  }


  [TestMethod]
  public async Task UpdateLoggedCustomer_ShouldReturn_OK()
  {
    //Arrange
    var customer = new Customer
    (
      new Name("Camila", "Costa"),
      new Email("camilaCosta@gmail.com"),
      new Phone("+55 (71) 93456-7890"),
      "password5",
      EGender.Feminine,
      DateTime.Now.AddYears(-29),
      new Address("Brazil", "Salvador", "BA-505", 505)
    );

    await _dbContext.Customers.AddAsync(customer);
    await _dbContext.SaveChangesAsync();

    var token = _tokenService.GenerateToken(customer);
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    var body = new UpdateUser("Jão", "Pedro", "+55 (41) 93651-3210", EGender.Feminine, DateTime.Now.AddYears(-20), "Brazil", "Curitiba", "PR-404", 404);

    //Act
    var response = await _client.PutAsJsonAsync(_baseUrl, body);

    //Assert
    var updatedCustomer = await _dbContext.Customers.FirstOrDefaultAsync(x => x.Id == customer.Id);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    Assert.AreEqual(updatedCustomer!.Id, customer.Id);
    Assert.AreEqual(updatedCustomer.Name.FirstName, body.FirstName);
    Assert.AreEqual(updatedCustomer.Name.LastName, body.LastName);
    Assert.AreEqual(updatedCustomer.Phone.Number, body.Phone);
    Assert.AreEqual(updatedCustomer.Gender, body.Gender);
    Assert.AreEqual(updatedCustomer.DateOfBirth, body.DateOfBirth);
    Assert.AreEqual(updatedCustomer!.Address!.Country, body.Country);
    Assert.AreEqual(updatedCustomer!.Address.City, body.City);
    Assert.AreEqual(updatedCustomer!.Address!.Number, body.Number);
    Assert.AreEqual(updatedCustomer!.Address.Street, body.Street);
  }

  [TestMethod]
  public async Task UpdateCustomerName_ShouldReturn_OK()
  {
    //Arrange
    var customer = new Customer
    (
      new Name("Lucas", "Ferreira"),
      new Email("lucasFerreira@gmail.com"),
      new Phone("+55 (61) 92345-6789"),
      "password6",
      EGender.Masculine,
      DateTime.Now.AddYears(-28),
      new Address("Brazil", "Brasília", "DF-606", 606)
    );

    await _dbContext.Customers.AddAsync(customer);
    await _dbContext.SaveChangesAsync();

    var token = _tokenService.GenerateToken(customer);
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    var body = new Name("John", "Wick");

    //Act
    var response = await _client.PatchAsJsonAsync($"{_baseUrl}/name", body);

    //Assert
    var updatedCustomer = await _dbContext.Customers.FirstOrDefaultAsync(x => x.Id == customer.Id);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    Assert.AreEqual(updatedCustomer!.Name.FirstName, body.FirstName);
    Assert.AreEqual(updatedCustomer.Name.LastName, body.LastName);
  }

  [TestMethod]
  public async Task UpdateEmailCustomer_ShouldReturn_OK()
  {
    //Arrange
    var customer = new Customer
    (
      new Name("Fernanda", "River"),
      new Email("fernandaRiver@gmail.com"),
      new Phone("+55 (51) 91219-5678"),
      "password7",
      EGender.Feminine,
      DateTime.Now.AddYears(-26),
      new Address("Brazil", "Porto Alegre", "RS-707", 707)
    );

    await _dbContext.Customers.AddAsync(customer);
    await _dbContext.SaveChangesAsync();

    var token = _tokenService.GenerateToken(customer);
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    var body = new Email("feeRriber@gmail.com");

    //Act
    var response = await _client.PatchAsJsonAsync($"{_baseUrl}/email", body);

    //Assert
    var updatedCustomer = await _dbContext.Customers.FirstOrDefaultAsync(x => x.Id == customer.Id);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    Assert.AreEqual(updatedCustomer!.Email.Address, body.Address);
  }

  [TestMethod]
  public async Task UpdatePhoneCustomer_ShouldReturn_OK()
  {
    //Arrange
    var customer = new Customer
    (
      new Name("Michele", "Silva"),
      new Email("micheleSilvaa100@gmail.com"),
      new Phone("+55 (62) 99846-1432"),
      "password8",
      EGender.Masculine,
      DateTime.Now.AddYears(-31),
      new Address("Brazil", "Goiânia", "GO-808", 808)
    );

    await _dbContext.Customers.AddAsync(customer);
    await _dbContext.SaveChangesAsync();

    var token = _tokenService.GenerateToken(customer);
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    var body = new Phone("+55 (62) 99156-3449");

    //Act
    var response = await _client.PatchAsJsonAsync($"{_baseUrl}/phone", body);

    //Assert
    var updatedCustomer = await _dbContext.Customers.FirstOrDefaultAsync(x => x.Id == customer.Id);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    Assert.AreEqual(updatedCustomer!.Phone.Number, body.Number);
  }

  [TestMethod]
  public async Task UpdateAddressCustomer_ShouldReturn_OK()
  {
    //Arrange
    var customer = new Customer
    (
      new Name("Vinicius", "Silva"),
      new Email("viniSilva@gmail.com"),
      new Phone("+55 (62) 91876-3432"),
      "password8",
      EGender.Masculine,
      DateTime.Now.AddYears(-31),
      new Address("Brazil", "Goiânia", "GO-808", 808)
    );

    await _dbContext.Customers.AddAsync(customer);
    await _dbContext.SaveChangesAsync();

    var token = _tokenService.GenerateToken(customer);
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    var body = new Address("Brazil", "Florianópolis", "SC-909", 909);

    //Act
    var response = await _client.PatchAsJsonAsync($"{_baseUrl}/address", body);

    //Assert
    var updatedCustomer = await _dbContext.Customers.FirstOrDefaultAsync(x => x.Id == customer.Id);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    Assert.AreEqual(updatedCustomer!.Address!.Country, body.Country);
    Assert.AreEqual(updatedCustomer!.Address.City, body.City);
    Assert.AreEqual(updatedCustomer!.Address!.Number, body.Number);
    Assert.AreEqual(updatedCustomer!.Address.Street, body.Street);
  }

  [TestMethod]
  public async Task UpdateGenderCustomer_ShouldReturn_OK()
  {
    //Arrange
    var customer = new Customer
    (
      new Name("Gustavo", "Souza"),
      new Email("gustavoSouza@gmail.com"),
      new Phone("+55 (27) 93456-7890"),
      "password10",
      EGender.Masculine,
      DateTime.Now.AddYears(-33),
      new Address("Brazil", "Vitória", "ES-1010", 1010)
    );

    await _dbContext.Customers.AddAsync(customer);
    await _dbContext.SaveChangesAsync();

    var token = _tokenService.GenerateToken(customer);
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    //Act
    var response = await _client.PatchAsJsonAsync($"{_baseUrl}/gender/2", new { });

    //Assert
    var updatedCustomer = await _dbContext.Customers.FirstOrDefaultAsync(x => x.Id == customer.Id);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    Assert.AreEqual(EGender.Feminine, updatedCustomer!.Gender);
  }

  [TestMethod]
  public async Task UpdateDateOfBirthCustomer_ShouldReturn_OK()
  {
    //Arrange
    var customer = new Customer
    (
      new Name("Geovane", "Silva"),
      new Email("geoSilv@gmail.com"),
      new Phone("+55 (27) 93113-7859"),
      "password10",
      EGender.Masculine,
      DateTime.Now.AddYears(-33),
      new Address("Brazil", "Vitória", "ES-1011", 1011)
    );

    await _dbContext.Customers.AddAsync(customer);
    await _dbContext.SaveChangesAsync();

    var token = _tokenService.GenerateToken(customer);
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    var body = new UpdateDateOfBirth(DateTime.Now.AddYears(-35));

    //Act
    var response = await _client.PatchAsJsonAsync($"{_baseUrl}/date-of-birth", body);

    //Assert
    var updatedCustomer = await _dbContext.Customers.FirstOrDefaultAsync(x => x.Id == customer.Id);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    Assert.AreEqual(body.DateOfBirth, updatedCustomer!.DateOfBirth);
  }
}
