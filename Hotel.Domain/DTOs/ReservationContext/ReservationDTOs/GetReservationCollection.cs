using Hotel.Domain.DTOs.Interfaces;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.ReservationContext.ReservationDTOs;

public class GetReservationCollection : IDataTransferObject
{
  public GetReservationCollection(Guid id, decimal dailyRate, int? hostedDays, DateTime checkIn, DateTime? checkOut, EReservationStatus status, int capacity, Guid roomId, Guid? invoiceId)
  {
    Id = id;
    DailyRate = dailyRate;
    HostedDays = hostedDays;
    CheckIn = checkIn;
    CheckOut = checkOut;
    Status = status;
    Capacity = capacity;
    RoomId = roomId;
    InvoiceId = invoiceId;
  }
  public Guid Id { get; private set; }
  public decimal DailyRate { get; private set; }
  public int? HostedDays { get; private set; }
  public DateTime CheckIn { get; private set; }
  public DateTime? CheckOut { get; private set; }
  public EReservationStatus Status { get; private set; }
  public int Capacity { get; private set; }
  public Guid RoomId { get; private set; }
  public Guid? InvoiceId { get; private set; }

}