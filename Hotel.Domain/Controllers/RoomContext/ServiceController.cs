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

  // Endpoint para buscar todos os serviços (com permissão)
  [HttpGet]
  [AuthorizePermissions([EPermissions.GetServices, EPermissions.DefaultEmployeePermission, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> GetAsync(
    [FromQuery] int? skip,
    [FromQuery] int? take,
    [FromQuery] string? name,
    [FromQuery] decimal? price,
    [FromQuery] string? priceOperator,
    [FromQuery] EPriority? priority,
    [FromQuery] bool? isActive,
    [FromQuery] int? timeInMinutes,
    [FromQuery] string? timeInMinutesOperator,
    [FromQuery] Guid? responsibilityId,
    [FromQuery] Guid? reservationId,
    [FromQuery] Guid? roomInvoiceId,
    [FromQuery] Guid? roomId,
    [FromQuery] DateTime? createdAt,
    [FromQuery] string? createdAtOperator
  )
  {
    var queryParameters = new ServiceQueryParameters(
        skip, take, name, price, priceOperator, priority, isActive,
        timeInMinutes, timeInMinutesOperator, responsibilityId, reservationId,
        roomInvoiceId, roomId, createdAt, createdAtOperator
    );

    return Ok(await _handler.HandleGetAsync(queryParameters));
  }


  // Endpoint para buscar um serviço por ID (com permissão)
  [HttpGet("{Id:guid}")]
  [AuthorizePermissions([EPermissions.GetService, EPermissions.DefaultEmployeePermission, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    => Ok(await _handler.HandleGetByIdAsync(id));

  // Endpoint para atualizar um serviço (com permissão)
  [HttpPut("{Id:guid}")]
  [AuthorizePermissions([EPermissions.UpdateService, EPermissions.DefaultEmployeePermission, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> PutAsync([FromBody] EditorService model, [FromRoute] Guid id)
    => Ok(await _handler.HandleUpdateAsync(model, id));

  // Endpoint para criar um novo serviço (com permissão)
  [HttpPost]
  [AuthorizePermissions([EPermissions.CreateService, EPermissions.DefaultEmployeePermission, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> PostAsync([FromBody] EditorService model)
    => Ok(await _handler.HandleCreateAsync(model));

  // Endpoint para deletar um serviço (com permissão)
  [HttpDelete("{Id:guid}")]
  [AuthorizePermissions([EPermissions.DeleteService, EPermissions.DefaultEmployeePermission, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    => Ok(await _handler.HandleDeleteAsync(id));

  // Endpoint para atribuir uma responsabilidade a um serviço (com permissão)
  [HttpPost("{Id:guid}/responsibilities/{responsibilityId:guid}")]
  [AuthorizePermissions([EPermissions.AssignServiceResponsibility, EPermissions.DefaultEmployeePermission, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> AssignResponsibilityAsync([FromRoute] Guid id, [FromRoute] Guid responsibilityId)
    => Ok(await _handler.HandleAssignResponsibilityAsync(id, responsibilityId));

  // Endpoint para remover uma responsabilidade de um serviço (com permissão)
  [HttpDelete("{Id:guid}/responsibilities/{responsibilityId:guid}")]
  [AuthorizePermissions([EPermissions.UnassignServiceResponsibility, EPermissions.DefaultEmployeePermission, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> UnassignResponsibilityAsync([FromRoute] Guid id, [FromRoute] Guid responsibilityId)
    => Ok(await _handler.HandleUnassignResponsibilityAsync(id, responsibilityId));
}
