using Hotel.Domain.Entities.Base.Interfaces;
using Hotel.Domain.Entities.CustomerContext;
using Hotel.Domain.Entities.ReservationContext.ReservationEntity;
using Hotel.Domain.Entities.RoomContext.RoomEntity;

namespace Hotel.Domain.Entities.Interfaces;

public interface IFeedback : IEntity
{
  string Comment { get; }
  int Rate { get; }
  int Likes { get; }
  int Deslikes { get; }
  DateTime UpdatedAt { get; }
  Guid CustomerId { get; }
  Customer? Customer { get; }
  Guid ReservationId { get; }
  Reservation? Reservation { get; }
  Guid RoomId { get; }
  Room? Room { get; }
}
