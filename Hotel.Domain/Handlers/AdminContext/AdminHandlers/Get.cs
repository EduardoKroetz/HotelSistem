using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.User;

namespace Hotel.Domain.Handlers.AdminContext.AdminHandlers;

public partial class AdminHandler
{
  public async Task<Response<IEnumerable<GetUser>>> HandleGetAsync()
  {
    var admins = await _repository.GetAsync();
    return new Response<IEnumerable<GetUser>>(200,"", admins);
  }
}