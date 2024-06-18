namespace Hotel.Domain.Entities.ReservationEntity;

partial class Reservation
{
    public static TimeSpan? GetTimeHosted(DateTime? checkIn, DateTime? checkOut)
    {
        if (checkIn == null)
            throw new ArgumentNullException("CheckIn inválido.");
        if (checkOut == null)
            throw new ArgumentNullException("CheckOut inválido.");

        return checkOut - checkIn;
    }

    public TimeSpan GetTimeHosted(DateTime checkIn, DateTime checkOut)
      => (TimeSpan)GetTimeHosted((DateTime?)checkIn, (DateTime?)checkOut)!;

    public TimeSpan? GetTimeHosted()
    {
        return GetTimeHosted(CheckIn, CheckOut);
    }

}