using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.RoomDTOs;

namespace Hotel.Domain.Handlers.RoomHandlers;

public partial class RoomHandler
{
    public async Task<Response<IEnumerable<GetRoom>>> HandleGetAsync(RoomQueryParameters queryParameters)
    {
        var rooms = await _repository.GetAsync(queryParameters);
        return new Response<IEnumerable<GetRoom>>("Sucesso!", rooms);
    }
}