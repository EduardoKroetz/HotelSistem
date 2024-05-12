using Hotel.Domain.DTOs.Interfaces;

namespace Hotel.Domain.DTOs.RoomContext.RoomDTOs;

public class EditorRoom : IDataTransferObject
{
  public EditorRoom(int number, decimal price, int capacity, string description, Guid categoryId)
  {
    Number = number;
    Price = price;
    Capacity = capacity;
    Description = description;
    CategoryId = categoryId;
  }

  public int Number { get; private set; }
  public decimal Price { get; private set; }
  public int Capacity { get; private set; }
  public string Description { get; private set; }
  public Guid CategoryId { get; private set; }
}

