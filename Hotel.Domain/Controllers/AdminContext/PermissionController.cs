using Hotel.Domain.DTOs.AdminContext.PermissionDTOs;
using Hotel.Domain.Handlers.AdminContext.PermissionHandlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Domain.Controllers.AdminContext;

[Authorize(Roles = "RootAdmin,Admin,Employee")]
public class PermissionController : ControllerBase
{
  private readonly PermissionHandler _handler;

  public PermissionController(PermissionHandler handler)
  => _handler = handler;

  [HttpGet("v1/permissions")]
  public async Task<IActionResult> GetAsync(
    [FromBody]PermissionQueryParameters queryParameters)
    => Ok(await _handler.HandleGetAsync(queryParameters));
  
  [HttpGet("v1/permissions/{Id:guid}")]
  public async Task<IActionResult> GetByIdAsync(
    [FromRoute]Guid id)
    => Ok(await _handler.HandleGetByIdAsync(id));
}