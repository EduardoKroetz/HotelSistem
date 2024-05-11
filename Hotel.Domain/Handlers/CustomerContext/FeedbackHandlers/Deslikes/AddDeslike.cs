using Hotel.Domain.DTOs;

namespace Hotel.Domain.Handlers.CustomerContext.FeedbackHandlers;

public partial class FeedbackHandler
{
  public async Task<Response<object>> HandleAddDeslikeAsync(Guid id)
  {
    var feedback = await _repository.GetEntityByIdAsync(id);

    if (feedback == null)
      throw new ArgumentException("Feedback não encontrado.");

    feedback.AddDeslike();

    _repository.Update(feedback);
    await _repository.SaveChangesAsync();

    return new Response<object>(200,"Deslike adicionado.", new { id });
  }
}