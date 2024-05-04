
using Hotel.Domain.Entities.CustomerContext;
using Hotel.Domain.Entities.PaymentContext.InvoiceRoomEntity;
using Hotel.Domain.Entities.RoomContext.RoomEntity;
using Hotel.Domain.Entities.RoomContext.ServiceEntity;
using Hotel.Domain.Enums;

namespace Hotel.Domain.Entities.Interfaces;

public interface IReservation
{
    EReservationStatus Status { get; }
    int Capacity { get; }
    Guid RoomId { get; }
    Room? Room { get; }
    HashSet<Customer> Customers { get; }
    InvoiceRoom? Invoice { get; }
    List<Service> Services { get; }
}