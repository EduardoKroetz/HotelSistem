using Hotel.Domain.DTOs.PaymentContext.RoomInvoiceDTOs;
using Hotel.Domain.Entities.PaymentContext.InvoiceRoomEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Repositories.PaymentContext;
using Microsoft.EntityFrameworkCore;


namespace Hotel.Tests.Repositories.PaymentContext;

[TestClass]
public class RoomInvoiceRepositoryTest : BaseRepositoryTest
{
  private List<RoomInvoice> _roomInvoices { get; set; } = [];
  private RoomInvoice _defaultRoomInvoice { get; set; } = null!;
  private RoomInvoiceRepository _roomInvoiceRepository { get; set; } = null!;

  [TestInitialize]
  public async Task StartupTest()
  {
    await Startup(null);
    _roomInvoiceRepository = new RoomInvoiceRepository(mockConnection.Context);

    _defaultRoomInvoice = _reservations[0].GenerateInvoice(EPaymentMethod.Pix, 20);
    _defaultRoomInvoice.FinishInvoice();

    _roomInvoices.AddRange(
    [
      _defaultRoomInvoice,
      _reservations[1].GenerateInvoice(EPaymentMethod.CreditCard,40),
      _reservations[2].GenerateInvoice(EPaymentMethod.Pix,10),
      _reservations[3].GenerateInvoice(EPaymentMethod.Pix,30),
      _reservations[4].GenerateInvoice(EPaymentMethod.CreditCard,16),
    ]);

    await mockConnection.Context.RoomInvoices.AddRangeAsync(_roomInvoices);
    await mockConnection.Context.SaveChangesAsync();
  }

  [TestCleanup]
  public async Task CleanupTest()
  {
    mockConnection.Context.RoomInvoices.RemoveRange(_roomInvoices);
    await mockConnection.Context.SaveChangesAsync();
    _roomInvoices.Clear();
    await Cleanup();
  }

  [TestMethod]
  public async Task GetByIdAsync_ReturnsWithCorrectParameters()
  {
    var roomInvoice = await _roomInvoiceRepository.GetByIdAsync(_defaultRoomInvoice.Id);

    Assert.IsNotNull(roomInvoice);
    Assert.AreEqual(_defaultRoomInvoice.Id, roomInvoice.Id);
    Assert.AreEqual(_defaultRoomInvoice.PaymentMethod, roomInvoice.PaymentMethod);
    Assert.AreEqual(_defaultRoomInvoice.TaxInformation, roomInvoice.TaxInformation);
    Assert.AreEqual(_defaultRoomInvoice.ReservationId, roomInvoice.ReservationId);
    Assert.AreEqual(_defaultRoomInvoice.Number, roomInvoice.Number);
    Assert.AreEqual(_defaultRoomInvoice.IssueDate, roomInvoice.IssueDate);
    Assert.AreEqual(_defaultRoomInvoice.TotalAmount, roomInvoice.TotalAmount);
    Assert.AreEqual(_defaultRoomInvoice.Status, roomInvoice.Status);

  }

  [TestMethod]
  public async Task GetAsync_ReturnWithCorrectParameters()
  {
    var parameters = new RoomInvoiceQueryParameters(0, 100, null, EPaymentMethod.Pix, _defaultRoomInvoice.TotalAmount, "eq", null, null, null, null, null, null, null, null);
    var roomInvoices = await _roomInvoiceRepository.GetAsync(parameters);

    var roomInvoice = roomInvoices.ToList()[0];

    Assert.IsNotNull(roomInvoice);
    Assert.AreEqual(_defaultRoomInvoice.Id, roomInvoice.Id);
    Assert.AreEqual(_defaultRoomInvoice.PaymentMethod, roomInvoice.PaymentMethod);
    Assert.AreEqual(_defaultRoomInvoice.TaxInformation, roomInvoice.TaxInformation);
    Assert.AreEqual(_defaultRoomInvoice.ReservationId, roomInvoice.ReservationId);
    Assert.AreEqual(_defaultRoomInvoice.Number, roomInvoice.Number);
    Assert.AreEqual(_defaultRoomInvoice.IssueDate, roomInvoice.IssueDate);
    Assert.AreEqual(_defaultRoomInvoice.TotalAmount, roomInvoice.TotalAmount);
    Assert.AreEqual(_defaultRoomInvoice.Status, roomInvoice.Status);
  }

  [TestMethod]
  public async Task GetAsync_WherePaymentMethodEqualsPix_ReturnsRoomInvoices()
  {
    var parameters = new RoomInvoiceQueryParameters(0, 100, null, EPaymentMethod.Pix, null, null, null, null, null, null, null, null, null, null);
    var roomInvoices = await _roomInvoiceRepository.GetAsync(parameters);

    Assert.IsTrue(roomInvoices.Any());
    foreach (var roomInvoice in roomInvoices)
    {
      Assert.IsNotNull(roomInvoice);
      Assert.AreEqual(EPaymentMethod.Pix,roomInvoice.PaymentMethod);
    }
  }

