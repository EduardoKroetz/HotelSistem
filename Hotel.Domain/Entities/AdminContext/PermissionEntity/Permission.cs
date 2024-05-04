using Hotel.Domain.Entities.Base;
using Hotel.Domain.Entities.Interfaces;

namespace Hotel.Domain.Entities.AdminContext.PermissionEntity;

public partial class Permission : Entity, IPermission
{
  public Permission(string name, string description)
  {
    Name = name;
    Description = description;
    IsActive = true;

    Validate();
  }

  public string Name { get; private set; }
  public string Description { get; private set; }
  public bool IsActive { get; private set; }
}