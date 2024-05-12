using Hotel.Domain.DTOs.Base;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.ReservationContext.ReservationDTOs;

public class ReservationQueryParameters : QueryParameters
{
  public ReservationQueryParameters(int? skip, int? take, DateTime? createdAt, string? createdAtOperator, int? hostedDays, decimal? dailyRate, DateTime? checkIn, string? checkInOperator, DateTime? checkOut, EReservationStatus? status, int? capacity, Guid? roomId, Guid? customerId, Guid? invoiceId, Guid? serviceId, string? checkOutOperator, string? capacityOperator, string? hostedDaysOperator, string? dailyRateOperator) : base(skip, take, createdAt, createdAtOperator)
  {
    HostedDays = hostedDays;
    HostedDaysOperator = hostedDaysOperator;
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
  }

  public int? HostedDays { get; private set; }
  public string? HostedDaysOperator { get; private set; }
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
}
