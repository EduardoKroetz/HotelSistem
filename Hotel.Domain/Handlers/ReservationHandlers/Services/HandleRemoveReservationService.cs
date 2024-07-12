using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Hotel.Domain.Handlers.ReservationHandlers;

public partial class ReservationHandler
{
    public async Task<Response> HandleRemoveServiceAsync(Guid id, Guid serviceId)
    {
        var transaction = await _repository.BeginTransactionAsync();

        try
        {
            var reservation = await _repository.GetReservationIncludesServices(id)
                ?? throw new NotFoundException("Reserva não encontrada.");

            var service = await _serviceRepository.GetEntityByIdAsync(serviceId)
                ?? throw new NotFoundException("Serviço não encontrado.");

            reservation.RemoveService(service);

            try
            {
                await _repository.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError($"Erro ao remove o serviço no banco de dados: {e.Message}");
                throw new DbUpdateException("Ocorreu um erro ao atualizar a reserva no banco de dados");
            }


            try
            {
                await _stripeService.RemovePaymentIntentProduct(reservation.StripePaymentIntentId, serviceId);
            }
            catch
            {
                _logger.LogError($"Erro ao remover produto do PaymentIntent no stripe");
            }
            
            await transaction.CommitAsync();

            return new Response("Serviço removido com sucesso!");
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}