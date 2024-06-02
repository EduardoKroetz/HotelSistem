using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.RoomContext.RoomHandlers;

public partial class RoomHandler
{
  public async Task<Response> HandleEnableRoom(Guid id)
  {
    var room = await _repository.GetEntityByIdAsync(id);
    if (room == null)
      throw new NotFoundException("Cômodo não encontrado.");

    room.Enable();

    await _repository.SaveChangesAsync();
    return new Response(200, "Cômodo ativado com sucesso!");
  }
}