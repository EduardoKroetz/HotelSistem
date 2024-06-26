using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Hotel.Domain.Handlers.ReservationHandlers;

public partial class ReservationHandler
{
    public async Task<Response> HandleFinishReservationAsync(Guid id, Guid customerId)
    {
        var transaction = await _repository.BeginTransactionAsync();

        try
        {
            var reservation = await _repository.GetReservationIncludesAll(id)
                ?? throw new NotFoundException("Reserva não encontrada.");

            if (reservation.CustomerId != customerId)
                throw new UnauthorizedAccessException("Você não tem permissão para finalizar reserva alheia.");

            //Create invoice and finalize Reservation
            var invoice = reservation.Finish(Enums.EPaymentMethod.CreditCard, 0);
      
            await _repository.SaveChangesAsync();
        
            try
            {
                await _stripeService.CapturePaymentIntentAsync(reservation.StripePaymentIntentId, reservation);
            }
            catch (StripeException e)
            {
                throw new StripeException($"Ocorreu um erro ao capturar a transação no Stripe. Erro: {e.Message}");
            }

            await transaction.CommitAsync();

            return new Response("Reserva finalizada e pagamento efetuado com sucesso!");
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }


    }
}