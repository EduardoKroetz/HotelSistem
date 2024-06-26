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

            await _repository.SaveChangesAsync();

            try
            {                
                await _stripeService.CreatePaymentMethodAsync(tokenId, reservation.StripePaymentIntentId);
                await _stripeService.ConfirmPaymentIntentAsync(reservation.StripePaymentIntentId);
            }
            catch (StripeException e)
            {
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