using Hotel.Domain.DTOs.Interfaces;

namespace Hotel.Domain.DTOs.ReservationContext.ReservationDTOs;

public class CreateReservation : IDataTransferObject
{
  public CreateReservation(DateTime checkIn, DateTime? checkOut, Guid roomId, List<Guid> customers)
  {
    CheckIn = checkIn;
    CheckOut = checkOut;
    RoomId = roomId;
    Customers = customers;
  }

  public DateTime CheckIn { get; private set; }
  public DateTime? CheckOut { get; private set; }
  public Guid RoomId { get; private set; }
  public List<Guid> Customers { get; private set; }
}