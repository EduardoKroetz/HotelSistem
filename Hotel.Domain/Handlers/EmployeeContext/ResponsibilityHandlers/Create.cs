using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.EmployeeContext.ResponsibilityDTOs;
using Hotel.Domain.Entities.EmployeeContext.ResponsibilityEntity;
using Hotel.Domain.Handlers.Interfaces;
using Hotel.Domain.Repositories.Interfaces.EmployeeContext;

namespace Hotel.Domain.Handlers.EmployeeContexty.ResponsibilityHandlers;

public partial class ResponsibilityHandler : IHandler
{
  private readonly IResponsibilityRepository  _repository;
  public ResponsibilityHandler(IResponsibilityRepository repository)
  => _repository = repository;

  public async Task<Response> HandleCreateAsync(EditorResponsibility model)
  {
    var responsibility = new Responsibility(model.Name,model.Description,model.Priority);

    await _repository.CreateAsync(responsibility);
    await _repository.SaveChangesAsync();

    return new Response(200,"Responsabilidade criada com sucesso!",new { responsibility.Id });
  }
}