using Hotel.Domain.Entities.Base;

namespace Hotel.Domain.Entities.AdminContext;

public class Permission : Entity
{
  public Permission(string name, string description, bool isActive)
  {
    Name = name;
    Description = description;
    IsActive = isActive;
  }

  public string Name { get; private set; }
  public string Description { get; private set; }
  public bool IsActive { get; private set; }
}