namespace Hotel.Domain.DTOs.Base;

abstract public class QueryParameters : IDataTransferObject
{
    public int? Skip { get; set; }
    public int? Take { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? CreatedAtOperator { get; set; }
}
