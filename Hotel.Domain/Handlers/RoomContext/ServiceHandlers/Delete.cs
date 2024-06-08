using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.RoomContext.ServiceHandler;

public partial class ServiceHandler
{
  public async Task<Response> HandleDeleteAsync(Guid id)
  {
    var service = await _repository.GetEntityByIdAsync(id);
    if (service == null)
      throw new NotFoundException("Serviço não encontrado.");

    _repository.Delete(service);
    await _repository.SaveChangesAsync();
    return new Response(200,"Serviço deletado com sucesso!", new { id });
  }
}