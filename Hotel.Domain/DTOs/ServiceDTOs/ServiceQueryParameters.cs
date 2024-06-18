using Hotel.Domain.DTOs.Base;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.ServiceDTOs;

public class ServiceQueryParameters : QueryParameters
{
    public ServiceQueryParameters(int? skip, int? take, string? name, decimal? price, string? priceOperator, EPriority? priority, bool? isActive, int? timeInMinutes, string? timeInMinutesOperator, Guid? responsibilityId, Guid? reservationId, Guid? roomInvoiceId, Guid? roomId, DateTime? createdAt, string? createdAtOperator) : base(skip, take, createdAt, createdAtOperator)
    {
        Name = name;
        Price = price;
        PriceOperator = priceOperator;
        Priority = priority;
        IsActive = isActive;
        TimeInMinutes = timeInMinutes;
        TimeInMinutesOperator = timeInMinutesOperator;
        ResponsibilityId = responsibilityId;
        ReservationId = reservationId;
        InvoiceId = roomInvoiceId;
        RoomId = roomId;
    }

    public string? Name { get; private set; }
    public decimal? Price { get; private set; }
    public string? PriceOperator { get; private set; }
    public EPriority? Priority { get; private set; }
    public bool? IsActive { get; private set; }
    public int? TimeInMinutes { get; private set; }
    public string? TimeInMinutesOperator { get; private set; }
    public Guid? ResponsibilityId { get; private set; }
    public Guid? ReservationId { get; private set; }
    public Guid? InvoiceId { get; private set; }
    public Guid? RoomId { get; private set; }
}
