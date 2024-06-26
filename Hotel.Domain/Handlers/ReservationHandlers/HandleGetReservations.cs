using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.ReservationDTOs;

namespace Hotel.Domain.Handlers.ReservationHandlers;

public partial class ReservationHandler
{
    public async Task<Response<IEnumerable<GetReservation>>> HandleGetAsync(ReservationQueryParameters queryParameters)
    {
        var reservations = await _repository.GetAsync(queryParameters);
        return new Response<IEnumerable<GetReservation>>("Sucesso!", reservations);
    }
}