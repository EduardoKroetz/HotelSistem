using Hotel.Domain.DTOs.Base;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.ServiceDTOs;

public class ServiceQueryParameters : QueryParameters
{
    public string? Name { get; set; }
    public decimal? Price { get; set; }
    public string? PriceOperator { get; set; }
    public EPriority? Priority { get; set; }
    public bool? IsActive { get; set; }
    public int? TimeInMinutes { get; set; }
    public string? TimeInMinutesOperator { get; set; }
    public Guid? ResponsibilityId { get; set; }
    public Guid? ReservationId { get; set; }
    public Guid? InvoiceId { get; set; }
    public Guid? RoomId { get; set; }
}
