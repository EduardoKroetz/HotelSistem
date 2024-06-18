namespace Hotel.Domain.DTOs.ReservationDTOs;

public class UpdateCheckIn
{
    public UpdateCheckIn(DateTime checkIn)
    => CheckIn = checkIn;

    public DateTime CheckIn { get; private set; }
}
