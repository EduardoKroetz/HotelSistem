using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.CustomerContext.FeedbackHandlers;

public partial class FeedbackHandler
{
  public async Task<Response> HandleRemoveLikeAsync(Guid feedbackId, Guid customerId)
  {
    var like = await _likeRepository.GetLikeAsync(feedbackId, customerId) ?? throw new NotFoundException("Não foi possível encontrar o registro no banco de dados.");

    _likeRepository.RemoveLike(like);

    await _feedbackRepository.SaveChangesAsync();

    return new Response(200,"Like removido com sucesso!");
  }
}