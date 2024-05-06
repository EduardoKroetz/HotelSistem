namespace Hotel.Domain.DTOs.CustomerContext.FeedbackDTOs;

public class CreateFeedback
{
  public CreateFeedback(string comment, int rate, Guid customerId, Guid reservationId, Guid roomId)
  {
    Comment = comment;
    Rate = rate;
    RoomId = roomId;
    CustomerId = customerId;
    ReservationId = reservationId;
  }

  public string Comment { get; set; }
  public int Rate { get; set; }
  public Guid CustomerId { get; set; }
  public Guid ReservationId { get; set; }
  public Guid RoomId { get; set; }
}

