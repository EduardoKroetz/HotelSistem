
using Hotel.Domain.Entities.CustomerEntity;
using Hotel.Domain.Entities.InvoiceEntity;
using Hotel.Domain.Entities.RoomEntity;
using Hotel.Domain.Entities.ServiceEntity;
using Hotel.Domain.Enums;

namespace Hotel.Domain.Entities.Interfaces;

public interface IReservation
{
    EReservationStatus Status { get; }
    TimeSpan? TimeHosted { get; }
    int Capacity { get; }
    Guid RoomId { get; }
    Room? Room { get; }
    Customer? Customer { get; }
    Invoice? Invoice { get; }
    ICollection<Service> Services { get; }
}