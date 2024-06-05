using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.ReservationContext.ReservationHandlers;

public partial class ReservationHandler
{
  public async Task<Response> HandleUpdateCheckOutAsync(Guid id, DateTime checkOut)
  {
    var reservation = await _repository.GetEntityByIdAsync(id)
    ?? throw new NotFoundException("Reserva não encontrada.");

    reservation.ChangeCheckOut(checkOut);

    await _repository.SaveChangesAsync();

    return new Response(200, "CheckOut atualizado com sucesso!.");
  }
}