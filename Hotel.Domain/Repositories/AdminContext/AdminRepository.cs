using Hotel.Domain.Data;
using Hotel.Domain.DTOs.AdminContext.AdminDTOs;
using Hotel.Domain.Entities.AdminContext.AdminEntity;
using Hotel.Domain.Entities.AdminContext.PermissionEntity;
using Hotel.Domain.Repositories.Base;
using Hotel.Domain.Repositories.Interfaces.AdminContext;
using Hotel.Domain.Services.Permissions;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Repositories.AdminContext;

public class AdminRepository : UserRepository<Admin>, IAdminRepository
{
  public AdminRepository(HotelDbContext context) : base(context) { }

  public async Task<IEnumerable<GetAdmin>> GetAsync(AdminQueryParameters queryParameters)
  {
    var query = base.GetAsync(queryParameters);

    if (queryParameters.IsRootAdmin.HasValue)
      query = query.Where(x => x.IsRootAdmin == queryParameters.IsRootAdmin);

    if (queryParameters.PermissionId != null)
      query = query.Where(x => x.Permissions.Any(y => y.Id == queryParameters.PermissionId));

    return await query.Select(x => new GetAdmin
    (
      x.Id,
      x.Name.FirstName,
      x.Name.LastName,
      x.Email.Address,
      x.Phone.Number,
      x.IsRootAdmin,
      x.Gender,
      x.DateOfBirth,
      x.Address,
      x.CreatedAt
    )).ToListAsync();
  }

  public async Task<Admin?> GetAdminIncludePermissions(Guid adminId)
  {
    return await _context.Admins
      .Where(x => x.Id == adminId)
      .Include(x => x.Permissions)
      .FirstOrDefaultAsync();
  }

  new public async Task<Admin?> GetEntityByEmailAsync(string email)
  {
    return await _context
      .Admins
      .AsNoTracking()
      .Include(x => x.Permissions)
      .Where(x => x.Email.Address == email)
      .FirstOrDefaultAsync();
  }

  public async Task<List<Permission>> GetAllDefaultPermissions()
  {
    var permissions = await _context.Permissions.ToListAsync();

    return permissions
      .Where(p => DefaultAdminPermissions.PermissionsName
          .Any(pEnum => p.Name.Contains(pEnum.ToString())))
      .ToList();
  }

  public async Task<Permission?> GetDefaultAdminPermission()
  {
    return await _context.Permissions
      .FirstOrDefaultAsync(x => x.Name.Contains("DefaultAdminPermission"));
  }
}