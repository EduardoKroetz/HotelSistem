using Hotel.Domain.Entities.ReservationEntity;
using Hotel.Domain.Entities.RoomEntity;


namespace Hotel.Tests.UnitTests.Entities;

[TestClass]
public class InvoiceEntityTest
{
    [TestMethod]
    public void NewInvoiceInstance_MustBeValid()
    {
        var room = new Room("Quarto",1, 50, 3, "Quarto padrão", TestParameters.Category);
        var reservation = new Reservation(room, DateTime.Now.Date, DateTime.Now.AddDays(1), TestParameters.Customer, 2);
        reservation.ToCheckIn();

        var invoice = reservation.Finish();

        Assert.IsTrue(invoice.IsValid);
        Assert.AreEqual(reservation.Customer, invoice.Customer);
        Assert.AreEqual(reservation.CustomerId, invoice.CustomerId);
        Assert.AreEqual(reservation.Services, invoice.Services);
        Assert.AreEqual(reservation.TotalAmount(), invoice.TotalAmount);
        Assert.AreEqual("card", invoice.PaymentMethod);
        Assert.AreEqual(reservation.Id, invoice.ReservationId);
    }

}