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

        await _repository.SaveChangesAsync();

        return new Response(200, "Capacidade atualizada com sucesso!");
    }
}