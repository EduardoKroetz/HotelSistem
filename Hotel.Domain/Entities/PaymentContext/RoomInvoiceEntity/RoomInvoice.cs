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
    Number = ""; //Serviço para gerar número
    IssueDate = DateTime.Now;
    Status = EStatus.Pending;
    PaymentMethod = paymentMethod;
    TaxInformation = taxInformation;
    Customers = reservation.Customers;
    Reservation = reservation;
    Services = reservation.Services;

    Validate();

    //Envio de email
  }

  public string Number { get; private set; } = string.Empty;
  public DateTime IssueDate { get; private set; }
  public decimal TotalAmount { get; private set; }
  public EStatus Status { get; private set; }
  public EPaymentMethod PaymentMethod { get; private set; }
  public decimal TaxInformation { get; private set; }
  public HashSet<Customer> Customers { get; private set; } = [];
  public Guid ReservationId { get; private set; }
  public Reservation? Reservation { get; private set; }
  public List<Service> Services { get; private set; } = [];
}