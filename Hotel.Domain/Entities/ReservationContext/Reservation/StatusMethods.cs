using Hotel.Domain.Enums;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Entities.ReservationContext.ReservationEntity;

partial class Reservation
{
  public void CheckInStatus()
  {
    if (CheckIn.Date != DateTime.Now.Date)
      throw new ValidationException("A data de CheckIn não é a mesma da data de hoje, não é possível fazer CheckIn");

    Status = EReservationStatus.CheckedIn;
    Room.ChangeStatus(ERoomStatus.Occupied);
  }



  public void NoShowStatus()
  {
    if (CheckIn.Date < DateTime.Now.Date)
      throw new ValidationException("A data de CheckIn não é a mesma da data de hoje.");

    Status = EReservationStatus.NoShow;
    Room.ChangeStatus(ERoomStatus.Reserved);
  }

  public void CancelStatus()
  {
    if (DateTime.Now > CheckIn)
      throw new ValidationException("A data de CheckIn já foi ultraprassada, não é possível cancelar a reserva.");

    Status = EReservationStatus.Cancelled;
    Room.ChangeStatus(ERoomStatus.Available);
  }
  
  public void CheckOutStatus()
  {
    Status = EReservationStatus.CheckedOut;
    Room.ChangeStatus(ERoomStatus.OutOfService);
  }
}