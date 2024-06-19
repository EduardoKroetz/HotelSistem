using Hotel.Domain.Data;
using Hotel.Domain.DTOs.RoomDTOs;
using Hotel.Domain.Entities.RoomEntity;
using Hotel.Domain.Extensions;
using Hotel.Domain.Repositories.Base;
using Hotel.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Repositories;

public class RoomRepository : GenericRepository<Room>, IRoomRepository
{
    public RoomRepository(HotelDbContext context) : base(context) { }

    public async Task<GetRoom?> GetByIdAsync(Guid id)
    {
        return await _context
            .Rooms
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Include(x => x.Images)
            .Select(x => new GetRoom(
                x.Id,
                x.Name,
                x.Number,
                x.Price,
                x.Status,
                x.Capacity,
                x.Description,
                x.CategoryId,
                x.Images,
                x.CreatedAt))
          .FirstOrDefaultAsync();

    }


    public async Task<IEnumerable<GetRoom>> GetAsync(RoomQueryParameters queryParameters)
    {
        var query = _context.Rooms.AsQueryable();

        if (queryParameters.Name != null)
            query = query.Where(x => x.Name.Contains(queryParameters.Name));

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

        return await query
            .Include(x => x.Images)
            .Select(x => new GetRoom(
                x.Id,
                x.Name,
                x.Number,
                x.Price,
                x.Status,
                x.Capacity,
                x.Description,
                x.CategoryId,
                x.Images,
                x.CreatedAt
        )).ToListAsync();
    }

    public async Task<Room?> GetRoomIncludesServices(Guid roomId)
    {
        return await _context.Rooms
          .Where(x => x.Id == roomId)
          .Include(x => x.Services)
          .FirstOrDefaultAsync();
    }

    public async Task<Room?> GetRoomIncludesReservations(Guid roomId)
    {
        return await _context.Rooms
          .Include(x => x.Reservations)
          .FirstOrDefaultAsync(x => x.Id == roomId);
    }
}