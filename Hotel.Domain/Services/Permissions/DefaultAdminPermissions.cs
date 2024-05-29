
using Hotel.Domain.Entities.AdminContext.AdminEntity;
using Hotel.Domain.Entities.AdminContext.PermissionEntity;
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
    "GetAdmins",
    "GetAdmin",
    "AdminEditName",
    "AdminEditEmail",
    "AdminEditPhone",
    "AdminEditAddress",
    "AdminEditGender",
    "AdminEditDateOfBirth",
    "EditCustomer",
    "DeleteCustomer",
    "GetEmployee",
    "GetEmployees",
    "DeleteEmployee",
    "EditEmployee",
    "CreateEmployee",
    "AssignEmployeeResponsability",
    "UnassignEmployeeResponsability",
    "AssignEmployeePermission",
    "UnassignEmployeePermission",
    "GetResponsabilities",
    "GetResponsability",
    "CreateResponsability",
    "EditResponsability",
    "DeleteResponsability"
  ];

  public static Permission? DefaultPermission { get; set; } = null!;
  public static List<Permission> DefaultPermissions { get; set; } = null!;
}
