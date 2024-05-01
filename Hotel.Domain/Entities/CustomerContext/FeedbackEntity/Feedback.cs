using Hotel.Domain.Entities.Base;
using Hotel.Domain.Entities.ReservationContext.ReservationEntity;
using Hotel.Domain.Entities.RoomContext.RoomEntity;
using Hotel.Domain.Entities.Validation;

namespace Hotel.Domain.Entities.CustomerContext.FeedbackEntity;

public partial class Feedback : Entity, IValidation
{
  public Feedback(string comment, int rate, Guid customerId, Guid roomId ,  Guid reservationId, Customer customer, Reservation reservation, Room room)
  {
    Comment = comment;
    Rate = rate;
    CustomerId = customerId;
    ReservationId = reservationId;
    RoomId = roomId;
    Room = room;
    Customer = customer;
    Reservation = reservation;
    UpdatedAt = DateTime.Now;
    Likes = 0;
    Deslikes = 0;

    Validate();
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
}