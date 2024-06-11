using Hotel.Domain.Data;
using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.Entities.AdminContext.AdminEntity;
using Hotel.Domain.Entities.AdminContext.PermissionEntity;
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
public class AdminControllerTests
{
  private static HotelWebApplicationFactory _factory = null!;
  private static HttpClient _client = null!;
  private static TokenService _tokenService = null!;
  private static HotelDbContext _dbContext = null!;
  private static string _rootAdminToken = null!;
  private const string _baseUrl = "v1/admins";

  [ClassInitialize]
  public static void ClassInitialize(TestContext context)
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
  public async Task GetAdmin_ShouldReturn_OK()
  {
    //Arrange
    var admin = new Admin
    (
      new Name("Jennifer", "Lawrence"),
      new Email("jenniferLawrenceOfficial@gmail.com"),
      new Phone("+44 (20) 97890-1234"),
      "789",
      EGender.Feminine,
      DateTime.Now.AddYears(-30),
      new Address("United States", "Los Angeles", "US-456", 789)
    );

    await _dbContext.Admins.AddAsync(admin);
    await _dbContext.SaveChangesAsync();

    //Act
    var response = await _client.GetAsync($"{_baseUrl}?take=1");

    //Assert
    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
  }

  [TestMethod]
  public async Task GetAdminById_ShouldReturn_OK()
  {
    //Arrange
    var admin = new Admin
    (
      new Name("Ana", "Souza"),
      new Email("anaSouzaOfficial@gmail.com"),
      new Phone("+55 (31) 91234-5678"),
      "789",
      EGender.Feminine,
      DateTime.Now.AddYears(-28),
      new Address("Brazil", "Belo Horizonte", "BR-123", 789)
    );

    await _dbContext.Admins.AddAsync(admin);
    await _dbContext.SaveChangesAsync();

    //Act
    var response = await _client.GetAsync($"{_baseUrl}/{admin.Id}");

    //Assert
    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
  }

  [TestMethod]
  public async Task DeleteAdmin_ShouldReturn_OK()
  {
    //Arrange
    var admin = new Admin
    (
      new Name("João", "Pereira"),
      new Email("joaoPereira@gmail.com"),
      new Phone("+55 (21) 98765-4321"),
      "password2",
      EGender.Masculine,
      DateTime.Now.AddYears(-30),
      new Address("Brazil", "Rio de Janeiro", "RJ-202", 202)
    );

    await _dbContext.Admins.AddAsync(admin);
    await _dbContext.SaveChangesAsync();

    //Act
    var response = await _client.DeleteAsync($"{_baseUrl}/{admin.Id}");

    //Assert
    var wasNotDeleted = await _dbContext.Admins.AnyAsync(x => x.Id == admin.Id);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    Assert.IsFalse(wasNotDeleted);
  }

  [TestMethod]
  public async Task DeleteLoggedAdmin_ShouldReturn_OK()
  {
    //Arrange
    var admin = new Admin
    (
      new Name("Emma", "Watson"),
      new Email("emmaWatson@gmail.com"),
      new Phone("+44 (20) 99346-1912"),
      "123",
      EGender.Feminine,
      DateTime.Now.AddYears(-31),
      new Address("United Kingdom", "London", "UK-123", 456)
    );

    var token = _tokenService.GenerateToken(admin);
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    var dbContext = _factory.Services.GetRequiredService<HotelDbContext>();
    await dbContext.Admins.AddAsync(admin);
    await dbContext.SaveChangesAsync();

    //Act
    var response = await _client.DeleteAsync(_baseUrl);

    //Assert
    var wasNotDeleted = await dbContext.Admins.AnyAsync(x => x.Id == admin.Id);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    Assert.IsFalse(wasNotDeleted);
  }

  [TestMethod]
  public async Task AddAdminPermission_ShouldReturn_OK()
  {
    //Arrange
    var admin = new Admin
    (
      new Name("Carlos", "Silva"),
      new Email("carlosSilvl@gmail.com"),
      new Phone("+55 (21) 99876-5432"),
      "456",
      EGender.Masculine,
      DateTime.Now.AddYears(-35),
      new Address("Brazil", "Rio de Janeiro", "BR-789", 123)
    );

    var permission = new Permission("GetAdmin", "Allows get admin by id");

    await _dbContext.Admins.AddAsync(admin);
    await _dbContext.Permissions.AddAsync(permission);
    await _dbContext.SaveChangesAsync();

    //Act
    var response = await _client.PostAsync($"{_baseUrl}/{admin.Id}/permissions/{permission.Id}", null);

    //Assert
    var adminWithPermissions = await _dbContext.Admins
      .Include(x => x.Permissions)
      .FirstOrDefaultAsync(x => x.Id == admin.Id);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    Assert.AreEqual(adminWithPermissions!.Id, admin.Id);
    Assert.AreEqual(adminWithPermissions.Permissions.First().Id, permission.Id);
  }

