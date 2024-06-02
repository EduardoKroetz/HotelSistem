using Hotel.Domain.DTOs;

namespace Hotel.Domain.Handlers.CustomerContext.FeedbackHandlers;

public partial class FeedbackHandler
{
  public async Task<Response> HandleRemoveDeslikeAsync(Guid id)
  {
    var feedback = await _repository.GetEntityByIdAsync(id);

    if (feedback == null)
      throw new ArgumentException("Feedback não encontrado.");

    feedback.RemoveDeslike();
    
    _repository.Update(feedback);
    await _repository.SaveChangesAsync();

    return new Response(200,"Deslike removido.", new { id });
  }
}