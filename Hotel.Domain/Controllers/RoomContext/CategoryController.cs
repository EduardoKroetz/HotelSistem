using Hotel.Domain.DTOs.RoomContext.CategoryDTOs;
using Hotel.Domain.Handlers.RoomContext.CategoryHandlers;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Domain.Controllers.RoomContext;

public class CategoryController : ControllerBase
{
  private readonly CategoryHandler _handler;

  public CategoryController(CategoryHandler handler)
  => _handler = handler;


  [HttpGet("v1/categories")]
  public async Task<IActionResult> GetAsync(
  [FromBody] CategoryQueryParameters queryParameters)
  => Ok(await _handler.HandleGetAsync(queryParameters));
  
  [HttpGet("v1/categories/{Id:guid}")]
  public async Task<IActionResult> GetByIdAsync(
    [FromRoute]Guid id
  )
  => Ok(await _handler.HandleGetByIdAsync(id));
  
  [HttpPut("v1/categories/{Id:guid}")]
  public async Task<IActionResult> PutAsync(
    [FromBody]EditorCategory model,
    [FromRoute]Guid id
  )
  => Ok(await _handler.HandleUpdateAsync(model,id));

  [HttpPost("v1/categories")]
  public async Task<IActionResult> PostAsync(
    [FromBody]EditorCategory model
  )
  => Ok(await _handler.HandleCreateAsync(model));
  
  
  [HttpDelete("v1/categories/{Id:guid}")]
  public async Task<IActionResult> DeleteAsync(
    [FromRoute]Guid id
  )
  => Ok(await _handler.HandleDeleteAsync(id));
  
}