using Hotel.Domain.DTOs.Interfaces;
using Hotel.Domain.DTOs.RoomContext.CategoryDTOs;
using Hotel.Domain.Entities.RoomContext.ImageEntity;
using Hotel.Domain.Entities.RoomContext.ServiceEntity;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.RoomContext.RoomDTOs;

public class GetRoom : IDataTransferObject
{
  public GetRoom(Guid id, int number, decimal price, ERoomStatus status, int capacity, string description, ICollection<Service> services, GetCategory category, ICollection<Image> images, DateTime createdAt)
  {
    Id = id;
    Number = number;
    Price = price;
    Status = status;
    Capacity = capacity;
    Description = description;
    Services = services;
    Category = category;
    Images = images;
    CreatedAt = createdAt;  
  }

  public Guid Id { get; set; }
  public int Number { get; private set; }
  public decimal Price { get; private set; }
  public ERoomStatus Status { get; private set; }
  public int Capacity { get; private set; }
  public string Description { get; private set; }
  public ICollection<Service> Services { get; private set; }
  public GetCategory Category { get; private set; }
  public ICollection<Image> Images { get; private set; }
  public DateTime CreatedAt { get; private set; }
}

