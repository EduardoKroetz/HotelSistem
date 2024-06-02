using Hotel.Domain.DTOs;

namespace Hotel.Domain.Handlers.CustomerContext.FeedbackHandlers;

public partial class FeedbackHandler
{
  public async Task<Response> HandleDeleteAsync(Guid id, Guid customerId)
  {
    var feedback = await _repository.GetEntityByIdAsync(id);
    if (feedback == null)
      throw new ArgumentException("Feedback não encontrado.");

    if (feedback.CustomerId != customerId)
      throw new ArgumentException("Apenas o autor do feedback pode excluí-lo.");

    _repository.Delete(feedback);
    await _repository.SaveChangesAsync();
    return new Response(200,"Feedback deletado com sucesso!.", new { id });
  }
}