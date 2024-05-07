using Hotel.Domain.DTOs.EmployeeContext.ResponsabilityDTOs;
using Hotel.Domain.Handlers.EmployeeContexty.ResponsabilityHandlers;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Domain.Controllers.CustomerContext;

public class ResponsabilityController : ControllerBase
{
  private readonly ResponsabilityHandler _handler;

  public ResponsabilityController(ResponsabilityHandler handler)
  {
    _handler = handler;
  }

  [HttpGet("v1/responsabilities")]
  public async Task<IActionResult> GetAsync()
  => Ok(await _handler.HandleGetAsync());
  
  [HttpGet("v1/responsabilities/{Id:guid}")]
  public async Task<IActionResult> GetByIdAsync(
    [FromRoute]Guid id
  )
  => Ok(await _handler.HandleGetByIdAsync(id));
  

  [HttpPost("v1/responsabilities")]
  public async Task<IActionResult> PostAsync(
    [FromBody]EditorResponsability model
  )
  => Ok(await _handler.HandleCreateAsync(model));
  

  [HttpPut("v1/responsabilities/{Id:guid}")]
  public async Task<IActionResult> PutAsync(
    [FromBody]EditorResponsability model,
    [FromRoute]Guid id
  )
  => Ok(await _handler.HandleUpdateAsync(model,id));
  
  [HttpDelete("v1/responsabilities/{Id:guid}")]
  public async Task<IActionResult> DeleteAsync(
    [FromRoute]Guid id
  )
  => Ok(await _handler.HandleDeleteAsync(id));
  
}