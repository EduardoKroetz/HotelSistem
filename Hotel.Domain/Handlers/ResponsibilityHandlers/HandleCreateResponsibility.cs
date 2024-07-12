using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.ResponsibilityDTOs;
using Hotel.Domain.Entities.ResponsibilityEntity;
using Hotel.Domain.Handlers.Base.Interfaces;
using Hotel.Domain.Repositories.Interfaces;

namespace Hotel.Domain.Handlers.y.ResponsibilityHandlers;

public partial class ResponsibilityHandler : IHandler
{
    private readonly IResponsibilityRepository _repository;
    private readonly ILogger<ResponsibilityHandler> _logger;

    public ResponsibilityHandler(IResponsibilityRepository repository, ILogger<ResponsibilityHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Response> HandleCreateAsync(EditorResponsibility model)
    {
        var responsibility = new Responsibility(model.Name, model.Description, model.Priority);

        try
        {
            await _repository.CreateAsync(responsibility);
            await _repository.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError($"Erro ao criar a responsabilidade no banco de dados. Erro: {e.Message}");
            throw;
        }

        return new Response("Responsabilidade criada com sucesso!", new { responsibility.Id });
    }
}