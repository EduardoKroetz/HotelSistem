using Hotel.Domain.Entities.Base;

namespace Hotel.Domain.Entities.AdminContext.PermissionEntity;

public class Permission : Entity
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


  public void ChangeName(string name)
  => Name = name;

  public void ChangeDescription(string description)
  => Description = description;

  public void Enable()
  => IsActive = true;

  public void Disable()
  => IsActive = false;
}