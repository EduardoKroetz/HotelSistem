
using Hotel.Domain.Entities.CustomerEntity;
using Hotel.Domain.Entities.InvoiceEntity;
using Hotel.Domain.Entities.ReservationEntity;
using Hotel.Domain.Entities.RoomEntity;
using Hotel.Domain.Entities.ServiceEntity;
using Hotel.Domain.Enums;

namespace Hotel.Domain.Entities.Interfaces;

public interface IReservation
{
    TimeSpan? TimeHosted { get; }
    decimal DailyRate { get; }
    TimeSpan ExpectedTimeHosted { get; }
    DateTime ExpectedCheckIn { get; }
    DateTime ExpectedCheckOut { get; }
    DateTime? CheckIn { get; }
    DateTime? CheckOut { get; }
    EReservationStatus Status { get; }
    int Capacity { get; }
    string StripePaymentIntentId { get; }
    Guid RoomId { get; }
    Room? Room { get; }
    Guid CustomerId { get; }
    Customer? Customer { get; }
    Guid? InvoiceId { get; }
    Invoice? Invoice { get; }
    ICollection<Service> Services { get; }

    void Validate();
    void AddService(Service service);
    void RemoveService(Service service);
    Invoice Finish(EPaymentMethod paymentMethod, decimal taxInformation = 0);
    Reservation ToCheckIn();
    Reservation ToNoShow();
    Reservation ToCancelled();
    decimal TotalAmount();
    decimal ExpectedTotalAmount();
    Reservation UpdateExpectedCheckOut(DateTime expectedCheckOut);
    Reservation UpdateExpectedCheckIn(DateTime checkIn);
    TimeSpan GetTimeHosted(DateTime checkIn, DateTime checkOut);
    TimeSpan? GetTimeHosted();
}