  [TestMethod]
  public async Task GetAsync_WherePaymentMethodEqualsCreditCard_ReturnsRoomInvoices()
  {
    var parameters = new RoomInvoiceQueryParameters(0, 100, null, EPaymentMethod.CreditCard, null, null, null, null, null, null, null, null, null, null);
    var roomInvoices = await _roomInvoiceRepository.GetAsync(parameters);

    Assert.IsTrue(roomInvoices.Any());
    foreach (var roomInvoice in roomInvoices)
    {
      Assert.IsNotNull(roomInvoice);
      Assert.AreEqual(EPaymentMethod.CreditCard, roomInvoice.PaymentMethod);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereTotalAmountGratherThan15_ReturnsRoomInvoices()
  {
    var parameters = new RoomInvoiceQueryParameters(0, 100, null, null, 15m, "gt", null, null, null, null, null, null, null, null);
    var roomInvoices = await _roomInvoiceRepository.GetAsync(parameters);

    Assert.IsTrue(roomInvoices.Any());

    foreach (var roomInvoice in roomInvoices)
      Assert.IsTrue(15 < roomInvoice.TotalAmount);
 
  }

  [TestMethod]
  public async Task GetAsync_WhereTotalAmountLessThan100_ReturnsRoomInvoices()
  {
    var parameters = new RoomInvoiceQueryParameters(0, 100, null, null, 100m, "lt", null, null, null, null, null, null, null, null);
    var roomInvoices = await _roomInvoiceRepository.GetAsync(parameters);

    Assert.IsTrue(roomInvoices.Any());
    foreach (var roomInvoice in roomInvoices)
    {
      Assert.IsNotNull(roomInvoice);
      Assert.IsTrue(100 > roomInvoice.TotalAmount);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereTotalAmountEquals40_ReturnsRoomInvoices()
  {
    var parameters = new RoomInvoiceQueryParameters(0, 100, null, null, 40m, "eq", null, null, null, null, null, null, null, null);
    var roomInvoices = await _roomInvoiceRepository.GetAsync(parameters);

    Assert.IsTrue(roomInvoices.Any());
    foreach (var roomInvoice in roomInvoices)
    {
      Assert.IsNotNull(roomInvoice);
      Assert.AreEqual(40,roomInvoice.TotalAmount);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereStatusEqualsPending_ReturnsRoomInvoices()
  {
    var parameters = new RoomInvoiceQueryParameters(0, 100, null, null, null, null, EStatus.Pending, null, null, null, null, null, null, null);
    var roomInvoices = await _roomInvoiceRepository.GetAsync(parameters);

    Assert.IsTrue(roomInvoices.Any());
    foreach (var roomInvoice in roomInvoices)
    {
      Assert.IsNotNull(roomInvoice);
      Assert.AreEqual(EStatus.Pending,roomInvoice.Status);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereStatusEqualsFinish_ReturnsRoomInvoices()
  {
    
    var parameters = new RoomInvoiceQueryParameters(0, 100, null, null, null, null, EStatus.Finish, null, null, null, null, null, null, null);
    var roomInvoices = await _roomInvoiceRepository.GetAsync(parameters);

    Assert.IsTrue(roomInvoices.Any());
    foreach (var roomInvoice in roomInvoices)
    {
      Assert.IsNotNull(roomInvoice);
      Assert.AreEqual(EStatus.Finish, roomInvoice.Status);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereCustomerId_ReturnsRoomInvoices()
  {
    var parameters = new RoomInvoiceQueryParameters(0, 100, null, null, null, null, null, _customer.Id, null, null, null, null, null, null);
    var roomInvoices = await _roomInvoiceRepository.GetAsync(parameters);

    Assert.IsTrue(roomInvoices.Any());
    foreach (var roomInvoice in roomInvoices)
    {
      var hasCustomer = await mockConnection.Context.RoomInvoices
        .Where(x => x.Id == roomInvoice.Id)
        .SelectMany(x => x.Customers)
        .AnyAsync(x => x.Id == _customer.Id);

      Assert.IsTrue(hasCustomer);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereReservationId_ReturnsRoomInvoices()
  {
    var parameters = new RoomInvoiceQueryParameters(0, 100, null, null, null, null, null, null, _reservations[1].Id, null, null, null, null, null);
    var roomInvoices = await _roomInvoiceRepository.GetAsync(parameters);

    Assert.IsTrue(roomInvoices.Any());

    foreach (var roomInvoice in roomInvoices)
      Assert.AreEqual(_reservations[1].Id,roomInvoice.ReservationId);
    
  }

  [TestMethod]
  public async Task GetAsync_WhereServiceId_ReturnsRoomInvoices()
  {
    var parameters = new RoomInvoiceQueryParameters(0, 100, null, null, null, null, null, null, null, _service.Id, null, null, null, null);
    var roomInvoices = await _roomInvoiceRepository.GetAsync(parameters);

    Assert.IsTrue(roomInvoices.Any());
    foreach (var roomInvoice in roomInvoices)
    {
      var hasService = await mockConnection.Context.RoomInvoices
        .Where(x => x.Id == roomInvoice.Id)
        .SelectMany(x => x.Services)
        .AnyAsync(x => x.Id == _service.Id);

      Assert.IsTrue(hasService);
    }
  }


  [TestMethod]
  public async Task GetAsync_WhereTaxInformationGratherThan13_ReturnsRoomInvoices()
  {
    var parameters = new RoomInvoiceQueryParameters(0, 100, null, null, null, null, null, null, null, null, 13, "gt", null, null);
    var roomInvoices = await _roomInvoiceRepository.GetAsync(parameters);

    Assert.IsTrue(roomInvoices.Any());

    foreach (var roomInvoice in roomInvoices)
      Assert.IsTrue(13 < roomInvoice.TaxInformation);
    
  }

  [TestMethod]
  public async Task GetAsync_WhereTaxInformationLessThan13_ReturnsRoomInvoices()
  {
    var parameters = new RoomInvoiceQueryParameters(0, 100, null, null, null, null, null, null, null, null, 13, "lt", null, null);
    var roomInvoices = await _roomInvoiceRepository.GetAsync(parameters);

    Assert.IsTrue(roomInvoices.Any());

    foreach (var roomInvoice in roomInvoices)
      Assert.IsTrue(13 > roomInvoice.TaxInformation);

  }

  [TestMethod]
  public async Task GetAsync_WhereTaxInformationEquals10_ReturnsRoomInvoices()
  {
    var parameters = new RoomInvoiceQueryParameters(0, 100, null, null, null, null, null, null, null, null, 10, "eq", null, null);
    var roomInvoices = await _roomInvoiceRepository.GetAsync(parameters);

    Assert.IsTrue(roomInvoices.Any());

    foreach (var roomInvoice in roomInvoices)
      Assert.AreEqual(10,roomInvoice.TaxInformation);

  }


  [TestMethod]
  public async Task GetAsync_WhereIssueDateGratherThanYesterday_ReturnsRoomInvoices()
  {
    var parameters = new RoomInvoiceQueryParameters(0, 100, null, null, null, null, null, null, null, null, null, null, DateTime.Now.AddDays(-1), "gt");
    var roomInvoices = await _roomInvoiceRepository.GetAsync(parameters);

    Assert.IsTrue(roomInvoices.Any());
    foreach (var roomInvoice in roomInvoices)
      Assert.IsTrue(DateTime.Now.AddDays(-1) < roomInvoice.IssueDate);
    
  }

  [TestMethod]
  public async Task GetAsync_WhereIssueDateLessThanToday_ReturnsRoomInvoices()
  {
    var parameters = new RoomInvoiceQueryParameters(0, 100, null, null, null, null, null, null, null, null, null, null, DateTime.Now.AddDays(1), "lt");
    var roomInvoices = await _roomInvoiceRepository.GetAsync(parameters);

    Assert.IsTrue(roomInvoices.Any());

    foreach (var roomInvoice in roomInvoices)
      Assert.IsTrue(DateTime.Now.AddDays(1) > roomInvoice.IssueDate);
 
  }

  [TestMethod]
  public async Task GetAsync_WhereIssueDateEquals_ReturnsRoomInvoices()
  {
    var parameters = new RoomInvoiceQueryParameters(0, 100, null, null, null, null, null, null, null, null, null, null, _defaultRoomInvoice.IssueDate, "eq");
    var roomInvoices = await _roomInvoiceRepository.GetAsync(parameters);

    Assert.IsTrue(roomInvoices.Any());
    foreach (var roomInvoice in roomInvoices)
    {
      Assert.IsNotNull(roomInvoice);
      Assert.AreEqual(_defaultRoomInvoice.IssueDate, roomInvoice.IssueDate);
    }
  }

  [TestMethod]
  public async Task GetAsync_WherePaymentMethodPix_And_TotalAmountGratherThan10_And_StatusPending_ReturnsRoomInvoices()
  {
    var parameters = new RoomInvoiceQueryParameters(0, 100, null, EPaymentMethod.Pix, 10, "gt", EStatus.Pending, null, null, null, null, null, null,null);

    var roomInvoices = await _roomInvoiceRepository.GetAsync(parameters);

    Assert.IsTrue(roomInvoices.Any());

    foreach (var roomInvoice in roomInvoices)
    { 
      Assert.AreEqual(EPaymentMethod.Pix,roomInvoice.PaymentMethod);
      Assert.IsTrue(10 < roomInvoice.TotalAmount);
      Assert.AreEqual(EStatus.Pending, roomInvoice.Status);
    }
  }

}
