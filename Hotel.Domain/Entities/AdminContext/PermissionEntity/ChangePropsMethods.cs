namespace Hotel.Domain.Entities.AdminContext.PermissionEntity;

public partial class Permission 
{
  public void ChangeName(string name)
  => Name = name;

  public void ChangeDescription(string description)
  => Description = description;

  public void Enable()
  => IsActive = true;

  public void Disable()
  => IsActive = false;
}