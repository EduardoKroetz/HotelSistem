
using Hotel.Domain.Entities.AdminContext.PermissionEntity;
using Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;
using Hotel.Domain.Repositories.Interfaces.EmployeeContext;

namespace Hotel.Domain.Services.Permissions;

public class DefaultEmployeePermissions
{
  public static async Task HandleDefaultPermission(Permission permission, Employee employee, IEmployeeRepository employeeRepository)
  {
    DefaultPermission = DefaultPermission ?? await employeeRepository.GetDefaultPermission(); //permissão padrão, que define o conjunto de permissões padrões
    DefaultPermissions = DefaultPermissions ?? await employeeRepository.GetAllDefaultPermissions(); // busca todas as permissões padrões
 
    if (DefaultPermissions.Contains(permission) && employee.Permissions.Contains(DefaultPermission!)) //Se a permissão que deseja remover inclui nas permissões padrões
    {
      employee.UnassignPermission(DefaultPermission!); //remove a permissão padrão
      foreach (var defaultPermission in DefaultPermissions)
        employee.AssignPermission(defaultPermission); // e então adiciona todas essas permissões padrões
    }
  }

  public static List<string> PermissionsName { get; set; } =
  [
    "GetEmployee",
    "GetEmployees",
    "GetRoomInvoices",
    "GetRoomInvoice"
  ];

  public static Permission? DefaultPermission { get; set; } = null!;
  public static List<Permission> DefaultPermissions { get; set; } = null!;
}
