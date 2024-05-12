using Hotel.Domain.Data;
using Hotel.Domain.DTOs.AdminContext.AdminDTOs;
using Hotel.Domain.Entities.AdminContext.AdminEntity;
using Hotel.Domain.Extensions;
using Hotel.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Repositories;

public class AdminRepository : UserRepository<Admin> ,IAdminRepository
{
  public AdminRepository(HotelDbContext context) : base(context) {}

  public async Task<IEnumerable<GetAdmin>> GetAsync(AdminQueryParameters queryParameters)
  {
    var query = base.GetAsync(queryParameters);

    if (queryParameters.IsRootAdmin.HasValue)
      query = query.Where(x => x.IsRootAdmin == queryParameters.IsRootAdmin);

    if (queryParameters.PermissionId != null)
      query = query.Where(x => x.Permissions.Any(y => y.Id == queryParameters.PermissionId));

    query = query.BaseQuery(queryParameters);
    
    return await query.Select(x => new GetAdmin
    (
      x.Id,
      x.Name.FirstName,
      x.Name.LastName,
      x.Email.Address,
      x.Phone.Number,
      x.IsRootAdmin,
      x.CreatedAt
    )).ToListAsync();
  }

}