using Hotel.Domain.DTOs.Interfaces;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.PaymentContext.RoomInvoiceDTOs;

public class GetRoomInvoice : IDataTransferObject
{
  public GetRoomInvoice(string number,EPaymentMethod paymentMethod, Guid reservationId,  DateTime issueDate, decimal totalAmount, EStatus status, decimal taxInformation)
  {
    PaymentMethod = paymentMethod;
    TaxInformation = taxInformation;
    ReservationId = reservationId;
    Number = number;
    IssueDate = issueDate;
    TotalAmount = totalAmount;
    Status = status;
  }

  public EPaymentMethod PaymentMethod { get; private set; }
  public decimal TaxInformation { get; private set; }
  public Guid ReservationId { get; private set; }
  public string Number { get; private set; }
  public DateTime IssueDate { get; private set; }
  public decimal TotalAmount { get; private set; }
  public EStatus Status { get; private set; }

}
