using Hotel.Domain.Data;
using Hotel.Domain.DTOs.PermissionDTOs;
using Hotel.Domain.Entities.PermissionEntity;
using Hotel.Domain.Extensions;
using Hotel.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Repositories;

public class PermissionRepository : IPermissionRepository
{
    private readonly HotelDbContext _context;

    public PermissionRepository(HotelDbContext context)
    => _context = context;

    public async Task<Permission?> GetEntityByIdAsync(Guid id)
    {
        return await _context
          .Permissions
          .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<Permission>> GetEntitiesAsync()
    {
        return await _context
          .Permissions
          .AsNoTracking()
          .ToListAsync();
    }

    public async Task<GetPermission?> GetByIdAsync(Guid id)
    {
        return await _context
          .Permissions
          .AsNoTracking()
          .Where(x => x.Id == id)
          .Select(x => new GetPermission(x.Id, x.Name, x.Description, x.IsActive, x.CreatedAt))
          .FirstOrDefaultAsync();

    }

    public async Task<IEnumerable<GetPermission>> GetAsync(PermissionQueryParameters queryParameters)
    {
        var query = _context.Permissions.AsQueryable();

        if (queryParameters.Name != null)
            query = query.Where(x => x.Name.Contains(queryParameters.Name));

        if (queryParameters.IsActive != null)
            query = query.Where(x => x.IsActive == queryParameters.IsActive);

        if (queryParameters.AdminId != null)
            query = query.Where(x => x.Admins.Any(x => x.Id == queryParameters.AdminId));

        query = query.BaseQuery(queryParameters);

        return await query.Select(x => new GetPermission
        (
          x.Id,
          x.Name,
          x.Description,
          x.IsActive,
          x.CreatedAt
        )).ToListAsync();
    }
}