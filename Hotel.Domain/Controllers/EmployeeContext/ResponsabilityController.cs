using Hotel.Domain.Attributes;
using Hotel.Domain.DTOs.EmployeeContext.ResponsabilityDTOs;
using Hotel.Domain.Enums;
using Hotel.Domain.Handlers.EmployeeContexty.ResponsabilityHandlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Domain.Controllers.EmployeeContext;

[ApiController]
[Route("v1/responsabilities")]
[Authorize(Roles = "RootAdmin,Admin,Employee")] //Somente tem acesso os administradores e os funcionários
public class ResponsabilityController : ControllerBase
{
  private readonly ResponsabilityHandler _handler;

  public ResponsabilityController(ResponsabilityHandler handler)
  => _handler = handler;

  //Buscar responsabilidades
  [HttpGet]
  [AuthorizePermissions([EPermissions.GetResponsabilities, EPermissions.DefaultEmployeePermission, EPermissions.DefaultAdminPermission])]
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
    var queryParameters = new ResponsabilityQueryParameters(
        skip, take, name, priority, employeeId, serviceId, createdAt, createdAtOperator
    );

    return Ok(await _handler.HandleGetAsync(queryParameters));
  }


  //Buscar responsabilidade pelo Id
  [HttpGet("{Id:guid}")]
  [AuthorizePermissions([EPermissions.GetResponsability, EPermissions.DefaultEmployeePermission, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    => Ok(await _handler.HandleGetByIdAsync(id));

  //Criar responsabilidade
  [HttpPost]
  [AuthorizePermissions([EPermissions.CreateResponsability, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> PostAsync([FromBody] EditorResponsability model)
    => Ok(await _handler.HandleCreateAsync(model));

  //Editar responsabilidade
  [HttpPut("{Id:guid}")]
  [AuthorizePermissions([EPermissions.EditResponsability, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> PutAsync([FromBody] EditorResponsability model,[FromRoute] Guid id)
    => Ok(await _handler.HandleUpdateAsync(model, id));

  //Deletar responsabilidade
  [HttpDelete("{Id:guid}")]
  [AuthorizePermissions([EPermissions.DeleteResponsability, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    => Ok(await _handler.HandleDeleteAsync(id));

}