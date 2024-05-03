namespace Hotel.Domain.Entities.AdminContext.PermissionEntity;

public partial class Permission 
{
  public void ChangeName(string name)
  {
    ValidateName(name);
    Name = name;
  }

  public void ChangeDescription(string description)
  {
    ValidateDescription(description);
    Description = description;
  }

  public void Enable()
  => IsActive = true;

  public void Disable()
  => IsActive = false;
}