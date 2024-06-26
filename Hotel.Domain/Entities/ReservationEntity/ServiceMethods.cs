using Hotel.Domain.Entities.ServiceEntity;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Entities.ReservationEntity;

partial class Reservation
{
    public void AddService(Service service)
    {
        if (Status == Enums.EReservationStatus.CheckedOut)
            throw new InvalidOperationException("Não é possível adicionar serviços pois a reserva já foi finalizada");
        if (Status == Enums.EReservationStatus.Canceled)
            throw new InvalidOperationException("Não é possível adicionar serviços pois a reserva foi cancelada");
        if (Status != Enums.EReservationStatus.CheckedIn)
            throw new InvalidOperationException("Não é possível adicionar serviços antes do check-in");

        if (service.IsActive)
            Services.Add(service);
        else
            throw new ValidationException("Esse serviço está desativado.");
    }

    public void RemoveService(Service service)
    {
        if (!Services.Remove(service))
            throw new ArgumentException("Esse serviço não está atribuido a essa reserva.");
    }

}