
namespace Hotel.Domain.DTOs.InvoiceDTOs;

public class InvoiceQueryParameters : IDataTransferObject
{
    public int? Skip { get; set; }
    public int? Take { get; set; }
    public decimal? TotalAmount { get; set; }
    public string? TotalAmountOperator { get; set; }
    public string? PaymentMethod { get; set; }
    public Guid? CustomerId { get; set; }
    public Guid? ReservationId { get; set; }
    public Guid? ServiceId { get; set; }
}
