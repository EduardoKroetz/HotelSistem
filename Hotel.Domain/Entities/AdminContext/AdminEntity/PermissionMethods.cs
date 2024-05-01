using Hotel.Domain.Entities.AdminContext.PermissionEntity;
using Hotel.Domain.Exceptions;


namespace Hotel.Domain.Entities.AdminContext.AdminEntity;

public partial class Admin 
{
  public void AddPermission(Permission permission)
  {
    if (!permission.IsActive)
      Permissions.Add(permission);
    else
      throw new ValidationException("Essa permissão não está ativa.");
  }

  public void RemovePermission(Permission permission)
  => Permissions.Remove(permission);

}