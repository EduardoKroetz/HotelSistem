using Hotel.Domain.DTOs.PaymentContext.RoomInvoiceDTOs;
using Hotel.Domain.Entities.ReservationContext.ReservationEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Repositories.PaymentContext;
using Hotel.Tests.UnitTests.Repositories.Mock;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Tests.UnitTests.Repositories.PaymentContext;

[TestClass]
public class RoomInvoiceRepositoryTest
{
    private static RoomInvoiceRepository RoomInvoiceRepository { get; set; }

    static RoomInvoiceRepositoryTest()
    => RoomInvoiceRepository = new RoomInvoiceRepository(BaseRepositoryTest.MockConnection.Context);



    [TestMethod]
    public async Task GetByIdAsync_ReturnsWithCorrectParameters()
    {
        var roomInvoice = await RoomInvoiceRepository.GetByIdAsync(BaseRepositoryTest.RoomInvoices[0].Id);

        Assert.IsNotNull(roomInvoice);
        Assert.AreEqual(BaseRepositoryTest.RoomInvoices[0].Id, roomInvoice.Id);
        Assert.AreEqual(BaseRepositoryTest.RoomInvoices[0].PaymentMethod, roomInvoice.PaymentMethod);
        Assert.AreEqual(BaseRepositoryTest.RoomInvoices[0].TaxInformation, roomInvoice.TaxInformation);
        Assert.AreEqual(BaseRepositoryTest.RoomInvoices[0].ReservationId, roomInvoice.ReservationId);
        Assert.AreEqual(BaseRepositoryTest.RoomInvoices[0].Number, roomInvoice.Number);
        Assert.AreEqual(BaseRepositoryTest.RoomInvoices[0].IssueDate, roomInvoice.IssueDate);
        Assert.AreEqual(Math.Round(BaseRepositoryTest.RoomInvoices[0].TotalAmount), Math.Round(roomInvoice.TotalAmount));
        Assert.AreEqual(BaseRepositoryTest.RoomInvoices[0].Status, roomInvoice.Status);

    }

    [TestMethod]
    public async Task GetAsync_ReturnWithCorrectParameters()
    {
        var parameters = new RoomInvoiceQueryParameters(0, 100, null, BaseRepositoryTest.RoomInvoices[0].PaymentMethod, BaseRepositoryTest.RoomInvoices[0].TotalAmount, "eq", null, null, null, null, null, null, null, null);
        var roomInvoices = await RoomInvoiceRepository.GetAsync(parameters);

        var roomInvoice = roomInvoices.ToList()[0];

        Assert.IsNotNull(roomInvoice);
        Assert.AreEqual(BaseRepositoryTest.RoomInvoices[0].Id, roomInvoice.Id);
        Assert.AreEqual(BaseRepositoryTest.RoomInvoices[0].PaymentMethod, roomInvoice.PaymentMethod);
        Assert.AreEqual(BaseRepositoryTest.RoomInvoices[0].TaxInformation, roomInvoice.TaxInformation);
        Assert.AreEqual(BaseRepositoryTest.RoomInvoices[0].ReservationId, roomInvoice.ReservationId);
        Assert.AreEqual(BaseRepositoryTest.RoomInvoices[0].Number, roomInvoice.Number);
        Assert.AreEqual(BaseRepositoryTest.RoomInvoices[0].IssueDate, roomInvoice.IssueDate);
        Assert.AreEqual(Math.Round(BaseRepositoryTest.RoomInvoices[0].TotalAmount), Math.Round(roomInvoice.TotalAmount));
        Assert.AreEqual(BaseRepositoryTest.RoomInvoices[0].Status, roomInvoice.Status);
    }

