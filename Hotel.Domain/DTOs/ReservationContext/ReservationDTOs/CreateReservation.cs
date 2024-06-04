using Hotel.Domain.DTOs.Interfaces;

namespace Hotel.Domain.DTOs.ReservationContext.ReservationDTOs;

public class CreateReservation : IDataTransferObject
{
  public CreateReservation(DateTime checkIn, DateTime? checkOut, Guid roomId, int capacity)
  {
    CheckIn = checkIn;
    CheckOut = checkOut;
    RoomId = roomId;
    Capacity = capacity;
  }

  public DateTime CheckIn { get; private set; }
  public DateTime? CheckOut { get; private set; }
  public Guid RoomId { get; private set; }
  public int Capacity { get; private set; }
}