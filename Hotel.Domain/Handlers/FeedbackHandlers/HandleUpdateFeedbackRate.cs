using Hotel.Domain.DTOs;

namespace Hotel.Domain.Handlers.FeedbackHandlers;

public partial class FeedbackHandler
{
    public async Task<Response> HandleUpdateRateAsync(Guid id, int rate, Guid customerId)
    {
        var feedback = await _feedbackRepository.GetEntityByIdAsync(id);
        if (feedback == null)
            throw new ArgumentException("Feedback não encontrado.");

        if (feedback.CustomerId != customerId)
            throw new ArgumentException("Apenas o autor do feedback pode editá-lo.");

        feedback.ChangeRate(rate);

        try
        {
            await _feedbackRepository.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Ocorreu um erro ao atualizar o feedback no banco de dados: {ex.Message}");
        }

        return new Response("Feedback atualizado com sucesso!");
    }
}