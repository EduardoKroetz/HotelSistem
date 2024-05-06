using Hotel.Domain.Data;
using Hotel.Domain.DTOs.AdminContext.PermissionDTOs;
using Hotel.Domain.Entities.AdminContext.PermissionEntity;
using Hotel.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Repositories;

public class PermisisionRepository : GenericRepository<Permission> ,IPermissionRepository
{
  public PermisisionRepository(HotelDbContext context) : base(context) {}

   public async Task<GetPermission?> GetByIdAsync(Guid id)
  {
    return await _context
      .Permissions
      .AsNoTracking()
      .Where(x => x.Id == id)
      .Select(x => new GetPermission(x.Id, x.Name, x.Description, x.IsActive))
      .FirstOrDefaultAsync();
  
  }
  public async Task<IEnumerable<GetPermission>> GetAsync()
  {
    return await _context
      .Permissions
      .AsNoTracking()
      .Select(x => new GetPermission(x.Id, x.Name, x.Description, x.IsActive))
      .ToListAsync();
  }
}