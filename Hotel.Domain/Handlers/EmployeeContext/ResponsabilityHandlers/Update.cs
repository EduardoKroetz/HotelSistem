using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.EmployeeContext.ResponsabilityDTOs;

namespace Hotel.Domain.Handlers.EmployeeContexty.ResponsabilityHandlers;

public partial class ResponsabilityHandler 
{
    public async Task<Response> HandleUpdateAsync(EditorResponsability model, Guid id)
  {
    var responsability = await _repository.GetEntityByIdAsync(id);
    if (responsability == null)
      throw new ArgumentException("Responsabilidade não encontrada.");

    responsability.ChangeName(model.Name);
    responsability.ChangeDescription(model.Description);
    responsability.ChangePriority(model.Priority);

    _repository.Update(responsability);
    await _repository.SaveChangesAsync();

    return new Response(200,"Responsabilidade foi atualizada.",new { responsability.Id });
  }
}