using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.EmployeeContext.ResponsibilityDTOs;

namespace Hotel.Domain.Handlers.EmployeeContexty.ResponsibilityHandlers;

public partial class ResponsibilityHandler 
{
  public async Task<Response<GetResponsibility>> HandleGetByIdAsync(Guid id)
  {
    var responsibility = await _repository.GetByIdAsync(id);
    if (responsibility == null)
      throw new ArgumentException("Responsabilidade não encontrada.");
    
    return new Response<GetResponsibility>(200,"Responsabilidade encontrada com sucesso!.", responsibility);
  }
}