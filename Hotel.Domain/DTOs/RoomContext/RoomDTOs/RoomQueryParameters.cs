using Hotel.Domain.DTOs.Base;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.RoomContext.RoomDTOs;

public class RoomQueryParameters : QueryParameters
{
  public RoomQueryParameters(int? skip, int? take, int? number, string? numberOperator, decimal? price, string? priceOperator, ERoomStatus? status, int? capacity, string? capacityOperator, Guid? serviceId, Guid? categoryId, DateTime? createdAt, string? createdAtOperator, bool? isActive) : base(skip, take, createdAt, createdAtOperator)
  {
    Number = number;
    NumberOperator = numberOperator;
    Price = price;
    PriceOperator = priceOperator;
    Status = status;
    Capacity = capacity;
    CapacityOperator = capacityOperator;
    ServiceId = serviceId;
    CategoryId = categoryId;
    IsActive = isActive;
  }

  public int? Number { get; private set; }
  public string? NumberOperator { get; private set; }
  public decimal? Price { get; private set; }
  public string? PriceOperator { get; private set; }
  public ERoomStatus? Status { get; private set; }
  public int? Capacity { get; private set; }
  public string? CapacityOperator { get; private set; }
  public Guid? ServiceId { get; private set; }
  public Guid? CategoryId { get; private set; }
  public bool? IsActive { get; private set; }
}
