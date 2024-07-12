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

        try
        {
            await _feedbackRepository.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Ocorreu um erro ao deletar um dislike: {ex.Message}");
        }

        return new Response("Deslike removido com sucesso!");
    }
}