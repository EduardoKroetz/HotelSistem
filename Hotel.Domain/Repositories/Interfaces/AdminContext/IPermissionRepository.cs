using Hotel.Domain.Entities.AdminContext.PermissionEntity;

namespace Hotel.Domain.Repositories.Interfaces;

public interface IPermissionRepository : IRepository<Permission>
{
  public void Enable(Permission permission);
  public void Disable(Permission permission);
}