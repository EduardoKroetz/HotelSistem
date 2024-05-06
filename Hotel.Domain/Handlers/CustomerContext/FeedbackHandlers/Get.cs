using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.CustomerContext.FeedbackDTOs;

namespace Hotel.Domain.Handlers.CustomerContext.FeedbackHandlers;

public partial class FeedbackHandler
{
  public async Task<Response<IEnumerable<GetFeedback>>> HandleGetAsync()
  {
    var feedbacks = await _repository.GetAsync();
    return new Response<IEnumerable<GetFeedback>>(200,"", feedbacks);
  }
}