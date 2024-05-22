using Hotel.Domain.DTOs.RoomContext.RoomDTOs;
using Hotel.Domain.Handlers.RoomContext.RoomHandlers;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Domain.Controllers.RoomContext;

public class RoomController : ControllerBase
{
  private readonly RoomHandler _handler;

  public RoomController(RoomHandler handler)
  => _handler = handler;


  [HttpGet("v1/rooms")]
  public async Task<IActionResult> GetAsync(
  [FromBody] RoomQueryParameters queryParameters)
  => Ok(await _handler.HandleGetAsync(queryParameters));

  [HttpGet("v1/rooms/{Id:guid}")]
  public async Task<IActionResult> GetByIdAsync(
    [FromRoute] Guid id
  )
  => Ok(await _handler.HandleGetByIdAsync(id));

  [HttpPut("v1/rooms/{Id:guid}")]
  public async Task<IActionResult> PutAsync(
    [FromBody] EditorRoom model,
    [FromRoute] Guid id
  )
  => Ok(await _handler.HandleUpdateAsync(model, id));

  [HttpPost("v1/rooms")]
  public async Task<IActionResult> PostAsync(
    [FromBody] EditorRoom model
  )
  => Ok(await _handler.HandleCreateAsync(model));


  [HttpDelete("v1/rooms/{Id:guid}")]
  public async Task<IActionResult> DeleteAsync(
    [FromRoute] Guid id
  )
  => Ok(await _handler.HandleDeleteAsync(id));

  [HttpPost("v1/rooms/{Id:guid}/services/{serviceId:guid}")]
  public async Task<IActionResult> AddServiceAsync(
    [FromRoute] Guid id,
    [FromRoute] Guid serviceId
  )
  => Ok(await _handler.HandleAddServiceAsync(id, serviceId));

  [HttpDelete("v1/rooms/{Id:guid}/services/{serviceId:guid}")]
  public async Task<IActionResult> RemoveServiceAsync(
  [FromRoute] Guid id,
  [FromRoute] Guid serviceId
  )
  => Ok(await _handler.HandleRemoveServiceAsync(id, serviceId));


  [HttpPatch("v1/rooms/{id:guid}/number/{number:int}")]
  public async Task<IActionResult> UpdateNumberAsync(
    [FromRoute] Guid id,
    [FromRoute] int number
  )
  => Ok(await _handler.HandleUpdateNumberAsync(id, number));

  [HttpPatch("v1/rooms/{id:guid}/capacity/{capacity:int}")]
  public async Task<IActionResult> UpdateCapacityAsync(
    [FromRoute] Guid id,
    [FromRoute] int capacity
  )
  => Ok(await _handler.HandleUpdateCapacityAsync(id, capacity));

  [HttpPatch("v1/rooms/{id:guid}/categories/{categoryId:guid}")]
  public async Task<IActionResult> UpdateCategoryAsync(
    [FromRoute] Guid id,
    [FromRoute] Guid categoryId
  )
  => Ok(await _handler.HandleUpdateCategoryAsync(id, categoryId));

  [HttpPatch("v1/rooms/{id:guid}/price/{price:decimal}")]
  public async Task<IActionResult> UpdateCategoryAsync(
  [FromRoute] Guid id,
  [FromRoute] decimal price
  )
  => Ok(await _handler.HandleUpdatePriceAsync(id, price));
}