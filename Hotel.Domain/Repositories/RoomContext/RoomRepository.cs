using Hotel.Domain.Data;
using Hotel.Domain.DTOs.PaymentContext.RoomInvoiceDTOs;
using Hotel.Domain.DTOs.RoomContext.RoomDTOs;
using Hotel.Domain.Entities.RoomContext.RoomEntity;
using Hotel.Domain.Extensions;
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


  public async Task<IEnumerable<GetRoomCollection>> GetAsync(RoomQueryParameters queryParameters)
  {
    var query = _context.Rooms.AsQueryable();

    if (queryParameters.Number.HasValue)
      query = query.FilterByOperator(queryParameters.NumberOperator, x => x.Number, queryParameters.Number);

    if (queryParameters.Price.HasValue)
      query = query.FilterByOperator(queryParameters.PriceOperator, x => x.Price, queryParameters.Price);

    if (queryParameters.Status.HasValue)
      query = query.Where(x => x.Status == queryParameters.Status);

    if (queryParameters.Capacity.HasValue)
      query = query.FilterByOperator(queryParameters.CapacityOperator, x => x.Capacity, queryParameters.Capacity);

    if (queryParameters.ServiceId.HasValue)
      query = query.Where(x => x.Services.Any(x => x.Id == queryParameters.ServiceId));

    if (queryParameters.CategoryId.HasValue)
      query = query.Where(x => x.CategoryId == queryParameters.CategoryId);

    query = query.BaseQuery(queryParameters);

    return await query.Select(x => new GetRoomCollection(
        x.Id,
        x.Number,
        x.Price,
        x.Status,
        x.Capacity,
        x.Description,
        x.CategoryId
    )).ToListAsync();
  }
}