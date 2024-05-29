using Hotel.Domain.DTOs.Interfaces;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.PaymentContext.RoomInvoiceDTOs;

public class RoomInvoiceQueryParameters : IDataTransferObject
{
  public RoomInvoiceQueryParameters(int? skip, int? take,string? number, EPaymentMethod? paymentMethod, decimal? totalAmount, string? totalAmountOperator, EStatus? status, Guid? customerId, Guid? reservationId, Guid? serviceId, decimal? taxInformation, string? taxInformationOperator, DateTime? issueDate, string? issueDateOperator) 
  {
    Skip = skip;
    Take = take;  
    Number = number;
    IssueDate = issueDate;
    TotalAmount = totalAmount;
    Status = status;
    PaymentMethod = paymentMethod;
    CustomerId = customerId;
    ReservationId = reservationId;
    ServiceId = serviceId;
    TaxInformation = taxInformation;
    IssueDateOperator = issueDateOperator;
    TotalAmountOperator = totalAmountOperator;
    TaxInformationOperator = taxInformationOperator;
  }
  public int? Skip { get; private set; }
  public int? Take { get; private set; }
  public string? Number { get; private set; }
  public DateTime? IssueDate { get; private set; }
  public string? IssueDateOperator { get; private set; }
  public decimal? TotalAmount { get; private set; }
  public string? TotalAmountOperator { get; private set; }
  public EStatus? Status { get; private set; }
  public EPaymentMethod? PaymentMethod { get; private set; }
  public Guid? CustomerId { get; set; }
  public Guid? ReservationId { get; private set; }
  public Guid? ServiceId { get; private set; }
  public decimal? TaxInformation { get; private set; }
  public string? TaxInformationOperator { get; private set; }
}
