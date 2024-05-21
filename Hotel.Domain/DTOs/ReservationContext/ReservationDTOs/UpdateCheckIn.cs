namespace Hotel.Domain.DTOs.ReservationContext.ReservationDTOs;

public class UpdateCheckIn
{
  public UpdateCheckIn(DateTime checkIn)
  => CheckIn = checkIn;

  public DateTime CheckIn { get; private set; }
}
