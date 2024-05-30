using Hotel.Domain.Enums;
using Hotel.Domain.Services.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hotel.Domain.Attributes;

/// <summary>
/// Atributo para autorização de usuário com base em suas permissões ou roles
/// </summary>
public class AuthorizePermissions : Attribute, IAuthorizationFilter
{
  
  private readonly ERoles[] _roles;
  private readonly EPermissions[] _permissions;
  public AuthorizePermissions(EPermissions[] permissions, ERoles[]? roles = null)
  {
    _roles = roles ?? [];
    _permissions = permissions;
  }

  public void OnAuthorization(AuthorizationFilterContext context)
  {
    var user = context.HttpContext.User;
    if (!user.Identity!.IsAuthenticated)
    {
      context.Result = new UnauthorizedResult();
      return;
    }

    var role = AuthorizationService.GetUserRole(user);
    if (role == ERoles.RootAdmin)
      return;

    var hasRole = _roles.Any(x => x == role);
    if (hasRole)
      return;

    var permissions = AuthorizationService.GetUserPermissions(user);

    var hasPermission = _permissions.Any(permission => permissions!.Contains(permission));

    if (!hasRole && !hasPermission) 
      context.Result = new ForbidResult();
  }
}
