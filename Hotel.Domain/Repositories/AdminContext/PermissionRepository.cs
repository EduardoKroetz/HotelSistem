using Hotel.Domain.Data;
using Hotel.Domain.Entities.AdminContext.PermissionEntity;
using Hotel.Domain.Repositories.Interfaces;

namespace Hotel.Domain.Repositories;

public class PermisisionRepository : GenericRepository<Permission> ,IPermissionRepository
{
  public PermisisionRepository(HotelDbContext context) : base(context) {}
}