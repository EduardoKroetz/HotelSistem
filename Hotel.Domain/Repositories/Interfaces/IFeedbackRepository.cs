using Hotel.Domain.DTOs.FeedbackDTOs;
using Hotel.Domain.Entities.FeedbackEntity;

namespace Hotel.Domain.Repositories.Interfaces;

public interface IFeedbackRepository : IRepository<Feedback>, IRepositoryQuery<GetFeedback, FeedbackQueryParameters>
{
}