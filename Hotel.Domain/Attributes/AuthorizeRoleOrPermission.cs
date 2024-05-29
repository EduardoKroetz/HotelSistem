using Hotel.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Hotel.Domain.Attributes;

public class AuthorizeRoleOrPermissions : Attribute, IAuthorizationFilter
{
  private readonly ERoles[] _roles;
  private readonly EPermissions[] _permissions;
  public AuthorizeRoleOrPermissions(EPermissions[] permissions, ERoles[]? roles = null)
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

    var role = (ERoles)Enum.Parse(typeof(ERoles),user.FindFirst(ClaimTypes.Role)!.Value); //Pegar a role atual
    if (role == ERoles.RootAdmin)
      return;

    var hasRole = _roles.Any(x => x == role);
    if (hasRole)
      return;

    var stringPermissions = user.FindFirst("permissions")!.Value.Split(","); //separar as permissões por vírgula
    //Convertendo as permissões de string para enumerador
    EPermissions[] permissions = stringPermissions.Select( permissionStr => 
    {
      return (EPermissions)Enum.Parse(typeof(EPermissions), permissionStr);
    }).ToArray();
    
    

    var hasPermission = _permissions.Any(permission => permissions!.Contains(permission));

    if (!hasRole && !hasPermission) 
      context.Result = new ForbidResult();
  }
}
