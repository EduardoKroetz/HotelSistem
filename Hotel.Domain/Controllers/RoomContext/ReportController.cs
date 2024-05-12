using Hotel.Domain.DTOs.RoomContext.ReportDTOs;
using Hotel.Domain.Handlers.RoomContext.ReportHandlers;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Domain.Controllers.RoomContext;

public class ReportController : ControllerBase
{
  private readonly ReportHandler _handler;

  public ReportController(ReportHandler handler)
  => _handler = handler;


  [HttpGet("v1/reports")]
  public async Task<IActionResult> GetAsync(
  [FromBody] ReportQueryParameters queryParameters)
  => Ok(await _handler.HandleGetAsync(queryParameters));
  
  [HttpGet("v1/reports/{Id:guid}")]
  public async Task<IActionResult> GetByIdAsync(
    [FromRoute]Guid id
  )
  => Ok(await _handler.HandleGetByIdAsync(id));
  
  [HttpPut("v1/reports/{Id:guid}")]
  public async Task<IActionResult> PutAsync(
    [FromBody]UpdateReport model,
    [FromRoute]Guid id
  )
  => Ok(await _handler.HandleUpdateAsync(model,id));

  [HttpPost("v1/reports")]
  public async Task<IActionResult> PostAsync(
    [FromBody]CreateReport model
  )
  => Ok(await _handler.HandleCreateAsync(model));
  
  
  [HttpDelete("v1/reports/{Id:guid}")]
  public async Task<IActionResult> DeleteAsync(
    [FromRoute]Guid id
  )
  => Ok(await _handler.HandleDeleteAsync(id));
  
}