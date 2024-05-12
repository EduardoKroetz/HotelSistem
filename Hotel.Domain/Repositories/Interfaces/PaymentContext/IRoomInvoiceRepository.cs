using Hotel.Domain.DTOs.PaymentContext.RoomInvoiceDTOs;
using Hotel.Domain.Entities.PaymentContext.InvoiceRoomEntity;

namespace Hotel.Domain.Repositories.Interfaces;

public interface IRoomInvoiceRepository : IRepository<RoomInvoice>, IRepositoryQuery<GetRoomInvoice,RoomInvoiceQueryParameters>
{
}