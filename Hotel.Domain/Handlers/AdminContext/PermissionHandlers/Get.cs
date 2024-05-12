using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.AdminContext.PermissionDTOs;

namespace Hotel.Domain.Handlers.AdminContext.PermissionHandlers;

public partial class PermissionHandler
{
  public async Task<Response<IEnumerable<GetPermission>>> HandleGetAsync(PermissionQueryParameters queryParameters)
  {
    var permissions = await _repository.GetAsync(queryParameters);
    return new Response<IEnumerable<GetPermission>>(200,"", permissions);
  }
}