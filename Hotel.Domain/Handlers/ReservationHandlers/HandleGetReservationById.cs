using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.ReservationDTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.ReservationHandlers;

public partial class ReservationHandler
{
    public async Task<Response<GetReservation>> HandleGetByIdAsync(Guid id)
    {
        var reservation = await _repository.GetByIdAsync(id)
          ?? throw new NotFoundException("Reserva não encontrada.");

        return new Response<GetReservation>("Sucesso!", reservation);
    }
}