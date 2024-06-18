using Hotel.Domain.Enums;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Entities.ReservationEntity;

partial class Reservation
{
    public Reservation ToCheckIn()
    {
        CheckIn = DateTime.Now;

        Status = EReservationStatus.CheckedIn;
        Room?.ChangeStatus(ERoomStatus.Occupied);
        return this;
    }

    public Reservation ToNoShow()
    {
        if (ExpectedCheckIn.Date < DateTime.Now.Date)
            throw new ValidationException("A data de CheckIn esperado deve ser ultrapassada para alterar o status");

        Status = EReservationStatus.NoShow;
        Room?.ChangeStatus(ERoomStatus.Reserved);
        return this;
    }

    public Reservation ToCancelled()
    {
        if (Status != EReservationStatus.Pending && Status != EReservationStatus.NoShow)
            throw new InvalidOperationException($"Não é possível cancelar a reserva com o status {Status}");

        if (DateTime.Now > CheckIn)
            throw new ValidationException("A data de CheckIn esperado já foi ultraprassada, não é possível cancelar a reserva.");

        Status = EReservationStatus.Cancelled;
        Room?.ChangeStatus(ERoomStatus.OutOfService);
        return this;
    }

    private Reservation ToCheckOut()
    {
        Status = EReservationStatus.CheckedOut;
        Room?.ChangeStatus(ERoomStatus.OutOfService);

        return this;
    }
}