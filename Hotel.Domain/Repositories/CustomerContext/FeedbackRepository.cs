using Hotel.Domain.Data;
using Hotel.Domain.DTOs.User;
using Hotel.Domain.Entities.CustomerContext.FeedbackEntity;
using Hotel.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Repositories;

public class FeedbackRepository : GenericRepository<Feedback> ,IFeedbackRepository
{
  public FeedbackRepository(HotelDbContext context) : base(context) {}


}