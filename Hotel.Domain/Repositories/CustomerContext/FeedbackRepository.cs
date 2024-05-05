using Hotel.Domain.Data;
using Hotel.Domain.Entities.CustomerContext.FeedbackEntity;
using Hotel.Domain.Repositories.Interfaces;

namespace Hotel.Domain.Repositories;

public class FeedbackRepository : GenericRepository<Feedback> ,IFeedbackRepository
{
  public FeedbackRepository(HotelDbContext context) : base(context) {}
}