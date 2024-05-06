using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.AdminContext.PermissionDTOs;

namespace Hotel.Domain.Handlers.AdminContext.AdminHandlers;

public partial class PermissionHandler
{
  public async Task<Response<IEnumerable<GetPermission>>> HandleGetAsync()
  {
    var permissions = await _repository.GetAsync();
    return new Response<IEnumerable<GetPermission>>(200,"", permissions);
  }
}