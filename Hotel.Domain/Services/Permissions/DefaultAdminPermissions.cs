
using Hotel.Domain.DTOs.ReservationContext.ReservationDTOs;
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
  

  public static List<string> PermissionsName { get; set; } = 
  [
    EPermissions.GetAdmins.ToString(),
    EPermissions.GetAdmin.ToString(),
    EPermissions.AdminEditName.ToString(),
    EPermissions.AdminEditEmail.ToString(),
    EPermissions.AdminEditPhone.ToString(),
    EPermissions.AdminEditAddress.ToString(),
    EPermissions.AdminEditGender.ToString(),
    EPermissions.AdminEditDateOfBirth.ToString(),
    EPermissions.EditCustomer.ToString(),
    EPermissions.DeleteCustomer.ToString(),
    EPermissions.GetEmployee.ToString(),
    EPermissions.GetEmployees.ToString(),
    EPermissions.DeleteEmployee.ToString(),
    EPermissions.EditEmployee.ToString(),
    EPermissions.CreateEmployee.ToString(),
    EPermissions.AssignEmployeeResponsability.ToString(),
    EPermissions.UnassignEmployeeResponsability.ToString(),
    EPermissions.AssignEmployeePermission.ToString(),
    EPermissions.UnassignEmployeePermission.ToString(),
    EPermissions.GetResponsabilities.ToString(),
    EPermissions.GetResponsability.ToString(),
    EPermissions.CreateResponsability.ToString(),
    EPermissions.EditResponsability.ToString(),
    EPermissions.DeleteResponsability.ToString(),
    EPermissions.GetRoomInvoices.ToString(),
    EPermissions.GetRoomInvoice.ToString(),
    EPermissions.DeleteRoomInvoice.ToString(),
    EPermissions.GetReservations.ToString(),
    EPermissions.DeleteReservation.ToString(),
    EPermissions.UpdateReservationCheckout.ToString(),
    EPermissions.UpdateReservationCheckIn.ToString(),
    EPermissions.AddServiceToReservation.ToString(),
    EPermissions.RemoveServiceFromReservation.ToString()
  ];

  public static Permission? DefaultPermission { get; set; } = null!;
  public static List<Permission> DefaultPermissions { get; set; } = null!;
}
