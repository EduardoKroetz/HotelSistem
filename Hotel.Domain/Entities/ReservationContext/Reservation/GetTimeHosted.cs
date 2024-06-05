namespace Hotel.Domain.Entities.ReservationContext.ReservationEntity;

partial class Reservation
{
  public static TimeSpan? GetTimeHosted(DateTime checkIn, DateTime? checkOut)
  {
    return checkOut - checkIn;
  }

  public TimeSpan? GetTimeHosted()
  {
    return GetTimeHosted(CheckIn, CheckOut);
  }
}