using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;
using Stripe;

namespace Hotel.Domain.Handlers.RoomHandlers;

public partial class RoomHandler
{
    public async Task<Response> HandleEnableRoom(Guid id)
    {
        var transaction = await _repository.BeginTransactionAsync();

        try
        {
            var room = await _repository.GetEntityByIdAsync(id);
            if (room == null)
                throw new NotFoundException("Hospedagem não encontrada.");

            room.Enable();
            await _repository.SaveChangesAsync();

            try
            {
                await _stripeService.UpdateProductAsync(room.StripeProductId, room.Name, room.Description, room.Price, true);
            }
            catch
            {
                throw new StripeException("Ocorreu um erro ao atualizar o produto no Stripe.");
            }

            await transaction.CommitAsync();

            return new Response("Hospedagem ativada com sucesso!");
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }


    }
}