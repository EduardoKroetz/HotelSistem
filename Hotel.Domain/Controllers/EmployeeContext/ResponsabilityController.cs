using Hotel.Domain.Attributes;
using Hotel.Domain.DTOs.EmployeeContext.ResponsabilityDTOs;
using Hotel.Domain.Enums;
using Hotel.Domain.Handlers.EmployeeContexty.ResponsabilityHandlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Domain.Controllers.EmployeeContext;

[ApiController]
[Route("v1/responsabilities")]
[Authorize(Roles = "RootAdmin,Admin,Employee")]
public class ResponsabilityController : ControllerBase
{
  private readonly ResponsabilityHandler _handler;

  public ResponsabilityController(ResponsabilityHandler handler)
  => _handler = handler;

  [HttpGet]
  [AuthorizeRoleOrPermissions([EPermissions.GetResponsabilities,EPermissions.DefaultEmployeePermission,EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> GetAsync(
    [FromBody] ResponsabilityQueryParameters queryParameters)
    => Ok(await _handler.HandleGetAsync(queryParameters));

  [HttpGet("{Id:guid}")]
  [AuthorizeRoleOrPermissions([EPermissions.GetResponsability, EPermissions.DefaultEmployeePermission, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> GetByIdAsync(
    [FromRoute] Guid id)
    => Ok(await _handler.HandleGetByIdAsync(id));

  [HttpPost]
  [AuthorizeRoleOrPermissions([EPermissions.CreateResponsability, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> PostAsync(
    [FromBody] EditorResponsability model)
    => Ok(await _handler.HandleCreateAsync(model));

  [HttpPut("{Id:guid}")]
  [AuthorizeRoleOrPermissions([EPermissions.EditResponsability, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> PutAsync(
    [FromBody] EditorResponsability model,
    [FromRoute] Guid id)
    => Ok(await _handler.HandleUpdateAsync(model, id));

  [HttpDelete("{Id:guid}")]
  [AuthorizeRoleOrPermissions([EPermissions.DeleteResponsability, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> DeleteAsync(
    [FromRoute] Guid id)
    => Ok(await _handler.HandleDeleteAsync(id));

}