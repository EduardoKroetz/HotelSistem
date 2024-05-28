using Hotel.Domain.DTOs;
using Hotel.Domain.Entities.CustomerContext;

namespace Hotel.Domain.Handlers.CustomerContext.FeedbackHandlers;

public partial class FeedbackHandler
{
  public async Task<Response<object>> HandleUpdateRateAsync(Guid id, int rate, Guid customerId)
  {
    var feedback = await _repository.GetEntityByIdAsync(id);
    if (feedback == null)
      throw new ArgumentException("Feedback não encontrado.");

    if (feedback.CustomerId != customerId)
      throw new ArgumentException("Apenas o autor do feedback pode editá-lo.");

    feedback.ChangeRate(rate);

    await _repository.SaveChangesAsync();

    return new Response<object>(200, "Feedback atualizado.");
  }
}