namespace Hotel.Domain.Entities.ReservationContext.ReservationEntity;

partial class Reservation
{
  public int? CalculeHostedDays()
  {
    if (CheckOut != null)
      return (CheckOut.Value.Date - CheckIn.Date).Days;
    return null;
  }

  public decimal CalculeDailyRate()
  => Room.Price * Capacity;
}