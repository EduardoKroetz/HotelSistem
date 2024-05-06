using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.AdminContext.PermissionDTOs;

namespace Hotel.Domain.Handlers.AdminContext.AdminHandlers;

public partial class PermissionHandler 
{
    public async Task<Response<object>> HandleUpdateAsync(EditorPermission model, Guid id)
  {
    var permission = await _repository.GetEntityByIdAsync(id);
    if (permission == null)
      throw new ArgumentException("Permissão não encontrada.");

    permission.ChangeName(model.Name);
    permission.ChangeDescription(model.Description);

    _repository.Update(permission);
    await _repository.SaveChangesAsync();

    return new Response<object>(200,"Permissão atualizada com sucesso!",new { permission.Id });
  }
}