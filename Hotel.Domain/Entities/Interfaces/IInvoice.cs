using Hotel.Domain.Entities.Base.Interfaces;
using Hotel.Domain.Entities.CustomerEntity;
using Hotel.Domain.Entities.ReservationEntity;
using Hotel.Domain.Entities.ServiceEntity;

namespace Hotel.Domain.Entities.Interfaces;

public interface IInvoice : IEntity
{
    decimal TotalAmount { get; }
    string PaymentMethod { get; }
    Customer? Customer { get; }
    Guid ReservationId { get; }
    Reservation? Reservation { get; }
    ICollection<Service> Services { get; }
}