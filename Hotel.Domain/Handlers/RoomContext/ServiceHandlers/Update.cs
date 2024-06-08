using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.RoomContext.ServiceDTOs;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Handlers.RoomContext.ServiceHandler;

public partial class  ServiceHandler
{
  public async Task<Response> HandleUpdateAsync(EditorService model, Guid id)
  {
    var service = await _repository.GetEntityByIdAsync(id);
    if (service == null)
      throw new ArgumentException("Serviço não encontrado.");

    service.ChangeName(model.Name);
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
      if (e.InnerException != null && e.InnerException.ToString().Contains(model.Name))
        return new Response(400, "Esse nome já está cadastrado.");
      else
        return new Response(500, "Algum erro ocorreu ao salvar no banco de dados.");
    }


    return new Response(200,"Serviço atualizado com sucesso!.",new { service.Id });
  }
}