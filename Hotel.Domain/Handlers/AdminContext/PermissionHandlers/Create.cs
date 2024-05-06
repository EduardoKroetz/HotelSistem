using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.AdminContext.PermissionDTOs;
using Hotel.Domain.Entities.AdminContext.PermissionEntity;
using Hotel.Domain.Handlers.Interfaces;
using Hotel.Domain.Repositories.Interfaces;

namespace Hotel.Domain.Handlers.AdminContext.PermissionHandlers;

public partial class PermissionHandler : IHandler
{
  private readonly IPermissionRepository  _repository;
  public PermissionHandler(IPermissionRepository repository)
  => _repository = repository;

  public async Task<Response<object>> HandleCreateAsync(EditorPermission model)
  {
    var permission = new Permission(model.Name,model.Description);

    await _repository.CreateAsync(permission);
    await _repository.SaveChangesAsync();

    return new Response<object>(200,"Permissão criada com sucesso!",new { permission.Id });
  }
}