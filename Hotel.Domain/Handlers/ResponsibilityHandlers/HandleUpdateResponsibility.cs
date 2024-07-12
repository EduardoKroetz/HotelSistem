using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.ResponsibilityDTOs;
using Hotel.Domain.Entities.ResponsibilityEntity;

namespace Hotel.Domain.Handlers.y.ResponsibilityHandlers;

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

        try
        {
            _repository.Update(Responsibility);
            await _repository.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError($"Erro ao atualizar a responsabilidade {id} no banco de dados. Erro: {e.Message}");
            throw;
        }


        return new Response("Responsabilidade atualizada com sucesso!", new { Responsibility.Id });
    }
}