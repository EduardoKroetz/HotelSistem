using Hotel.Domain.Entities.RoomContext.ServiceEntity;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Entities.RoomContext.RoomEntity;

public partial class Room
{
  public void AddService(Service service)
  {
    if (Services.Contains(service))
      throw new ValidationException("Erro de validação: Esse serviço já foi adicionado.");
    Services.Add(service);
  }
  
  public void RemoveService(Service service)
  {
    if (!Services.Remove(service))
      throw new ValidationException("Erro de validação: Serviço não foi adicionado.");
  }


}