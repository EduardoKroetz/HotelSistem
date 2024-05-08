using Hotel.Domain.DTOs.RoomContext.ServiceDTOs;
using Hotel.Domain.Handlers.RoomContext.ServiceHandler;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Domain.Controllers.RoomContext;

public class ServiceController : ControllerBase
{
  private readonly ServiceHandler _handler;

  public ServiceController(ServiceHandler handler)
  => _handler = handler;


  [HttpGet("v1/services")]
  public async Task<IActionResult> GetAsync()
  => Ok(await _handler.HandleGetAsync());
  
  [HttpGet("v1/services/{Id:guid}")]
  public async Task<IActionResult> GetByIdAsync(
    [FromRoute]Guid id
  )
  => Ok(await _handler.HandleGetByIdAsync(id));
  
  [HttpPut("v1/services/{Id:guid}")]
  public async Task<IActionResult> PutAsync(
    [FromBody]EditorService model,
    [FromRoute]Guid id
  )
  => Ok(await _handler.HandleUpdateAsync(model,id));

  [HttpPost("v1/services")]
  public async Task<IActionResult> PostAsync(
    [FromBody]EditorService model
  )
  => Ok(await _handler.HandleCreateAsync(model));
  
  
  [HttpDelete("v1/services/{Id:guid}")]
  public async Task<IActionResult> DeleteAsync(
    [FromRoute]Guid id
  )
  => Ok(await _handler.HandleDeleteAsync(id));
  
}