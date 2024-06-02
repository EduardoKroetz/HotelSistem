using Hotel.Domain.DTOs;

namespace Hotel.Domain.Handlers.AdminContext.AdminHandlers;
partial class AdminHandler
{
  public async Task<Response> HandleAddPermission(Guid adminId, Guid permissionId)
  {
    //Buscar admin
    var admin = await _repository.GetAdminIncludePermissions(adminId);
    if (admin == null)
      throw new ArgumentException("Administrador não encontrado.");

    //Buscar permissão
    var permission = await _permissionRepository.GetEntityByIdAsync(permissionId);
    if (permission == null)
      throw new ArgumentException("Permissão não encontrada.");

    admin.AddPermission(permission);
 
    await _repository.SaveChangesAsync();
  
    return new Response(200, "Permissão adicionada! Faça login novamente para aplicar as alterações.");
  }
}
