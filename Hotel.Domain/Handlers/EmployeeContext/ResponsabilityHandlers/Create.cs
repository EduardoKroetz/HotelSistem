using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.EmployeeContext.ResponsabilityDTOs;
using Hotel.Domain.Entities.EmployeeContext.ResponsabilityEntity;
using Hotel.Domain.Handlers.Interfaces;
using Hotel.Domain.Repositories.Interfaces.EmployeeContext;

namespace Hotel.Domain.Handlers.EmployeeContexty.ResponsabilityHandlers;

public partial class ResponsabilityHandler : IHandler
{
  private readonly IResponsabilityRepository  _repository;
  public ResponsabilityHandler(IResponsabilityRepository repository)
  => _repository = repository;

  public async Task<Response> HandleCreateAsync(EditorResponsability model)
  {
    var responsability = new Responsability(model.Name,model.Description,model.Priority);

    await _repository.CreateAsync(responsability);
    await _repository.SaveChangesAsync();

    return new Response(200,"Responsabilidade criada com sucesso!",new { responsability.Id });
  }
}