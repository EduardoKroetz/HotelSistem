using Hotel.Domain.Entities.AdminContext.PermissionEntity;
using Hotel.Domain.Exceptions;


namespace Hotel.Domain.Entities.AdminContext.AdminEntity;

public partial class Admin 
{
  public void AddPermission(Permission permission)
  {
    if (Permissions.Contains(permission))
      throw new ValidationException("Erro de validação: Essa permissão já foi associada a esse administrador.");
    if (permission.IsActive)
      Permissions.Add(permission);
    else
      throw new ValidationException("Erro de validação: Essa permissão não está ativa.");

  }

  public void RemovePermission(Permission permission)
  {
    if (Permissions.Contains(permission))
      Permissions.Remove(permission);
    else
      throw new ValidationException("Erro de validação: Essa permissão não está associada a esse administrador.");
  }




}