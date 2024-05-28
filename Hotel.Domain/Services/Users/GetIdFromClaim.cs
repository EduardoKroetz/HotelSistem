using System.Security.Claims;

namespace Hotel.Domain.Services.Users;

public static class UserServices
{
  public static Guid GetIdFromClaim(ClaimsPrincipal User)
  => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
