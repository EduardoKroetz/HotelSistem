using Hotel.Domain.DTOs.Interfaces;

namespace Hotel.Domain.DTOs.CustomerContext.FeedbackDTOs;

public class GetFeedback : IDataTransferObject
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
  public Guid Id { get; private set; }
  public string Comment { get; private set; }
  public int Rate { get; private set; }
  public int Likes { get; private set; }
  public int Deslikes { get; private set; }
  public Guid CustomerId { get; private set; }
  public Guid ReservationId { get; private set; }
  public Guid RoomId { get; private set; }
}

