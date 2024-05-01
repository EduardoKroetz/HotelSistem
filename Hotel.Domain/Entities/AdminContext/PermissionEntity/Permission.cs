using Hotel.Domain.Entities.Base;

namespace Hotel.Domain.Entities.AdminContext.PermissionEntity;

public partial class Permission : Entity
{
  public Permission(string name, string description)
  {
    Name = name;
    Description = description;
    IsActive = true;
  }

  public string Name { get; private set; }
  public string Description { get; private set; }
  public bool IsActive { get; private set; }
}