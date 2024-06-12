using Hotel.Domain.Attributes;
using Hotel.Domain.DTOs.EmployeeContext.ResponsibilityDTOs;
using Hotel.Domain.Enums;
using Hotel.Domain.Handlers.EmployeeContexty.ResponsibilityHandlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Domain.Controllers.EmployeeContext;

[ApiController]
[Route("v1/responsibilities")]
[Authorize(Roles = "RootAdmin,Admin,Employee")] //Somente tem acesso os administradores e os funcionários
public class ResponsibilityController : ControllerBase
{
  private readonly ResponsibilityHandler _handler;

  public ResponsibilityController(ResponsibilityHandler handler)
  => _handler = handler;

  //Buscar responsibilidades
  [HttpGet]
  [AuthorizePermissions([EPermissions.GetResponsibilities, EPermissions.DefaultEmployeePermission, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> GetAsync(
    [FromQuery] int? skip,
    [FromQuery] int? take,
    [FromQuery] string? name,
    [FromQuery] EPriority? priority,
    [FromQuery] Guid? employeeId,
    [FromQuery] Guid? serviceId,
    [FromQuery] DateTime? createdAt,
    [FromQuery] string? createdAtOperator
  )
  {
    var queryParameters = new ResponsibilityQueryParameters(
        skip, take, name, priority, employeeId, serviceId, createdAt, createdAtOperator
    );

    return Ok(await _handler.HandleGetAsync(queryParameters));
  }


  //Buscar responsibilidade pelo Id
  [HttpGet("{Id:guid}")]
  [AuthorizePermissions([EPermissions.GetResponsibility, EPermissions.DefaultEmployeePermission, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    => Ok(await _handler.HandleGetByIdAsync(id));

  //Criar responsibilidade
  [HttpPost]
  [AuthorizePermissions([EPermissions.CreateResponsibility, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> PostAsync([FromBody] EditorResponsibility model)
    => Ok(await _handler.HandleCreateAsync(model));

  //Editar responsibilidade
  [HttpPut("{Id:guid}")]
  [AuthorizePermissions([EPermissions.EditResponsibility, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> PutAsync([FromBody] EditorResponsibility model,[FromRoute] Guid id)
    => Ok(await _handler.HandleUpdateAsync(model, id));

  //Deletar responsibilidade
  [HttpDelete("{Id:guid}")]
  [AuthorizePermissions([EPermissions.DeleteResponsibility, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    => Ok(await _handler.HandleDeleteAsync(id));

}