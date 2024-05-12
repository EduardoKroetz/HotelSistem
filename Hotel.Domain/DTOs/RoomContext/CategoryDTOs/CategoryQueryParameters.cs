using Hotel.Domain.DTOs.Base;

namespace Hotel.Domain.DTOs.RoomContext.CategoryDTOs;

public class CategoryQueryParameters : QueryParameters
{
  public CategoryQueryParameters(int? skip, int? take, DateTime? createdAt, string? createdAtOperator, string? name, decimal? averagePrice, Guid? roomId) : base(skip, take, createdAt, createdAtOperator)
  {
    Name = name;
    AveragePrice = averagePrice;
    RoomId = roomId;
  }

  public string? Name { get; private set; }
  public decimal? AveragePrice { get; private set; }
  public Guid? RoomId { get; private set; }
}
