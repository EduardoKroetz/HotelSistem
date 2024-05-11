using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.AdminContext.AdminDTOs;
using Hotel.Domain.Entities.AdminContext.AdminEntity;

namespace Hotel.Domain.Handlers.AdminContext.AdminHandlers;

public partial class AdminHandler
{
  public async Task<Response<IEnumerable<GetAdmin>>> HandleGetAsync(AdminQuery queryParameters)
  {
    var admins = await _repository.Query(queryParameters);
    return new Response<IEnumerable<GetAdmin>>(200,"", admins);
  }
}