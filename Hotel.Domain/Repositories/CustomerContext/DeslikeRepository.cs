using Hotel.Domain.Data;
using Hotel.Domain.Entities.CustomerContext.FeedbackContext;
using Hotel.Domain.Repositories.Interfaces.CustomerContext;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Repositories.CustomerContext;

public class DeslikeRepository : IDeslikeRepository
{
  private readonly HotelDbContext _context;
  public DeslikeRepository(HotelDbContext context)
  => _context = context;

  public void RemoveDeslike(Deslike dislike)
  => _context.Dislikes.Remove(dislike);

  public async Task CreateDeslike(Deslike dislike)
  => await _context.Dislikes.AddAsync(dislike);

  public async Task<Deslike?> GetDeslikeAsync(Guid feedbackId, Guid customerId)
  {
    return await _context.Dislikes
      .FirstOrDefaultAsync(x => x.FeedbackId == feedbackId && x.CustomerId == customerId);
  }
}
