using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.EmployeeContext.ResponsibilityDTOs;

namespace Hotel.Domain.Handlers.EmployeeContexty.ResponsibilityHandlers;

public partial class ResponsibilityHandler 
{
  public async Task<Response> HandleUpdateAsync(EditorResponsibility model, Guid id)
  {
    var Responsibility = await _repository.GetEntityByIdAsync(id);
    if (Responsibility == null)
      throw new ArgumentException("Responsabilidade não encontrada.");

    Responsibility.ChangeName(model.Name);
    Responsibility.ChangeDescription(model.Description);
    Responsibility.ChangePriority(model.Priority);

    _repository.Update(Responsibility);
    await _repository.SaveChangesAsync();

    return new Response(200,"Responsabilidade atualizado com sucesso!",new { Responsibility.Id });
  }
}