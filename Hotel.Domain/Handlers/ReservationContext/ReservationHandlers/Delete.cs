using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.ReservationContext.ReservationHandlers;

public partial class ReservationHandler
{
  public async Task<Response> HandleDeleteAsync(Guid id, Guid customerId)
  {
    var reservation = await _repository.GetEntityByIdAsync(id)
      ?? throw new NotFoundException("Reserva n�o encontrada.");

    if (reservation.Status == Enums.EReservationStatus.CheckedIn)
      throw new InvalidOperationException("N�o � poss�vel deletar a reserva sem antes finaliza-la.");

    _repository.Delete(reservation);
    await _repository.SaveChangesAsync();
    return new Response(200,"Reserva deletada com sucesso!", new { id });
  }
}