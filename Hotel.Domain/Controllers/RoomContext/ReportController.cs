using Hotel.Domain.Attributes;
using Hotel.Domain.DTOs.RoomContext.ReportDTOs;
using Hotel.Domain.Enums;
using Hotel.Domain.Handlers.RoomContext.ReportHandlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Domain.Controllers.RoomContext;

[ApiController]
[Route("v1/reports")]
[Authorize(Roles = "RootAdmin,Admin,Employee")]
public class ReportController : ControllerBase
{
  private readonly ReportHandler _handler;

  public ReportController(ReportHandler handler)
  => _handler = handler;

  [HttpGet]
  [AuthorizeRoleOrPermissions([EPermissions.GetReports, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission])]
  public async Task<IActionResult> GetAsync(
  [FromBody] ReportQueryParameters queryParameters)
  => Ok(await _handler.HandleGetAsync(queryParameters));
  
  [HttpGet("{Id:guid}")]
  [AuthorizeRoleOrPermissions([EPermissions.GetReport, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission])]
  public async Task<IActionResult> GetByIdAsync(
    [FromRoute]Guid id)
    => Ok(await _handler.HandleGetByIdAsync(id));
  
  [HttpPut("{Id:guid}")]
  [AuthorizeRoleOrPermissions([EPermissions.EditReport, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission])]
  public async Task<IActionResult> PutAsync(
    [FromBody]UpdateReport model,
    [FromRoute]Guid id)
    => Ok(await _handler.HandleUpdateAsync(model,id));

  [HttpPost]
  [AuthorizeRoleOrPermissions([EPermissions.CreateReport, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission])]
  public async Task<IActionResult> PostAsync(
    [FromBody]CreateReport model)
    => Ok(await _handler.HandleCreateAsync(model));

  [HttpDelete("my/{Id:guid}")]
  public async Task<IActionResult> DeleteAsync([FromRoute]Guid id)
  {
    //Implementar verificação para ver se o usuário quem criou é o mesmo que vai realizar a ação
    return Ok(await _handler.HandleDeleteAsync(id));
  }

  [HttpPatch("finish/{Id:guid}")]
  [AuthorizeRoleOrPermissions([EPermissions.FinishReport, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission])]
  public async Task<IActionResult> FinishAsync([FromRoute] Guid id)
  {
    return Ok(await _handler.HandleFinishAsync(id));
  }

  [HttpPatch("cancel/{Id:guid}")]
  public async Task<IActionResult> CancelAsync([FromRoute] Guid id)
  {
    return Ok(await _handler.HandleCancelAsync(id));
  }

} 