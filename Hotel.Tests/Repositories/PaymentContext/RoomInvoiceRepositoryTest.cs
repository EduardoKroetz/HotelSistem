using Hotel.Domain.DTOs.PaymentContext.RoomInvoiceDTOs;
using Hotel.Domain.Enums;
using Hotel.Domain.Repositories.PaymentContext;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Tests.Repositories.PaymentContext;

[TestClass]
public class RoomInvoiceRepositoryTest : BaseRepositoryTest
{
  private static RoomInvoiceRepository RoomInvoiceRepository { get; set; } = null!;

  [ClassInitialize]
  public static async Task Startup(TestContext? context)
  {
    await Startup();
    RoomInvoiceRepository = new RoomInvoiceRepository(MockConnection.Context);
  }

  [ClassCleanup]
  public static async Task Dispose()
  => await Cleanup();

  [TestMethod]
  public async Task GetByIdAsync_ReturnsWithCorrectParameters()
  {
    var roomInvoice = await RoomInvoiceRepository.GetByIdAsync(RoomInvoices[0].Id);

    Assert.IsNotNull(roomInvoice);
    Assert.AreEqual(RoomInvoices[0].Id, roomInvoice.Id);
    Assert.AreEqual(RoomInvoices[0].PaymentMethod, roomInvoice.PaymentMethod);
    Assert.AreEqual(RoomInvoices[0].TaxInformation, roomInvoice.TaxInformation);
    Assert.AreEqual(RoomInvoices[0].ReservationId, roomInvoice.ReservationId);
    Assert.AreEqual(RoomInvoices[0].Number, roomInvoice.Number);
    Assert.AreEqual(RoomInvoices[0].IssueDate, roomInvoice.IssueDate);
    Assert.AreEqual(RoomInvoices[0].TotalAmount, roomInvoice.TotalAmount);
    Assert.AreEqual(RoomInvoices[0].Status, roomInvoice.Status);

  }

  [TestMethod]
  public async Task GetAsync_ReturnWithCorrectParameters()
  {
    var parameters = new RoomInvoiceQueryParameters(0, 100, null, RoomInvoices[0].PaymentMethod, RoomInvoices[0].TotalAmount, "eq", null, null, null, null, null, null, null, null);
    var roomInvoices = await RoomInvoiceRepository.GetAsync(parameters);

    var roomInvoice = roomInvoices.ToList()[0];

    Assert.IsNotNull(roomInvoice);
    Assert.AreEqual(RoomInvoices[0].Id, roomInvoice.Id);
    Assert.AreEqual(RoomInvoices[0].PaymentMethod, roomInvoice.PaymentMethod);
    Assert.AreEqual(RoomInvoices[0].TaxInformation, roomInvoice.TaxInformation);
    Assert.AreEqual(RoomInvoices[0].ReservationId, roomInvoice.ReservationId);
    Assert.AreEqual(RoomInvoices[0].Number, roomInvoice.Number);
    Assert.AreEqual(RoomInvoices[0].IssueDate, roomInvoice.IssueDate);
    Assert.AreEqual(RoomInvoices[0].TotalAmount, roomInvoice.TotalAmount);
    Assert.AreEqual(RoomInvoices[0].Status, roomInvoice.Status);
  }

  [TestMethod]
  public async Task GetAsync_WherePaymentMethodEqualsPix_ReturnsRoomInvoices()
  {
    var parameters = new RoomInvoiceQueryParameters(0, 100, null, EPaymentMethod.Pix, null, null, null, null, null, null, null, null, null, null);
    var roomInvoices = await RoomInvoiceRepository.GetAsync(parameters);

    Assert.IsTrue(roomInvoices.Any());
    foreach (var roomInvoice in roomInvoices)
      Assert.AreEqual(EPaymentMethod.Pix,roomInvoice.PaymentMethod);
  }

  [TestMethod]
  public async Task GetAsync_WherePaymentMethodEqualsCreditCard_ReturnsRoomInvoices()
  {
    var parameters = new RoomInvoiceQueryParameters(0, 100, null, EPaymentMethod.CreditCard, null, null, null, null, null, null, null, null, null, null);
    var roomInvoices = await RoomInvoiceRepository.GetAsync(parameters);

    Assert.IsTrue(roomInvoices.Any());
    foreach (var roomInvoice in roomInvoices)
      Assert.AreEqual(EPaymentMethod.CreditCard, roomInvoice.PaymentMethod);
    
  }