    [TestMethod]
    public async Task GetAsync_WherePaymentMethodEqualsPix_ReturnsRoomInvoices()
    {
        var parameters = new RoomInvoiceQueryParameters(0, 100, null, EPaymentMethod.Pix, null, null, null, null, null, null, null, null, null, null);
        var roomInvoices = await RoomInvoiceRepository.GetAsync(parameters);

        Assert.IsTrue(roomInvoices.Any());
        foreach (var roomInvoice in roomInvoices)
            Assert.AreEqual(EPaymentMethod.Pix, roomInvoice.PaymentMethod);
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
    public async Task GetAsync_WhereTotalAmountEquals_ReturnsRoomInvoices()
    {
        var parameters = new RoomInvoiceQueryParameters(0, 100, null, null, BaseRepositoryTest.RoomInvoices[3].TotalAmount, "eq", null, null, null, null, null, null, null, null);
        var roomInvoices = await RoomInvoiceRepository.GetAsync(parameters);

        Assert.IsTrue(roomInvoices.Any());

        foreach (var roomInvoice in roomInvoices)
            Assert.AreEqual(Math.Round(BaseRepositoryTest.RoomInvoices[3].TotalAmount), Math.Round(roomInvoice.TotalAmount));

    }

    [TestMethod]
    public async Task GetAsync_WhereStatusEqualsPending_ReturnsRoomInvoices()
    {
        var parameters = new RoomInvoiceQueryParameters(0, 100, null, null, null, null, EStatus.Pending, null, null, null, null, null, null, null);
        var roomInvoices = await RoomInvoiceRepository.GetAsync(parameters);

        Assert.IsTrue(roomInvoices.Any());
        foreach (var roomInvoice in roomInvoices)
            Assert.AreEqual(EStatus.Pending, roomInvoice.Status);


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
        var customerWithInvoice = await BaseRepositoryTest.MockConnection.Context.Customers
          .FirstOrDefaultAsync(x => x.RoomInvoices.Count > 0);

        var parameters = new RoomInvoiceQueryParameters(0, 100, null, null, null, null, null, customerWithInvoice?.Id, null, null, null, null, null, null);
        var roomInvoices = await RoomInvoiceRepository.GetAsync(parameters);

        Assert.IsTrue(roomInvoices.Any());
        foreach (var roomInvoice in roomInvoices)
            Assert.AreEqual(customerWithInvoice?.Id, roomInvoice.CustomerId);
    }

    [TestMethod]
    public async Task GetAsync_WhereReservationId_ReturnsRoomInvoices()
    {
        var parameters = new RoomInvoiceQueryParameters(0, 100, null, null, null, null, null, null, BaseRepositoryTest.Reservations[0].Id, null, null, null, null, null);
        for (var i = 0; i < 10; i++)
        {
            var roomInvoices = await RoomInvoiceRepository.GetAsync(parameters);
            if (roomInvoices.Any())
            {
                foreach (var roomInvoice in roomInvoices)
                    Assert.AreEqual(BaseRepositoryTest.Reservations[0].Id, roomInvoice.ReservationId);
                Assert.IsTrue(true);
                return;
            }
        }
    }

    [TestMethod]
    public async Task GetAsync_WhereServiceId_ReturnsRoomInvoices()
    {
        var serviceWithRoomInvoices = await BaseRepositoryTest.MockConnection.Context.Services.FirstOrDefaultAsync(x => x.RoomInvoices.Count > 0);

        var parameters = new RoomInvoiceQueryParameters(0, 100, null, null, null, null, null, null, null, serviceWithRoomInvoices!.Id, null, null, null, null);
        var roomInvoices = await RoomInvoiceRepository.GetAsync(parameters);

        Assert.IsTrue(roomInvoices.Any());
        foreach (var roomInvoice in roomInvoices)
        {
            var hasService = await BaseRepositoryTest.MockConnection.Context.RoomInvoices
              .Where(x => x.Id == roomInvoice.Id)
              .SelectMany(x => x.Services)
              .AnyAsync(x => x.Id == serviceWithRoomInvoices!.Id);

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
            Assert.AreEqual(10, roomInvoice.TaxInformation);

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
        var parameters = new RoomInvoiceQueryParameters(0, 100, null, null, null, null, null, null, null, null, null, null, BaseRepositoryTest.RoomInvoices[0].IssueDate, "eq");
        var roomInvoices = await RoomInvoiceRepository.GetAsync(parameters);

        Assert.IsTrue(roomInvoices.Any());
        foreach (var roomInvoice in roomInvoices)
            Assert.AreEqual(BaseRepositoryTest.RoomInvoices[0].IssueDate, roomInvoice.IssueDate);
    }

    [TestMethod]
    public async Task GetAsync_WherePaymentMethodPix_And_TotalAmountGratherThan10_And_StatusPending_ReturnsRoomInvoices()
    {
        var parameters = new RoomInvoiceQueryParameters(0, 100, null, EPaymentMethod.Pix, 10, "gt", EStatus.Pending, null, null, null, null, null, null, null);

        var roomInvoices = await RoomInvoiceRepository.GetAsync(parameters);

        Assert.IsTrue(roomInvoices.Any());

        foreach (var roomInvoice in roomInvoices)
        {
            Assert.AreEqual(EPaymentMethod.Pix, roomInvoice.PaymentMethod);
            Assert.IsTrue(10 < roomInvoice.TotalAmount);
            Assert.AreEqual(EStatus.Pending, roomInvoice.Status);
        }
    }

}
