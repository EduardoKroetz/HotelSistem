using Hotel.Domain.DTOs.CustomerContext.FeedbackDTOs;
using Hotel.Domain.Entities.CustomerContext.FeedbackEntity;

namespace Hotel.Domain.Repositories.Interfaces;

public interface IFeedbackRepository : IRepository<Feedback>, IRepositoryQuery<GetFeedback,FeedbackQueryParameters>
{
}