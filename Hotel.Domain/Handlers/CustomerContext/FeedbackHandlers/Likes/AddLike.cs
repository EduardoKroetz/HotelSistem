using Hotel.Domain.DTOs;

namespace Hotel.Domain.Handlers.CustomerContext.FeedbackHandlers;

public partial class FeedbackHandler
{
  public async Task<Response> HandleAddLikeAsync(Guid id)
  {
    var feedback = await _repository.GetEntityByIdAsync(id);

    if (feedback == null)
      throw new ArgumentException("Feedback não encontrado.");

    feedback.AddLike();

    _repository.Update(feedback);
    await _repository.SaveChangesAsync();

    return new Response(200,"Curtida adicionada.", new { id });
  }
}