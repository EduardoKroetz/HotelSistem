using Hotel.Domain.DTOs.Base;

namespace Hotel.Domain.DTOs.RoomContext.CategoryDTOs;

public class CategoryQueryParameters : QueryParameters
{
  public CategoryQueryParameters(int? skip, int? take, DateTime? createdAt, string? createdAtOperator, string? name, decimal? averagePrice, string? averagePriceOperator, Guid? roomId) : base(skip, take, createdAt, createdAtOperator)
  {
    Name = name;
    AveragePrice = averagePrice;
    AveragePriceOperator = averagePriceOperator;
    RoomId = roomId;
  }

  public string? Name { get; private set; }
  public decimal? AveragePrice { get; private set; }
  public string? AveragePriceOperator { get; private set; }
  public Guid? RoomId { get; private set; }
}
