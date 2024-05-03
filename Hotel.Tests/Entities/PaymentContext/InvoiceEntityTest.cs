using Hotel.Domain.Entities.PaymentContext.InvoiceRoomEntity;
using Hotel.Domain.Entities.ReservationContext.ReservationEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Exceptions;


namespace Hotel.Tests.Entities.EmplyeeContext;

[TestClass]
public class InvoiceEntityTest
{

  [TestMethod]
  public void CreateValidInvoice_MustBeValid()
  {
    var reservation = new Reservation(TestParameters.Room,DateTime.Now.AddDays(3),[TestParameters.Customer]);
    reservation.GenerateInvoiceAndCheckOut(EPaymentMethod.Pix,0);
    Assert.AreEqual(true, reservation?.Invoice?.IsValid);
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  public void CreateInvalidInvoice_ExpectedException()
  {
    var reservation = new Reservation(TestParameters.Room,DateTime.Now.AddDays(3),[TestParameters.Customer]);
    reservation.GenerateInvoiceAndCheckOut(EPaymentMethod.Pix,-1);
    Assert.Fail();
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  public void CreateInvoice_Without_CheckOutStatusReservation_ExpectedException()
  {
    var reservation = new Reservation(TestParameters.Room,DateTime.Now.AddDays(3),[TestParameters.Customer]);
    new InvoiceRoom(EPaymentMethod.Pix,reservation);
    Assert.Fail();
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  public void CreateInvoice_Without_Customers_ExpectedException()
  {
    var reservation = new Reservation(TestParameters.Room,DateTime.Now.AddDays(3),[]);
    reservation.GenerateInvoiceAndCheckOut(EPaymentMethod.Pix);
    Assert.Fail();
  }

  [TestMethod]
  public void Invoice_WithTax_MustBeAddedToTotalAmount()
  {
    var reservation = new Reservation(TestParameters.Room,DateTime.Now.AddDays(3),[TestParameters.Customer]);
    var invoice = reservation.GenerateInvoiceAndCheckOut(EPaymentMethod.Pix,1);
    Assert.AreEqual(reservation.TotalAmount() + 1, invoice.TotalAmount);
  }

  [TestMethod]
  public void ReservationWithCustomer_MustBeSameInInvoice()
  {
    var reservation = new Reservation(TestParameters.Room,DateTime.Now.AddDays(3),[TestParameters.Customer]);
    var invoice = reservation.GenerateInvoiceAndCheckOut(EPaymentMethod.Pix);
    Assert.AreEqual(reservation.Customers[0],invoice.Customers[0]);
  }

  [TestMethod]
  public void ChangeToFinishStatus_MustBeFinishStatus()
  {
    var reservation = new Reservation(TestParameters.Room,DateTime.Now.AddDays(3),[TestParameters.Customer]);
    var invoice = reservation.GenerateInvoiceAndCheckOut(EPaymentMethod.Pix,0);
    invoice.FinishInvoice();
    Assert.AreEqual(EStatus.Finish,invoice.Status);
  }




}