using Hotel.Domain.DTOs;
using Hotel.Domain.Entities.ResponsibilityEntity;

namespace Hotel.Domain.Handlers.y.ResponsibilityHandlers;

public partial class ResponsibilityHandler
{
    public async Task<Response> HandleDeleteAsync(Guid id)
    {
        _repository.Delete(id);

        try
        {
            await _repository.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError($"Erro ao deletar a responsabilidade {id} do banco de dados. Erro: {e.Message}");
            throw;
        }

        return new Response("Responsabilidade deletada com sucesso!", new { id });
    }
}