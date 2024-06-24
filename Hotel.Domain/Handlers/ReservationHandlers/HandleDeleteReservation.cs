using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Hotel.Domain.Handlers.ReservationHandlers;

public partial class ReservationHandler
{
    public async Task<Response> HandleDeleteAsync(Guid id, Guid customerId)
    {
        var transaction = await _repository.BeginTransactionAsync();
        
        try
        {
            var reservation = await _repository.GetEntityByIdAsync(id)
                ?? throw new NotFoundException("Reserva não encontrada.");

            if (reservation.Status == Enums.EReservationStatus.CheckedIn)
                throw new InvalidOperationException("Não é possível deletar a reserva sem antes finaliza-la.");

            try
            {
                _repository.Delete(reservation);
                await _repository.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Ocorreu um erro ao deletar a reserva do banco de dados");
            }

            try
            {
                await _stripeService.CancelPaymentIntentAsync(reservation.StripePaymentIntentId);
            }
            catch (StripeException)
            {
                throw new StripeException("Ocorreu um erro ao cancelar o PaymentIntent no Stripe");
            }

            await transaction.CommitAsync();

            return new Response("Reserva deletada com sucesso!", new { reservation.Id, reservation.StripePaymentIntentId });
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}