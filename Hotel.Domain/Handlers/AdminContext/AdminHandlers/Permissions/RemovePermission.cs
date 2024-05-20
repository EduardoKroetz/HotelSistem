
using Hotel.Domain.DTOs;

namespace Hotel.Domain.Handlers.AdminContext.AdminHandlers;
partial class AdminHandler
{
  public async Task<Response<object>> HandleRemovePermission(Guid adminId, Guid permissionId)
  {
    //Buscar admin
    var admin = await _repository.GetAdminIncludePermissions(adminId);
    if (admin == null)
      throw new ArgumentException("Administrador não encontrado.");

    //Buscar permissão
    var permission = await _permissionRepository.GetEntityByIdAsync(permissionId);
    if (permission == null)
      throw new ArgumentException("Permissão não encontrada.");

    admin.RemovePermission(permission);

    await _repository.SaveChangesAsync();

    return new Response<object>(200, "Permissão removida.",null!);
  }
}
