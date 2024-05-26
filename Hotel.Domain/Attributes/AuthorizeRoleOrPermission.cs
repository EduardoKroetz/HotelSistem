using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hotel.Domain.Attributes;

public class AuthorizeRoleOrPermissions : Attribute, IAuthorizationFilter
{
  private readonly string[] _roles;
  private readonly string[] _permissions;
  public AuthorizeRoleOrPermissions(string[] roles, string[] permissions)
  {
    _roles = roles;
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

    var permissions = user.FindFirst("permissions")!.Value.Split(","); //separar as permissões por vírgula
    var hasRole = _roles.Any(role => user.IsInRole(role));
    var hasPermission = _permissions.Any(permission => permissions!.Contains(permission));

    if (!hasRole && !hasPermission) 
      context.Result = new ForbidResult();
  }
}
