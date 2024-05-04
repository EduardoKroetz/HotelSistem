using Hotel.Domain.Entities.AdminContext.PermissionEntity;
using Hotel.Domain.Entities.Base.Interfaces;

namespace Hotel.Domain.Entities.Interfaces;

public interface IAdmin : IUser
{
  bool IsRootAdmin { get; }
  HashSet<Permission> Permissions { get; } 
}