  [TestMethod]
  public async Task GetAsync_WhereTotalAmountGratherThan15_ReturnsRoomInvoices()
  {
    var parameters = new RoomInvoiceQueryParameters(0, 100, null, null, 15m, "gt", null, null, null, null, null, null, null, null);
    var roomInvoices = await RoomInvoiceRepository.GetAsync(parameters);

    Assert.IsTrue(roomInvoices.Any());

    foreach (var roomInvoice in roomInvoices)
      Assert.IsTrue(15 < roomInvoice.TotalAmount);
 
  }

  [TestMethod]
  public async Task GetAsync_WhereTotalAmountLessThan100_ReturnsRoomInvoices()
  {
    var parameters = new RoomInvoiceQueryParameters(0, 100, null, null, 100m, "lt", null, null, null, null, null, null, null, null);
    var roomInvoices = await RoomInvoiceRepository.GetAsync(parameters);

    Assert.IsTrue(roomInvoices.Any());
    foreach (var roomInvoice in roomInvoices)
      Assert.IsTrue(100 > roomInvoice.TotalAmount);
    
  }

  [TestMethod]
  public async Task GetAsync_WhereTotalAmountEquals30_ReturnsRoomInvoices()
  {
    var parameters = new RoomInvoiceQueryParameters(0, 100, null, null, 30m, "eq", null, null, null, null, null, null, null, null);
    var roomInvoices = await RoomInvoiceRepository.GetAsync(parameters);

    Assert.IsTrue(roomInvoices.Any());

    foreach (var roomInvoice in roomInvoices)
      Assert.AreEqual(30,roomInvoice.TotalAmount);

  }

  [TestMethod]
  public async Task GetAsync_WhereStatusEqualsPending_ReturnsRoomInvoices()
  {
    var parameters = new RoomInvoiceQueryParameters(0, 100, null, null, null, null, EStatus.Pending, null, null, null, null, null, null, null);
    var roomInvoices = await RoomInvoiceRepository.GetAsync(parameters);

    Assert.IsTrue(roomInvoices.Any());
    foreach (var roomInvoice in roomInvoices)
      Assert.AreEqual(EStatus.Pending,roomInvoice.Status);
 

  }

  [TestMethod]
  public async Task GetAsync_WhereStatusEqualsFinish_ReturnsRoomInvoices()
  {
    
    var parameters = new RoomInvoiceQueryParameters(0, 100, null, null, null, null, EStatus.Finish, null, null, null, null, null, null, null);
    var roomInvoices = await RoomInvoiceRepository.GetAsync(parameters);

    Assert.IsTrue(roomInvoices.Any());
    foreach (var roomInvoice in roomInvoices)
      Assert.AreEqual(EStatus.Finish, roomInvoice.Status);
  }

