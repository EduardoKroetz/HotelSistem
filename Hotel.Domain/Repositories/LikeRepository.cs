using Hotel.Domain.Data;
using Hotel.Domain.Entities.LikeEntity;
using Hotel.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Repositories;

public class LikeRepository : ILikeRepository
{
    private readonly HotelDbContext _context;
    public LikeRepository(HotelDbContext context)
    => _context = context;

    public void RemoveLike(Like like)
    => _context.Likes.Remove(like);

    public async Task CreateLike(Like like)
    => await _context.Likes.AddAsync(like);

    public async Task<Like?> GetLikeAsync(Guid feedbackId, Guid customerId)
    {
        return await _context.Likes
          .FirstOrDefaultAsync(x => x.FeedbackId == feedbackId && x.CustomerId == customerId);
    }
}
