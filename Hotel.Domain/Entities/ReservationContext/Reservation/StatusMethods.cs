using Hotel.Domain.Enums;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Entities.ReservationContext.ReservationEntity;

partial class Reservation
{
  public Reservation StatusToCheckedIn()
  {
    if (CheckIn.Date != DateTime.Now.Date)
      throw new ValidationException("Erro de validação: A data de CheckIn não é a mesma da data de hoje, não é possível fazer CheckIn");

    Status = EReservationStatus.CheckedIn;
    Room?.ChangeStatus(ERoomStatus.Occupied);
    return this;
  }



  public Reservation StatusToNoShow()
  {
    if (CheckIn.Date < DateTime.Now.Date)
      throw new ValidationException("Erro de validação: A data de CheckIn não é a mesma da data de hoje.");

    Status = EReservationStatus.NoShow;
    Room?.ChangeStatus(ERoomStatus.Reserved);
    return this;
  }

  public Reservation StatusToCancelled()
  {
    if (DateTime.Now > CheckIn)
      throw new ValidationException("Erro de validação: A data de CheckIn já foi ultraprassada, não é possível cancelar a reserva.");

    Status = EReservationStatus.Cancelled;
    Room?.ChangeStatus(ERoomStatus.Available);
    return this;
  }
  
  private Reservation StatusToCheckedOut()
  {
    Status = EReservationStatus.CheckedOut;
    Room?.ChangeStatus(ERoomStatus.OutOfService);
    return this;
  }
}