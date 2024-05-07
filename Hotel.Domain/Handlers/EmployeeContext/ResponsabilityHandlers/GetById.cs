using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.EmployeeContext.ResponsabilityDTOs;

namespace Hotel.Domain.Handlers.EmployeeContexty.ResponsabilityHandlers;

public partial class ResponsabilityHandler 
{
  public async Task<Response<GetReponsability>> HandleGetByIdAsync(Guid id)
  {
    var reponsability = await _repository.GetByIdAsync(id);
    if (reponsability == null)
      throw new ArgumentException("Responsabilidade não encontrada.");
    
    return new Response<GetReponsability>(200,"Responsabilidade encontrada.", reponsability);
  }
}