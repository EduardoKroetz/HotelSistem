using Hotel.Domain.Entities.ImageEntity;
using Hotel.Domain.Entities.ServiceEntity;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.RoomDTOs;

public class GetRoom : IDataTransferObject
{
    public GetRoom(Guid id,string name , int number, decimal price, ERoomStatus status, int capacity, string description, Guid categoryId, ICollection<Image> images, DateTime createdAt)
    {
        Id = id;
        Name = name;
        Number = number;
        Price = price;
        Status = status;
        Capacity = capacity;
        Description = description;
        CategoryId = categoryId;
        Images = images;
        CreatedAt = createdAt;
    }

    public Guid Id { get; set; }
    public string Name { get; private set; }
    public int Number { get; private set; }
    public decimal Price { get; private set; }
    public ERoomStatus Status { get; private set; }
    public int Capacity { get; private set; }
    public string Description { get; private set; }
    public Guid CategoryId { get; private set; }
    public ICollection<Image> Images { get; private set; }
    public DateTime CreatedAt { get; private set; }
}

