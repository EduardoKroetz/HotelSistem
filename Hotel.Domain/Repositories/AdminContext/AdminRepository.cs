using Hotel.Domain.Data;
using Hotel.Domain.DTOs.AdminContext.AdminDTOs;
using Hotel.Domain.Entities.AdminContext.AdminEntity;
using Hotel.Domain.Repositories.Interfaces;

namespace Hotel.Domain.Repositories;

public class AdminRepository : UserRepository<Admin> ,IAdminRepository
{
  public AdminRepository(HotelDbContext context) : base(context) {}

  public async Task<IEnumerable<GetAdmin>> Query(AdminQuery queryParameters)
  {
    var admins = await base.Query(queryParameters);
    if (queryParameters.IsRootAdmin.HasValue)
      admins.Where(x => x.IsRootAdmin == queryParameters.IsRootAdmin);

    var result = admins.Select(x => new GetAdmin
    (
      x.Id,
      x.Name.FirstName,
      x.Name.LastName,
      x.Email.Address,
      x.Phone.Number,
      x.IsRootAdmin,
      x.CreatedAt
    )).Skip(queryParameters.Skip ?? 0).Take(queryParameters.Take ?? 0);

    return result;
  }
}