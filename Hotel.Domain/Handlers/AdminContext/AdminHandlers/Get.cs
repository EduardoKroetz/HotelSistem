using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.AdminContext.AdminDTOs;
using Hotel.Domain.Handlers.Interfaces;

namespace Hotel.Domain.Handlers.AdminContext.AdminHandlers;

public partial class AdminHandler
{
  public async Task<Response<IEnumerable<GetAdmin>>> HandleGetAsync()
  {
    var admins = await _repository.GetAsync();
    return new Response<IEnumerable<GetAdmin>>(200,"", admins);
  }
}