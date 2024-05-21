using Hotel.Domain.DTOs;

namespace Hotel.Domain.Handlers.ReservationContext.ReservationHandlers;

public partial class ReservationHandler
{
  public async Task<Response<object>> HandleUpdateCheckInAsync(Guid id, DateTime checkIn)
  {
    var reservation = await _repository.GetEntityByIdAsync(id);
    if (reservation == null)
      throw new ArgumentException("Reserva não encontrada.");

    reservation.ChangeCheckIn(checkIn);

    await _repository.SaveChangesAsync();

    return new Response<object>(200, "CheckIn atualizado.");
  }
}