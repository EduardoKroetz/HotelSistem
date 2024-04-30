using Hotel.Domain.Entities.Base;

namespace Hotel.Domain.Entities;

public class Feedback : Entity
{
  public Feedback(string comment, int rate, int like, int deslikes, Guid customerId, Guid roomId ,  Guid reservationId, Customer? customer, Reservation? reservation, Room? room)
  {
    Comment = comment;
    Rate = rate;
    Like = like;
    Deslikes = deslikes;
    CustomerId = customerId;
    Customer = customer;
    ReservationId = reservationId;
    Reservation = reservation;
    RoomId = roomId;
    Room = room;
    UpdatedAt = DateTime.Now;
  }

  public string Comment { get; private set; }
  public int Rate { get; private set; }
  public int Like { get; private set; }
  public int Deslikes { get; private set; }
  public DateTime UpdatedAt { get; private set; }
  public Guid CustomerId { get; private set; }
  public Customer? Customer { get; private set; }
  public Guid ReservationId { get; private set; }
  public Reservation? Reservation { get; private set; }
  public Guid RoomId { get; private set; }
  public Room? Room { get; private set; }
}