using Hotel.Domain.DTOs;
using Hotel.Domain.Entities.DislikeEntity;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.FeedbackHandlers;

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

        var dislikeExists = await _dislikeRepository.GetDeslikeAsync(feedbackId, customerId);
        if (dislikeExists != null)
            throw new InvalidOperationException("Você já tem um dislike atribuido a esse feedback.");

        var dislike = new Dislike(customer, feedback);

        await _dislikeRepository.CreateDeslike(dislike);

        await _feedbackRepository.SaveChangesAsync();

        return new Response("Deslike adicionado com sucesso!");
    }
}