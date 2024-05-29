using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.EmployeeContext.EmployeeDTOs;

namespace Hotel.Domain.Handlers.EmployeeContext.EmployeeHandlers;

public partial class EmployeeHandler
{
  public async Task<Response<IEnumerable<GetEmployee>>> HandleGetAsync(EmployeeQueryParameters queryParameters)
  {
    var employees = await _repository.GetAsync(queryParameters);
    return new Response<IEnumerable<GetEmployee>>(200,"", employees);
  }
}