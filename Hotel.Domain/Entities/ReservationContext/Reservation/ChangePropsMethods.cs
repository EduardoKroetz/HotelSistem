namespace Hotel.Domain.Entities.ReservationContext.ReservationEntity;

partial class Reservation
{
  
  public void ChangeCheckOut(DateTime checkOut)
  {
    ValidateCheckOut(checkOut);
    CheckOut = checkOut;
  }

  public void ChangeCheckIn(DateTime checkIn)
  {
    ValidateCheckIn(checkIn);
    CheckIn = checkIn;
  }
}