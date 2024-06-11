using Hotel.Domain.Data;
using Hotel.Domain.Entities.AdminContext.PermissionEntity;
using Hotel.Domain.Services.TokenServices;
using Hotel.Tests.IntegrationTests.Factories;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Headers;

namespace Hotel.Tests.IntegrationTests.Controllers;

[TestClass]
public class PermissionControllerTests
{
  private static HotelWebApplicationFactory _factory = null!;
  private static HttpClient _client = null!;
  private static HotelDbContext _dbContext = null!;
  private static string _rootAdminToken = null!; //Allows access all endpoints
  private const string _baseUrl = "v1/permissions";

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

  [TestInitialize]
  public void TestInitialize()
  {
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _rootAdminToken);
  }


  [TestMethod]
  public async Task GetPermissions_ShouldReturn_OK()
  {
    //Arrange
    var permission = new Permission("CreateService", "Allows create an service");

    await _dbContext.Permissions.AddAsync(permission);
    await _dbContext.SaveChangesAsync();

    //Act
    var response = await _client.GetAsync($"{_baseUrl}?take=1");

    //Assert
    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
  }

  [TestMethod]
  public async Task GetPermissionById_ShouldReturn_OK()
  {
    //Arrange
    var permission = new Permission("UpdateAdminName","Allows update admin name");

    await _dbContext.Permissions.AddAsync(permission);
    await _dbContext.SaveChangesAsync();

    //Act
    var response = await _client.GetAsync($"{_baseUrl}/{permission.Id}");

    //Assert
    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
  }

}
