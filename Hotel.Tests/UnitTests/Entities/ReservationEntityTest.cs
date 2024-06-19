using Hotel.Domain.Entities.CustomerEntity;
using Hotel.Domain.Entities.ReservationEntity;
using Hotel.Domain.Entities.RoomEntity;
using Hotel.Domain.Entities.ServiceEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Exceptions;

namespace Hotel.Tests.UnitTests.Entities;

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
        var room = new Room("Quarto padrão para 3 pessoas",1, 50, 3, "Quarto padrão", TestParameters.Category);
        var reservation = new Reservation(room, TwoDaysFromNow, ThreeDaysFromNow, TestParameters.Customer, 3);
        Assert.IsTrue(reservation.IsValid);
    }

    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    public void InvalidExpectedCheckIn_ExpectedException()
    {
        var room = new Room("Quarto padrão para 3 pessoas",1, 50, 3, "Quarto padrão", TestParameters.Category);
        new Reservation(room, CurrentDate.AddDays(-1), ThreeDaysFromNow, TestParameters.Customer, 2);
        Assert.Fail();
    }

    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    public void InvalidExpectedCheckOut_ExpectedException()
    {
        var room = new Room("Quarto padrão para 3 pessoas",1, 50, 3, "Quarto padrão", TestParameters.Category);
        new Reservation(room, OneDayFromNow, CurrentDate.AddDays(-1), TestParameters.Customer, 2);
        Assert.Fail();
    }

    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    public void ExpectedCheckInBiggerThanExpectedCheckOut_ExpectedException()
    {
        var room = new Room("Quarto padrão para 3 pessoas",1, 50, 3, "Quarto padrão", TestParameters.Category);
        new Reservation(room, TwoDaysFromNow, OneDayFromNow, TestParameters.Customer, 2);
        Assert.Fail();
    }

    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    public void OvertakingTheRoomCapacity_ExpectedException()
    {
        var room = new Room("Quarto padrão para 3 pessoas",22, 50m, 1, "Um quarto para hospedagem.", TestParameters.Category);
        var customer = new Customer(TestParameters.Name, TestParameters.Email, TestParameters.Phone, "password123");
        new Reservation(room, OneDayFromNow, TwoDaysFromNow, customer, 2);
        Assert.Fail();
    }

    [TestMethod]
    public void ToCheckIn_WithSameDate_UpdateStatus_And_RoomOccupiedStatus()
    {
        var room = new Room("Quarto padrão para 3 pessoas",1, 50, 3, "Quarto padrão", TestParameters.Category);
        var reservation = new Reservation(room, CurrentDate, TwoDaysFromNow, TestParameters.Customer, 3);
        reservation.ToCheckIn();
        Assert.AreEqual(EReservationStatus.CheckedIn, reservation.Status);
        Assert.AreEqual(ERoomStatus.Occupied, reservation?.Room?.Status);
    }

    [TestMethod]
    public void ToCancelled_MustBeCancelledStatus_And_RoomAvailableStatus()
    {
        var room = new Room("Quarto padrão para 3 pessoas",1, 50, 3, "Quarto padrão", TestParameters.Category);
        var reservation = new Reservation(room, TwoDaysFromNow, ThreeDaysFromNow, TestParameters.Customer, 2);
        reservation.ToCancelled();
        Assert.AreEqual(EReservationStatus.Cancelled, reservation.Status);
        Assert.AreEqual(ERoomStatus.OutOfService, reservation?.Room?.Status);
    }

    [TestMethod]
    public void ToNoShow_MustBeNoShowStatus_And_ReservedRoomStatus()
    {
        var room = new Room("Quarto padrão para 3 pessoas",1, 50, 3, "Quarto padrão", TestParameters.Category);
        var reservation = new Reservation(room, TwoDaysFromNow, ThreeDaysFromNow, TestParameters.Customer, 2);
        reservation.ToNoShow();
        Assert.AreEqual(EReservationStatus.NoShow, reservation.Status);
        Assert.AreEqual(ERoomStatus.Reserved, reservation?.Room?.Status);
    }

    [TestMethod]
    public void FinishMethod_MustFinish_ChangeCheckoutDateTime_ChangeToCheckOutStatus_And_RoomOutOfServiceStatus()
    {
        var room = new Room("Quarto padrão para 3 pessoas",1, 50, 3, "Quarto padrão", TestParameters.Category);
        var reservation = new Reservation(room, CurrentDate, ThreeDaysFromNow, TestParameters.Customer, 2);

        reservation.ToCheckIn();
        reservation.Finish(EPaymentMethod.Pix);
        Assert.AreNotEqual(null, reservation.Invoice);
        Assert.AreEqual(EReservationStatus.CheckedOut, reservation.Status);
        Assert.AreEqual(ERoomStatus.OutOfService, reservation?.Room?.Status);
        Assert.AreEqual(CurrentDate.DayOfYear, reservation?.CheckOut?.DayOfYear);
    }

    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    public void CalculateTotalAmount_WithOutCheckIn_ExpectedException()
    {
        var room = new Room("Quarto padrão para 3 pessoas",1, 50, 3, "Quarto padrão", TestParameters.Category);
        var reservation = new Reservation(room, CurrentDate, ThreeDaysFromNow, TestParameters.Customer, 2);
        reservation.TotalAmount();
        Assert.Fail();
    }

    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    public void CalculateTotalAmount_WithOutCheckOut_ExpectedException()
    {
        var room = new Room("Quarto padrão para 3 pessoas",1, 50, 3, "Quarto padrão", TestParameters.Category);
        var reservation = new Reservation(room, CurrentDate, ThreeDaysFromNow, TestParameters.Customer, 2);
        reservation.ToCheckIn();
        reservation.TotalAmount();
        Assert.Fail();
    }

    [TestMethod]
    public void CalculateTotalAmount()
    {
        var room = new Room("Quarto padrão para 3 pessoas",1, 50, 3, "Quarto padrão", TestParameters.Category);
        var reservation = new Reservation(room, CurrentDate, ThreeDaysFromNow, TestParameters.Customer, 2);
        reservation.ToCheckIn();
        reservation.Finish(EPaymentMethod.Pix);
        var totalAmount = Math.Round(reservation.TotalAmount());
        Assert.AreEqual(0, totalAmount); //Não ficou hospedado nem por um minuto 
    }

    [TestMethod]
    public void CalculateExpectedTotalAmount()
    {
        var room = new Room("Quarto padrão para 3 pessoas",1, 50, 3, "Quarto padrão", TestParameters.Category);
        var reservation = new Reservation(room, CurrentDate, ThreeDaysFromNow, TestParameters.Customer, 2);
        var expectedTotalAmount = Math.Round(reservation.ExpectedTotalAmount());
        Assert.AreEqual(150, expectedTotalAmount);
    }

    [TestMethod]
    public void StayedForTwoDays_TheExpectedTimeHostedMustBe2Days()
    {
        var room = new Room("Quarto padrão para 3 pessoas",1, 50, 3, "Quarto padrão", TestParameters.Category);
        var reservation = new Reservation(room, CurrentDate, TwoDaysFromNow, TestParameters.Customer, 2);

        var timeHosted = Reservation.GetTimeHosted(CurrentDate, TwoDaysFromNow);

        Assert.AreEqual(timeHosted, reservation.ExpectedTimeHosted);
    }

    [TestMethod]
    public void StayedFor7Days_TheExpectedTimeHostedMustBe7()
    {
        var room = new Room("Quarto padrão para 3 pessoas",1, 50, 3, "Quarto padrão", TestParameters.Category);
        var reservation = new Reservation(room, CurrentDate, CurrentDate.AddDays(7), TestParameters.Customer, 2);

        var timeHosted = Reservation.GetTimeHosted(CurrentDate, CurrentDate.AddDays(7));
        Assert.AreEqual(timeHosted, reservation.ExpectedTimeHosted);
    }

    [TestMethod]
    public void RoomPriceAt50_Per24Hours_TheExpectedTotalAmountMustBe50()
    {
        var room = new Room("Quarto padrão para 3 pessoas",1, 50, 3, "Quarto padrão", TestParameters.Category);
        var reservation = new Reservation(room, CurrentDate, OneDayFromNow, TestParameters.Customer, 2);

        Assert.AreEqual(50, Math.Round(reservation.ExpectedTotalAmount()));
    }

    [TestMethod]
    public void RoomPriceAt50_For48Hours_TheExpectedTotalValueMustBe100()
    {
        var room = new Room("Quarto padrão para 3 pessoas",1, 50, 3, "Quarto padrão", TestParameters.Category);
        var reservation = new Reservation(room, CurrentDate, TwoDaysFromNow, TestParameters.Customer, 2);
        Assert.AreEqual(100, Math.Round(reservation.ExpectedTotalAmount()));
    }

    [TestMethod]
    public void AddService_TheServicesCountMustBe1()
    {
        var room = new Room("Quarto padrão para 3 pessoas",1, 50, 3, "Quarto padrão", TestParameters.Category);
        var reservation = new Reservation(room, CurrentDate, ThreeDaysFromNow, TestParameters.Customer, 2);
        reservation.AddService(TestParameters.Service);
        Assert.AreEqual(1, reservation.Services.Count);
    }

    [TestMethod]
    public void AddSameServices_TheServicesCountMustBe2()
    {
        var room = new Room("Quarto padrão para 3 pessoas",1, 50, 3, "Quarto padrão", TestParameters.Category);
        var reservation = new Reservation(room, CurrentDate, ThreeDaysFromNow, TestParameters.Customer, 2);
        reservation.AddService(TestParameters.Service);
        reservation.AddService(TestParameters.Service);
        Assert.AreEqual(2, reservation.Services.Count);
    }


    [TestMethod]
    public void RoomAtPrice50_For3Days_WithTwoServicesAtPrice5_TheExpectedTotalPriceMustBe160()
    {
        var room = new Room("Quarto padrão para 3 pessoas",1, 50, 3, "Quarto padrão", TestParameters.Category);
        var service = new Service("Serviço de limpeza", "Serviço de limpeza", 5m, EPriority.Low, 20);
        var reservation = new Reservation(room, CurrentDate, ThreeDaysFromNow, TestParameters.Customer, 2);

        reservation.AddService(service);
        reservation.AddService(service);


        var totalAmount = Math.Round(reservation.ExpectedTotalAmount());
        Assert.AreEqual(160, totalAmount);
    }

    [TestMethod]
    public void RoomAtPrice19_For0Minutes_With3ServicesAtPrice5_With7ServicesAtPrice15_TheTotalPriceMustBe120()
    {
        var room = new Room("Quarto padrão para 3 pessoas",23, 19m, 7, "Um quarto para hospedagem.", TestParameters.Category);
        var cleanService = new Service("Serviço de limpeza", "Serviço de limpeza", 5m, EPriority.Low, 20);
        var lunchService = new Service("Almoço", "Almoço", 15m, EPriority.Medium, 50);
        var reservation = new Reservation(room, DateTime.Now, DateTime.Now.AddDays(1), TestParameters.Customer, 2);

        for (int i = 1; i <= 7; i++)
        {
            if (i <= 3)
                reservation.AddService(cleanService);
            reservation.AddService(lunchService);
        }

        reservation.ToCheckIn();
        reservation.Finish(EPaymentMethod.Pix);

        var totalAmount = Math.Round(reservation.TotalAmount());
        Assert.AreEqual(120, totalAmount);
    }


    [TestMethod]
    public void RoomAtPrice19_For24Hours_With3ServicesAtPrice5_With7ServicesAtPrice15_TheTotalPriceMustBe139()
    {
        var room = new Room("Quarto padrão para 3 pessoas",23, 19m, 7, "Um quarto para hospedagem.", TestParameters.Category);
        var cleanService = new Service("Serviço de limpeza", "Serviço de limpeza", 5m, EPriority.Low, 20);
        var lunchService = new Service("Almoço", "Almoço", 15m, EPriority.Medium, 50);
        var reservation = new Reservation(room, CurrentDate, CurrentDate.AddDays(1), TestParameters.Customer, 2);

        for (int i = 1; i <= 7; i++)
        {
            if (i <= 3)
                reservation.AddService(cleanService);
            reservation.AddService(lunchService);
        }

        var totalAmount = Math.Round(reservation.ExpectedTotalAmount());
        Assert.AreEqual(139, totalAmount);
    }

    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    public void UpdateExpectedCheckIn_WithInvalidStatus_ExpectedException()
    {
        var room = new Room("Quarto padrão para 3 pessoas",23, 19m, 7, "Um quarto para hospedagem.", TestParameters.Category);
        var reservation = new Reservation(room, CurrentDate, ThreeDaysFromNow, TestParameters.Customer, 2);
        reservation.ToCheckIn();
        reservation.UpdateExpectedCheckIn(DateTime.Now.AddDays(1));
        Assert.Fail();
    }

    [TestMethod]
    public void UpdateExpectedCheckIn()
    {
        var room = new Room("Quarto padrão para 3 pessoas",1, 50, 3, "Quarto padrão", TestParameters.Category);
        var reservation = new Reservation(room, CurrentDate, ThreeDaysFromNow, TestParameters.Customer, 2);
        reservation.UpdateExpectedCheckIn(ThreeDaysFromNow);
        Assert.AreEqual(ThreeDaysFromNow, reservation.ExpectedCheckIn);
    }

    [TestMethod]
    public void UpdateExpectedCheckOut()
    {
        var room = new Room("Quarto padrão para 3 pessoas",1, 50, 3, "Quarto padrão", TestParameters.Category);
        var reservation = new Reservation(room, CurrentDate, TwoDaysFromNow, TestParameters.Customer, 2);
        reservation.UpdateExpectedCheckOut(TwoDaysFromNow);
        var timespan = TwoDaysFromNow - CurrentDate;

        Assert.AreEqual(TwoDaysFromNow, reservation.ExpectedCheckOut);
        Assert.AreEqual(timespan.TotalMinutes, reservation.ExpectedTimeHosted.TotalMinutes);
    }

    [TestMethod]
    public void ToCheckIn()
    {
        var room = new Room("Quarto padrão para 3 pessoas",1, 50, 3, "Quarto padrão", TestParameters.Category);
        var reservation = new Reservation(room, CurrentDate, TwoDaysFromNow, TestParameters.Customer, 2);
        reservation.ToCheckIn();

        Assert.AreEqual(EReservationStatus.CheckedIn, reservation.Status);
        Assert.AreEqual(ERoomStatus.Occupied, reservation?.Room?.Status);
        Assert.AreEqual(DateTime.Now.Date, reservation?.CheckIn!.Value.Date);
    }


}