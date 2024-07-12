using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.FeedbackHandlers;

public partial class FeedbackHandler
{
    public async Task<Response> HandleRemoveLikeAsync(Guid feedbackId, Guid customerId)
    {
        var like = await _likeRepository.GetLikeAsync(feedbackId, customerId) ?? throw new NotFoundException("Não foi possível encontrar o registro no banco de dados.");

        _likeRepository.RemoveLike(like);

        try
        {
            await _feedbackRepository.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Ocorreu um erro ao deletar um like no banco de dados: {ex.Message}");
        }

        return new Response("Like removido com sucesso!");
    }
}