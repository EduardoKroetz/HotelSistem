using Hotel.Domain.Data;
using Hotel.Domain.Entities.DislikeEntity;
using Hotel.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Repositories;

public class DeslikeRepository : IDeslikeRepository
{
    private readonly HotelDbContext _context;
    public DeslikeRepository(HotelDbContext context)
    => _context = context;

    public void RemoveDeslike(Dislike dislike)
    => _context.Dislikes.Remove(dislike);

    public async Task CreateDeslike(Dislike dislike)
    => await _context.Dislikes.AddAsync(dislike);

    public async Task<Dislike?> GetDeslikeAsync(Guid feedbackId, Guid customerId)
    {
        return await _context.Dislikes
          .FirstOrDefaultAsync(x => x.FeedbackId == feedbackId && x.CustomerId == customerId);
    }
}
