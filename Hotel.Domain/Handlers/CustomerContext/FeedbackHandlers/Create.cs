using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.CustomerContext.FeedbackDTOs;
using Hotel.Domain.Entities.CustomerContext.FeedbackEntity;
using Hotel.Domain.Handlers.Interfaces;
using Hotel.Domain.Repositories.Interfaces.CustomerContext;

namespace Hotel.Domain.Handlers.CustomerContext.FeedbackHandlers;

public partial class FeedbackHandler : IHandler
{
  private readonly IFeedbackRepository  _repository;
  public FeedbackHandler(IFeedbackRepository repository)
  => _repository = repository;

  public async Task<Response<object>> HandleCreateAsync(CreateFeedback model)
  {
    var feedback = new Feedback(
      model.Comment,model.Rate,model.CustomerId,model.ReservationId,model.RoomId
    );

    await _repository.CreateAsync(feedback);
    await _repository.SaveChangesAsync();

    return new Response<object>(200,"Feedback registrado.",new { feedback.Id });
  }
}