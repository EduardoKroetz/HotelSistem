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
  internal Reservation(){}

  public Reservation(Room room, DateTime expectedCheckIn, DateTime expectedCheckOut, Customer customer, int capacity)
  {
    Status = EReservationStatus.Pending;
    Capacity = capacity;
    Room = room;
    RoomId = room.Id;
    Customer = customer;
    CustomerId = customer.Id;

    ExpectedCheckIn = expectedCheckIn;
    ExpectedCheckOut = expectedCheckOut;
    ExpectedTimeHosted = GetTimeHosted(ExpectedCheckIn, ExpectedCheckOut);
    
    DailyRate = room.Price;
    Invoice = null;

    Validate();
    Room.ChangeStatus(ERoomStatus.Reserved);
  }

  public TimeSpan? TimeHosted { get; private set; }
  public decimal DailyRate { get; private set; }
  public TimeSpan ExpectedTimeHosted { get; private set; }
  public DateTime ExpectedCheckIn { get; private set; }
  public DateTime ExpectedCheckOut { get; private set; }
  public DateTime? CheckIn { get; private set; }
  private DateTime? _checkOut { get; set; }
  public DateTime? CheckOut 
  { get 
    {
      return _checkOut;
    }
    private set
    {
      _checkOut = value;
      TimeHosted = GetTimeHosted(CheckIn, _checkOut);
    }
  }
  public EReservationStatus Status { get; private set; }
  public int Capacity { get; private set; }
  public Guid RoomId { get; private set; }
  public Room? Room { get; private set; }
  public Guid CustomerId { get; private set; }
  public Customer? Customer { get; private set; }
  public Guid? InvoiceId { get; private set; }
  public RoomInvoice? Invoice { get; private set; }
  public ICollection<Service> Services { get; private set; } = [];
}