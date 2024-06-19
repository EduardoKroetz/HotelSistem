using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.ResponsibilityDTOs;
using Hotel.Domain.Entities.ResponsibilityEntity;
using Hotel.Domain.Handlers.Base.Interfaces;
using Hotel.Domain.Repositories.Interfaces;

namespace Hotel.Domain.Handlers.y.ResponsibilityHandlers;

public partial class ResponsibilityHandler : IHandler
{
    private readonly IResponsibilityRepository _repository;
    public ResponsibilityHandler(IResponsibilityRepository repository)
    => _repository = repository;

    public async Task<Response> HandleCreateAsync(EditorResponsibility model)
    {
        var responsibility = new Responsibility(model.Name, model.Description, model.Priority);

        await _repository.CreateAsync(responsibility);
        await _repository.SaveChangesAsync();

        return new Response("Responsabilidade criada com sucesso!", new { responsibility.Id });
    }
}