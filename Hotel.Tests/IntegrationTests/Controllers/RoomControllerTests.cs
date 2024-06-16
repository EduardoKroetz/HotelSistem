using Hotel.Domain.Data;
using Hotel.Tests.IntegrationTests.Factories;
using Microsoft.Extensions.DependencyInjection;


namespace Hotel.Tests.IntegrationTests.Controllers;

[TestClass]
public class RoomReportControllerTests
{
  private static HotelWebApplicationFactory _factory = null!;
  private static HttpClient _client = null!;
  private static HotelDbContext _dbContext = null!;
  private static string _rootAdminToken = null!;
  private const string _baseUrl = "v1/rooms";

  [ClassInitialize]
  public static void ClassInitialize(TestContext context)
  {
    _factory = new HotelWebApplicationFactory();
    _client = _factory.CreateClient();
    _dbContext = _factory.Services.GetRequiredService<HotelDbContext>();

    _rootAdminToken = _factory.LoginFullAccess().Result;
    _factory.Login(_client, _rootAdminToken);
  }

  [ClassCleanup]
  public static void ClassCleanup()
  {
    _factory.Dispose();
  }

  [TestInitialize]
  public void TestInitialize()
  {
    _factory.Login(_client, _rootAdminToken);
  }

  [TestMethod]
  public async Task CreateRoom_ShouldReturn_OK()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task CreateRoom_WithNumberAlreadyRegistered_ShouldReturn_BAD_REQUEST()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task UpdateRoom_ShouldReturn_OK()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task UpdateRoom_WithNumberAlreadyRegistered_ShouldReturn_BAD_REQUEST()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task UpdateRoom_WithPriceUpdatedAndPendingReservationAssociated_ShouldReturn_BAD_REQUEST()
  {
    Assert.Fail();
  }


  [TestMethod]
  public async Task DeleteRoom_ShouldReturn_OK()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task DeleteRoom_WithReservationAssociated_ShouldReturn_BAD_REQUEST()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task GetRooms_ShouldReturn_OK()
  {
    Assert.Fail();
  }


  [TestMethod]
  public async Task GetRoomById_ShouldReturn_OK()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task AddService_ShouldReturn_OK()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task AddNonexistService_ShouldReturn_NOT_FOUND()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task RemoveService_ShouldReturn_OK()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task RemoveNonexistService_ShouldReturn_NOT_FOUND()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task UpdateRoomNumber_ShouldReturn_OK()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task UpdateRoomCategory_ShouldReturn_OK()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task UpdateRoomCategory_WithNonexistCategory_ShouldReturn_NOT_FOUND()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task UpdateRoomPrice_ShouldReturn_OK()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task UpdateRoomPrice_WithPendingReservationAssociated_ShouldReturn_BAD_REQUEST()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task UpdateRoomPrice_WithAssociatedNonPendingReservation_ShouldReturn_OK()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task UpdateRoomCapacity_ShouldReturn_OK()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task EnableRoom_ShouldReturn_OK()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task DisableRoom_ShouldReturn_OK()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task DisableRoom_WithAssociatedPendingReservations_ShouldReturn_BAD_REQUEST()
  {
    Assert.Fail();
  }

  [TestMethod]
  public async Task UpdateToAvailableStatus_ShouldReturn_OK()
  {
    Assert.Fail();
  }
}
