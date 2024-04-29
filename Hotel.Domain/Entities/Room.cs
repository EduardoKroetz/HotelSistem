namespace Hotel.Domain.Entities;

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
        CategoryId = categoryId;
        Category = category;
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
  public Category Category { get; private set; }
}