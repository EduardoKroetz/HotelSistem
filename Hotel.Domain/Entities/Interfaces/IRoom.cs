using Hotel.Domain.Entities.Base.Interfaces;
using Hotel.Domain.Entities.CategoryEntity;
using Hotel.Domain.Entities.ImageEntity;
using Hotel.Domain.Entities.ReservationEntity;
using Hotel.Domain.Entities.ServiceEntity;
using Hotel.Domain.Enums;

namespace Hotel.Domain.Entities.Interfaces;

public interface IRoom : IEntity
{
    string Name { get; }    
    int Number { get; }
    decimal Price { get; }
    ERoomStatus Status { get; }
    int Capacity { get; }
    string Description { get; }
    bool IsActive { get; }  
    string StripeProductId { get; }
    ICollection<Service> Services { get; }
    Guid CategoryId { get; }
    Category? Category { get; }
    ICollection<Image> Images { get; }
    ICollection<Reservation> Reservations { get; }
}