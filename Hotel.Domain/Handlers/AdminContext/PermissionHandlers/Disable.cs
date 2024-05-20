using Hotel.Domain.DTOs;

namespace Hotel.Domain.Handlers.AdminContext.PermissionHandlers;

public partial class PermissionHandler
{
  public async Task<Response<object>> HandleDisableAsync(Guid id)
  {
    var permission = await _repository.GetEntityByIdAsync(id);
    if (permission == null)
      throw new ArgumentException("Permissão não encontrada.");

    permission.Disable();

    await _repository.SaveChangesAsync();
    return new Response<object>(200,"Permissão desabilitada.",null!);
  }
}