using Hotel.Domain.DTOs.Base;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.ReservationContext.ReservationDTOs;

public class ReservationQueryParameters : QueryParameters
{

  public ReservationQueryParameters(int? skip, int? take, TimeSpan? timeHosted, string? timeHostedOperator, decimal? dailyRate, string? dailyRateOperator, DateTime? checkIn, string? checkInOperator, DateTime? checkOut, string? checkOutOperator, EReservationStatus? status, int? capacity, string? capacityOperator, Guid? roomId, Guid? customerId, Guid? invoiceId, Guid? serviceId, DateTime? createdAt, string? createdAtOperator, DateTime? expectedCheckIn, string? expectedCheckInOperator, DateTime? expectedCheckOut, string? expectedCheckOutOperator, TimeSpan? expectedTimeHosted, string? expectedTimeHostedOperator) : base(skip, take, createdAt, createdAtOperator)
  {
    TimeHosted = timeHosted;
    TimeHostedOperator = timeHostedOperator;
    DailyRate = dailyRate;
    DailyRateOperator = dailyRateOperator;
    CheckIn = checkIn;
    CheckInOperator = checkInOperator;
    CheckOut = checkOut;
    CheckOutOperator = checkOutOperator;
    Status = status;
    Capacity = capacity;
    CapacityOperator = capacityOperator;
    RoomId = roomId;
    CustomerId = customerId;
    InvoiceId = invoiceId;
    ServiceId = serviceId;
    ExpectedCheckIn = expectedCheckIn;
    ExpectedCheckInOperator = expectedCheckInOperator;
    ExpectedCheckOut = expectedCheckOut;
    ExpectedCheckOutOperator = expectedCheckOutOperator;
    ExpectedTimeHostedOperator = expectedTimeHostedOperator;
    ExpectedTimeHosted = expectedTimeHosted;
  }

  public TimeSpan? TimeHosted { get; private set; }
  public string? TimeHostedOperator { get; private set; }
  public decimal? DailyRate { get; private set; }
  public string? DailyRateOperator { get; private set; }
  public DateTime? CheckIn { get; private set; }
  public string? CheckInOperator { get; private set; }
  public DateTime? CheckOut { get; private set; }
  public string? CheckOutOperator { get; private set; }
  public EReservationStatus? Status { get; private set; }
  public int? Capacity { get; private set; }
  public string? CapacityOperator { get; private set; }
  public Guid? RoomId { get; private set; }
  public Guid? CustomerId { get; private set; }
  public Guid? InvoiceId { get; private set; }
  public Guid? ServiceId { get; private set; }
  public TimeSpan? ExpectedTimeHosted { get; private set; }
  public string? ExpectedTimeHostedOperator { get; private set; }
  public DateTime? ExpectedCheckIn { get; private set; }
  public string? ExpectedCheckInOperator { get; private set; }
  public DateTime? ExpectedCheckOut { get; private set; }
  public string? ExpectedCheckOutOperator { get; private set; }

}
