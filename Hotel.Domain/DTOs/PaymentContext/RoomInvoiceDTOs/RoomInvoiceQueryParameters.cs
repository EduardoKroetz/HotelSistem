using Hotel.Domain.DTOs.Base;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.PaymentContext.RoomInvoiceDTOs;

public class RoomInvoiceQueryParameters : QueryParameters
{
  public RoomInvoiceQueryParameters(int? skip, int? take, DateTime? createdAt, string? createdAtOperator,string? number, DateTime? issueDate, string? issueDateOperator, decimal? totalAmount, string? totalAmountOperator, EStatus? status, EPaymentMethod? paymentMethod, Guid? customerId, Guid? reservationId, Guid? serviceId, decimal? taxInformation, string? taxInformationOperator) : base(skip, take, createdAt, createdAtOperator)
  {
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

  public string? Number { get; private set; }
  public DateTime? IssueDate { get; private set; }
  public string? IssueDateOperator { get; private set; }
  public decimal? TotalAmount { get; private set; }
  public string? TotalAmountOperator { get; private set; }
  public EStatus? Status { get; private set; }
  public EPaymentMethod? PaymentMethod { get; private set; }
  public Guid? CustomerId { get; private set; }
  public Guid? ReservationId { get; private set; }
  public Guid? ServiceId { get; private set; }
  public decimal? TaxInformation { get; private set; }
  public string? TaxInformationOperator { get; private set; }
}
