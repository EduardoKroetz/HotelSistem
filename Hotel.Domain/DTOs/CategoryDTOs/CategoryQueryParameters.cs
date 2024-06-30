namespace Hotel.Domain.DTOs.CategoryDTOs;

public class CategoryQueryParameters : IDataTransferObject
{
    public int? Skip { get; set; }
    public int? Take { get; set; }
    public string? Name { get; set; }
    public decimal? AveragePrice { get; set; }
    public string? AveragePriceOperator { get; set; }
    public Guid? RoomId { get; set; }
}
