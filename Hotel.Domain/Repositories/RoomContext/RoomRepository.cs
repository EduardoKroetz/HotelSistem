using Hotel.Domain.Data;
using Hotel.Domain.DTOs.PaymentContext.RoomInvoiceDTOs;
using Hotel.Domain.Entities.RoomContext.RoomEntity;
using Hotel.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Repositories;

public class RoomRepository :  GenericRepository<Room> ,IRoomRepository
{
  public RoomRepository(HotelDbContext context) : base(context) {}

  public async Task<GetRoom?> GetByIdAsync(Guid id)
  {
    return await _context
      .Rooms
      .AsNoTracking()
      .Where(x => x.Id == id)
      .Include(x => x.Services)
      .Include(x => x.Category)
      .Include(x => x.Images)
      .Select(x => new GetRoom(
        x.Id,
        x.Number,
        x.Price,
        x.Status,
        x.Capacity,
        x.Description,
        x.Services,
        new GetCategory(x.CategoryId, x.Category!.Name ,x.Category!.Description,x.Category!.AveragePrice) ,
        x.Images))
      .FirstOrDefaultAsync();
  
  }
  public async Task<IEnumerable<GetRoomCollection>> GetAsync()
  {
    return await _context
      .Rooms
      .AsNoTracking()
      .Select(x => new GetRoomCollection(x.Id,x.Number,x.Price,x.Status,x.Capacity,x.Description,x.CategoryId))
      .ToListAsync();
  }

}