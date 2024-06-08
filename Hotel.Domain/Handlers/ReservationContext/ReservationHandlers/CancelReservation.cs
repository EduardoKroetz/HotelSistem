using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.ReservationContext.ReservationHandlers;

public partial class ReservationHandler
{
  public async Task<Response> HandleCancelReservationAsync(Guid id, Guid customerId)
  {
    var reservation = await _repository.GetReservationIncludesAll(id)
      ?? throw new NotFoundException("Reserva não encontrada.");

    if (reservation.CustomerId != customerId)
      throw new UnauthorizedAccessException("Você não tem permissão para cancelar reserva alheia.");

    reservation.ToCancelled();

    await _repository.SaveChangesAsync();

    return new Response(200, "Reserva cancelada com sucesso!");
  }
}