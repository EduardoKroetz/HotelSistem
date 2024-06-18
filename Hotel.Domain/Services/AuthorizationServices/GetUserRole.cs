using Hotel.Domain.Enums;
using System.Security.Claims;

namespace Hotel.Domain.Services.Authorization;

public partial class AuthorizationService
{
    /// <summary>
    /// Serviço para buscar a Role do usuário
    /// </summary>
    /// <param name="user">Aceita uma ClaimsPrincipal como parâmetro</param>
    /// <returns>Retorna a role do usuário do formato do enumerador ERoles</returns>
    public static ERoles GetUserRole(ClaimsPrincipal user)
    {
        var claimRole = user.FindFirst(ClaimTypes.Role);
        if (claimRole == null)
            throw new UnauthorizedAccessException("Você não tem acesso a esse serviço.");

        //Busca a role na claim e converte para o enumerador ERoles
        return (ERoles)Enum.Parse(
          typeof(ERoles),
          claimRole.Value); //Pegar a role atual
    }
}
