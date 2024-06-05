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
      throw new ValidationException("Esse servi�o est� desativado.");
  }

  public void RemoveService(Service service)
  {
    if (!Services.Remove(service))
      throw new NotFoundException("Esse servi�o n�o est� atribuido a essa reserva.");
  }
  
}