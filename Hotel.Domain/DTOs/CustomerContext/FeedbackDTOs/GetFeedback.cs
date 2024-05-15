using Hotel.Domain.DTOs.Base;

namespace Hotel.Domain.DTOs.CustomerContext.FeedbackDTOs;

public class GetFeedback : GetDataTransferObject
{
  public GetFeedback(Guid id,string comment, int rate,int likes , int deslikes ,Guid customerId, Guid reservationId, Guid roomId,DateTime updatedAt, DateTime createdAt) : base(id,createdAt)
  {
    Comment = comment;
    Rate = rate;
    RoomId = roomId;
    Likes = likes;
    Deslikes = deslikes;
    CustomerId = customerId;
    ReservationId = reservationId;
    UpdatedAt = updatedAt;
  }
  public string Comment { get; private set; }
  public int Rate { get; private set; }
  public int Likes { get; private set; }
  public int Deslikes { get; private set; }
  public Guid CustomerId { get; private set; }
  public Guid ReservationId { get; private set; }
  public Guid RoomId { get; private set; }
  public DateTime? UpdatedAt { get; private set; }
}

