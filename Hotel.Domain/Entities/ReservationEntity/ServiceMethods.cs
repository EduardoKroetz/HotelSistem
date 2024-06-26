using Hotel.Domain.Entities.ServiceEntity;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Entities.ReservationEntity;

partial class Reservation
{
    public void AddService(Service service)
    {
        if (Status == Enums.EReservationStatus.CheckedOut)
            throw new InvalidOperationException("N�o � poss�vel adicionar servi�os pois a reserva j� foi finalizada");
        if (Status == Enums.EReservationStatus.Canceled)
            throw new InvalidOperationException("N�o � poss�vel adicionar servi�os pois a reserva foi cancelada");
        if (Status != Enums.EReservationStatus.CheckedIn)
            throw new InvalidOperationException("N�o � poss�vel adicionar servi�os antes do check-in");

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