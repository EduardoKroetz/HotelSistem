using Hotel.Domain.DTOs.Interfaces;
using Hotel.Domain.Entities.CustomerContext;
using Hotel.Domain.Entities.RoomContext.ServiceEntity;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.PaymentContext.RoomInvoiceDTOs;

public class GetReservationCollection : IDataTransferObject
{
  public GetReservationCollection(Guid id,decimal dailyRate, int? hostedDays, DateTime checkIn, DateTime? checkOut, EReservationStatus status, int capacity, Guid roomId, Guid? invoiceId)
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
  public Guid Id { get; set; }
  public decimal DailyRate { get; set; }
  public int? HostedDays { get; set; }
  public DateTime CheckIn { get; set; }
  public DateTime? CheckOut { get;set; }
  public EReservationStatus Status { get; set; }
  public int Capacity { get; set; }
  public Guid RoomId { get; set; }
  public Guid? InvoiceId { get; set; }

}