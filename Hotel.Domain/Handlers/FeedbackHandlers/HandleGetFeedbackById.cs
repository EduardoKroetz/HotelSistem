using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.FeedbackDTOs;

namespace Hotel.Domain.Handlers.FeedbackHandlers;

public partial class FeedbackHandler
{
    public async Task<Response<GetFeedback>> HandleGetByIdAsync(Guid id)
    {
        var feedback = await _feedbackRepository.GetByIdAsync(id);
        if (feedback == null)
            throw new ArgumentException("Feedback não encontrado.");

        return new Response<GetFeedback>(200, "Sucesso!", feedback);
    }
}