using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.DTOs.EmployeeContext.EmployeeDTOs;

namespace Hotel.Domain.Handlers.EmployeeContexty.EmployeeHandlers;

public partial class EmployeeHandler
{
  public async Task<Response<IEnumerable<GetUser>>> HandleGetAsync(EmployeeQueryParameters queryParameters)
  {
    var employees = await _repository.GetAsync(queryParameters);
    return new Response<IEnumerable<GetUser>>(200,"", employees);
  }
}