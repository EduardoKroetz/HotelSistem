using Hotel.Domain.DTOs.Interfaces;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.RoomContext.RoomDTOs;

public class GetRoomCollection : IDataTransferObject
{
  public GetRoomCollection(Guid id, int number, decimal price, ERoomStatus status, int capacity, string description, Guid categoryId, DateTime createdAt)
  {
    Id = id;
    Number = number;
    Price = price;
    Status = status;
    Capacity = capacity;
    Description = description;
    CategoryId = categoryId;
    CreatedAt = createdAt;
  }

  public Guid Id { get; set; }
  public int Number { get; private set; }
  public decimal Price { get; private set; }
  public ERoomStatus Status { get; private set; }
  public int Capacity { get; private set; }
  public string Description { get; private set; }
  public Guid CategoryId { get; private set; }
  public DateTime CreatedAt { get; private set; }
}

