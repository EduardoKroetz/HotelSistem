using Hotel.Domain.DTOs;
using Hotel.Domain.Entities.LikeEntity;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.FeedbackHandlers;

public partial class FeedbackHandler
{
    public async Task<Response> HandleAddLikeAsync(Guid feedbackId, Guid customerId)
    {
        var feedback = await _feedbackRepository.GetEntityByIdAsync(feedbackId);
        if (feedback == null)
            throw new NotFoundException("Feedback não encontrado.");

        var customer = await _customerRepository.GetEntityByIdAsync(customerId);
        if (customer == null)
            throw new NotFoundException("Usuário não encontrado.");

        var likeExists = await _likeRepository.GetLikeAsync(feedbackId, customerId);
        if (likeExists != null)
            throw new InvalidOperationException("Você já tem um like atribuido a esse feedback.");

        var like = new Like(customer, feedback);

        await _likeRepository.CreateLike(like);

        await _feedbackRepository.SaveChangesAsync();

        return new Response(200, "Like adicionado com sucesso!");
    }
}