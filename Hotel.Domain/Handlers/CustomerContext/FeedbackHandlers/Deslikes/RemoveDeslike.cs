using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.CustomerContext.FeedbackHandlers;

public partial class FeedbackHandler
{
  public async Task<Response> HandleRemoveDeslikeAsync(Guid feedbackId, Guid customerId)
  {
    var deslike = await _deslikeRepository.GetDeslikeAsync(feedbackId, customerId);
    if (deslike == null)
      throw new NotFoundException("Deslike não encontrado.");

    _deslikeRepository.RemoveDeslike(deslike);

    await _feedbackRepository.SaveChangesAsync();

    return new Response(200, "Deslike removido com sucesso!");
  }
}