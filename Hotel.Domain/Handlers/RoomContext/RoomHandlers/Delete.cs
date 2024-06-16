using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.RoomContext.RoomHandlers;

public partial class RoomHandler
{
  public async Task<Response> HandleDeleteAsync(Guid id)
  {
    var room = await _repository.GetRoomIncludesReservations(id)
      ?? throw new NotFoundException("Cômodo não encontrado.");

    if (room.Reservations.Count > 0)
      throw new InvalidOperationException("Não foi possível deletar o cômodo pois tem reservas associadas a ele. Sugiro que desative o cômodo.");

    _repository.Delete(room);
    await _repository.SaveChangesAsync();
    return new Response(200,"Cômodo deletado com sucesso!", new { id });
  }
}