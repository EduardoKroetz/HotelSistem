﻿using Hotel.Domain.DTOs.Base;

namespace Hotel.Domain.DTOs.FeedbackDTOs;

public class FeedbackQueryParameters : QueryParameters
{
    public FeedbackQueryParameters(int? skip, int? take, DateTime? createdAt, string? createdAtOperator, string? comment, int? rate, string? rateOperator, int? likes, string? likesOperator, int? dislikes, string? dislikesOperator, DateTime? updatedAt, string? updatedAtOperator, Guid? customerId, Guid? reservationId, Guid? roomId) : base(skip, take, createdAt, createdAtOperator)
    {
        Comment = comment;
        Rate = rate;
        RateOperator = rateOperator;
        Likes = likes;
        LikesOperator = likesOperator;
        Dislikes = dislikes;
        DislikesOperator = dislikesOperator;
        UpdatedAt = updatedAt;
        UpdatedAtOperator = updatedAtOperator;
        CustomerId = customerId;
        ReservationId = reservationId;
        RoomId = roomId;
    }

    public string? Comment { get; private set; }
    public int? Rate { get; private set; }
    public string? RateOperator { get; private set; }
    public int? Likes { get; private set; }
    public string? LikesOperator { get; private set; }
    public int? Dislikes { get; private set; }
    public string? DislikesOperator { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public string? UpdatedAtOperator { get; private set; }
    public Guid? CustomerId { get; private set; }
    public Guid? ReservationId { get; private set; }
    public Guid? RoomId { get; private set; }
}
