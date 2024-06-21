using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.RoomHandlers;

public partial class RoomHandler
{
    public async Task<Response> HandleUpdateNameAsync(Guid id, string newName)
    {
        var room = await _repository.GetEntityByIdAsync(id)
          ?? throw new NotFoundException("Hospedagem não encontrada.");

        var numberAlreadyExists = await _repository.GetRoomByName(newName) is not null ? true : false;
        if (numberAlreadyExists)
            throw new ArgumentException("Esse nome já está cadastrado.");

        room.ChangeName(newName);

        await _repository.SaveChangesAsync();

        await _stripeService.UpdateProductAsync(room.StripeProductId, room.Name, room.Description, room.Price, room.IsActive);

        return new Response("Nome atualizado com sucesso!");
    }
}