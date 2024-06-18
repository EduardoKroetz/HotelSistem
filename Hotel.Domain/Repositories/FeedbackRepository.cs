using Hotel.Domain.Data;
using Hotel.Domain.DTOs.FeedbackDTOs;
using Hotel.Domain.Entities.FeedbackEntity;
using Hotel.Domain.Extensions;
using Hotel.Domain.Repositories.Base;
using Hotel.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Repositories;

public class FeedbackRepository : GenericRepository<Feedback>, IFeedbackRepository
{
    public FeedbackRepository(HotelDbContext context) : base(context) { }

    public async Task<GetFeedback?> GetByIdAsync(Guid id)
    {
        return await _context
          .Feedbacks
          .AsNoTracking()
          .Where(x => x.Id == id)
          .Select(x => new GetFeedback(x.Id, x.Comment, x.Rate, x.Likes.Count, x.Dislikes.Count, x.CustomerId, x.ReservationId, x.RoomId, x.UpdatedAt, x.CreatedAt))
          .FirstOrDefaultAsync();


    }

    public async Task<IEnumerable<GetFeedback>> GetAsync(FeedbackQueryParameters queryParameters)
    {
        var query = _context.Feedbacks.AsQueryable();

        if (queryParameters.Comment != null)
            query = query.Where(x => x.Comment.Contains(queryParameters.Comment));

        if (queryParameters.Rate.HasValue)
            query = query.FilterByOperator(queryParameters.RateOperator, x => x.Rate, queryParameters.Rate);

        if (queryParameters.Likes.HasValue)
            query = query.FilterByOperator(queryParameters.LikesOperator, x => x.Likes.Count, queryParameters.Likes);

        if (queryParameters.Dislikes.HasValue)
            query = query.FilterByOperator(queryParameters.DislikesOperator, x => x.Dislikes.Count, queryParameters.Dislikes);

        if (queryParameters.UpdatedAt.HasValue)
            query = query.FilterByOperator(queryParameters.UpdatedAtOperator, x => x.UpdatedAt, queryParameters.UpdatedAt);

        if (queryParameters.CustomerId.HasValue)
            query = query.Where(x => x.CustomerId == queryParameters.CustomerId);

        if (queryParameters.ReservationId.HasValue)
            query = query.Where(x => x.ReservationId == queryParameters.ReservationId);

        if (queryParameters.RoomId.HasValue)
            query = query.Where(x => x.RoomId == queryParameters.RoomId);

        query = query.BaseQuery(queryParameters);

        return await query.Select(x => new GetFeedback
        (
          x.Id,
          x.Comment,
          x.Rate,
          x.Likes.Count,
          x.Dislikes.Count,
          x.CustomerId,
          x.ReservationId,
          x.RoomId,
          x.UpdatedAt,
          x.CreatedAt
        )).ToListAsync();
    }
}