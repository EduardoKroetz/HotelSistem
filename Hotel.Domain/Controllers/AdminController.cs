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
  public async Task<IActionResult> Post(
    [FromBody]CreateAdmin model
  )
  {  
    return Ok(await _handler.HandleCreateAsync(model));
  }
}