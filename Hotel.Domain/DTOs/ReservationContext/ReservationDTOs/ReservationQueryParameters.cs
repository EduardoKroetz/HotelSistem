using Hotel.Domain.DTOs.Base;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.ReservationContext.ReservationDTOs;

public class ReservationQueryParameters : QueryParameters
{
  public ReservationQueryParameters(int? skip, int? take, DateTime? createdAt, string? createdAtOperator, int? hostedDays, decimal? dailyRate, DateTime? checkIn, DateTime? checkOut, EReservationStatus? status, int? capacity, Guid? roomId, Guid? customerId, Guid? invoiceId, Guid? serviceId) : base(skip, take, createdAt, createdAtOperator)
  {
    HostedDays = hostedDays;
    DailyRate = dailyRate;
    CheckIn = checkIn;
    CheckOut = checkOut;
    Status = status;
    Capacity = capacity;
    RoomId = roomId;
    CustomerId = customerId;
    InvoiceId = invoiceId;
    ServiceId = serviceId;
  }

  public int? HostedDays { get; private set; }
  public decimal? DailyRate { get; private set; }
  public DateTime? CheckIn { get; private set; }
  public DateTime? CheckOut { get; private set; }
  public EReservationStatus? Status { get; private set; }
  public int? Capacity { get; private set; }
  public Guid? RoomId { get; private set; }
  public Guid? CustomerId { get; private set; }
  public Guid? InvoiceId { get; private set; }
  public Guid? ServiceId { get; private set; }
}
