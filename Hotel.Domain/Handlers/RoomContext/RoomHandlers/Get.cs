using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.PaymentContext.RoomInvoiceDTOs;

namespace Hotel.Domain.Handlers.RoomContext.RoomHandlers;

public partial class RoomHandler
{
  public async Task<Response<IEnumerable<GetRoomCollection>>> HandleGetAsync()
  {
    var rooms = await _repository.GetAsync();
    return new Response<IEnumerable<GetRoomCollection>>(200,"", rooms);
  }
} 