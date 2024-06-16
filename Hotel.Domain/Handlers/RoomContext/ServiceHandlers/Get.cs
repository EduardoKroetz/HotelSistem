using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.RoomContext.ServiceDTOs;

namespace Hotel.Domain.Handlers.RoomContext.ServiceHandler;

public partial class ServiceHandler
{
  public async Task<Response<IEnumerable<GetService>>> HandleGetAsync(ServiceQueryParameters queryParameters)
  {
    var services = await _repository.GetAsync(queryParameters);
    return new Response<IEnumerable<GetService>>(200, "Sucesso!", services);
  }
}