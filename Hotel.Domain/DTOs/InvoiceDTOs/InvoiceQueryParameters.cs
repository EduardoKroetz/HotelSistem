using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.InvoiceDTOs;

public class InvoiceQueryParameters : IDataTransferObject
{
    public InvoiceQueryParameters(int? skip, int? take, string? paymentMethod, decimal? totalAmount, string? totalAmountOperator, Guid? customerId, Guid? reservationId, Guid? serviceId)
    {
        Skip = skip;
        Take = take;
        TotalAmount = totalAmount;
        PaymentMethod = paymentMethod;
        CustomerId = customerId;
        ReservationId = reservationId;
        ServiceId = serviceId;
        TotalAmountOperator = totalAmountOperator;
    }
    public int? Skip { get; private set; }
    public int? Take { get; private set; }
    public decimal? TotalAmount { get; private set; }
    public string? TotalAmountOperator { get; private set; }
    public string? PaymentMethod { get; private set; }
    public Guid? CustomerId { get; set; }
    public Guid? ReservationId { get; private set; }
    public Guid? ServiceId { get; private set; }
}
