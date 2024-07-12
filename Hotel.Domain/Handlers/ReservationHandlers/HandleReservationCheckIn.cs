using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;
using Stripe;

namespace Hotel.Domain.Handlers.ReservationHandlers;

public partial class ReservationHandler
{
    public async Task<Response> HandleReservationCheckInAsync(Guid id, string tokenId)
    {
        var transaction = await _repository.BeginTransactionAsync();

        try
        {
            var reservation = await _repository.GetEntityByIdAsync(id)
                ?? throw new NotFoundException("Reserva não encontrada.");

            reservation.ToCheckIn();

            try
            {
                await _repository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Erro ao atualizar o status da reserva para CheckedIn no banco de dados: {e.Message}");
            }


            try
            {
                await _stripeService.CreatePaymentMethodAsync(tokenId, reservation.StripePaymentIntentId);
            }
            catch (StripeException e)
            {
                _logger.LogError($"Erro ao criar método de pagamento no Stripe: {e.Message}");
                throw new StripeException($"Ocorreu um erro ao lidar com o serviço de pagamento. Erro: {e.Message}");
            }

            try
            {                
                await _stripeService.ConfirmPaymentIntentAsync(reservation.StripePaymentIntentId);
            }
            catch (StripeException e)
            {
                _logger.LogError($"Erro ao confirmar o PaymentIntent no Stripe: {e.Message}");
                throw new StripeException($"Ocorreu um erro ao lidar com o serviço de pagamento. Erro: {e.Message}");
            }

            await transaction.CommitAsync();

            return new Response("Check-In realizado com sucesso!");
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}