using Hotel.Domain.DTOs.Interfaces;

namespace Hotel.Domain.DTOs.PaymentContext.RoomInvoiceDTOs;

public class CreateReservation : IDataTransferObject
{
  public CreateReservation(DateTime checkIn,DateTime? checkOut,  Guid roomId, List<Guid> customers)
  {
    CheckIn = checkIn;
    CheckOut = checkOut;
    RoomId = roomId;
    Customers = customers;
  }

  public DateTime CheckIn { get; set; }
  public DateTime? CheckOut { get; set; }
  public Guid RoomId { get; set; }
  public List<Guid> Customers { get; set; }
}