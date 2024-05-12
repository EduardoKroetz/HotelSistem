using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.ReservationContext.ReservationDTOs;

namespace Hotel.Domain.Handlers.ReservationContext.ReservationHandlers;

public partial class ReservationHandler
{
  public async Task<Response<IEnumerable<GetReservationCollection>>> HandleGetAsync(ReservationQueryParameters queryParameters)
  {
    var reservations = await _repository.GetAsync(queryParameters);
    return new Response<IEnumerable<GetReservationCollection>>(200,"", reservations);
  }
}