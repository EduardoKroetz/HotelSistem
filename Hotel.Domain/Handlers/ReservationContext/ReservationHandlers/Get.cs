using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.ReservationContext.ReservationDTOs;

namespace Hotel.Domain.Handlers.ReservationContext.ReservationHandlers;

public partial class ReservationHandler
{
  public async Task<Response<IEnumerable<GetReservation>>> HandleGetAsync(ReservationQueryParameters queryParameters)
  {
    var reservations = await _repository.GetAsync(queryParameters);
    return new Response<IEnumerable<GetReservation>>(200, "Sucesso!", reservations);
  }
}