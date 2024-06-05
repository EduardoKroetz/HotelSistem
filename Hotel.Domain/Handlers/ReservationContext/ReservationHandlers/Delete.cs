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
      throw new InvalidOperationException("N�o � poss�vel deletar a reserva sem primeiro finaliza-la.");

    if (reservation.CustomerId != customerId)
      throw new UnauthorizedAccessException("Voc� n�o tem autoriza��o para deletar essa reserva.");

    _repository.Delete(reservation);
    await _repository.SaveChangesAsync();
    return new Response(200,"Reserva deletada com sucesso!", new { id });
  }
}