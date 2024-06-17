using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.RoomContext.RoomHandlers;

public partial class RoomHandler
{
  public async Task<Response> HandleDeleteAsync(Guid id)
  {
    var room = await _repository.GetRoomIncludesReservations(id)
      ?? throw new NotFoundException("Hospedagem não encontrada.");

    if (room.Reservations.Count > 0)
      throw new InvalidOperationException("Não foi possível deletar a hospedagem pois tem reservas associadas a ela. Sugiro que desative a hospedagem.");

    _repository.Delete(room);
    await _repository.SaveChangesAsync();
    return new Response(200,"Hospedagem deletada com sucesso!", new { id });
  }
}