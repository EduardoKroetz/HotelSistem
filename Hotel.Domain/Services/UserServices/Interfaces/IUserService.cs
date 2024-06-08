using Hotel.Domain.DTOs;
using Hotel.Domain.ValueObjects;
using System.Security.Claims;

namespace Hotel.Domain.Services.UserServices.Interfaces;

public interface IUserService
{
  Guid GetUserIdentifier(ClaimsPrincipal user);
}
