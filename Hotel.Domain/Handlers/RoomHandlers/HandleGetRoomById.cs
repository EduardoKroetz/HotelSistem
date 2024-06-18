using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.RoomDTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.RoomHandlers;

public partial class RoomHandler
{
    public async Task<Response<GetRoom>> HandleGetByIdAsync(Guid id)
    {
        var room = await _repository.GetByIdAsync(id);
        if (room == null)
            throw new NotFoundException("Hospedagem não encontrada.");

        return new Response<GetRoom>(200, "Sucesso!", room);
    }
}