using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.RoomContext.RoomDTOs;

namespace Hotel.Domain.Handlers.RoomContext.RoomHandlers;

public partial class RoomHandler
{
  public async Task<Response<IEnumerable<GetRoomCollection>>> HandleGetAsync(RoomQueryParameters queryParameters)
  {
    var rooms = await _repository.GetAsync(queryParameters);
    return new Response<IEnumerable<GetRoomCollection>>(200,"", rooms);
  }
} 