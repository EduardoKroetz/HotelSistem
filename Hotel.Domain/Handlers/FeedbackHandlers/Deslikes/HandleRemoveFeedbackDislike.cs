using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.FeedbackHandlers;

public partial class FeedbackHandler
{
    public async Task<Response> HandleRemoveDeslikeAsync(Guid feedbackId, Guid customerId)
    {
        var dislike = await _dislikeRepository.GetDeslikeAsync(feedbackId, customerId);
        if (dislike == null)
            throw new NotFoundException("Deslike não encontrado.");

        _dislikeRepository.RemoveDeslike(dislike);

        await _feedbackRepository.SaveChangesAsync();

        return new Response(200, "Deslike removido com sucesso!");
    }
}