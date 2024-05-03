using Hotel.Domain.Entities.RoomContext.ServiceEntity;

namespace Hotel.Domain.Entities.RoomContext.RoomEntity;

public partial class Room
{
  public void AddService(Service service)
  => Services.Add(service);
  
  public void RemoveService(Service service)
  => Services.Remove(service);

}