using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.RoomContext.ServiceDTOs;

namespace Hotel.Domain.Handlers.RoomContext.ServiceHandler;

public partial class ServiceHandler 
{
  public async Task<Response<GetService>> HandleGetByIdAsync(Guid id)
  {
    var service = await _repository.GetByIdAsync(id);
    if (service == null)
      throw new ArgumentException("Serviço não encontrado.");
    
    return new Response<GetService>(200,"Serviço encontrado.", service);
  }
}