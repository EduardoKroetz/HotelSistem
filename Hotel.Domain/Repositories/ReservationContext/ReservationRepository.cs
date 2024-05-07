using Hotel.Domain.Data;
using Hotel.Domain.DTOs.PaymentContext.RoomInvoiceDTOs;
using Hotel.Domain.Entities.ReservationContext.ReservationEntity;
using Hotel.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Repositories;

public class ReservationRepository :  GenericRepository<Reservation> ,IReservationRepository
{
  public ReservationRepository(HotelDbContext context) : base(context){}
  public async Task<GetReservation?> GetByIdAsync(Guid id)
  {
    return await _context
      .Reservations
      .AsNoTracking()
      .Where(x => x.Id == id)
      .Include(x => x.Customers)
      .Include(x => x.Services)
      .Select(x => new GetReservation(x.Id,x.DailyRate,x.HostedDays,x.CheckIn,x.CheckOut,x.Status,x.Capacity,x.RoomId,x.Customers,x.InvoiceId,x.Services))
      .FirstOrDefaultAsync();
    
  }
  public async Task<IEnumerable<GetReservationCollection>> GetAsync()
  {
    return await _context
      .Reservations
      .AsNoTracking()
      .Select(x => new GetReservationCollection(x.Id,x.DailyRate,x.HostedDays,x.CheckIn,x.CheckOut,x.Status,x.Capacity,x.RoomId,x.InvoiceId))
      .ToListAsync();
  }
}