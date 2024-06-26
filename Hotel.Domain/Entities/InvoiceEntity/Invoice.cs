using Hotel.Domain.Entities.Base;
using Hotel.Domain.Entities.CustomerEntity;
using Hotel.Domain.Entities.Interfaces;
using Hotel.Domain.Entities.ReservationEntity;
using Hotel.Domain.Entities.ServiceEntity;

namespace Hotel.Domain.Entities.InvoiceEntity;

public partial class Invoice : Entity, IInvoice
{
    internal Invoice() { }

    public Invoice(Reservation reservation)
    {
        TotalAmount = reservation.TotalAmount();
        PaymentMethod = "card";
        CustomerId = reservation.CustomerId;
        Customer = reservation.Customer;
        ReservationId = reservation.Id;
        Services = reservation.Services;

        Validate();

    }

    public decimal TotalAmount { get; private set; }
    public string PaymentMethod { get; private set; } = null!;
    public Guid CustomerId { get; private set; }
    public Customer? Customer { get; private set; }
    public Guid ReservationId { get; private set; }
    public Reservation? Reservation { get; private set; }
    public ICollection<Service> Services { get; private set; } = [];
}