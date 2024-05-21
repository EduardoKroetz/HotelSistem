using Hotel.Domain.Enums;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Entities.ReservationContext.ReservationEntity;

partial class Reservation
{
  
  public Reservation ChangeCheckOut(DateTime checkOut)
  {
    ValidateCheckInAndCheckOut(CheckIn,checkOut);
    CheckOut = checkOut;
    HostedDays = CalculeHostedDays();
    return this;
  }

  public Reservation ChangeCheckIn(DateTime checkIn)
  {
    if (Status == EReservationStatus.Cancelled || Status == EReservationStatus.CheckedIn || Status == EReservationStatus.CheckedOut)
      throw new ValidationException($"Erro de validação: Não é possível alterar o CheckIn com o status da reserva {Status}.");
    ValidateCheckIn(checkIn);
    ValidateCheckInAndCheckOut(checkIn, CheckOut);
    CheckIn = checkIn;
    return this;
  }
}