using Hotel.Domain.DTOs;
using Hotel.Domain.Entities.CustomerContext.FeedbackContext;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.CustomerContext.FeedbackHandlers;

public partial class FeedbackHandler
{
  public async Task<Response> HandleAddDeslikeAsync(Guid feedbackId, Guid customerId)
  {
    var feedback = await _feedbackRepository.GetEntityByIdAsync(feedbackId);

    if (feedback == null)
      throw new NotFoundException("Feedback não encontrado.");

    var customer = await _customerRepository.GetEntityByIdAsync(customerId);
    if (customer == null)
      throw new NotFoundException("Usuário não encontrado.");

    var deslikeExists = await _deslikeRepository.GetDeslikeAsync(feedbackId, customerId);
    if (deslikeExists != null)
      throw new InvalidOperationException("Você já tem um deslike atribuido a esse feedback.");

    var deslike = new Deslike(customer,feedback);

    await _deslikeRepository.CreateDeslike(deslike);

    await _feedbackRepository.SaveChangesAsync();

    return new Response(200,"Deslike adicionado com sucesso!");
  }
}