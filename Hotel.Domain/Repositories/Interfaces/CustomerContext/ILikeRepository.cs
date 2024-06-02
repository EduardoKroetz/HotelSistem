using Hotel.Domain.Entities.CustomerContext.FeedbackContext;

namespace Hotel.Domain.Repositories.Interfaces.CustomerContext;

public interface ILikeRepository
{
  Task<Like?> GetLikeAsync(Guid feedbackId, Guid customerId);
  void RemoveLike(Like like);
  Task CreateLike(Like like);
}
