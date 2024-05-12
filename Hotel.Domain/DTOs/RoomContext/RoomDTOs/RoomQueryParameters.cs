using Hotel.Domain.DTOs.Base;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.RoomContext.RoomDTOs;

public class RoomQueryParameters : QueryParameters
{
  public RoomQueryParameters(int? skip, int? take, DateTime? createdAt, string? createdAtOperator, int? number, decimal? price, ERoomStatus? status, int? capacity, Guid? serviceId, Guid? categoryId) : base(skip, take, createdAt, createdAtOperator)
  {
    Number = number;
    Price = price;
    Status = status;
    Capacity = capacity;
    ServiceId = serviceId;
    CategoryId = categoryId;
  }

  public int? Number { get; private set; }
  public decimal? Price { get; private set; }
  public ERoomStatus? Status { get; private set; }
  public int? Capacity { get; private set; }
  public Guid? ServiceId { get; private set; }
  public Guid? CategoryId { get; private set; }

}
