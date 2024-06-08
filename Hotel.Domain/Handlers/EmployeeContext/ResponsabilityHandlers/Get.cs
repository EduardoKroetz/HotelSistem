using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.EmployeeContext.ResponsabilityDTOs;

namespace Hotel.Domain.Handlers.EmployeeContexty.ResponsabilityHandlers;

public partial class ResponsabilityHandler
{
  public async Task<Response<IEnumerable<GetReponsability>>> HandleGetAsync(ResponsabilityQueryParameters queryParameters)
  {
    var reponsabilities = await _repository.GetAsync(queryParameters);
    return new Response<IEnumerable<GetReponsability>>(200,"Sucesso!", reponsabilities);
  }
} 