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

  public static List<EPermissions> PermissionsName { get; set; } =
  [
    EPermissions.GetEmployee,
    EPermissions.GetEmployees,
    EPermissions.GetResponsabilities,
    EPermissions.GetResponsability,
    EPermissions.GetRoomInvoices,
    EPermissions.GetRoomInvoice,
    EPermissions.UpdateReservationCheckout,
    EPermissions.UpdateReservationCheckIn,
    EPermissions.AddServiceToReservation,
    EPermissions.GetServices,
    EPermissions.GetService,
    EPermissions.UpdateService,
    EPermissions.CreateService,
    EPermissions.DeleteService,
    EPermissions.AssignServiceResponsability,
    EPermissions.UnassignServiceResponsability,

    EPermissions.EditRoom,
    EPermissions.CreateRoom,
    EPermissions.DeleteRoom,
    EPermissions.AddRoomService,
    EPermissions.RemoveRoomService,
    EPermissions.UpdateRoomNumber,
    EPermissions.UpdateRoomCapacity,
    EPermissions.UpdateRoomCategory,
    EPermissions.UpdateRoomPrice,
    EPermissions.AvailableRoomStatus,

    EPermissions.GetReports,
    EPermissions.GetReport,
    EPermissions.EditReport,
    EPermissions.CreateReport,
    EPermissions.FinishReport,
    EPermissions.CreateCategory,
    EPermissions.EditCategory,
    EPermissions.EnableRoom,
    EPermissions.DisableRoom
  ];

  public static Permission? DefaultPermission { get; set; } = null!;
  public static IEnumerable<Permission> DefaultPermissions { get; set; } = null!;
}