  [TestMethod]
  public async Task RemoveAdminPermission_ShouldReturn_OK()
  {
    var permission = new Permission("CreateAdmin", "Allows create admin");

    //Arrange
    var admin = new Admin
    (
      new Name("Mariana", "Lima"),
      new Email("marianaLima@gmail.com"),
      new Phone("+55 (11) 91234-4678"),
      "password1",
      EGender.Feminine,
      DateTime.Now.AddYears(-25),
      new Address("Brazil", "São Paulo", "SP-101", 101),
      [permission]
    );

    await _dbContext.Permissions.AddAsync(permission);
    await _dbContext.Admins.AddAsync(admin);
    await _dbContext.SaveChangesAsync();

    //Act
    var response = await _client.DeleteAsync($"{_baseUrl}/{admin.Id}/permissions/{permission.Id}");

    //Assert
    var adminWithPermissions = await _dbContext.Admins
      .Where(x => x.Id == admin.Id)
      .Include(x => x.Permissions)
      .FirstOrDefaultAsync();

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    Assert.AreEqual(adminWithPermissions!.Id, admin.Id);
    Assert.AreEqual(adminWithPermissions.Permissions.Count, 0);
  }

  //[TestMethod]
  //public async Task UpdateToRootAdmin_ShouldReturn_OK()
  //{
  //  //Arrange
  //  var admin = new Admin
  //  (
  //    new Name("Beatriz", "Santos"),
  //    new Email("beatrizSantos@gmail.com"),
  //    new Phone("+55 (31) 99876-5432"),
  //    "password3",
  //    EGender.Feminine,
  //    DateTime.Now.AddYears(-27),
  //    new Address("Brazil", "Belo Horizonte", "MG-303", 303)
  //  );

  //  await _dbContext.Admins.AddAsync(admin);
  //  await _dbContext.SaveChangesAsync();

  //  //Act
  //  var response = await _client.PostAsync($"{_baseUrl}/to-root-admin/{admin.Id}", null);

  //  //Assert
  //  var updatedAdmin = await _dbContext.Admins.FirstOrDefaultAsync(x => x.Id == admin.Id);

  //  Assert.IsNotNull(response);
  //  Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
  //  Assert.IsTrue(updatedAdmin!.IsRootAdmin);
  //}

  [TestMethod]
  public async Task UpdateAdmin_ShouldReturn_OK()
  {
    //Arrange
    var admin = new Admin
    (
      new Name("Rafael", "Oliveira"),
      new Email("rafaelOliveira@gmail.com"),
      new Phone("+55 (41) 97654-3210"),
      "password4",
      EGender.Masculine,
      DateTime.Now.AddYears(-32),
      new Address("Brazil", "Curitiba", "PR-404", 404)
    );

    await _dbContext.Admins.AddAsync(admin);
    await _dbContext.SaveChangesAsync();

    var body = new UpdateUser("Jão", "Pedro", "+55 (41) 97654-3210", EGender.Feminine, DateTime.Now.AddYears(-20), "Brazil", "Curitiba", "PR-404", 404);

    //Act
    var response = await _client.PutAsJsonAsync($"{_baseUrl}/{admin.Id}", body);

    //Assert
    var updatedAdmin = await _dbContext.Admins.FirstOrDefaultAsync(x => x.Id == admin.Id);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    Assert.AreEqual(updatedAdmin!.Id, admin.Id);
    Assert.AreEqual(updatedAdmin.Name.FirstName, body.FirstName);
    Assert.AreEqual(updatedAdmin.Name.LastName, body.LastName);
    Assert.AreEqual(updatedAdmin.Phone.Number, body.Phone);
    Assert.AreEqual(updatedAdmin.Gender, body.Gender);
    Assert.AreEqual(updatedAdmin.DateOfBirth, body.DateOfBirth);
    Assert.AreEqual(updatedAdmin!.Address!.Country, body.Country);
    Assert.AreEqual(updatedAdmin!.Address.City, body.City);
    Assert.AreEqual(updatedAdmin!.Address!.Number, body.Number);
    Assert.AreEqual(updatedAdmin!.Address.Street, body.Street);
  }


