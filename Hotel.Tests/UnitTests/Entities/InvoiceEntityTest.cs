using Hotel.Domain.Entities.InvoiceEntity;
using Hotel.Domain.Entities.ReservationEntity;
using Hotel.Domain.Entities.RoomEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Exceptions;


namespace Hotel.Tests.UnitTests.Entities;

[TestClass]
public class InvoiceEntityTest
{
    [TestMethod]
    public void TheCustomersInReservation_MustBeSameOnInvoice()
    {
        var room = new Room("Quarto",1, 50, 3, "Quarto padrão", TestParameters.Category);
        var reservation = new Reservation(room, DateTime.Now.Date, DateTime.Now.AddDays(1), TestParameters.Customer, 2);
        reservation.ToCheckIn();

        var invoice = reservation.Finish();
        Assert.AreEqual(reservation.Customer, invoice.Customer);
    }

}