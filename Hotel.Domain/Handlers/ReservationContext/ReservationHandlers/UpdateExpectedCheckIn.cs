using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.ReservationContext.ReservationHandlers;

public partial class ReservationHandler
{
  public async Task<Response> HandleUpdateExpectedCheckInAsync(Guid id, DateTime expectedCheckIn)
  {
    var reservation = await _repository.GetEntityByIdAsync(id)
      ?? throw new NotFoundException("Reserva não encontrada.");

    reservation.UpdateExpectedCheckIn(expectedCheckIn);

    await _repository.SaveChangesAsync();

    return new Response(200, "CheckIn esperado atualizado com sucesso!");
  }
  }