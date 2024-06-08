using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.ReservationContext.ReservationHandlers;

public partial class ReservationHandler
{
  public async Task<Response> HandleUpdateExpectedCheckOutAsync(Guid id, DateTime expectedCheckOut)
  {
    var reservation = await _repository.GetEntityByIdAsync(id)
      ?? throw new NotFoundException("Reserva não encontrada.");

    reservation.UpdateExpectedCheckOut(expectedCheckOut);

    await _repository.SaveChangesAsync();

    return new Response(200, "CheckOut esperado atualizado com sucesso!.");
  }
}