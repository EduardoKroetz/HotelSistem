using Hotel.Domain.Data;
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

namespace Hotel.Tests.IntegrationTests.Controllers;

[TestClass]
public class AdminControllerTests
{
  private static HotelWebApplicationFactory _factory = null!;
  private static HttpClient _client = null!;
  private static TokenService _tokenService = null!;
  private static HotelDbContext _dbContext = null!;
  private const string _baseUrl = "v1/admins";

  [ClassInitialize]
  public static void ClassInitialize(TestContext context)
  {
    _factory = new HotelWebApplicationFactory();
    _client = _factory.CreateClient();
    _dbContext = _factory.Services.GetRequiredService<HotelDbContext>();
    _tokenService = _factory.Services.GetRequiredService<TokenService>();

    var token = _factory.LoginFullAccess().Result;
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
  }

  [ClassCleanup]
  public static void ClassCleanup()
  {
    _factory.Dispose();
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

    var client = _factory.CreateClient();
    var token = _tokenService.GenerateToken(admin);
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    var dbContext = _factory.Services.GetRequiredService<HotelDbContext>();
    await dbContext.Admins.AddAsync(admin);
    await dbContext.SaveChangesAsync();

    //Act
    var response = await client.DeleteAsync(_baseUrl);

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
    var permission = new Permission("GetAdmin", "Allows get admin by id");

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

  //  var adminRoot = await _dbContext.Admins.ToListAsync();

  //  //Act
  //  var response = await _client.PostAsync($"{_baseUrl}/to-root-admin/{admin.Id}", null);

  //  //Assert
  //  var updatedAdmin = await _dbContext.Admins.FirstOrDefaultAsync(x => x.Id == admin.Id);
      
  //  Assert.IsNotNull(response);
  //  Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
  //  Assert.IsTrue(updatedAdmin!.IsRootAdmin);
  //}
}
