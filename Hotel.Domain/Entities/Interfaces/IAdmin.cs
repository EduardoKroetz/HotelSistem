using Hotel.Domain.Entities.Base.Interfaces;
using Hotel.Domain.Entities.PermissionEntity;

namespace Hotel.Domain.Entities.Interfaces;

public interface IAdmin : IUser
{
    bool IsRootAdmin { get; }
    ICollection<Permission> Permissions { get; }
}
