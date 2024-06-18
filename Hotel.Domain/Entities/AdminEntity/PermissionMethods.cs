using Hotel.Domain.Entities.PermissionEntity;
using Hotel.Domain.Exceptions;


namespace Hotel.Domain.Entities.AdminEntity;

public partial class Admin
{
    public void AddPermission(Permission permission)
    {
        if (Permissions.Contains(permission))
            throw new ValidationException("Essa permissão já foi associada a esse administrador.");
        if (permission.IsActive)
            Permissions.Add(permission);
        else
            throw new ValidationException("Essa permissão não está ativa.");

    }

    public void RemovePermission(Permission permission)
    {
        if (Permissions.Contains(permission))
            Permissions.Remove(permission);
        else
            throw new ValidationException("Essa permissão não está associada a esse administrador.");
    }




}