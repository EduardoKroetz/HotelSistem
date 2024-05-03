using Hotel.Domain.Entities.RoomContext.ServiceEntity;

namespace Hotel.Domain.Entities.ReservationContext.ReservationEntity;

partial class Reservation
{
  public void AddService(Service service)
  => Services.Add(service);
  
}