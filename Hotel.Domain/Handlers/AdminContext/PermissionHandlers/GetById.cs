using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.AdminContext.PermissionDTOs;

namespace Hotel.Domain.Handlers.AdminContext.PermissionHandlers;

public partial class PermissionHandler 
{
  public async Task<Response<GetPermission>> HandleGetByIdAsync(Guid id)
  {
    var permission = await _repository.GetByIdAsync(id);
    if (permission == null)
      throw new ArgumentException("Permissão não encontrada.");
    
    return new Response<GetPermission>(200,"Sucesso!", permission);
  }
}