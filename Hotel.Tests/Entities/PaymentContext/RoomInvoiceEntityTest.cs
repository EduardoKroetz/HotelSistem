using Hotel.Domain.Entities.PaymentContext.InvoiceRoomEntity;
using Hotel.Domain.Entities.ReservationContext.ReservationEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Exceptions;


namespace Hotel.Tests.Entities.EmplyeeContext;

[TestClass]
public class InvoiceEntityTest
{

  [TestMethod]
  public void ValidInvoice_MustBeValid()
  {
    var reservation = new Reservation(TestParameters.Room,DateTime.Now.Date,[TestParameters.Customer]);
    reservation.GenerateInvoice(EPaymentMethod.Pix,0);
    Assert.IsTrue(reservation?.Invoice?.IsValid);
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  public void InvalidInvoice_ExpectedException()
  {
    var reservation = new Reservation(TestParameters.Room,DateTime.Now.Date,[TestParameters.Customer]);
    reservation.GenerateInvoice(EPaymentMethod.Pix,-1);
    Assert.Fail();
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  public void InvoiceWithoutCheckOutStatusReservation_ExpectedException()
  {
    var reservation = new Reservation(TestParameters.Room,DateTime.Now.Date,[TestParameters.Customer]);
    new RoomInvoice(EPaymentMethod.Pix,reservation);
    Assert.Fail();
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  public void InvoiceWithoutCustomers_ExpectedException()
  {
    var reservation = new Reservation(TestParameters.Room,DateTime.Now.Date,[]);
    reservation.GenerateInvoice(EPaymentMethod.Pix);
    Assert.Fail();
  }

  [TestMethod]
  public void InvoiceWithTax_MustBeAddedToTotalAmount()
  {
    var reservation = new Reservation(TestParameters.Room,DateTime.Now.Date,[TestParameters.Customer]);
    var invoice = reservation.GenerateInvoice(EPaymentMethod.Pix,1);
    Assert.AreEqual(reservation.TotalAmount() + 1, invoice.TotalAmount);
  }

  [TestMethod]
  public void TheCustomersInReservation_MustBeSameOnInvoice()
  {
    var reservation = new Reservation(TestParameters.Room,DateTime.Now.Date,[TestParameters.Customer]);
    var invoice = reservation.GenerateInvoice(EPaymentMethod.Pix);
    Assert.AreEqual(reservation.Customers,invoice.Customers);
  }

  [TestMethod]
  public void ChangeToFinishInvoiceStatus_MustBeFinishStatus()
  {
    var reservation = new Reservation(TestParameters.Room,DateTime.Now.Date,[TestParameters.Customer]);
    var invoice = reservation.GenerateInvoice(EPaymentMethod.Pix,0);
    invoice.FinishInvoice();
    Assert.AreEqual(EStatus.Finish,invoice.Status);
  }




}