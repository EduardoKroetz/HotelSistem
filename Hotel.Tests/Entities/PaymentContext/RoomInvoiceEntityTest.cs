using Hotel.Domain.Entities.PaymentContext.InvoiceRoomEntity;
using Hotel.Domain.Entities.ReservationContext.ReservationEntity;
using Hotel.Domain.Entities.RoomContext.RoomEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Exceptions;


namespace Hotel.Tests.Entities.PaymentContext;

[TestClass]
public class InvoiceEntityTest
{

  [TestMethod]
  public void ValidInvoice_MustBeValid()
  {
    var room = new Room(1, 50, 3, "Quarto padrão", TestParameters.Category.Id);
    var reservation = new Reservation(room,DateTime.Now.Date, DateTime.Now.AddDays(1),TestParameters.Customer, 2);

    reservation.ToCheckIn();
    reservation.Finish(EPaymentMethod.Pix,0);
    Assert.IsTrue(reservation?.Invoice?.IsValid);
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  public void InvalidInvoice_ExpectedException()
  {
    var room = new Room(1, 50, 3, "Quarto padrão", TestParameters.Category.Id);
    var reservation = new Reservation(room,DateTime.Now.Date, DateTime.Now.AddDays(1),TestParameters.Customer, 2);
    reservation.ToCheckIn();
    reservation.Finish(EPaymentMethod.Pix,-1);
    Assert.Fail();
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  public void InvoiceWithoutCheckOutStatusReservation_ExpectedException()
  {
    var room = new Room(1, 50, 3, "Quarto padrão", TestParameters.Category.Id);
    var reservation = new Reservation(room,DateTime.Now.Date, DateTime.Now.AddDays(1),TestParameters.Customer, 2);
    reservation.ToCheckIn();

    new RoomInvoice(EPaymentMethod.Pix,reservation);
    Assert.Fail();
  }

  [TestMethod]
  public void InvoiceWithTax_MustBeAddedToTotalAmount()
  {
    var room = new Room(1, 50, 3, "Quarto padrão", TestParameters.Category.Id);
    var reservation = new Reservation(room,DateTime.Now.Date, DateTime.Now.AddDays(1),TestParameters.Customer, 2);
    reservation.ToCheckIn();

    var invoice = reservation.Finish(EPaymentMethod.Pix,1);
    Assert.AreEqual(reservation.TotalAmount() + 1, invoice.TotalAmount);
  }

  [TestMethod]
  public void TheCustomersInReservation_MustBeSameOnInvoice()
  {
    var room = new Room(1, 50, 3, "Quarto padrão", TestParameters.Category.Id);
    var reservation = new Reservation(room,DateTime.Now.Date, DateTime.Now.AddDays(1),TestParameters.Customer, 2);
    reservation.ToCheckIn();

    var invoice = reservation.Finish(EPaymentMethod.Pix);
    Assert.AreEqual(reservation.Customer,invoice.Customer);
  }

  [TestMethod]
  public void ChangeToFinishInvoiceStatus_MustBeFinishStatus()
  {
    var room = new Room(1, 50, 3, "Quarto padrão", TestParameters.Category.Id);
    var reservation = new Reservation(room,DateTime.Now.Date, DateTime.Now.AddDays(1),TestParameters.Customer, 2);
    reservation.ToCheckIn();

    var invoice = reservation.Finish(EPaymentMethod.Pix,0);
    invoice.FinishInvoice();
    Assert.AreEqual(EStatus.Finish,invoice.Status);
  }




}