using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.RoomContext.RoomHandlers;

public partial class RoomHandler
{
  public async Task<Response> HandleDisableRoom(Guid id)
  {
    var room = await _repository.GetRoomIncludesPendingReservations(id);
    if (room == null)
      throw new NotFoundException("Cômodo não encontrado.");

    if (room.Reservations.Count > 0)
      throw new InvalidOperationException("Não é possível desativar o cômodo quando tem reservas pendentes relacionadas.");

    room.Disable();

    await _repository.SaveChangesAsync();
    return new Response(200, "Cômodo desativado com sucesso!");
  }
}