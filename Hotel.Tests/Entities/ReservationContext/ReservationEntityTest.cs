using Hotel.Domain.Entities.CustomerContext;
using Hotel.Domain.Entities.PaymentContext.InvoiceRoomEntity;
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
    var reservation = new Reservation(TestParameters.Room,ThreeDaysFromNow,[TestParameters.Customer]);
    Assert.IsTrue(reservation.IsValid);
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  public void WithOutCustomers_ExpectedException()
  {
    new Reservation(TestParameters.Room,OneDayFromNow,[]);
    Assert.Fail();
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  public void InvalidCheckIn_ExpectedException()
  {
    new Reservation(TestParameters.Room,CurrentDate.AddDays(-1),[TestParameters.Customer]);
    Assert.Fail();
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  public void InvalidCheckOut_ExpectedException()
  {
    new Reservation(TestParameters.Room,OneDayFromNow,[TestParameters.Customer],CurrentDate.AddDays(-1));
    Assert.Fail();
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  public void CheckInBiggerThanCheckOut_ExpectedException()
  {
    new Reservation(TestParameters.Room,TwoDaysFromNow,[TestParameters.Customer],OneDayFromNow);
    Assert.Fail();
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  public void OvertakingTheRoomCapacity_ExpectedException()
  {
    var room = new Room(22,50m,1,"Um quarto para hospedagem.",TestParameters.Category);
    var customer = new Customer(TestParameters.Name,TestParameters.Email,TestParameters.Phone,"password123");
    new Reservation(room,OneDayFromNow,[TestParameters.Customer,customer]);
    Assert.Fail();
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  public void ChangeToStatusToCheckedInWithOutSameDate_ExpectedException()
  {
    var reservation = new Reservation(TestParameters.Room,TwoDaysFromNow,[TestParameters.Customer]);
    reservation.StatusToCheckedIn();
    Assert.Fail();
  }

  [TestMethod]
  public void ChangeToStatusToCheckedIn_WithSameDate_MustBeStatusToCheckedIns_And_RoomOccupiedStatus()
  {
    var reservation = new Reservation(TestParameters.Room,CurrentDate,[TestParameters.Customer]);
    reservation.StatusToCheckedIn();
    Assert.AreEqual(EReservationStatus.CheckedIn, reservation.Status);
    Assert.AreEqual(ERoomStatus.Occupied, reservation.Room.Status);
  }

  [TestMethod]
  public void ChangeToCheckCancelledStatus_MustBeCancelledStatus_And_RoomAvailableStatus()
  {
    var reservation = new Reservation(TestParameters.Room,TwoDaysFromNow,[TestParameters.Customer]);
    reservation.StatusToCancelled();
    Assert.AreEqual(EReservationStatus.Cancelled, reservation.Status);
    Assert.AreEqual(ERoomStatus.Available, reservation.Room.Status);
  }

  [TestMethod]
  public void ChangeToNoShowStatus_MustBeNoShowStatus_And_RoomReservedStatus()
  {
    var reservation = new Reservation(TestParameters.Room,TwoDaysFromNow,[TestParameters.Customer]);
    reservation.StatusToNoShow();
    Assert.AreEqual(EReservationStatus.NoShow, reservation.Status);
    Assert.AreEqual(ERoomStatus.Reserved, reservation.Room.Status);
  }

  [TestMethod]
  public void GenerateInvoiceMethod_MustGenerateInvoice_ChangeCheckoutDateTime_ChangeToCheckOutStatus_And_RoomOutOfServiceStatus()
  {
    var reservation = new Reservation(TestParameters.Room,CurrentDate.Date,[TestParameters.Customer]);
    reservation.GenerateInvoice(EPaymentMethod.Pix);
    Assert.AreNotEqual(null, reservation.Invoice);
    Assert.AreEqual(EReservationStatus.CheckedOut, reservation.Status);
    Assert.AreEqual(ERoomStatus.OutOfService, reservation.Room.Status);
    Assert.AreEqual(CurrentDate.DayOfYear, reservation?.CheckOut?.DayOfYear);
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  public void CalculateTotalAmount_WithOutCheckOut_ExpectedException()
  {
    var reservation = new Reservation(TestParameters.Room,CurrentDate.Date,[TestParameters.Customer]);
    reservation.TotalAmount();
    Assert.Fail();
  }

  [TestMethod]
  public void StayedForTwoDays_TheHostedDaysMustBe2()
  {
    var reservation = new Reservation(TestParameters.Room,CurrentDate.Date,[TestParameters.Customer]);
    reservation.ChangeCheckOut(TwoDaysFromNow);
    Assert.AreEqual(2,reservation.HostedDays);
  }

  [TestMethod]
  public void StayedFor7Days_TheHostedDaysMustBe7()
  {
    var reservation = new Reservation(TestParameters.Room,CurrentDate.Date,[TestParameters.Customer],CurrentDate.AddDays(7));
    Assert.AreEqual(7,reservation.HostedDays);
  }

  [TestMethod]
  public void RoomPriceAt50_ForOneDay_TheTotalValueMustBe50()
  {
    var reservation = new Reservation(TestParameters.Room,CurrentDate.Date,[TestParameters.Customer]);
    reservation.ChangeCheckOut(OneDayFromNow);
    Assert.AreEqual(50,reservation.TotalAmount());
  }

  [TestMethod]
  public void RoomPriceAt50_ForTwoDays_TheTotalValueMustBe100()
  {
    var reservation = new Reservation(TestParameters.Room,CurrentDate.Date,[TestParameters.Customer]);
    reservation.ChangeCheckOut(TwoDaysFromNow);
    Assert.AreEqual(100,reservation.TotalAmount());
  }

  [TestMethod]
  public void AddService_TheServicesCountMustBe1()
  {
    var reservation = new Reservation(TestParameters.Room,CurrentDate.Date,[TestParameters.Customer]);
    reservation.AddService(TestParameters.Service);
    Assert.AreEqual(1,reservation.Services.Count);
  }

  [TestMethod]
  public void AddIdenticalServices_TheServicesCountMustBe2()
  {
    var reservation = new Reservation(TestParameters.Room,CurrentDate.Date,[TestParameters.Customer]);
    reservation.AddService(TestParameters.Service);
    reservation.AddService(TestParameters.Service);
    Assert.AreEqual(2,reservation.Services.Count);
  }

  
  [TestMethod]
  public void RoomAtPrice50_For3Days_WithTwoServicesAtPrice5_TheTotalPriceMustBe160()
  {
    var service = new Service("Serviço de limpeza",5m,false,EPriority.Low,20);
    var reservation = new Reservation(TestParameters.Room,CurrentDate.Date,[TestParameters.Customer]);
    reservation.ChangeCheckOut(ThreeDaysFromNow);
    reservation.AddService(service);
    reservation.AddService(service);
    Assert.AreEqual(160,reservation.TotalAmount());
  }

  [TestMethod]
  public void RoomAtPrice19_For14Days_With3ServicesAtPrice5_With7ServicesAtPrice15_TheTotalPriceMustBe386()
  {
    var room = new Room(23,19m,7,"Um quarto para hospedagem.",TestParameters.Category);
    var cleanService = new Service("Serviço de limpeza",5m,false,EPriority.Low,20);
    var lunchService = new Service("Almoço",15m,false,EPriority.Medium,50);
    var reservation = new Reservation(room,CurrentDate.Date,[TestParameters.Customer]);
    reservation.ChangeCheckOut(CurrentDate.AddDays(14));

    for (int i = 1; i <= 7;i++)
    {
      if (i <= 3)
        reservation.AddService(cleanService);
      reservation.AddService(lunchService);
    }
  
    Assert.AreEqual(386,reservation.TotalAmount());
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  public void ChangeCheckIn_WithInvalidStatus_ExpectedException()
  {
    var room = new Room(23,19m,7,"Um quarto para hospedagem.",TestParameters.Category);
    var reservation = new Reservation(room,CurrentDate.Date,[TestParameters.Customer]);
    reservation.StatusToCheckedIn().ChangeCheckIn(ThreeDaysFromNow);;
    Assert.Fail();
  }

  [TestMethod]
  public void ChangeCheckIn_BeingValid_MustBeChanged()
  {
    var reservation = new Reservation(TestParameters.Room,CurrentDate.Date,[TestParameters.Customer]);
    reservation.ChangeCheckIn(ThreeDaysFromNow);
    Assert.AreEqual(ThreeDaysFromNow.Date,reservation.CheckIn.Date);
  }

  [TestMethod]
  public void ChangeCheckOut_BeingValid_MustBeChanged_AndUpdate_HostedDays()
  {
    var reservation = new Reservation(TestParameters.Room,CurrentDate.Date,[TestParameters.Customer]);
    reservation.ChangeCheckOut(ThreeDaysFromNow);

    Assert.AreEqual(ThreeDaysFromNow.Date,reservation?.CheckOut?.Date);
    Assert.AreEqual(3,reservation?.HostedDays);
  }
}