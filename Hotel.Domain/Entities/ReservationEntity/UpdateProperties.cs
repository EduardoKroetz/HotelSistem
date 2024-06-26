using Hotel.Domain.Enums;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Entities.ReservationEntity;

partial class Reservation
{

    public Reservation UpdateExpectedCheckOut(DateTime expectedCheckOut)
    {
        if (Status == EReservationStatus.CheckedOut || Status == EReservationStatus.Canceled)
            throw new ValidationException($"Não é possível alterar o CheckOut esperado com o status da reserva {Status}.");

        ValidateCheckInAndCheckOut(CheckIn, expectedCheckOut);
        ExpectedCheckOut = expectedCheckOut;
        ExpectedTimeHosted = GetTimeHosted(ExpectedCheckIn, ExpectedCheckOut);
        return this;
    }

    public Reservation UpdateExpectedCheckIn(DateTime checkIn)
    {
        if (Status == EReservationStatus.Canceled || Status == EReservationStatus.CheckedIn || Status == EReservationStatus.CheckedOut)
            throw new ValidationException("Só é possível alterar o CheckIn esperado se o status for 'Pending' ou 'NoShow'.");

        ValidateCheckIn(checkIn);
        ValidateCheckInAndCheckOut(checkIn, ExpectedCheckOut);
        ExpectedCheckIn = checkIn;
        ExpectedTimeHosted = GetTimeHosted(checkIn, ExpectedCheckOut);
        return this;
    }

    private Reservation UpdateCheckOut(DateTime checkOut)
    {
        if (Status == EReservationStatus.CheckedOut || Status == EReservationStatus.Canceled)
            throw new ValidationException($"Não é possível alterar o CheckOut esperado com o status da reserva {Status}.");

        ValidateCheckInAndCheckOut(CheckIn, checkOut);
        CheckOut = checkOut;
        TimeHosted = GetTimeHosted();
        return this;
    }

    private Reservation UpdateCheckIn(DateTime checkIn)
    {
        if (Status == EReservationStatus.Canceled || Status == EReservationStatus.CheckedIn || Status == EReservationStatus.CheckedOut)
            throw new ValidationException($"Não é possível alterar o CheckIn com o status da reserva {Status}.");

        ValidateCheckIn(checkIn);
        ValidateCheckInAndCheckOut(checkIn, CheckOut);
        CheckIn = checkIn;
        TimeHosted = GetTimeHosted();
        return this;
    }
}