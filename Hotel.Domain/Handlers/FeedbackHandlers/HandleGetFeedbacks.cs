using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.FeedbackDTOs;

namespace Hotel.Domain.Handlers.FeedbackHandlers;

public partial class FeedbackHandler
{
    public async Task<Response<IEnumerable<GetFeedback>>> HandleGetAsync(FeedbackQueryParameters queryParameters)
    {
        var feedbacks = await _feedbackRepository.GetAsync(queryParameters);
        return new Response<IEnumerable<GetFeedback>>(200, "Sucesso!", feedbacks);
    }
}