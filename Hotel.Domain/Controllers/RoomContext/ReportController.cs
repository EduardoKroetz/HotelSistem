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

  // Endpoint para buscar todos os relat�rios
  [HttpGet]
  [AuthorizePermissions([EPermissions.GetReports, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission])]
  public async Task<IActionResult> GetAsync([FromBody] ReportQueryParameters queryParameters)
    => Ok(await _handler.HandleGetAsync(queryParameters));

  // Endpoint para buscar um relat�rio por ID
  [HttpGet("{Id:guid}")]
  [AuthorizePermissions([EPermissions.GetReport, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission])]
  public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    => Ok(await _handler.HandleGetByIdAsync(id));

  // Endpoint para atualizar um relat�rio
  [HttpPut("{Id:guid}")]
  [AuthorizePermissions([EPermissions.EditReport, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission])]
  public async Task<IActionResult> PutAsync([FromBody] UpdateReport model, [FromRoute] Guid id)
    => Ok(await _handler.HandleUpdateAsync(model, id));

  // Endpoint para criar um novo relat�rio
  [HttpPost]
  [AuthorizePermissions([EPermissions.CreateReport, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission])]
  public async Task<IActionResult> PostAsync([FromBody] CreateReport model)
    => Ok(await _handler.HandleCreateAsync(model));

  // Endpoint para deletar um relat�rio criado pelo usu�rio autenticado
  [HttpDelete("my/{Id:guid}")]
  public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
  {
    // Implementar verifica��o para garantir que o usu�rio que criou o relat�rio � quem est� realizando a a��o
    return Ok(await _handler.HandleDeleteAsync(id));
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
}
