using Hotel.Domain.Enums;
using System.Security.Claims;

namespace Hotel.Domain.Services.Authorization;

public static partial class AuthorizationService
{
    /// <summary>
    /// Serviço para buscar todas as permissões da ClaimPrincipal
    /// </summary>
    /// <param name="user">Aceita uma ClaimPrincipal como parâmetro</param>
    /// <returns>Retorna um array de permissões</returns>
    public static IEnumerable<EPermissions> GetUserPermissions(ClaimsPrincipal user)
    {
        var permissionsClaim = user.FindFirst("permissions");
        if (permissionsClaim == null)
            throw new UnauthorizedAccessException("Você não tem acesso a esse serviço.");
       
        var stringPermissions = permissionsClaim.Value.Split(","); // separar as permissões por vírgula(',')

        //Convertendo as permissões de string para enumerador
        var permissions = new List<EPermissions>();
        foreach (var permissionName in stringPermissions)
        {
            permissions.Add(ConvertToPermission(permissionName.Trim()));
        }

        return permissions;
    }
}
