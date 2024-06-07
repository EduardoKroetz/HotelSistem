using Hotel.Domain.DTOs.Interfaces;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.ReservationContext.ReservationDTOs;

public class GetReservation : IDataTransferObject
{
  public GetReservation(Guid id, decimal dailyRate, TimeSpan expectedTimeHosted, DateTime expectedCheckIn, DateTime expectedCheckOut, TimeSpan? timeHosted, DateTime? checkIn, DateTime? checkOut, EReservationStatus status, int capacity, Guid roomId, Guid customerId, Guid? invoiceId)
  {
    Id = id;
    DailyRate = dailyRate;
    ExpectedTimeHosted = expectedTimeHosted;
    ExpectedCheckIn = expectedCheckIn;
    ExpectedCheckOut = expectedCheckOut;
    TimeHosted = timeHosted;
    CheckIn = checkIn;
    CheckOut = checkOut;
    Status = status;
    Capacity = capacity;
    RoomId = roomId;
    CustomerId = customerId;
    InvoiceId = invoiceId;
  }

  public Guid Id { get; private set; }
  public decimal DailyRate { get; private set; }
  public TimeSpan ExpectedTimeHosted { get; private set; }
  public DateTime ExpectedCheckIn { get; private set; }
  public DateTime ExpectedCheckOut { get; private set; }
  public TimeSpan? TimeHosted { get; private set; }
  public DateTime? CheckIn { get; private set; }
  public DateTime? CheckOut { get; private set; }
  public EReservationStatus Status { get; private set; }
  public int Capacity { get; private set; }
  public Guid RoomId { get; private set; }
  public Guid CustomerId { get; private set; }
  public Guid? InvoiceId { get; private set; }
}