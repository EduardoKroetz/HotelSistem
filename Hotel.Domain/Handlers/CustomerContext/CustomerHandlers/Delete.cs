using Hotel.Domain.DTOs;

namespace Hotel.Domain.Handlers.CustomerContext.CustomerHandlers;

public partial class CustomerHandler
{
  public async Task<Response> HandleDeleteAsync(Guid id)
  {
    _repository.Delete(id);
    await _repository.SaveChangesAsync();
    return new Response(200,"Usuário deletado com sucesso!", new { id });
  }
}