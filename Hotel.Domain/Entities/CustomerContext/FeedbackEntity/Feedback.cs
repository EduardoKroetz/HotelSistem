using Hotel.Domain.Entities.Base;
using Hotel.Domain.Entities.Interfaces;
using Hotel.Domain.Entities.ReservationContext.ReservationEntity;
using Hotel.Domain.Entities.RoomContext.RoomEntity;

namespace Hotel.Domain.Entities.CustomerContext.FeedbackEntity;

public partial class Feedback : Entity, IFeedback
{
  internal Feedback(){}
  
  public Feedback(string comment, int rate, Customer customer, Reservation reservation, Room room)
  {
    Comment = comment;
    Rate = rate;
    Room = room;
    RoomId = room.Id;
    Customer = customer;
    CustomerId = customer.Id;
    Reservation = reservation;
    ReservationId = reservation.Id;
    UpdatedAt = DateTime.Now;
    Likes = 0;
    Deslikes = 0;

    Validate();
  }

  public string Comment { get; private set; } = string.Empty;
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
}