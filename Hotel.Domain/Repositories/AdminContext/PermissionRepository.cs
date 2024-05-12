using Hotel.Domain.Data;
using Hotel.Domain.DTOs.AdminContext.PermissionDTOs;
using Hotel.Domain.Entities.AdminContext.PermissionEntity;
using Hotel.Domain.Extensions;
using Hotel.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Hotel.Domain.Repositories;

public class PermissionRepository : GenericRepository<Permission> ,IPermissionRepository
{
  public PermissionRepository(HotelDbContext context) : base(context) {}

   public async Task<GetPermission?> GetByIdAsync(Guid id)
  {
    return await _context
      .Permissions
      .AsNoTracking()
      .Where(x => x.Id == id)
      .Select(x => new GetPermission(x.Id, x.Name, x.Description, x.IsActive))
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
      x.IsActive
    )).ToListAsync();
  }
}