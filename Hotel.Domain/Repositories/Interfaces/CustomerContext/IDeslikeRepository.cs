using Hotel.Domain.Entities.CustomerContext.FeedbackContext;

namespace Hotel.Domain.Repositories.Interfaces.CustomerContext;

public interface IDeslikeRepository
{
  Task<Deslike?> GetDeslikeAsync(Guid feedbackId, Guid customerId);
  void RemoveDeslike(Deslike deslike);
  Task CreateDeslike(Deslike deslike);
}
