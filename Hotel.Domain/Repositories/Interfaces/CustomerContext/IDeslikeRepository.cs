using Hotel.Domain.Entities.CustomerContext.FeedbackContext;

namespace Hotel.Domain.Repositories.Interfaces.CustomerContext;

public interface IDeslikeRepository
{
  Task<Deslike?> GetDeslikeAsync(Guid feedbackId, Guid customerId);
  void RemoveDeslike(Deslike dislike);
  Task CreateDeslike(Deslike dislike);
}
