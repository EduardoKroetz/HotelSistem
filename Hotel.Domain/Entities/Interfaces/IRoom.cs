using Hotel.Domain.Entities.RoomContext.CategoryEntity;
using Hotel.Domain.Entities.RoomContext.ImageEntity;
using Hotel.Domain.Entities.RoomContext.ServiceEntity;
using Hotel.Domain.Enums;

namespace Hotel.Domain.Entities.Interfaces;

public interface IRoom
{
    int Number { get; }
    decimal Price { get; }
    ERoomStatus Status { get; }
    int Capacity { get; }
    string Description { get; }
    HashSet<Service> Services { get; }
    Guid CategoryId { get; }
    Category? Category { get; }
    HashSet<Image> Images { get; }
}