using Hotel.Domain.DTOs.AdminContext.AdminDTOs;
using Hotel.Domain.Handlers.AdminContext.AdminHandlers;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Domain.Controllers;

public class AdminController : ControllerBase
{
  private readonly AdminHandler _handler;

  public AdminController(AdminHandler handler)
  {
    _handler = handler;
  }

  [HttpPost("v1/admins")]
  public async Task<IActionResult> PostAsync(
    [FromBody]CreateAdmin model
  )
  {  
    return Ok(await _handler.HandleCreateAsync(model));
  }

  [HttpPut("v1/admins/{Id:guid}")]
  public async Task<IActionResult> PutAsync(
    [FromBody]UpdateAdmin model,
    [FromRoute]Guid id
  )
  {  
    return Ok(await _handler.HandleUpdateAsync(model,id));
  }

  [HttpGet("v1/admins/{Id:guid}")]
  public async Task<IActionResult> GetByIdAsync(
    [FromRoute]Guid id
  )
  {  
    return Ok(await _handler.HandleGetByIdAsync(id));
  }
}