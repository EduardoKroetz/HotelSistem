using Hotel.Domain.DTOs.Base;

namespace Hotel.Domain.DTOs.FeedbackDTOs;

public class FeedbackQueryParameters : QueryParameters
{
    public string? Comment { get; set; }
    public int? Rate { get; set; }
    public string? RateOperator { get; set; }
    public int? Likes { get; set; }
    public string? LikesOperator { get; set; }
    public int? Dislikes { get; set; }
    public string? DislikesOperator { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedAtOperator { get; set; }
    public Guid? CustomerId { get; set; }
    public Guid? ReservationId { get; set; }
    public Guid? RoomId { get; set; }
}
