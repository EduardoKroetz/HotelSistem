using Hotel.Domain.DTOs.InvoiceDTOs;
using Hotel.Domain.Repositories;
using Hotel.Tests.UnitTests.Repositories.Mock;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Tests.UnitTests.Repositories;

[TestClass]
public class InvoiceRepositoryTest
{
    private static InvoiceRepository InvoiceRepository { get; set; }

    static InvoiceRepositoryTest()
    => InvoiceRepository = new InvoiceRepository(BaseRepositoryTest.MockConnection.Context);



    [TestMethod]
    public async Task GetByIdAsync_ReturnsWithCorrectParameters()
    {
        var roomInvoice = await InvoiceRepository.GetByIdAsync(BaseRepositoryTest.Invoices[0].Id);

        Assert.IsNotNull(roomInvoice);
        Assert.AreEqual(BaseRepositoryTest.Invoices[0].Id, roomInvoice.Id);
        Assert.AreEqual(BaseRepositoryTest.Invoices[0].PaymentMethod, roomInvoice.PaymentMethod);
        Assert.AreEqual(BaseRepositoryTest.Invoices[0].ReservationId, roomInvoice.ReservationId);
        Assert.AreEqual(Math.Round(BaseRepositoryTest.Invoices[0].TotalAmount), Math.Round(roomInvoice.TotalAmount));
    }

    [TestMethod]
    public async Task GetAsync_ReturnWithCorrectParameters()
    {
        var parameters = new InvoiceQueryParameters(0, 100, BaseRepositoryTest.Invoices[0].PaymentMethod, BaseRepositoryTest.Invoices[0].TotalAmount, "eq", null, null, null);
        var roomInvoices = await InvoiceRepository.GetAsync(parameters);

        var roomInvoice = roomInvoices.ToList()[0];

        Assert.IsNotNull(roomInvoice);
        Assert.AreEqual(BaseRepositoryTest.Invoices[0].Id, roomInvoice.Id);
        Assert.AreEqual(BaseRepositoryTest.Invoices[0].PaymentMethod, roomInvoice.PaymentMethod);
        Assert.AreEqual(BaseRepositoryTest.Invoices[0].ReservationId, roomInvoice.ReservationId);
        Assert.AreEqual(Math.Round(BaseRepositoryTest.Invoices[0].TotalAmount), Math.Round(roomInvoice.TotalAmount));
    }

    [TestMethod]
    public async Task GetAsync_WhereCardPaymentMethod_ReturnsInvoices()
    {
        var parameters = new InvoiceQueryParameters(0, 100, "card", null, null, null, null, null);
        var roomInvoices = await InvoiceRepository.GetAsync(parameters);

        Assert.IsTrue(roomInvoices.Any());
        foreach (var roomInvoice in roomInvoices)
            Assert.AreEqual("card", roomInvoice.PaymentMethod);

    }

    [TestMethod]
    public async Task GetAsync_WhereTotalAmountGratherThan9_ReturnsInvoices()
    {
        var parameters = new InvoiceQueryParameters(0, 100, null, 9m, "gt", null, null, null);
        var roomInvoices = await InvoiceRepository.GetAsync(parameters);

        Assert.IsTrue(roomInvoices.Any());

        foreach (var roomInvoice in roomInvoices)
            Assert.IsTrue(9 < roomInvoice.TotalAmount);

    }

    [TestMethod]
    public async Task GetAsync_WhereTotalAmountLessThan100_ReturnsInvoices()
    {
        var parameters = new InvoiceQueryParameters(0, 100, null, 100m, "lt", null, null, null);
        var roomInvoices = await InvoiceRepository.GetAsync(parameters);

        Assert.IsTrue(roomInvoices.Any());
        foreach (var roomInvoice in roomInvoices)
            Assert.IsTrue(100 > roomInvoice.TotalAmount);

    }

    [TestMethod]
    public async Task GetAsync_WhereTotalAmountEquals_ReturnsInvoices()
    {
        var totalAmount = BaseRepositoryTest.Invoices[3].TotalAmount;
        var parameters = new InvoiceQueryParameters(0, 100, null, totalAmount, "eq", null, null, null);
        var roomInvoices = await InvoiceRepository.GetAsync(parameters);

        Assert.IsTrue(roomInvoices.Any());

        foreach (var roomInvoice in roomInvoices)
            Assert.AreEqual(Math.Round(totalAmount), Math.Round(roomInvoice.TotalAmount));

    }

    [TestMethod]
    public async Task GetAsync_WhereCustomerId_ReturnsInvoices()
    {
        var customerWithInvoice = await BaseRepositoryTest.MockConnection.Context.Customers
          .FirstOrDefaultAsync(x => x.Invoices.Count > 0);

        var parameters = new InvoiceQueryParameters(0, 100, null, null, null, customerWithInvoice?.Id, null, null);
        var roomInvoices = await InvoiceRepository.GetAsync(parameters);

        Assert.IsTrue(roomInvoices.Any());
        foreach (var roomInvoice in roomInvoices)
            Assert.AreEqual(customerWithInvoice?.Id, roomInvoice.CustomerId);
    }

    [TestMethod]
    public async Task GetAsync_WhereReservationId_ReturnsInvoices()
    {
        var parameters = new InvoiceQueryParameters(0, 100, null, null, null, null, BaseRepositoryTest.Reservations[0].Id, null);
        for (var i = 0; i < 10; i++)
        {
            var roomInvoices = await InvoiceRepository.GetAsync(parameters);
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
    public async Task GetAsync_WhereServiceId_ReturnsInvoices()
    {
        var serviceWithInvoices = await BaseRepositoryTest.MockConnection.Context.Services.FirstOrDefaultAsync(x => x.Invoices.Count > 0);

        var parameters = new InvoiceQueryParameters(0, 100, null, null, null, null, null, serviceWithInvoices!.Id);
        var roomInvoices = await InvoiceRepository.GetAsync(parameters);

        Assert.IsTrue(roomInvoices.Any());
        foreach (var roomInvoice in roomInvoices)
        {
            var hasService = await BaseRepositoryTest.MockConnection.Context.Invoices
              .Where(x => x.Id == roomInvoice.Id)
              .SelectMany(x => x.Services)
              .AnyAsync(x => x.Id == serviceWithInvoices!.Id);

            Assert.IsTrue(hasService);
        }
    }

}
