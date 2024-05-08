using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.CustomerContext.FeedbackDTOs;

namespace Hotel.Domain.Handlers.CustomerContext.FeedbackHandlers;

public partial class FeedbackHandler
{
  public async Task<Response<object>> HandleUpdateAsync(UpdateFeedback model, Guid id)
  {
    var feedback = await _repository.GetEntityByIdAsync(id);
    if (feedback == null)
      throw new ArgumentException("Feedback não encontrado.");

    feedback.ChangeComment(model.Comment);
    feedback.ChangeRate(model.Rate);

    _repository.Update(feedback);
    await _repository.SaveChangesAsync();

    return new Response<object>(200,"O Feedback foi atualizado.",new { feedback.Id });
  }
}