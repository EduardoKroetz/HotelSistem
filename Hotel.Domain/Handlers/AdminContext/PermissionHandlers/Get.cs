using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.AdminContext.PermissionDTOs;
using Hotel.Domain.Repositories.Interfaces.AdminContext;

namespace Hotel.Domain.Handlers.AdminContext.PermissionHandlers;

public partial class PermissionHandler
{
  private readonly IPermissionRepository _repository;
  public PermissionHandler(IPermissionRepository permissionRepository)
  => _repository = permissionRepository;
       
  public async Task<Response<IEnumerable<GetPermission>>> HandleGetAsync(PermissionQueryParameters queryParameters)
  {
    var permissions = await _repository.GetAsync(queryParameters);
    return new Response<IEnumerable<GetPermission>>(200,"Sucesso!", permissions);
  }
}