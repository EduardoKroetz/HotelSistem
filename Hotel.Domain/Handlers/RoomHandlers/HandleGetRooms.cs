using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.RoomDTOs;

namespace Hotel.Domain.Handlers.RoomHandlers;

public partial class RoomHandler
{
    public async Task<Response<IEnumerable<GetRoomCollection>>> HandleGetAsync(RoomQueryParameters queryParameters)
    {
        var rooms = await _repository.GetAsync(queryParameters);
        return new Response<IEnumerable<GetRoomCollection>>(200, "Sucesso!", rooms);
    }
}