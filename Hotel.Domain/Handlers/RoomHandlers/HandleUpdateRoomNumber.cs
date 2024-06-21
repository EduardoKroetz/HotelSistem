using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.RoomHandlers;

public partial class RoomHandler
{
    public async Task<Response> HandleUpdateNumberAsync(Guid id, int newNumber)
    {
        var room = await _repository.GetEntityByIdAsync(id)
          ?? throw new NotFoundException("Hospedagem não encontrada.");

        var numberAlreadyExists = await _repository.GetRoomByNumber(newNumber) is not null ? true : false;
        if (numberAlreadyExists)
            throw new ArgumentException("Esse número já está cadastrado.");

        room.ChangeNumber(newNumber);

        await _repository.SaveChangesAsync();

        return new Response("Número atualizado com sucesso!");
    }
}