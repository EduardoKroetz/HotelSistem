using Hotel.Domain.DTOs.User;
using Hotel.Domain.Handlers.CustomerContext.CustomerHandlers;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Domain.Controllers.CustomerContext;

public class CustomerController : ControllerBase
{
  private readonly CustomerHandler _handler;

  public CustomerController(CustomerHandler handler)
  {
    _handler = handler;
  }

  [HttpGet("v1/customers")]
  public async Task<IActionResult> GetAsync()
  => Ok(await _handler.HandleGetAsync());
  
  [HttpGet("v1/customers/{Id:guid}")]
  public async Task<IActionResult> GetByIdAsync(
    [FromRoute]Guid id
  )
  => Ok(await _handler.HandleGetByIdAsync(id));
  

  [HttpPost("v1/customers")]
  public async Task<IActionResult> PostAsync(
    [FromBody]CreateUser model
  )
  => Ok(await _handler.HandleCreateAsync(model));
  

  [HttpPut("v1/customers/{Id:guid}")]
  public async Task<IActionResult> PutAsync(
    [FromBody]UpdateUser model,
    [FromRoute]Guid id
  )
  => Ok(await _handler.HandleUpdateAsync(model,id));
  
  [HttpDelete("v1/customers/{Id:guid}")]
  public async Task<IActionResult> DeleteAsync(
    [FromRoute]Guid id
  )
  => Ok(await _handler.HandleDeleteAsync(id));
  
}