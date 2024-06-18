using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.RoomHandlers;

public partial class RoomHandler
{
    public async Task<Response> HandleDisableRoom(Guid id)
    {
        var room = await _repository.GetRoomIncludesReservations(id)
          ?? throw new NotFoundException("Hospedagem não encontrada.");

        var pendingReservations = room.Reservations.Where(x => x.Status == Enums.EReservationStatus.Pending).ToList();
        if (pendingReservations.Count > 0)
            throw new InvalidOperationException("Não foi possível desativar a hospedagem pois tem reservas pendentes relacionadas.");

        room.Disable();

        await _repository.SaveChangesAsync();
        return new Response(200, "Hospedagem desativada com sucesso!");
    }
}