using Hotel.Domain.DTOs.Interfaces;

namespace Hotel.Domain.DTOs.Base;

abstract public class QueryParameters : IDataTransferObject
{
  public QueryParameters(int? skip, int? take, DateTime? createdAt, string? createdAtOperator)
  {
    Skip = skip;
    Take = take;
    CreatedAt = createdAt;
    CreatedAtOperator = createdAtOperator;
  }

  public int? Skip { get; private set; }
  public int? Take { get; private set; }
  public DateTime? CreatedAt { get; private set; }
  public string? CreatedAtOperator{ get; private set; }
}
