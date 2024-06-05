using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.ReservationContext.ReservationHandlers;

public partial class ReservationHandler
{
  public async Task<Response> HandleUpdateCheckInAsync(Guid id, DateTime checkIn)
  {
    var reservation = await _repository.GetEntityByIdAsync(id)
    ?? throw new NotFoundException("Reserva não encontrada.");

    reservation.ChangeCheckIn(checkIn);

    await _repository.SaveChangesAsync();

    return new Response(200, "CheckIn atualizado com sucesso!.");
  }
}