  [TestMethod]
  public async Task GetAsync_WhereCustomerId_ReturnsRoomInvoices()
  {
    var parameters = new RoomInvoiceQueryParameters(0, 100, null, null, null, null, null, Customers[0].Id, null, null, null, null, null, null);
    var roomInvoices = await RoomInvoiceRepository.GetAsync(parameters);

    Assert.IsTrue(roomInvoices.Any());
    foreach (var roomInvoice in roomInvoices)
    {
      var hasCustomer = await MockConnection.Context.RoomInvoices
        .Where(x => x.Id == roomInvoice.Id)
        .SelectMany(x => x.Customers)
        .AnyAsync(x => x.Id == Customers[0].Id);

      Assert.IsTrue(hasCustomer);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereReservationId_ReturnsRoomInvoices()
  {
    var parameters = new RoomInvoiceQueryParameters(0, 100, null, null, null, null, null, null, Reservations[1].Id, null, null, null, null, null);
    var roomInvoices = await RoomInvoiceRepository.GetAsync(parameters);

    Assert.IsTrue(roomInvoices.Any());

    foreach (var roomInvoice in roomInvoices)
      Assert.AreEqual(Reservations[1].Id,roomInvoice.ReservationId);
    
  }

  [TestMethod]
  public async Task GetAsync_WhereServiceId_ReturnsRoomInvoices()
  {
    var parameters = new RoomInvoiceQueryParameters(0, 100, null, null, null, null, null, null, null, Services[0].Id, null, null, null, null);
    var roomInvoices = await RoomInvoiceRepository.GetAsync(parameters);

    Assert.IsTrue(roomInvoices.Any());
    foreach (var roomInvoice in roomInvoices)
    {
      var hasService = await MockConnection.Context.RoomInvoices
        .Where(x => x.Id == roomInvoice.Id)
        .SelectMany(x => x.Services)
        .AnyAsync(x => x.Id == Services[0].Id);

      Assert.IsTrue(hasService);
    }
  }


  [TestMethod]
  public async Task GetAsync_WhereTaxInformationGratherThan13_ReturnsRoomInvoices()
  {
    var parameters = new RoomInvoiceQueryParameters(0, 100, null, null, null, null, null, null, null, null, 13, "gt", null, null);
    var roomInvoices = await RoomInvoiceRepository.GetAsync(parameters);

    Assert.IsTrue(roomInvoices.Any());

    foreach (var roomInvoice in roomInvoices)
      Assert.IsTrue(13 < roomInvoice.TaxInformation);
    
  }

  [TestMethod]
  public async Task GetAsync_WhereTaxInformationLessThan13_ReturnsRoomInvoices()
  {
    var parameters = new RoomInvoiceQueryParameters(0, 100, null, null, null, null, null, null, null, null, 13, "lt", null, null);
    var roomInvoices = await RoomInvoiceRepository.GetAsync(parameters);

    Assert.IsTrue(roomInvoices.Any());

    foreach (var roomInvoice in roomInvoices)
      Assert.IsTrue(13 > roomInvoice.TaxInformation);

  }

  [TestMethod]
  public async Task GetAsync_WhereTaxInformationEquals10_ReturnsRoomInvoices()
  {
    var parameters = new RoomInvoiceQueryParameters(0, 100, null, null, null, null, null, null, null, null, 10, "eq", null, null);
    var roomInvoices = await RoomInvoiceRepository.GetAsync(parameters);

    Assert.IsTrue(roomInvoices.Any());

    foreach (var roomInvoice in roomInvoices)
      Assert.AreEqual(10,roomInvoice.TaxInformation);

  }


  [TestMethod]
  public async Task GetAsync_WhereIssueDateGratherThanYesterday_ReturnsRoomInvoices()
  {
    var parameters = new RoomInvoiceQueryParameters(0, 100, null, null, null, null, null, null, null, null, null, null, DateTime.Now.AddDays(-1), "gt");
    var roomInvoices = await RoomInvoiceRepository.GetAsync(parameters);

    Assert.IsTrue(roomInvoices.Any());
    foreach (var roomInvoice in roomInvoices)
      Assert.IsTrue(DateTime.Now.AddDays(-1) < roomInvoice.IssueDate);
    
  }

  [TestMethod]
  public async Task GetAsync_WhereIssueDateLessThanToday_ReturnsRoomInvoices()
  {
    var parameters = new RoomInvoiceQueryParameters(0, 100, null, null, null, null, null, null, null, null, null, null, DateTime.Now.AddDays(1), "lt");
    var roomInvoices = await RoomInvoiceRepository.GetAsync(parameters);

    Assert.IsTrue(roomInvoices.Any());

    foreach (var roomInvoice in roomInvoices)
      Assert.IsTrue(DateTime.Now.AddDays(1) > roomInvoice.IssueDate);
 
  }

  [TestMethod]
  public async Task GetAsync_WhereIssueDateEquals_ReturnsRoomInvoices()
  {
    var parameters = new RoomInvoiceQueryParameters(0, 100, null, null, null, null, null, null, null, null, null, null, RoomInvoices[0].IssueDate, "eq");
    var roomInvoices = await RoomInvoiceRepository.GetAsync(parameters);

    Assert.IsTrue(roomInvoices.Any());
    foreach (var roomInvoice in roomInvoices)
      Assert.AreEqual(RoomInvoices[0].IssueDate, roomInvoice.IssueDate);
  }

  [TestMethod]
  public async Task GetAsync_WherePaymentMethodPix_And_TotalAmountGratherThan10_And_StatusPending_ReturnsRoomInvoices()
  {
    var parameters = new RoomInvoiceQueryParameters(0, 100, null, EPaymentMethod.Pix, 10, "gt", EStatus.Pending, null, null, null, null, null, null,null);

    var roomInvoices = await RoomInvoiceRepository.GetAsync(parameters);

    Assert.IsTrue(roomInvoices.Any());

    foreach (var roomInvoice in roomInvoices)
    { 
      Assert.AreEqual(EPaymentMethod.Pix,roomInvoice.PaymentMethod);
      Assert.IsTrue(10 < roomInvoice.TotalAmount);
      Assert.AreEqual(EStatus.Pending, roomInvoice.Status);
    }
  }

}
