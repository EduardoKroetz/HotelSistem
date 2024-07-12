using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;
using Stripe;

namespace Hotel.Domain.Handlers.RoomHandlers;

public partial class RoomHandler
{
    public async Task<Response> HandleUpdateNameAsync(Guid id, string newName)
    {
        var transaction = await _repository.BeginTransactionAsync();

        try
        {
            var room = await _repository.GetEntityByIdAsync(id)
                ?? throw new NotFoundException("Hospedagem não encontrada.");

            var roomWithName = await _repository.GetRoomByName(newName);
            if (roomWithName != null && roomWithName.Id != room.Id)
                throw new ArgumentException("Esse nome já está cadastrado.");

            room.ChangeName(newName);

            try
            {
                await _repository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Erro ao atualizar nome do cômodo {room.Id} no banco de dados. Erro: {e.Message}");
            }

            try
            {
                await _stripeService.UpdateProductAsync(room.StripeProductId, room.Name, room.Description, room.Price, room.IsActive);
            }
            catch (Exception e)
            {
                _logger.LogError($"Erro ao atualizar nome do produto {room.StripeProductId} no Stripe. Erro: {e.Message}");
                throw new StripeException("Um erro ocorreu ao atualizar o produto no Stripe.");
            }

            await transaction.CommitAsync();
            return new Response("Nome atualizado com sucesso!");

        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}