using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.RoomContext.ServiceDTOs;

namespace Hotel.Domain.Handlers.RoomContext.ServiceHandler;

public partial class ServiceHandler
{
  public async Task<Response<IEnumerable<GetServiceCollection>>> HandleGetAsync(ServiceQueryParameters queryParameters)
  {
    var services = await _repository.GetAsync(queryParameters);
    return new Response<IEnumerable<GetServiceCollection>>(200, "Sucesso!", services);
  }
}