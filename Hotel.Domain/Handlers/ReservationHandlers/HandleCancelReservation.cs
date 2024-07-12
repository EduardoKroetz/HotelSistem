using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Hotel.Domain.Handlers.ReservationHandlers;

public partial class ReservationHandler
{
    public async Task<Response> HandleCancelReservationAsync(Guid id, Guid customerId)
    {
        var transaction = await _repository.BeginTransactionAsync();

        try
        {
            var reservation = await _repository.GetReservationIncludesAll(id)
              ?? throw new NotFoundException("Reserva não encontrada.");

            if (reservation.CustomerId != customerId)
                throw new UnauthorizedAccessException("Você não tem permissão para cancelar reserva alheia.");

            reservation.ToCancelled();

            try
            {
                await _repository.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError($"Erro ao atualizar o status da reserva no banco de dados: {e.Message}");
                throw new DbUpdateException("Ocorreu um erro ao atualizar a reserva no banco de dados");
            }

            try
            {
                await _stripeService.CancelPaymentIntentAsync(reservation.StripePaymentIntentId);
            }
            catch (StripeException e)
            {
                _logger.LogError($"Erro ao cancelar o PaymentIntent no stripe: {e.Message}");
                throw new StripeException($"Ocorreu um erro ao lidar com o serviço de pagamento. Erro: {e.Message}");
            }

            await transaction.CommitAsync();

            return new Response("Reserva cancelada com sucesso!");
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

    }
}