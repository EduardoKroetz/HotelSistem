using Hotel.Domain.DTOs.Base;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.RoomContext.ServiceDTOs;

public class ServiceQueryParameters : QueryParameters
{
  public ServiceQueryParameters(int? skip, int? take, DateTime? createdAt, string? createdAtOperator, string? name, decimal? price, EPriority? priority, bool? isActive, int? timeInMinutes, Guid? responsabilityId, Guid? reservationId, Guid? roomInvoiceId, Guid? roomId) : base(skip, take, createdAt, createdAtOperator)
  {
    Name = name;
    Price = price;
    Priority = priority;
    IsActive = isActive;
    TimeInMinutes = timeInMinutes;
    ResponsabilityId = responsabilityId;
    ReservationId = reservationId;
    RoomInvoiceId = roomInvoiceId;
    RoomId = roomId;
  }

  public string? Name { get; private set; }
  public decimal? Price { get; private set; }
  public EPriority? Priority { get; private set; }
  public bool? IsActive { get; private set; }
  public int? TimeInMinutes { get; private set; }
  public Guid? ResponsabilityId { get; private set; }
  public Guid? ReservationId { get; private set; }
  public Guid? RoomInvoiceId { get; private set; }
  public Guid? RoomId { get; private set; }
}