  [TestMethod]
  public async Task UpdateLoggedAdmin_ShouldReturn_OK()
  {
    //Arrange
    var admin = new Admin
    (
      new Name("Camila", "Costa"),
      new Email("camilaCosta@gmail.com"),
      new Phone("+55 (71) 93456-7890"),
      "password5",
      EGender.Feminine,
      DateTime.Now.AddYears(-29),
      new Address("Brazil", "Salvador", "BA-505", 505)
    );

    await _dbContext.Admins.AddAsync(admin);
    await _dbContext.SaveChangesAsync();

    var token = _tokenService.GenerateToken(admin);
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    var body = new UpdateUser("Jão", "Pedro", "+55 (41) 93651-3210", EGender.Feminine, DateTime.Now.AddYears(-20), "Brazil", "Curitiba", "PR-404", 404);

    //Act
    var response = await _client.PutAsJsonAsync(_baseUrl, body);

    //Assert
    var updatedAdmin = await _dbContext.Admins.FirstOrDefaultAsync(x => x.Id == admin.Id);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    Assert.AreEqual(updatedAdmin!.Id, admin.Id);
    Assert.AreEqual(updatedAdmin.Name.FirstName, body.FirstName);
    Assert.AreEqual(updatedAdmin.Name.LastName, body.LastName);
    Assert.AreEqual(updatedAdmin.Phone.Number, body.Phone);
    Assert.AreEqual(updatedAdmin.Gender, body.Gender);
    Assert.AreEqual(updatedAdmin.DateOfBirth, body.DateOfBirth);
    Assert.AreEqual(updatedAdmin!.Address!.Country, body.Country);
    Assert.AreEqual(updatedAdmin!.Address.City, body.City);
    Assert.AreEqual(updatedAdmin!.Address!.Number, body.Number);
    Assert.AreEqual(updatedAdmin!.Address.Street, body.Street);
  }

  [TestMethod]
  public async Task UpdateAdminName_ShouldReturn_OK()
  {
    //Arrange
    var admin = new Admin
    (
      new Name("Lucas", "Ferreira"),
      new Email("lucasFerreira@gmail.com"),
      new Phone("+55 (61) 92345-6789"),
      "password6",
      EGender.Masculine,
      DateTime.Now.AddYears(-28),
      new Address("Brazil", "Brasília", "DF-606", 606)
    );

    await _dbContext.Admins.AddAsync(admin);
    await _dbContext.SaveChangesAsync();

    var token = _tokenService.GenerateToken(admin);
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    var body = new Name("John", "Wick");

    //Act
    var response = await _client.PatchAsJsonAsync($"{_baseUrl}/name", body);

    //Assert
    var updatedAdmin = await _dbContext.Admins.FirstOrDefaultAsync(x => x.Id == admin.Id);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    Assert.AreEqual(updatedAdmin!.Name.FirstName, body.FirstName);
    Assert.AreEqual(updatedAdmin.Name.LastName, body.LastName);
  }

  [TestMethod]
  public async Task UpdateEmailAdmin_ShouldReturn_OK()
  {
    //Arrange
    var admin = new Admin
    (
      new Name("Fernanda", "Ribeiro"),
      new Email("fernandaRibeiro@gmail.com"),
      new Phone("+55 (51) 91234-5678"),
      "password7",
      EGender.Feminine,
      DateTime.Now.AddYears(-26),
      new Address("Brazil", "Porto Alegre", "RS-707", 707)
    );

    await _dbContext.Admins.AddAsync(admin);
    await _dbContext.SaveChangesAsync();

    var token = _tokenService.GenerateToken(admin);
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    var body = new Email("feeRriber@gmail.com");

    //Act
    var response = await _client.PatchAsJsonAsync($"{_baseUrl}/email", body);

    //Assert
    var updatedAdmin = await _dbContext.Admins.FirstOrDefaultAsync(x => x.Id == admin.Id);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    Assert.AreEqual(updatedAdmin!.Email.Address, body.Address);
  }

  [TestMethod]
  public async Task UpdatePhoneAdmin_ShouldReturn_OK()
  {
    //Arrange
    var admin = new Admin
    (
      new Name("Pedro", "Silva"),
      new Email("pedroSilvaa100@gmail.com"),
      new Phone("+55 (62) 99846-1432"),
      "password8",
      EGender.Masculine,
      DateTime.Now.AddYears(-31),
      new Address("Brazil", "Goiânia", "GO-808", 808)
    );

    await _dbContext.Admins.AddAsync(admin);
    await _dbContext.SaveChangesAsync();

    var token = _tokenService.GenerateToken(admin);
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    var body = new Phone("+55 (62) 99156-3449");

    //Act
    var response = await _client.PatchAsJsonAsync($"{_baseUrl}/phone", body);

    //Assert
    var updatedAdmin = await _dbContext.Admins.FirstOrDefaultAsync(x => x.Id == admin.Id);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    Assert.AreEqual(updatedAdmin!.Phone.Number, body.Number);
  }

