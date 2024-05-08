using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.RoomContext.ServiceDTOs;

namespace Hotel.Domain.Handlers.RoomContext.ServiceHandler;

public partial class  ServiceHandler
{
    public async Task<Response<object>> HandleUpdateAsync(EditorService model, Guid id)
  {
    var service = await _repository.GetEntityByIdAsync(id);
    if (service == null)
      throw new ArgumentException("Serviço não encontrado.");

    service.ChangeName(model.Name);
    service.ChangePriority(model.Priority);
    service.ChangeTime(model.TimeInMinutes);
    service.ChangePrice(model.Price);

    _repository.Update(service);
    await _repository.SaveChangesAsync();

    return new Response<object>(200,"Serviço foi atualizado.",new { service.Id });
  }
}