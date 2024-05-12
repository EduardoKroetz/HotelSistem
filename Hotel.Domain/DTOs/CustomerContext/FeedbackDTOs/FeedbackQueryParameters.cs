using Hotel.Domain.DTOs.Base;

namespace Hotel.Domain.DTOs.CustomerContext.FeedbackDTOs;

public class FeedbackQueryParameters : QueryParameters
{
  public FeedbackQueryParameters(int? skip,int? take, DateTime? createdAt, string? createdAtOperator,string? comment, int? rate, int? likes, int? deslikes, DateTime? updatedAt, Guid? customerId, Guid? reservationId, Guid? roomId) : base(skip,take,createdAt,createdAtOperator)
  {
    Comment = comment;
    Rate = rate;
    Likes = likes;
    Deslikes = deslikes;
    UpdatedAt = updatedAt;
    CustomerId = customerId;
    ReservationId = reservationId;
    RoomId = roomId;
  }

  public string? Comment { get; private set; }
  public int? Rate { get; private set; }
  public int? Likes { get; private set; }
  public int? Deslikes { get; private set; }
  public DateTime? UpdatedAt { get; private set; }
  public Guid? CustomerId { get; private set; }
  public Guid? ReservationId { get; private set; }
  public Guid? RoomId { get; private set; }
}
