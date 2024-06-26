using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Hotel.Domain.Handlers.ReservationHandlers;

public partial class ReservationHandler
{
    public async Task<Response> HandleUpdateExpectedCheckOutAsync(Guid id, DateTime expectedCheckOut)
    {
        var transaction = await _repository.BeginTransactionAsync();

        try
        {
            var reservation = await _repository.GetEntityByIdAsync(id)
                ?? throw new NotFoundException("Reserva não encontrada.");

            reservation.UpdateExpectedCheckOut(expectedCheckOut);

            try
            {
                await _repository.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Ocorreu um erro ao atualizar a reserva no banco de dados");
            }

            try
            {
                await _stripeService.UpdatePaymentIntentAsync(reservation.StripePaymentIntentId, reservation.ExpectedTotalAmount());
            }
            catch (StripeException e)
            {
                throw new StripeException($"Ocorreu um erro ao lidar com o serviço de pagamento. Erro: {e.Message}");
            }

            await transaction.CommitAsync();

            return new Response("CheckOut esperado atualizado com sucesso!");
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}