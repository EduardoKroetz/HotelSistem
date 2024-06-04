using Hotel.Domain.Entities.Base.Interfaces;
using Hotel.Domain.Entities.CustomerContext;
using Hotel.Domain.Entities.ReservationContext.ReservationEntity;
using Hotel.Domain.Entities.RoomContext.ServiceEntity;
using Hotel.Domain.Enums;

namespace Hotel.Domain.Entities.Interfaces;

public interface IRoomInvoice : IEntity
{
    string Number { get; }
    DateTime IssueDate { get; }
    decimal TotalAmount { get; }
    EStatus Status { get; }
    EPaymentMethod PaymentMethod { get; }
    decimal TaxInformation { get; }
    Customer Customer { get; }
    Guid ReservationId { get; }
    Reservation? Reservation { get; }
    ICollection<Service> Services { get; }
}