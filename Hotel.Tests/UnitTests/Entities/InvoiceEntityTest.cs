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
    public void ValidInvoice_MustBeValid()
    {
        var room = new Room("Quarto",1, 50, 3, "Quarto padrão", TestParameters.Category);
        var reservation = new Reservation(room, DateTime.Now.Date, DateTime.Now.AddDays(1), TestParameters.Customer, 2);

        reservation.ToCheckIn();
        reservation.Finish();
        Assert.IsTrue(reservation?.Invoice?.IsValid);
    }

    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    public void InvoiceWithoutCheckOutStatusReservation_ExpectedException()
    {
        var room = new Room("Quarto",1, 50, 3, "Quarto padrão", TestParameters.Category);
        var reservation = new Reservation(room, DateTime.Now.Date, DateTime.Now.AddDays(1), TestParameters.Customer, 2);
        reservation.ToCheckIn();

        new Invoice(EPaymentMethod.Pix, reservation);
        Assert.Fail();
    }

    [TestMethod]
    public void TheCustomersInReservation_MustBeSameOnInvoice()
    {
        var room = new Room("Quarto",1, 50, 3, "Quarto padrão", TestParameters.Category);
        var reservation = new Reservation(room, DateTime.Now.Date, DateTime.Now.AddDays(1), TestParameters.Customer, 2);
        reservation.ToCheckIn();

        var invoice = reservation.Finish();
        Assert.AreEqual(reservation.Customer, invoice.Customer);
    }

    [TestMethod]
    public void ChangeToFinishInvoiceStatus_MustBeFinishStatus()
    {
        var room = new Room("Quarto",1, 50, 3, "Quarto padrão", TestParameters.Category);
        var reservation = new Reservation(room, DateTime.Now.Date, DateTime.Now.AddDays(1), TestParameters.Customer, 2);
        reservation.ToCheckIn();

        var invoice = reservation.Finish();
        invoice.FinishInvoice();
        Assert.AreEqual(EStatus.Finish, invoice.Status);
    }




}