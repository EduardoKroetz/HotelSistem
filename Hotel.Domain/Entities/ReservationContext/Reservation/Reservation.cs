using Hotel.Domain.Entities.Base;
using Hotel.Domain.Entities.CustomerContext;
using Hotel.Domain.Entities.Interfaces;
using Hotel.Domain.Entities.PaymentContext.InvoiceRoomEntity;
using Hotel.Domain.Entities.RoomContext.RoomEntity;
using Hotel.Domain.Entities.RoomContext.ServiceEntity;
using Hotel.Domain.Enums;


namespace Hotel.Domain.Entities.ReservationContext.ReservationEntity;

public partial class Reservation : Entity, IReservation
{
  public Reservation(Room room, DateTime checkIn, HashSet<Customer> customers ,DateTime? checkOut = null)
  {
    CheckIn = checkIn;
    _checkOut = checkOut;
    Status = EReservationStatus.Pending;
    Capacity = customers.Count;
    Room = room;
    RoomId = room.Id;
    Customers = customers;

    var hostedDays = CalculeHostedDays();
    HostedDays = hostedDays == 0 ? null : hostedDays;
    
    DailyRate = room.Price;
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
  public Room? Room { get; private set; }
  public HashSet<Customer> Customers { get; private set; } = [];
  public Guid? InvoiceId { get; private set; }
  public RoomInvoice? Invoice { get; private set; }
  public List<Service> Services { get; private set; } = [];
}