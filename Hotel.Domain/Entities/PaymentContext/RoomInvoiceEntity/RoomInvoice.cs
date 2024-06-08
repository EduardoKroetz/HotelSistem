using Hotel.Domain.Entities.Base;
using Hotel.Domain.Entities.CustomerContext;
using Hotel.Domain.Entities.Interfaces;
using Hotel.Domain.Entities.ReservationContext.ReservationEntity;
using Hotel.Domain.Entities.RoomContext.ServiceEntity;
using Hotel.Domain.Enums;

namespace Hotel.Domain.Entities.PaymentContext.InvoiceRoomEntity;

public partial class RoomInvoice : Entity, IRoomInvoice
{
  internal RoomInvoice(){}

  public RoomInvoice(EPaymentMethod paymentMethod,Reservation reservation,decimal taxInformation = 0)
  {
    TotalAmount = reservation.TotalAmount();
    Number = Guid.NewGuid().ToString(); //Serviço para gerar número
    IssueDate = DateTime.Now;
    Status = EStatus.Pending;
    PaymentMethod = paymentMethod;
    TaxInformation = taxInformation;
    CustomerId = reservation.CustomerId;
    Customer = reservation.Customer;
    Reservation = reservation;
    ReservationId = reservation.Id;
    Services = reservation.Services;

    Validate();

  }

  public string Number { get; private set; } = string.Empty;
  public DateTime IssueDate { get; private set; }
  public decimal TotalAmount { get; private set; }
  public EStatus Status { get; private set; }
  public EPaymentMethod PaymentMethod { get; private set; }
  public decimal TaxInformation { get; private set; }
  public Guid CustomerId { get; private set; }
  public Customer? Customer { get; private set; }
  public Guid ReservationId { get; private set; }
  public Reservation? Reservation { get; private set; }
  public ICollection<Service> Services { get; private set; } = [];
}