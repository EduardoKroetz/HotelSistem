using Hotel.Domain.Enums;

namespace Hotel.Domain.Services.Authorization;

public static partial class AuthorizationService
{
    /// <summary>
    /// Serviço para transformar uma string em uma permissão
    /// </summary>
    /// <param name="user">Aceita o nome da permissão como parâmetro</param>
    /// <returns>Retorna um array de permissões</returns>
    public static EPermissions ConvertToPermission(string permissionName)
    {
        EPermissions permission;
        if (!Enum.TryParse(permissionName, out permission))
            throw new ArgumentException($"Permissão {permissionName} é inválida.");
        return permission;
    }
}
