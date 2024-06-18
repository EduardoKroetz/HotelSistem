namespace Hotel.Domain.DTOs.ReservationDTOs;

public class UpdateCheckOut
{
    public UpdateCheckOut(DateTime checkOut)
    => CheckOut = checkOut;

    public DateTime CheckOut { get; private set; }
}