  [TestMethod]
  public async Task UpdateAddressAdmin_ShouldReturn_OK()
  {
    //Arrange
    var admin = new Admin
    (
      new Name("Pedro", "Silva"),
      new Email("pedroSilva999@gmail.com"),
      new Phone("+55 (62) 99876-3432"),
      "password8",
      EGender.Masculine,
      DateTime.Now.AddYears(-31),
      new Address("Brazil", "Goiânia", "GO-808", 808)
    );

    await _dbContext.Admins.AddAsync(admin);
    await _dbContext.SaveChangesAsync();

    var token = _tokenService.GenerateToken(admin);
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    var body = new Address("Brazil", "Florianópolis", "SC-909", 909);

    //Act
    var response = await _client.PatchAsJsonAsync($"{_baseUrl}/address", body);

    //Assert
    var updatedAdmin = await _dbContext.Admins.FirstOrDefaultAsync(x => x.Id == admin.Id);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    Assert.AreEqual(updatedAdmin!.Address!.Country, body.Country);
    Assert.AreEqual(updatedAdmin!.Address.City, body.City);
    Assert.AreEqual(updatedAdmin!.Address!.Number, body.Number);
    Assert.AreEqual(updatedAdmin!.Address.Street, body.Street);
  }

  [TestMethod]
  public async Task UpdateGenderAdmin_ShouldReturn_OK()
  {
    //Arrange
    var admin = new Admin
    (
      new Name("Gustavo", "Souza"),
      new Email("gustavoSouza@gmail.com"),
      new Phone("+55 (27) 93456-7890"),
      "password10",
      EGender.Masculine,
      DateTime.Now.AddYears(-33),
      new Address("Brazil", "Vitória", "ES-1010", 1010)
    );

    await _dbContext.Admins.AddAsync(admin);
    await _dbContext.SaveChangesAsync();

    var token = _tokenService.GenerateToken(admin);
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    //Act
    var response = await _client.PatchAsJsonAsync($"{_baseUrl}/gender/2", new { });

    //Assert
    var updatedAdmin = await _dbContext.Admins.FirstOrDefaultAsync(x => x.Id == admin.Id);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    Assert.AreEqual(EGender.Feminine ,updatedAdmin!.Gender);
  }

  [TestMethod]
  public async Task UpdateDateOfBirthAdmin_ShouldReturn_OK()
  {
    //Arrange
    var admin = new Admin
    (
      new Name("Gabriel", "Souza"),
      new Email("gabriSouza@gmail.com"),
      new Phone("+55 (27) 93153-7810"),
      "password10",
      EGender.Masculine,
      DateTime.Now.AddYears(-33),
      new Address("Brazil", "Vitória", "ES-1011", 1011)
    );

    await _dbContext.Admins.AddAsync(admin);
    await _dbContext.SaveChangesAsync();

    var token = _tokenService.GenerateToken(admin);
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    var body = new UpdateDateOfBirth(DateTime.Now.AddYears(-35));

    //Act
    var response = await _client.PatchAsJsonAsync($"{_baseUrl}/date-of-birth", body);

    //Assert
    var updatedAdmin = await _dbContext.Admins.FirstOrDefaultAsync(x => x.Id == admin.Id);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    Assert.AreEqual(body.DateOfBirth, updatedAdmin!.DateOfBirth);
  }



  //
  //TESTES DE PERMISSÂO
  //

  [TestMethod]
  public async Task AdminWithoutPermissionToGet_ShouldReturn_OK()
  {
    //Arrange
    var admin = new Admin
    (
      new Name("Gabriel", "Souza"),
      new Email("gabriSouza@gmail.com"),
      new Phone("+55 (27) 93153-7810"),
      "password10",
      EGender.Masculine,
      DateTime.Now.AddYears(-33),
      new Address("Brazil", "Vitória", "ES-1011", 1011)
    );

    await _dbContext.Admins.AddAsync(admin);
    await _dbContext.SaveChangesAsync();

    var token = _tokenService.GenerateToken(admin);
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    var body = new UpdateDateOfBirth(DateTime.Now.AddYears(-35));

    //Act
    var response = await _client.PatchAsJsonAsync($"{_baseUrl}/date-of-birth", body);

    //Assert
    var updatedAdmin = await _dbContext.Admins.FirstOrDefaultAsync(x => x.Id == admin.Id);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    Assert.AreEqual(body.DateOfBirth, updatedAdmin!.DateOfBirth);
  }


}
