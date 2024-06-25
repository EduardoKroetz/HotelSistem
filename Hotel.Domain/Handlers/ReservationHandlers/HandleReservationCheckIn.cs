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
                var paymentMethod = await _stripeService.CreatePaymentMethodAsync(tokenId);
               
                await _stripeService.AddPaymentMethodToPaymentIntent(reservation.StripePaymentIntentId, paymentMethod.Id);
            }
            catch (StripeException e)
            {
                throw new StripeException("Ocorreu um erro ao atualizar o método de pagamento");
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