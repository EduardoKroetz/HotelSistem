using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.User;

namespace Hotel.Domain.Handlers.CustomerContext.CustomerHandlers;

public partial class CustomerHandler
{
  public async Task<Response<IEnumerable<GetUser>>> HandleGetAsync()
  {
    var customers = await _repository.GetAsync();
    return new Response<IEnumerable<GetUser>>(200,"", customers);
  }
}