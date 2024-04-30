using Hotel.Domain.Entities.Base;
using Hotel.Domain.Enums;

namespace Hotel.Domain.Entities;

public class Reservation : Entity
{
  public Reservation(int hostedDays, decimal dailyRate, DateTime checkIn, DateTime? checkOut, EReservationStatus status, int capacity, Guid roomId, Room? room)
  {
    HostedDays = hostedDays;
    DailyRate = dailyRate;
    CheckIn = checkIn;
    CheckOut = checkOut;
    Status = status;
    Capacity = capacity;
    RoomId = roomId;
    Room = room;
    CreatedAt = DateTime.Now;
    Customers = [];
  }

  public int HostedDays { get; private set; }
  public decimal DailyRate { get; private set; }
  public DateTime CheckIn { get; private set; }
  public DateTime? CheckOut { get; private set; }
  public EReservationStatus Status { get; private set; }
  public int Capacity { get; private set; }
  public DateTime CreatedAt { get; private set; }
  public Guid RoomId { get; private set; }
  public Room? Room { get; private set; }
  public List<Customer> Customers { get; private set; }
}