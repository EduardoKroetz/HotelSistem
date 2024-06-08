using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.CustomerContext.FeedbackDTOs;

namespace Hotel.Domain.Handlers.CustomerContext.FeedbackHandlers;

public partial class FeedbackHandler
{
  public async Task<Response> HandleUpdateAsync(UpdateFeedback model, Guid id, Guid customerId)
  {
    var feedback = await _feedbackRepository.GetEntityByIdAsync(id);
    if (feedback == null)
      throw new ArgumentException("Feedback não encontrado.");

    if (feedback.CustomerId != customerId)
      throw new ArgumentException("Apenas o autor do feedback pode editá-lo.");

    feedback.ChangeComment(model.Comment);
    feedback.ChangeRate(model.Rate);

    _feedbackRepository.Update(feedback);
    await _feedbackRepository.SaveChangesAsync();

    return new Response(200,"Feedback atualizado com sucesso!",new { feedback.Id });
  }
}