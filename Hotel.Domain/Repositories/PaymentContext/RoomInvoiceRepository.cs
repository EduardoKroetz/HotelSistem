using Hotel.Domain.Data;
using Hotel.Domain.DTOs.PaymentContext.RoomInvoiceDTOs;
using Hotel.Domain.Entities.PaymentContext.InvoiceRoomEntity;
using Hotel.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Repositories;

public class RoomInvoiceRepository : GenericRepository<RoomInvoice> ,IRoomInvoiceRepository
{
  public RoomInvoiceRepository(HotelDbContext context) : base(context) {}


  public async Task<GetRoomInvoice?> GetByIdAsync(Guid id)
  {
    return await _context
      .RoomInvoices
      .AsNoTracking()
      .Where(x => x.Id == id)
      .Select(x => new GetRoomInvoice(x.Number,x.PaymentMethod,x.ReservationId,x.IssueDate,x.TotalAmount,x.Status,x.TaxInformation))
      .FirstOrDefaultAsync();
    
  }
  public async Task<IEnumerable<GetRoomInvoice>> GetAsync()
  {
    return await _context
      .RoomInvoices
      .AsNoTracking()
      .Select(x => new GetRoomInvoice(x.Number,x.PaymentMethod,x.ReservationId,x.IssueDate,x.TotalAmount,x.Status,x.TaxInformation))
      .ToListAsync();
  }
}