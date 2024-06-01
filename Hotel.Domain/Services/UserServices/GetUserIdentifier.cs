using Hotel.Domain.Services.UserServices.Interfaces;
using System.Security.Claims;

namespace Hotel.Domain.Services.UserServices;

public partial class UserService : IUserService
{
  public Guid GetUserIdentifier(ClaimsPrincipal User)
  => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
