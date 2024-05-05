using Hotel.Domain.Data;
using Hotel.Domain.Entities.PaymentContext.InvoiceRoomEntity;
using Hotel.Domain.Repositories.Interfaces;

namespace Hotel.Domain.Repositories;

public class RoomInvoiceRepository : GenericRepository<RoomInvoice> ,IRoomInvoiceRepository
{
  public RoomInvoiceRepository(HotelDbContext context) : base(context) {}

}