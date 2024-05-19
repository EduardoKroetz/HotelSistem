using Hotel.Domain.DTOs.AdminContext.AdminDTOs;
using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.Handlers.AdminContext.AdminHandlers;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Domain.Controllers.AdminContext;

public class AdminController : ControllerBase
{
  private readonly AdminHandler _handler;

  public AdminController(AdminHandler handler)
  {
    _handler = handler;
  }

  [HttpGet("v1/admins")]
  public async Task<IActionResult> GetAsync(
    [FromBody]AdminQueryParameters queryParameters)
  => Ok(await _handler.HandleGetAsync(queryParameters));
  
  [HttpGet("v1/admins/{Id:guid}")]
  public async Task<IActionResult> GetByIdAsync(
    [FromRoute]Guid id
  )
  => Ok(await _handler.HandleGetByIdAsync(id));
  

  [HttpPost("v1/admins")]
  public async Task<IActionResult> PostAsync(
    [FromBody]CreateUser model
  )
  => Ok(await _handler.HandleCreateAsync(model));
  

  [HttpPut("v1/admins/{Id:guid}")]
  public async Task<IActionResult> PutAsync(
    [FromBody]UpdateUser model,
    [FromRoute]Guid id
  )
  => Ok(await _handler.HandleUpdateAsync(model,id));
 

  [HttpDelete("v1/admins/{Id:guid}")]
  public async Task<IActionResult> DeleteAsync(
    [FromRoute]Guid id
  )
  => Ok(await _handler.HandleDeleteAsync(id));


  [HttpPost("v1/admins/{adminId:guid}/permissions/{permissionId:guid}")]
  public async Task<IActionResult> AddPermission(
    [FromRoute] Guid adminId,
    [FromRoute] Guid permissionId)
  => Ok(await _handler.HandleAddPermission(adminId, permissionId));

  [HttpDelete("v1/admins/{adminId:guid}/permissions/{permissionId:guid}")]
  public async Task<IActionResult> RemovePermission(
    [FromRoute] Guid adminId,
    [FromRoute] Guid permissionId)
  => Ok(await _handler.HandleRemovePermission(adminId, permissionId));
}