namespace Hotel.Domain.DTOs.CustomerContext.FeedbackDTOs;

public class GetFeedback
{
  public GetFeedback(Guid id,string comment, int rate,int likes , int deslikes ,Guid customerId, Guid reservationId, Guid roomId)
  {
    Id = id;
    Comment = comment;
    Rate = rate;
    RoomId = roomId;
    Likes = likes;
    Deslikes = deslikes;
    CustomerId = customerId;
    ReservationId = reservationId;
  }
  public Guid Id { get; set; }
  public string Comment { get; set; }
  public int Rate { get; set; }
  public int Likes { get; set; }
  public int Deslikes { get; set; }
  public Guid CustomerId { get; set; }
  public Guid ReservationId { get; set; }
  public Guid RoomId { get; set; }
}

