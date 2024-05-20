using Hotel.Domain.DTOs.AdminContext.PermissionDTOs;
using Hotel.Domain.DTOs.Base;
using Hotel.Domain.Handlers.AdminContext.PermissionHandlers;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Domain.Controllers.AdminContext;

public class PermissionController : ControllerBase
{
  private readonly PermissionHandler _handler;

  public PermissionController(PermissionHandler handler)
  {
    _handler = handler;
  }

  [HttpGet("v1/permissions")]
  public async Task<IActionResult> GetAsync([FromBody]PermissionQueryParameters queryParameters)
  => Ok(await _handler.HandleGetAsync(queryParameters));
  
  [HttpGet("v1/permissions/{Id:guid}")]
  public async Task<IActionResult> GetByIdAsync(
    [FromRoute]Guid id
  )
  => Ok(await _handler.HandleGetByIdAsync(id));
  

  [HttpPost("v1/permissions")]
  public async Task<IActionResult> PostAsync(
    [FromBody]EditorPermission model
  )
  => Ok(await _handler.HandleCreateAsync(model));
  

  [HttpPut("v1/permissions/{Id:guid}")]
  public async Task<IActionResult> PutAsync(
    [FromBody]EditorPermission model,
    [FromRoute]Guid id
  )
  => Ok(await _handler.HandleUpdateAsync(model,id));
  

  

  [HttpDelete("v1/permissions/{Id:guid}")]
  public async Task<IActionResult> DeleteAsync(
    [FromRoute]Guid id
  )
  => Ok(await _handler.HandleDeleteAsync(id));
  

  [HttpPost("v1/permissions/disable/{id:guid}")]
  public async Task<IActionResult> DisableAsync(
    [FromRoute] Guid id
  )
  => Ok(await _handler.HandleDisableAsync(id));


  [HttpPost("v1/permissions/enable/{id:guid}")]
  public async Task<IActionResult> EnableAsync(
    [FromRoute] Guid id
  )
  => Ok(await _handler.HandleEnableAsync(id));

}