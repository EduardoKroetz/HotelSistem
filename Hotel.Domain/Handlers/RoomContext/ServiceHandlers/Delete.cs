using Hotel.Domain.DTOs;

namespace Hotel.Domain.Handlers.RoomContext.ServiceHandler;

public partial class ServiceHandler
{
  public async Task<Response<object>> HandleDeleteAsync(Guid id)
  {
    _repository.Delete(id);
    await _repository.SaveChangesAsync();
    return new Response<object>(200,"Serviço deletado.", new { id });
  }
}