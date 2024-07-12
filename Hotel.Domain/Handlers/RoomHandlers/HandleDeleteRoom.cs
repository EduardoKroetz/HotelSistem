using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;
using Stripe;

namespace Hotel.Domain.Handlers.RoomHandlers;

public partial class RoomHandler
{
    public async Task<Response> HandleDeleteAsync(Guid id)
    {
        var transaction = await _repository.BeginTransactionAsync();

        try
        {
            var room = await _repository.GetRoomIncludesReservations(id)
                ?? throw new NotFoundException("Hospedagem não encontrada.");

            //Validate if has associated pending reservations 
            if (room.Reservations.Count > 0)
                throw new InvalidOperationException("Não foi possível deletar a hospedagem pois tem reservas associadas a ela. Sugiro que desative a hospedagem.");

            try
            {
                _repository.Delete(room);
                await _repository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Erro ao deletar cômodo {room.Id} no banco de dados. Erro: {e.Message}");
            }


            try
            {
                //Disable room in Stripe
                var stripeResponse = await _stripeService.DisableProductAsync(room.StripeProductId) ??
                    throw new Exception();
            }
            catch (Exception e)
            {
                _logger.LogError($"Erro ao desativar produto no Stripe ao deletar cômodo {room.Id}. Erro: {e.Message}");
                throw new StripeException("Um erro ocorreu ao desativar o produto no Stripe.");
            }

            await transaction.CommitAsync();

            return new Response("Hospedagem deletada com sucesso!", new { id });
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}