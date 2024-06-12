using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.EmployeeContext.ResponsibilityDTOs;

namespace Hotel.Domain.Handlers.EmployeeContexty.ResponsibilityHandlers;

public partial class ResponsibilityHandler
{
  public async Task<Response<IEnumerable<GetResponsibility>>> HandleGetAsync(ResponsibilityQueryParameters queryParameters)
  {
    var responsibilities = await _repository.GetAsync(queryParameters);
    return new Response<IEnumerable<GetResponsibility>>(200,"Sucesso!", responsibilities);
  }
} 