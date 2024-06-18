using Hotel.Domain.Entities.ServiceEntity;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Entities.ReservationEntity;

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
            throw new ArgumentException("Esse servi�o n�o est� atribuido a essa reserva.");
    }

}