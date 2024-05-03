using Hotel.Domain.Entities.Base;
using Hotel.Domain.Entities.CustomerContext;
using Hotel.Domain.Entities.PaymentContext.InvoiceEntity;
using Hotel.Domain.Entities.RoomContext.RoomEntity;
using Hotel.Domain.Enums;

namespace Hotel.Domain.Entities.ReservationContext.ReservationEntity;

public partial class Reservation : Entity
{
  public Reservation(int capacity, Room room, DateTime checkIn, List<Customer> customers ,DateTime? checkOut = null)
  {
    CheckIn = checkIn;
    _checkOut = checkOut;
    Status = EReservationStatus.Pending;
    Capacity = capacity;
    Room = room;
    RoomId = room.Id;
    Customers = customers;
    HostedDays = CalculeHostedDays();
    DailyRate = CalculeDailyRate();
    Invoice = null;

    Validate();
    Room.ChangeStatus(ERoomStatus.Reserved);
  }

  public int? HostedDays { get; private set; }
  public decimal DailyRate { get; private set; }
  public DateTime CheckIn { get; private set; }
  private DateTime? _checkOut { get; set; }
  public DateTime? CheckOut 
  { get 
    {
      return _checkOut;
    }
    private set
    {
      HostedDays = CalculeHostedDays();
      _checkOut = value;
    }
  }
  public EReservationStatus Status { get; private set; }
  public int Capacity { get; private set; }
  public Guid RoomId { get; private set; }
  public Room Room { get; private set; }
  public List<Customer> Customers { get; private set; } = [];
  public Invoice? Invoice { get; private set; }

  public Invoice GenerateInvoiceAndCheckOut(EPaymentMethod paymentMethod,decimal taxInformation = 0)
  {
    ChangeCheckOut(DateTime.Now);
    CheckOutStatus();
    Invoice = new Invoice(paymentMethod,this,Customers,taxInformation);
    return Invoice;
  }

}