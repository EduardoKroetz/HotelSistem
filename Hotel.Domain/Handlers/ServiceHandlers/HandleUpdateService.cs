using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.ServiceDTOs;
using Hotel.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Handlers.ServiceHandler;

public partial class ServiceHandler
{
    public async Task<Response> HandleUpdateAsync(EditorService model, Guid id)
    {
        var service = await _repository.GetEntityByIdAsync(id)
        ?? throw new NotFoundException("Serviço não encontrado.");

        service.ChangeName(model.Name);
        service.ChangeDescription(model.Description);
        service.ChangePriority(model.Priority);
        service.ChangeTime(model.TimeInMinutes);
        service.ChangePrice(model.Price);

        try
        {
            _repository.Update(service);
            await _repository.SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
            if (e.InnerException != null && e.InnerException.ToString().Contains("Name"))
                throw new ArgumentException("Esse nome já está cadastrado.");
            else
                throw new Exception("Algum erro ocorreu ao salvar no banco de dados.");
        }

        return new Response("Serviço atualizado com sucesso!", new { service.Id });
    }
}