using Hotel.Domain.Entities.AdminContext.AdminEntity;
using Hotel.Domain.Entities.AdminContext.PermissionEntity;

namespace Hotel.Domain.Repositories.Interfaces;

public interface IAdminRepository : IRepository<Admin>
{
  public void AddPermission(Permission Permission);
  public void RemovePermission(Permission Permission);
}