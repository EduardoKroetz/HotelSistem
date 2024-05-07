using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.User;

namespace Hotel.Domain.Handlers.EmployeeContexty.EmployeeHandlers;

public partial class EmployeeHandler
{
  public async Task<Response<IEnumerable<GetUser>>> HandleGetAsync()
  {
    var employees = await _repository.GetAsync();
    return new Response<IEnumerable<GetUser>>(200,"", employees);
  }
}