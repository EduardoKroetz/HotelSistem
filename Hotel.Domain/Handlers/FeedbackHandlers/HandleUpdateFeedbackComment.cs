using Hotel.Domain.DTOs;

namespace Hotel.Domain.Handlers.FeedbackHandlers;

public partial class FeedbackHandler
{
    public async Task<Response> HandleUpdateCommentAsync(Guid id, string comment, Guid customerId)
    {
        var feedback = await _feedbackRepository.GetEntityByIdAsync(id);
        if (feedback == null)
            throw new ArgumentException("Feedback não encontrado.");

        if (feedback.CustomerId != customerId)
            throw new ArgumentException("Apenas o autor do feedback pode editá-lo.");

        feedback.ChangeComment(comment);

        await _feedbackRepository.SaveChangesAsync();

        return new Response("Feedback atualizado com sucesso!");
    }
}