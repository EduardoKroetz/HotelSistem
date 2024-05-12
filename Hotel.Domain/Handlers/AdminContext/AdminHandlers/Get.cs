using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.AdminContext.AdminDTOs;

namespace Hotel.Domain.Handlers.AdminContext.AdminHandlers;

public partial class AdminHandler
{
  public async Task<Response<IEnumerable<GetAdmin>>> HandleGetAsync(AdminQueryParameters queryParameters)
  {
    var admins = await _repository.GetAsync(queryParameters);
    return new Response<IEnumerable<GetAdmin>>(200,"", admins);
  }
}