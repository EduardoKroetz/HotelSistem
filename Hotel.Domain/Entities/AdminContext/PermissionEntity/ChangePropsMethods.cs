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
  {
    if (IsActive)
      throw new InvalidOperationException("Essa permiss�o j� est� habilitada.");
    IsActive = true;
  }

  public void Disable()
  {
    if (!IsActive)
      throw new InvalidOperationException("Essa permiss�o j� est� desabilitada.");
    IsActive = false;
  }

}