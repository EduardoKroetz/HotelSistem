using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.PaymentContext.RoomInvoiceDTOs;

namespace Hotel.Domain.Handlers.ReservationContext.ReservationHandlers;

public partial class ReservationHandler
{
  public async Task<Response<IEnumerable<GetReservationCollection>>> HandleGetAsync()
  {
    var reservations = await _repository.GetAsync();
    return new Response<IEnumerable<GetReservationCollection>>(200,"", reservations);
  }
}