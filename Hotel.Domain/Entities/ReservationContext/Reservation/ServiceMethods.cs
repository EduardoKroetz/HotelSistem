using Hotel.Domain.Entities.RoomContext.ServiceEntity;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Entities.ReservationContext.ReservationEntity;

partial class Reservation
{
  public void AddService(Service service)
  {
    if (service.IsActive)
      Services.Add(service);
    else
      throw new ValidationException("Esse serviço está desativado.");
  }

  public void RemoveService(Service service)
  => Services.Remove(service);
  
}