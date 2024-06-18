using Hotel.Domain.Entities.Base.Interfaces;
using Hotel.Domain.Entities.CustomerEntity;
using Hotel.Domain.Entities.ReservationEntity;
using Hotel.Domain.Entities.ServiceEntity;
using Hotel.Domain.Enums;

namespace Hotel.Domain.Entities.Interfaces;

public interface IInvoice : IEntity
{
    string Number { get; }
    DateTime IssueDate { get; }
    decimal TotalAmount { get; }
    EStatus Status { get; }
    EPaymentMethod PaymentMethod { get; }
    decimal TaxInformation { get; }
    Customer? Customer { get; }
    Guid ReservationId { get; }
    Reservation? Reservation { get; }
    ICollection<Service> Services { get; }
}