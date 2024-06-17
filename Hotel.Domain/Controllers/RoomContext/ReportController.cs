using Hotel.Domain.Attributes;
using Hotel.Domain.DTOs.RoomContext.ReportDTOs;
using Hotel.Domain.Enums;
using Hotel.Domain.Handlers.RoomContext.ReportHandlers;
using Hotel.Domain.Services.UserServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Domain.Controllers.RoomContext;

[ApiController]
[Route("v1/reports")]
[Authorize(Roles = "RootAdmin,Admin,Employee")]
public class ReportController : ControllerBase
{
  private readonly ReportHandler _handler;
  private readonly IUserService _userService;

  public ReportController(ReportHandler handler, IUserService userService)
  {
    _handler = handler;
    _userService = userService;
  }


  // Endpoint para buscar todos os relat�rios
  [HttpGet]
  [AuthorizePermissions([EPermissions.GetReports, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission])]
  public async Task<IActionResult> GetAsync(
    [FromQuery] int? skip,
    [FromQuery] int? take,
    [FromQuery] string? summary,
    [FromQuery] EStatus? status,
    [FromQuery] EPriority? priority,
    [FromQuery] Guid? employeeId,
    [FromQuery] DateTime? createdAt,
    [FromQuery] string? createdAtOperator
  )
  {
    var queryParameters = new ReportQueryParameters(
        skip, take, summary, status, priority, employeeId, createdAt, createdAtOperator
    );

    return Ok(await _handler.HandleGetAsync(queryParameters));
  }


  // Endpoint para buscar um relat�rio por ID
  [HttpGet("{Id:guid}")]
  [AuthorizePermissions([EPermissions.GetReport, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission])]
  public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    => Ok(await _handler.HandleGetByIdAsync(id));

  // Endpoint para atualizar um relat�rio
  [HttpPut("{Id:guid}")]
  [AuthorizePermissions([EPermissions.EditReport, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission])]
  public async Task<IActionResult> PutAsync([FromBody] EditorReport model, [FromRoute] Guid id)
    => Ok(await _handler.HandleUpdateAsync(model, id));

  // Endpoint para criar um novo relat�rio
  [HttpPost]
  [AuthorizePermissions([EPermissions.CreateReport, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission])]
  public async Task<IActionResult> PostAsync([FromBody] EditorReport model)
    => Ok(await _handler.HandleCreateAsync(model));

  // Endpoint para deletar um relat�rio criado pelo usu�rio autenticado
  [HttpDelete("my/{Id:guid}")]
  public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
  {
    var userId = _userService.GetUserIdentifier(User);
    return Ok(await _handler.HandleDeleteAsync(id, userId ));
  }

  // Endpoint para marcar um relat�rio como conclu�do
  [HttpPatch("finish/{Id:guid}")]
  [AuthorizePermissions([EPermissions.FinishReport, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission])]
  public async Task<IActionResult> FinishAsync([FromRoute] Guid id)
  {
    return Ok(await _handler.HandleFinishAsync(id));
  }

  // Endpoint para cancelar um relat�rio
  [HttpPatch("cancel/{Id:guid}")]
  public async Task<IActionResult> CancelAsync([FromRoute] Guid id)
  {
    return Ok(await _handler.HandleCancelAsync(id));
  }

  // Endpoint para atualizar prioridade
  [HttpPatch("{id:guid}/priority/{priority:int}")]
  public async Task<IActionResult> CancelAsync([FromRoute] Guid id, [FromRoute] int priority)
    => Ok(await _handler.HandleUpdatePriorityAsync((EPriority) priority, id));
}
