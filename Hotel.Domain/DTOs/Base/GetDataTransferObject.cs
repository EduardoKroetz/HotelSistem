using Hotel.Domain.DTOs.Interfaces;

namespace Hotel.Domain.DTOs.Base;

public class GetDataTransferObject : IDataTransferObject
{
  public GetDataTransferObject(Guid id, DateTime createdAt)
  {
    Id = id;
    CreatedAt = createdAt;
  }

  public Guid Id { get; private set; }
  public DateTime CreatedAt { get; private set; }
}
