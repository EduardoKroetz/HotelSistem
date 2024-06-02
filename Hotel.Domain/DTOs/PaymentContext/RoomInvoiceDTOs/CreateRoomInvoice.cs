using Hotel.Domain.DTOs.Interfaces;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.PaymentContext.RoomInvoiceDTOs;

public class CreateRoomInvoice : IDataTransferObject
{

  public CreateRoomInvoice(EPaymentMethod paymentMethod,Guid reservationId, decimal taxInformation = 0)
  {
    PaymentMethod = paymentMethod;
    TaxInformation = taxInformation;
    ReservationId = reservationId;
  }
  public EPaymentMethod PaymentMethod { get; private set; }
  public decimal TaxInformation { get; private set; }
  public Guid ReservationId { get; private set; }

}