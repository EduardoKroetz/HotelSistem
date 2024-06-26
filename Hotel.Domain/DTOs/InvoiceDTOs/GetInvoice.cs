
using Hotel.Domain.DTOs.ServiceDTOs;

namespace Hotel.Domain.DTOs.InvoiceDTOs;

public class GetInvoice : IDataTransferObject
{
    public GetInvoice(Guid id, string paymentMethod, Guid reservationId, decimal totalAmount, Guid customerId, IEnumerable<GetService> services)
    {
        Id = id;
        PaymentMethod = paymentMethod;
        ReservationId = reservationId;
        TotalAmount = totalAmount;
        CustomerId = customerId;
        Services = services;
    }

    public Guid? Id { get; private set; }
    public string PaymentMethod { get; private set; }
    public Guid ReservationId { get; private set; }
    public decimal TotalAmount { get; private set; }
    public Guid CustomerId { get; private set; }
    public IEnumerable<GetService> Services { get; private set; }

}
