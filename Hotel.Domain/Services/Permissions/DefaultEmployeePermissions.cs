using Hotel.Domain.Entities.AdminContext.PermissionEntity;
using Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;
using Hotel.Domain.Enums;
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
    EPermissions.GetEmployee.ToString(),
    EPermissions.GetEmployees.ToString(),
    EPermissions.GetResponsabilities.ToString(),
    EPermissions.GetResponsability.ToString(),
    EPermissions.GetRoomInvoices.ToString(),
    EPermissions.GetRoomInvoice.ToString(),
    EPermissions.GetReservations.ToString(),
    EPermissions.UpdateReservationCheckout.ToString(),
    EPermissions.UpdateReservationCheckIn.ToString(),
    EPermissions.AddServiceToReservation.ToString(),
    EPermissions.GetServices.ToString(),
    EPermissions.GetService.ToString(),
    EPermissions.UpdateService.ToString(),
    EPermissions.CreateService.ToString(),
    EPermissions.DeleteService.ToString(),
    EPermissions.AssignServiceResponsability.ToString(),
    EPermissions.UnassignServiceResponsability.ToString()
    EPermissions.EditRoom.ToString(),
    EPermissions.CreateRoom.ToString(),
    EPermissions.DeleteRoom.ToString(),
    EPermissions.AddServiceToRoom.ToString(),
    EPermissions.RemoveServiceToRoom.ToString(),
    EPermissions.UpdateRoomNumber.ToString(),
    EPermissions.UpdateRoomCapacity.ToString(),
    EPermissions.UpdateRoomCategory.ToString(),
    EPermissions.UpdateRoomPrice.ToString(),
    EPermissions.GetReports.ToString(),
    EPermissions.GetReport.ToString(),
    EPermissions.EditReport.ToString(),
    EPermissions.CreateReport.ToString(),
    EPermissions.FinishReport.ToString(),
    EPermissions.CreateCategory.ToString(),
    EPermissions.EditCategory.ToString()
  ];

  public static Permission? DefaultPermission { get; set; } = null!;
  public static List<Permission> DefaultPermissions { get; set; } = null!;
}
