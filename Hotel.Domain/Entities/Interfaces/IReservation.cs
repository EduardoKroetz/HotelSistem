
using Hotel.Domain.Entities.CustomerContext;
using Hotel.Domain.Entities.PaymentContext.InvoiceRoomEntity;
using Hotel.Domain.Entities.RoomContext.RoomEntity;
using Hotel.Domain.Entities.RoomContext.ServiceEntity;
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
    RoomInvoice? Invoice { get; }
    ICollection<Service> Services { get; }
}