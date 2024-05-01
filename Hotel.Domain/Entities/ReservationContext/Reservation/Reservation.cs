using Hotel.Domain.Entities.Base;
using Hotel.Domain.Entities.CustomerContext;
using Hotel.Domain.Entities.RoomContext.RoomEntity;
using Hotel.Domain.Enums;

namespace Hotel.Domain.Entities.ReservationContext.ReservationEntity;

public partial class Reservation : Entity
{
  public Reservation(int capacity, Room room, DateTime checkIn ,DateTime? checkOut)
  {
    CheckIn = checkIn;
    CheckOut = checkOut;
    Status = EReservationStatus.Pending;
    Capacity = capacity;
    Room = room;
    RoomId = room.Id;
    Customers = [];
    HostedDays = CalculeHostedDays();
    DailyRate = CalculeDailyRate();

    Room.ChangeStatus(ERoomStatus.Reserved);
  }

  public int? HostedDays { get; private set; }
  public decimal DailyRate { get; private set; }
  public DateTime CheckIn { get; private set; }
  public DateTime? CheckOut { get; private set; }
  public EReservationStatus Status { get; private set; }
  public int Capacity { get; private set; }
  public Guid RoomId { get; private set; }
  public Room Room { get; private set; }
  public List<Customer> Customers { get; private set; }


}