using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.RoomHandlers;

public partial class RoomHandler
{
    public async Task<Response> HandleEnableRoom(Guid id)
    {
        var room = await _repository.GetEntityByIdAsync(id);
        if (room == null)
            throw new NotFoundException("Hospedagem não encontrada.");

        await _stripeService.UpdateProductAsync(room.StripeProductId, room.Name, room.Description, room.Price, true);

        room.Enable();

        await _repository.SaveChangesAsync();
        return new Response("Hospedagem ativada com sucesso!");
    }
}