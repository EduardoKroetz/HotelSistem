using Hotel.Domain.Entities.Base;

namespace Hotel.Domain.Entities;

public class Feedback : Entity
{
  public Feedback(string comment, int rate, int like, int deslikes, DateTime createdAt, DateTime updatedAt, Guid customerId, Customer? customer, Guid reservationId, Reservation? reservation, Guid roomId, Room? room)
  {
    Comment = comment;
    Rate = rate;
    Like = like;
    Deslikes = deslikes;
    CreatedAt = createdAt;
    UpdatedAt = updatedAt;
    CustomerId = customerId;
    Customer = customer;
    ReservationId = reservationId;
    Reservation = reservation;
    RoomId = roomId;
    Room = room;
  }

  public string Comment { get; private set; }
  public int Rate { get; private set; }
  public int Like { get; private set; }
  public int Deslikes { get; private set; }
  public DateTime CreatedAt { get; private set; }
  public DateTime UpdatedAt { get; private set; }
  public Guid CustomerId { get; private set; }
  public Customer? Customer { get; private set; }
  public Guid ReservationId { get; private set; }
  public Reservation? Reservation { get; private set; }
  public Guid RoomId { get; private set; }
  public Room? Room { get; private set; }
}