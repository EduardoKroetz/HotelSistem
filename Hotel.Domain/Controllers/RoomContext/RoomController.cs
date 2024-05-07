using Hotel.Domain.DTOs.PaymentContext.RoomInvoiceDTOs;
using Hotel.Domain.Handlers.RoomContext.RoomHandlers;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Domain.Controllers.CustomerContext;

public class RoomController : ControllerBase
{
  private readonly RoomHandler _handler;

  public RoomController(RoomHandler handler)
  => _handler = handler;


  [HttpGet("v1/rooms")]
  public async Task<IActionResult> GetAsync()
  => Ok(await _handler.HandleGetAsync());
  
  [HttpGet("v1/rooms/{Id:guid}")]
  public async Task<IActionResult> GetByIdAsync(
    [FromRoute]Guid id
  )
  => Ok(await _handler.HandleGetByIdAsync(id));
  
  [HttpPut("v1/rooms/{Id:guid}")]
  public async Task<IActionResult> PutAsync(
    [FromBody]EditorRoom model,
    [FromRoute]Guid id
  )
  => Ok(await _handler.HandleUpdateAsync(model,id));

  [HttpPost("v1/rooms")]
  public async Task<IActionResult> PostAsync(
    [FromBody]EditorRoom model
  )
  => Ok(await _handler.HandleCreateAsync(model));
  
  
  [HttpDelete("v1/rooms/{Id:guid}")]
  public async Task<IActionResult> DeleteAsync(
    [FromRoute]Guid id
  )
  => Ok(await _handler.HandleDeleteAsync(id));
  
}