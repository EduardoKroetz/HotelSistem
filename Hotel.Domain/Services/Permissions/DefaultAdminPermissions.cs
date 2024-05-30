using Hotel.Domain.Entities.AdminContext.AdminEntity;
using Hotel.Domain.Entities.AdminContext.PermissionEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Repositories.Interfaces.AdminContext;

namespace Hotel.Domain.Services.Permissions;

public class DefaultAdminPermissions
{
  public static async Task HandleDefaultPermission(Permission permission,Admin admin, IAdminRepository adminRepository)
  {
    //Esse esquema é feito para economizar a quantidade de dados no token, sendo que só vai ser adicionado no token as permissões
    // quando as permissões padrões forem removidas

    //Se a alteração em permissões vai remover alguma das permissões padrões, então vai ser removido a permissão padrão de administrador
    //e adicionado todas as permissões padrões para poder editar em cima dessas permissões

    DefaultPermission = DefaultPermission ?? await adminRepository.GetDefaultAdminPermission(); //permissão padrão, que define o conjunto de permissões padrões
    DefaultPermissions = DefaultPermissions ?? await adminRepository.GetAllDefaultPermissions(); // busca todas as permissões padrões

    if (DefaultPermissions.Contains(permission) && admin.Permissions.Contains(DefaultPermission!)) //Se a permissão que deseja remover inclui nas permissões padrões
    {
      admin.RemovePermission(DefaultPermission!); //remove a permissão padrão
      foreach (var defaultPermission in DefaultPermissions)
        admin.AddPermission(defaultPermission); // e então adiciona todas essas permissões padrões
    }
  }
  

  public static IEnumerable<EPermissions> PermissionsName { get; set; } = 
  [
    EPermissions.GetAdmins,
    EPermissions.GetAdmin,
    EPermissions.EditCustomer,
    EPermissions.DeleteCustomer,
    EPermissions.GetEmployee,
    EPermissions.GetEmployees,
    EPermissions.DeleteEmployee,
    EPermissions.EditEmployee,
    EPermissions.CreateEmployee,
    EPermissions.AssignEmployeeResponsability,
    EPermissions.UnassignEmployeeResponsability,
    EPermissions.AssignEmployeePermission,
    EPermissions.UnassignEmployeePermission,
    EPermissions.GetResponsabilities,
    EPermissions.GetResponsability,
    EPermissions.CreateResponsability,
    EPermissions.EditResponsability,
    EPermissions.DeleteResponsability,
    EPermissions.GetRoomInvoices,
    EPermissions.GetRoomInvoice,
    EPermissions.DeleteRoomInvoice,
    EPermissions.GetReservations,
    EPermissions.DeleteReservation,
    EPermissions.UpdateReservationCheckout,
    EPermissions.UpdateReservationCheckIn,
    EPermissions.AddServiceToReservation,
    EPermissions.RemoveServiceFromReservation,
    EPermissions.GetServices,
    EPermissions.GetService,
    EPermissions.UpdateService,
    EPermissions.CreateService,
    EPermissions.DeleteService,
    EPermissions.AssignServiceResponsability,
    EPermissions.UnassignServiceResponsability,
    EPermissions.CreateRoom,
    EPermissions.DeleteRoom,
    EPermissions.AddRoomService,
    EPermissions.RemoveRoomService,
    EPermissions.UpdateRoomNumber,
    EPermissions.UpdateRoomCapacity,
    EPermissions.UpdateRoomCategory,
    EPermissions.UpdateRoomPrice,
    EPermissions.CreateCategory,
    EPermissions.EditCategory,
    EPermissions.DeleteCategory,
    EPermissions.GetReports,
    EPermissions.GetReport,
    EPermissions.EditReport,
    EPermissions.CreateReport,
    EPermissions.FinishReport
  ];

  public static Permission? DefaultPermission { get; set; } = null!;
  public static IEnumerable<Permission> DefaultPermissions { get; set; } = null!;
}
