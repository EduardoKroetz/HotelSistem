using Hotel.Domain.Entities.Base.Interfaces;
using Hotel.Domain.Entities.CustomerEntity;
using Hotel.Domain.Entities.ReservationEntity;
using Hotel.Domain.Entities.RoomEntity;

namespace Hotel.Domain.Entities.Interfaces;

public interface IFeedback : IEntity
{
    string Comment { get; }
    int Rate { get; }
    DateTime UpdatedAt { get; }
    Guid CustomerId { get; }
    Customer? Customer { get; }
    Guid ReservationId { get; }
    Reservation? Reservation { get; }
    Guid RoomId { get; }
    Room? Room { get; }
}
