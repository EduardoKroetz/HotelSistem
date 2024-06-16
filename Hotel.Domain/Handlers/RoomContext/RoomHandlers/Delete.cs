using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.RoomContext.RoomHandlers;

public partial class RoomHandler
{
  public async Task<Response> HandleDeleteAsync(Guid id)
  {
    var room = await _repository.GetRoomIncludesReservations(id)
      ?? throw new NotFoundException("C�modo n�o encontrado.");

    if (room.Reservations.Count > 0)
      throw new InvalidOperationException("N�o foi poss�vel deletar o c�modo pois tem reservas associadas a ele. Sugiro que desative o c�modo.");

    _repository.Delete(room);
    await _repository.SaveChangesAsync();
    return new Response(200,"C�modo deletado com sucesso!", new { id });
  }
}