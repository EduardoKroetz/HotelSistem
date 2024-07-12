using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.RoomHandlers;

public partial class RoomHandler
{
    public async Task<Response> HandleUpdateCapacityAsync(Guid id, int newCapacity)
    {
        var room = await _repository.GetEntityByIdAsync(id);
        if (room == null)
            throw new NotFoundException("Hospedagem não encontrada.");

        room.ChangeCapacity(newCapacity);

        try
        {
            await _repository.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError($"Erro ao atualizar capacidade do cômodo {room.Id} no banco de dados. Erro: {e.Message}");
        }

        return new Response("Capacidade atualizada com sucesso!");
    }
}