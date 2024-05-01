using Hotel.Domain.Entities.Base;
using Hotel.Domain.Entities.ReservationContext;
using Hotel.Domain.Entities.RoomContext;

namespace Hotel.Domain.Entities.CustomerContext;

public class Feedback : Entity
{
  public Feedback(string comment, int rate, Guid customerId, Guid roomId ,  Guid reservationId, Customer? customer, Reservation? reservation, Room? room)
  {
    Comment = comment;
    Rate = rate;
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
  public int Likes { get; private set; }
  public int Deslikes { get; private set; }
  public DateTime UpdatedAt { get; private set; }
  public Guid CustomerId { get; private set; }
  public Customer? Customer { get; private set; }
  public Guid ReservationId { get; private set; }
  public Reservation? Reservation { get; private set; }
  public Guid RoomId { get; private set; }
  public Room? Room { get; private set; }

  public void AddLike()
  => Likes++; 
  public void RemoveLike()
  => Likes--; 
  
  public void AddDeslike()
  => Deslikes++; 
  public void RemoveDeslike()
  => Deslikes--; 
}