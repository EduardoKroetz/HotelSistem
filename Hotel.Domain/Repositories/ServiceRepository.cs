using Hotel.Domain.Data;
using Hotel.Domain.DTOs.ServiceDTOs;
using Hotel.Domain.Entities.ServiceEntity;
using Hotel.Domain.Extensions;
using Hotel.Domain.Repositories.Base;
using Hotel.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Repositories;

public class ServiceRepository : GenericRepository<Service>, IServiceRepository
{
    public ServiceRepository(HotelDbContext context) : base(context) { }

    public async Task<GetService?> GetByIdAsync(Guid id)
    {
        return await _context
          .Services
          .AsNoTracking()
          .Where(x => x.Id == id)
          .Include(x => x.Responsibilities)
          .Select(x => new GetService(
            x.Id,
            x.Name,
            x.Price,
            x.Priority,
            x.IsActive,
            x.TimeInMinutes,
            x.CreatedAt
          ))
          .FirstOrDefaultAsync();

    }

    public async Task<IEnumerable<GetService>> GetAsync(ServiceQueryParameters queryParameters)
    {
        var query = _context.Services.AsQueryable();

        if (queryParameters.Name != null)
            query = query.Where(x => x.Name.Contains(queryParameters.Name));

        if (queryParameters.Price.HasValue)
            query = query.FilterByOperator(queryParameters.PriceOperator, x => x.Price, queryParameters.Price);

        if (queryParameters.Priority.HasValue)
            query = query.Where(x => x.Priority == queryParameters.Priority);

        if (queryParameters.IsActive.HasValue)
            query = query.Where(x => x.IsActive == queryParameters.IsActive);

        if (queryParameters.TimeInMinutes.HasValue)
            query = query.FilterByOperator(queryParameters.TimeInMinutesOperator, x => x.TimeInMinutes, queryParameters.TimeInMinutes);

        if (queryParameters.ResponsibilityId.HasValue)
            query = query.Where(x => x.Responsibilities.Any(y => y.Id == queryParameters.ResponsibilityId));

        if (queryParameters.ReservationId.HasValue)
            query = query.Where(x => x.Reservations.Any(y => y.Id == queryParameters.ReservationId));

        if (queryParameters.InvoiceId.HasValue)
            query = query.Where(x => x.Invoices.Any(y => y.Id == queryParameters.InvoiceId));

        if (queryParameters.RoomId.HasValue)
            query = query.Where(x => x.Rooms.Any(y => y.Id == queryParameters.RoomId));

        query = query.BaseQuery(queryParameters);

        return await query.Select(x => new GetService(
            x.Id,
            x.Name,
            x.Price,
            x.Priority,
            x.IsActive,
            x.TimeInMinutes,
            x.CreatedAt
        )).ToListAsync();
    }

    public async Task<Service?> GetServiceIncludeResponsibilities(Guid serviceId)
    {
        return await _context.Services
          .Where(x => x.Id == serviceId)
          .Include(x => x.Responsibilities)
          .FirstOrDefaultAsync();
    }

    public async Task<ICollection<Service>> GetServicesByListId(ICollection<Guid> servicesIds)
    {
        return await _context.Services
          .Where(x => servicesIds.Contains(x.Id))
          .ToListAsync();
    }
}