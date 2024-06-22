using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.RoomHandlers;

public partial class RoomHandler
{
    public async Task<Response> HandleUpdateNumberAsync(Guid id, int newNumber)
    {
        var room = await _repository.GetEntityByIdAsync(id)
          ?? throw new NotFoundException("Hospedagem não encontrada.");

        var roomWithNumber = await _repository.GetRoomByNumber(newNumber);
        if (roomWithNumber != null && roomWithNumber.Id != room.Id)
            throw new ArgumentException("Esse número já está cadastrado.");

        room.ChangeNumber(newNumber);

        await _repository.SaveChangesAsync();

        return new Response("Número atualizado com sucesso!");
    }
}