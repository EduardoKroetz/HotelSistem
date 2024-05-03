using Hotel.Domain.Entities.AdminContext.PermissionEntity;
using Hotel.Domain.Exceptions;


namespace Hotel.Domain.Entities.AdminContext.AdminEntity;

public partial class Admin 
{
  public void AddPermission(Permission permission)
  {
    if (Permissions.Contains(permission))
      throw new ValidationException("Erro de validação: Essa permissão já foi adicionada.");

    if (permission.IsActive && permission.IsValid)
      Permissions.Add(permission);
    else
      throw new ValidationException("Erro de validação: Essa permissão não está ativa ou é inválida.");
  }

  public void RemovePermission(Permission permission)
  {
    if (!Permissions.Remove(permission))
      throw new ValidationException("Erro de validação: Permissão não encontrada.");
  }


}