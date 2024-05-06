using Hotel.Domain.DTOs;

namespace Hotel.Domain.Handlers.AdminContext.PermissionHandlers;

public partial class PermissionHandler
{
  public async Task<Response<object>> HandleDeleteAsync(Guid id)
  {
    _repository.Delete(id);
    await _repository.SaveChangesAsync();
    return new Response<object>(200,"Permissão deletada com sucesso!", new { id });
  }
}