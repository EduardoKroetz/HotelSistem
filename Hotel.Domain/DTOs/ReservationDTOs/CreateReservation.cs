namespace Hotel.Domain.DTOs.ReservationDTOs;

public class CreateReservation : IDataTransferObject
{
    public CreateReservation(DateTime expectedCheckIn, DateTime expectedCheckOut, Guid roomId, int capacity)
    {
        ExpectedCheckIn = expectedCheckIn;
        ExpectedCheckOut = expectedCheckOut;
        RoomId = roomId;
        Capacity = capacity;
    }

    public DateTime ExpectedCheckIn { get; private set; }
    public DateTime ExpectedCheckOut { get; private set; }
    public Guid RoomId { get; private set; }
    public int Capacity { get; private set; }
}