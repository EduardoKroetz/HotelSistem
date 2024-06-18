namespace Hotel.Domain.DTOs.CategoryDTOs;

public class CategoryQueryParameters : IDataTransferObject
{
    public CategoryQueryParameters(int? skip, int? take, string? name, decimal? averagePrice, string? averagePriceOperator, Guid? roomId)
    {
        Skip = skip;
        Take = take;
        Name = name;
        AveragePrice = averagePrice;
        AveragePriceOperator = averagePriceOperator;
        RoomId = roomId;
    }

    public int? Skip { get; private set; }
    public int? Take { get; private set; }
    public string? Name { get; private set; }
    public decimal? AveragePrice { get; private set; }
    public string? AveragePriceOperator { get; private set; }
    public Guid? RoomId { get; private set; }
}
