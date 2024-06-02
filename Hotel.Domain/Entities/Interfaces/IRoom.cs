using Hotel.Domain.Entities.ReservationContext.ReservationEntity;
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
  ICollection<Service> Services { get; }
  Guid CategoryId { get; }
  Category? Category { get; }
  ICollection<Image> Images { get; }
  ICollection<Reservation> Reservations { get; }
}