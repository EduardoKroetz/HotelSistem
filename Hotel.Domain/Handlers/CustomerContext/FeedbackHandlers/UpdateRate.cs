using Hotel.Domain.DTOs;

namespace Hotel.Domain.Handlers.CustomerContext.FeedbackHandlers;

public partial class FeedbackHandler
{
  public async Task<Response> HandleUpdateRateAsync(Guid id, int rate, Guid customerId)
  {
    var feedback = await _feedbackRepository.GetEntityByIdAsync(id);
    if (feedback == null)
      throw new ArgumentException("Feedback não encontrado.");

    if (feedback.CustomerId != customerId)
      throw new ArgumentException("Apenas o autor do feedback pode editá-lo.");

    feedback.ChangeRate(rate);

    await _feedbackRepository.SaveChangesAsync();

    return new Response(200, "Feedback atualizado com sucesso!");
  }
}