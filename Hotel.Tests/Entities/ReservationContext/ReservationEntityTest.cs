using Hotel.Domain.Entities.CustomerContext;
using Hotel.Domain.Entities.ReservationContext.ReservationEntity;
using Hotel.Domain.Entities.RoomContext.RoomEntity;
using Hotel.Domain.Entities.RoomContext.ServiceEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Exceptions;

namespace Hotel.Tests.Entities.ReservationContext;

[TestClass]
public class ReservationEntityTest
{
  
  private readonly DateTime CurrentDate = DateTime.Now;
  private readonly DateTime ThreeDaysFromNow = DateTime.Now.AddDays(3);
  private readonly DateTime TwoDaysFromNow = DateTime.Now.AddDays(2);
  private readonly DateTime OneDayFromNow = DateTime.Now.AddDays(1);

  [TestMethod]
  public void ValidReservation_MustBeValid()
  {
    var reservation = new Reservation(TestParameters.Room,ThreeDaysFromNow,TestParameters.Customer, 3);
    Assert.IsTrue(reservation.IsValid);
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  public void InvalidCheckIn_ExpectedException()
  {
    new Reservation(TestParameters.Room,CurrentDate.AddDays(-1),TestParameters.Customer, 2);
    Assert.Fail();
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  public void InvalidCheckOut_ExpectedException()
  {
    new Reservation(TestParameters.Room,OneDayFromNow,TestParameters.Customer, 2,CurrentDate.AddDays(-1));
    Assert.Fail();
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  public void CheckInBiggerThanCheckOut_ExpectedException()
  {
    new Reservation(TestParameters.Room,TwoDaysFromNow,TestParameters.Customer, 2,OneDayFromNow);
    Assert.Fail();
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  public void OvertakingTheRoomCapacity_ExpectedException()
  {
    var room = new Room(22,50m,1,"Um quarto para hospedagem.",TestParameters.Category.Id);
    var customer = new Customer(TestParameters.Name,TestParameters.Email,TestParameters.Phone,"password123");
    new Reservation(room,OneDayFromNow,customer, 2);
    Assert.Fail();
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  public void ChangeToStatusToCheckedInWithOutSameDate_ExpectedException()
  {
    var reservation = new Reservation(TestParameters.Room,TwoDaysFromNow,TestParameters.Customer, 2);
    reservation.StatusToCheckedIn();
    Assert.Fail();
  }

  [TestMethod]
  public void ChangeToStatusToCheckedIn_WithSameDate_MustBeStatusToCheckedIns_And_RoomOccupiedStatus()
  {
    var reservation = new Reservation(TestParameters.Room,CurrentDate,TestParameters.Customer, 3);
    reservation.StatusToCheckedIn();
    Assert.AreEqual(EReservationStatus.CheckedIn, reservation.Status);
    Assert.AreEqual(ERoomStatus.Occupied, reservation?.Room?.Status);
  }

  [TestMethod]
  public void ChangeToCheckCancelledStatus_MustBeCancelledStatus_And_RoomAvailableStatus()
  {
    var reservation = new Reservation(TestParameters.Room,TwoDaysFromNow,TestParameters.Customer, 2);
    reservation.StatusToCancelled();
    Assert.AreEqual(EReservationStatus.Cancelled, reservation.Status);
    Assert.AreEqual(ERoomStatus.Available, reservation?.Room?.Status);
  }

  [TestMethod]
  public void ChangeToNoShowStatus_MustBeNoShowStatus_And_RoomReservedStatus()
  {
    var reservation = new Reservation(TestParameters.Room,TwoDaysFromNow,TestParameters.Customer, 2);
    reservation.StatusToNoShow();
    Assert.AreEqual(EReservationStatus.NoShow, reservation.Status);
    Assert.AreEqual(ERoomStatus.Reserved, reservation?.Room?.Status);
  }

  [TestMethod]
  public void GenerateInvoiceMethod_MustGenerateInvoice_ChangeCheckoutDateTime_ChangeToCheckOutStatus_And_RoomOutOfServiceStatus()
  {
    var reservation = new Reservation(TestParameters.Room,CurrentDate,TestParameters.Customer, 2);
    reservation.GenerateInvoice(EPaymentMethod.Pix);
    Assert.AreNotEqual(null, reservation.Invoice);
    Assert.AreEqual(EReservationStatus.CheckedOut, reservation.Status);
    Assert.AreEqual(ERoomStatus.OutOfService, reservation?.Room?.Status);
    Assert.AreEqual(CurrentDate.DayOfYear, reservation?.CheckOut?.DayOfYear);
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  public void CalculateTotalAmount_WithOutCheckOut_ExpectedException()
  {
    var reservation = new Reservation(TestParameters.Room,CurrentDate,TestParameters.Customer, 2);
    reservation.TotalAmount();
    Assert.Fail();
  }

  [TestMethod]
  public void StayedForTwoDays_TheTimeHostedMustBe2()
  {
    var reservation = new Reservation(TestParameters.Room,CurrentDate,TestParameters.Customer, 2);
    reservation.ChangeCheckOut(TwoDaysFromNow);

    var timeHosted = Reservation.GetTimeHosted(CurrentDate, TwoDaysFromNow);

    Assert.AreEqual(timeHosted, reservation.TimeHosted);
  }

  [TestMethod]
  public void StayedFor7Days_TheTimeHostedMustBe7()
  {
    var reservation = new Reservation(TestParameters.Room,CurrentDate,TestParameters.Customer, 2,CurrentDate.AddDays(7));

    var timeHosted = Reservation.GetTimeHosted(CurrentDate, CurrentDate.AddDays(7));
    Assert.AreEqual(timeHosted, reservation.TimeHosted);
  }

  [TestMethod]
  public void RoomPriceAt50_Per24Hours_TheTotalAmountMustBe50()
  {
    var reservation = new Reservation(TestParameters.Room,CurrentDate,TestParameters.Customer, 2, OneDayFromNow);

    Assert.AreEqual(50,Math.Round(reservation.TotalAmount()));
  }

  [TestMethod]
  public void RoomPriceAt50_For48Hours_TheTotalValueMustBe100()
  {
    var reservation = new Reservation(TestParameters.Room,CurrentDate,TestParameters.Customer, 2, TwoDaysFromNow);
    Assert.AreEqual(100, Math.Round(reservation.TotalAmount()));
  }

  [TestMethod]
  public void AddService_TheServicesCountMustBe1()
  {
    var reservation = new Reservation(TestParameters.Room,CurrentDate,TestParameters.Customer, 2);
    reservation.AddService(TestParameters.Service);
    Assert.AreEqual(1,reservation.Services.Count);
  }

  [TestMethod]
  public void AddSameServices_TheServicesCountMustBe2()
  {
    var reservation = new Reservation(TestParameters.Room,CurrentDate,TestParameters.Customer, 2);
    reservation.AddService(TestParameters.Service);
    reservation.AddService(TestParameters.Service);
    Assert.AreEqual(2,reservation.Services.Count);
  }

  
  [TestMethod]
  public void RoomAtPrice50_For3Days_WithTwoServicesAtPrice5_TheTotalPriceMustBe160()
  {
    var service = new Service("Serviço de limpeza",5m,EPriority.Low,20);
    var reservation = new Reservation(TestParameters.Room,CurrentDate,TestParameters.Customer, 2, ThreeDaysFromNow);

    reservation.AddService(service);
    reservation.AddService(service);

    var totalAmount = Math.Round(reservation.TotalAmount());
    Assert.AreEqual(160, totalAmount);
  }

  [TestMethod]
  public void RoomAtPrice19_For14Days_With3ServicesAtPrice5_With7ServicesAtPrice15_TheTotalPriceMustBe386()
  {
    var room = new Room(23,19m,7,"Um quarto para hospedagem.",TestParameters.Category.Id);
    var cleanService = new Service("Serviço de limpeza",5m,EPriority.Low,20);
    var lunchService = new Service("Almoço",15m,EPriority.Medium,50);
    var reservation = new Reservation(room,CurrentDate,TestParameters.Customer, 2);
    reservation.ChangeCheckOut(CurrentDate.AddDays(14));

    for (int i = 1; i <= 7;i++)
    {
      if (i <= 3)
        reservation.AddService(cleanService);
      reservation.AddService(lunchService);
    }

    var totalAmount = Math.Round(reservation.TotalAmount());
    Assert.AreEqual(386,totalAmount);
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  public void ChangeCheckIn_WithInvalidStatus_ExpectedException()
  {
    var room = new Room(23,19m,7,"Um quarto para hospedagem.",TestParameters.Category.Id);
    var reservation = new Reservation(room,CurrentDate,TestParameters.Customer, 2);
    reservation.StatusToCheckedIn().ChangeCheckIn(ThreeDaysFromNow);;
    Assert.Fail();
  }

  [TestMethod]
  public void ChangeCheckIn_BeingValid_MustBeChanged()
  {
    var reservation = new Reservation(TestParameters.Room,CurrentDate,TestParameters.Customer, 2);
    reservation.ChangeCheckIn(ThreeDaysFromNow);
    Assert.AreEqual(ThreeDaysFromNow.Date,reservation.CheckIn.Date);
  }

  [TestMethod]
  public void ChangeCheckOut_Update_TimeHosted()
  {
    var reservation = new Reservation(TestParameters.Room,CurrentDate,TestParameters.Customer, 2, TwoDaysFromNow);
    reservation.ChangeCheckOut(ThreeDaysFromNow);
    var timespan = ThreeDaysFromNow - CurrentDate;

    Assert.AreEqual(ThreeDaysFromNow, reservation?.CheckOut);
    Assert.AreEqual(timespan.Minutes, reservation?.TimeHosted!.Value.Minutes);
  }
}