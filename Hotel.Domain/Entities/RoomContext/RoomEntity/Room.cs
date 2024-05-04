using Hotel.Domain.Entities.Base;
using Hotel.Domain.Entities.Interfaces;
using Hotel.Domain.Entities.RoomContext.CategoryEntity;
using Hotel.Domain.Entities.RoomContext.ImageEntity;
using Hotel.Domain.Entities.RoomContext.ServiceEntity;
using Hotel.Domain.Enums;

namespace Hotel.Domain.Entities.RoomContext.RoomEntity;

public partial class Room : Entity, IRoom
{
  private Room(){}
  public Room(int number, decimal price, int capacity, string description, Category category)
  {
    Number = number;
    Price = price;
    Capacity = capacity;
    Description = description;
    Category = category;
    CategoryId = category.Id;
    Status = ERoomStatus.OutOfService;
    Services = [];
    Images = [];

    Validate();
  }

  public int Number { get; private set; }
  public decimal Price { get; private set; }
  public ERoomStatus Status { get; private set; }
  public int Capacity { get; private set; }
  public string Description { get; private set; }
  public HashSet<Service> Services { get; private set; } 
  public Guid CategoryId { get; private set; }
  public Category? Category { get; private set; }
  public HashSet<Image> Images { get; private set; } 
}