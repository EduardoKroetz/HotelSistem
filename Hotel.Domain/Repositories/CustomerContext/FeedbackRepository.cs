using Hotel.Domain.Data;
using Hotel.Domain.DTOs.CustomerContext.FeedbackDTOs;
using Hotel.Domain.Entities.CustomerContext.FeedbackEntity;
using Hotel.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Repositories;

public class FeedbackRepository : GenericRepository<Feedback> ,IFeedbackRepository
{
  public FeedbackRepository(HotelDbContext context) : base(context) {}

  public async Task<GetFeedback?> GetByIdAsync(Guid id)
  {
    return await _context
      .Feedbacks
      .AsNoTracking()
      .Where(x => x.Id == id)
      .Select(x => new GetFeedback(x.Id,x.Comment,x.Rate,x.Likes,x.Deslikes,x.CustomerId,x.ReservationId,x.RoomId))
      .FirstOrDefaultAsync();
    

  }
  public async Task<IEnumerable<GetFeedback>> GetAsync()
  {
    return await _context
      .Feedbacks
      .AsNoTracking()
      .Select(x => new GetFeedback(x.Id,x.Comment,x.Rate,x.Likes,x.Deslikes,x.CustomerId,x.ReservationId,x.RoomId))
      .ToListAsync();
  }
  
}