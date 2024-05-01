using Hotel.Domain.Entities.RoomContext.ServiceEntity;

namespace Hotel.Domain.Entities.RoomContext.RoomEntity;

public partial class Room
{
  public void AddService(Service service)
  {
    service.Validate();
    if (Services.Contains(service))
      throw new ArgumentException("Esse serviço já está atribuido à este quarto.");
    Services.Add(service);
  }

  
  public void RemoveService(Service service)
   {
    if ( !Services.Contains(service))
      throw new ArgumentException("Esse serviço não está atribuido à este quarto.");
    Services.Remove(service);
  }
}