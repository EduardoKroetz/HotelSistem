using Hotel.Domain.Entities.PermissionEntity;
using Hotel.Domain.Exceptions;


namespace Hotel.Domain.Entities.EmployeeEntity;

public partial class Employee
{
    public void AssignPermission(Permission permission)
    {
        if (Permissions.Contains(permission))
            throw new ValidationException("Essa permissão já foi associada a esse funcionário.");
        if (permission.IsActive)
            Permissions.Add(permission);
        else
            throw new ValidationException("Essa permissão não está ativa.");

    }

    public void UnassignPermission(Permission permission)
    {
        if (Permissions.Contains(permission))
            Permissions.Remove(permission);
        else
            throw new ValidationException("Essa permissão não está associada a esse funcionário.");
    }

}