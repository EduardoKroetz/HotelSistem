using Hotel.Domain.Attributes;
using Hotel.Domain.DTOs.RoomContext.ServiceDTOs;
using Hotel.Domain.Enums;
using Hotel.Domain.Handlers.RoomContext.ServiceHandler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Domain.Controllers.RoomContext;

[ApiController]
[Route("v1/services")]
[Authorize(Roles = "RootAdmin,Admin,Employee")]
public class ServiceController : ControllerBase
{
  private readonly ServiceHandler _handler;

  public ServiceController(ServiceHandler handler)
  => _handler = handler;

  [HttpGet]
  [AuthorizeRoleOrPermissions([EPermissions.GetServices, EPermissions.DefaultEmployeePermission, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> GetAsync(
    [FromBody] ServiceQueryParameters queryParameters)
    => Ok(await _handler.HandleGetAsync(queryParameters));
  
  [HttpGet("{Id:guid}")]
  [AuthorizeRoleOrPermissions([EPermissions.GetService, EPermissions.DefaultEmployeePermission, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> GetByIdAsync(
    [FromRoute]Guid id)
    => Ok(await _handler.HandleGetByIdAsync(id));
  
  [HttpPut("{Id:guid}")]
  [AuthorizeRoleOrPermissions([EPermissions.UpdateService, EPermissions.DefaultEmployeePermission, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> PutAsync(
    [FromBody]EditorService model,
    [FromRoute]Guid id)
    => Ok(await _handler.HandleUpdateAsync(model,id));

  [HttpPost]
  [AuthorizeRoleOrPermissions([EPermissions.CreateService, EPermissions.DefaultEmployeePermission, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> PostAsync(
    [FromBody]EditorService model)
    => Ok(await _handler.HandleCreateAsync(model));
  
  [HttpDelete("{Id:guid}")]
  [AuthorizeRoleOrPermissions([EPermissions.DeleteService, EPermissions.DefaultEmployeePermission, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> DeleteAsync(
    [FromRoute]Guid id)
    => Ok(await _handler.HandleDeleteAsync(id));

  [HttpPost("{Id:guid}/responsabilities/{responsabilityId:guid}")]
  [AuthorizeRoleOrPermissions([EPermissions.AssignServiceResponsability, EPermissions.DefaultEmployeePermission, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> AssignResponsabilityAsync(
    [FromRoute] Guid id,
    [FromRoute] Guid responsabilityId)
    => Ok(await _handler.HandleAssignResponsabilityAsync(id,responsabilityId));

  [HttpDelete("{Id:guid}/responsabilities/{responsabilityId:guid}")]
  [AuthorizeRoleOrPermissions([EPermissions.UnassignServiceResponsability, EPermissions.DefaultEmployeePermission, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> UnassignResponsabilityAsync(
    [FromRoute] Guid id,
    [FromRoute] Guid responsabilityId)
    => Ok(await _handler.HandleUnassignResponsabilityAsync(id, responsabilityId));
}