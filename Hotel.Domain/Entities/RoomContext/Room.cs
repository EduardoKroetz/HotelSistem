using Hotel.Domain.Entities.Base;

namespace Hotel.Domain.Entities.RoomContext;

public class Room : Entity
{
  public Room(int number, decimal price, bool isActive, bool isReserved, int capacity, string description, Guid roomServicesId, Guid categoryId, Category category)
  {
    Number = number;
    Price = price;
    IsActive = isActive;
    IsReserved = isReserved;
    Capacity = capacity;
    Description = description;
    RoomServicesId = roomServicesId;
    CategoryId = categoryId;
    Category = category;
    Services = [];
    Images = [];
  }

  public int Number { get; private set; }
  public decimal Price { get; private set; }
  public bool IsActive { get; private set; }
  public bool IsReserved { get; private set; }
  public int Capacity { get; private set; }
  public string Description { get; private set; }
  public Guid RoomServicesId { get; private set; }
  public List<Service> Services { get; private set; } 
  public Guid CategoryId { get; private set; }
  public Category? Category { get; private set; }
  public List<Image> Images { get; private set; } 
}