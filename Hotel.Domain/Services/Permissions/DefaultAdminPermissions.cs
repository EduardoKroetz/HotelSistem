
using Hotel.Domain.Entities.AdminContext.AdminEntity;
using Hotel.Domain.Entities.AdminContext.PermissionEntity;
using Hotel.Domain.Repositories.Interfaces.AdminContext;

namespace Hotel.Domain.Services.Permissions;

public class DefaultAdminPermissions
{
  public static async Task RemoveDefaultPermissionIfExists(Permission permission,Admin admin, IAdminRepository adminRepository)
  {
    //Esse esquema é feito para economizar a quantidade de dados no token, sendo que só vai ser adicionado no token as permissões
    // quando as permissões padrões forem removidas

    //Se a alteração em permissões vai remover alguma das permissões padrões, então vai ser removido a permissão padrão de administrador
    //e adicionado todas as permissões padrões para poder editar em cima dessas permissões

    var permissions = await adminRepository.GetAllDefaultPermissions(); // busca todas as permissões padrões
    var defaultAdminPermission = await adminRepository.GetDefaultAdminPermission(); //busca a permissão padrão

    if (permissions.Contains(permission) && admin.Permissions.Contains(defaultAdminPermission!)) //Se a permissão que deseja remover inclui nas permissões padrões
    {
      admin.RemovePermission(defaultAdminPermission!); //remove a permissão padrão
      foreach (var defaultPermission in permissions)
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
    "AdminEditDateOfBirth"
  ];
}
