using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.EmployeeContext.ResponsabilityDTOs;

namespace Hotel.Domain.Handlers.EmployeeContexty.ResponsabilityHandlers;

public partial class ResponsabilityHandler
{
  public async Task<Response<IEnumerable<GetReponsability>>> HandleGetAsync()
  {
    var reponsabilities = await _repository.GetAsync();
    return new Response<IEnumerable<GetReponsability>>(200,"", reponsabilities);
  }
} 