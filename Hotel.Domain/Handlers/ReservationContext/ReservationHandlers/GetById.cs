using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.ReservationContext.ReservationDTOs;

namespace Hotel.Domain.Handlers.ReservationContext.ReservationHandlers;

public partial class ReservationHandler 
{
  public async Task<Response<GetReservation>> HandleGetByIdAsync(Guid id)
  {
    var reservation = await _repository.GetByIdAsync(id);
    if (reservation == null)
      throw new ArgumentException("Reserva não encontrada.");
    
    return new Response<GetReservation>(200, "Sucesso!", reservation);
  }
}