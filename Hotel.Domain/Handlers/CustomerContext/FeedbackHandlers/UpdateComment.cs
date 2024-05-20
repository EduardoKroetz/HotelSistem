using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.CustomerContext.FeedbackDTOs;

namespace Hotel.Domain.Handlers.CustomerContext.FeedbackHandlers;

public partial class FeedbackHandler
{
  public async Task<Response<object>> HandleUpdateCommentAsync(Guid id, UpdateComment updateComment)
  {
    var feedback = await _repository.GetEntityByIdAsync(id);
    if (feedback == null)
      throw new ArgumentException("Feedback não encontrado.");

    feedback.ChangeComment(updateComment.Comment);

    await _repository.SaveChangesAsync();

    return new Response<object>(200, "Feedback atualizado.");
  }
}