using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.RoomContext.ServiceDTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.RoomContext.ServiceHandler;

public partial class ServiceHandler 
{
  public async Task<Response<GetService>> HandleGetByIdAsync(Guid id)
  {
    var service = await _repository.GetByIdAsync(id)
      ?? throw new NotFoundException("Serviço não encontrado.");
    
    return new Response<GetService>(200, "Sucesso!", service);
  }
}