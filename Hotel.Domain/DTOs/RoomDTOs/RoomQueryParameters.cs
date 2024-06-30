using Hotel.Domain.DTOs.Base;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.RoomDTOs;

public class RoomQueryParameters : QueryParameters
{
    public string? Name { get; set; } 
    public int? Number { get; set; }
    public string? NumberOperator { get; set; }
    public decimal? Price { get; set; }
    public string? PriceOperator { get; set; }
    public ERoomStatus? Status { get; set; }
    public int? Capacity { get; set; }
    public string? CapacityOperator { get; set; }
    public Guid? ServiceId { get; set; }
    public Guid? CategoryId { get; set; }

}
