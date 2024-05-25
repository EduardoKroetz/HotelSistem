using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.Enums;
using Hotel.Domain.Handlers.CustomerContext.CustomerHandlers;
using Hotel.Domain.ValueObjects;
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
  public async Task<IActionResult> GetAsync(
    [FromBody]UserQueryParameters queryParameters)
  => Ok(await _handler.HandleGetAsync(queryParameters));
  
  [HttpGet("v1/customers/{Id:guid}")]
  public async Task<IActionResult> GetByIdAsync(
    [FromRoute]Guid id
  )
  => Ok(await _handler.HandleGetByIdAsync(id));
  
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

  [HttpPatch("v1/customers/{customerId:guid}/name")]
  public async Task<IActionResult> UpdateNameAsync(
    [FromRoute] Guid customerId,
    [FromBody] Name name
  )
  => Ok(await _handler.HandleUpdateNameAsync(customerId, name));

  [HttpPatch("v1/customers/{customerId:guid}/email")]
  public async Task<IActionResult> UpdateEmailAsync(
    [FromRoute] Guid customerId,
    [FromBody] Email email
  )
  => Ok(await _handler.HandleUpdateEmailAsync(customerId, email));

  [HttpPatch("v1/customers/{customerId:guid}/phone")]
  public async Task<IActionResult> UpdatePhoneAsync(
    [FromRoute] Guid customerId,
    [FromBody] Phone phone
  )
  => Ok(await _handler.HandleUpdatePhoneAsync(customerId, phone));

  [HttpPatch("v1/customers/{customerId:guid}/address")]
  public async Task<IActionResult> UpdateAddressAsync(
    [FromRoute] Guid customerId,
    [FromBody] Address address
  )
  => Ok(await _handler.HandleUpdateAddressAsync(customerId, address));


  [HttpPatch("v1/customers/{customerId:guid}/gender/{gender:int}")]
  public async Task<IActionResult> UpdateGenderAsync(
    [FromRoute] Guid customerId,
    [FromRoute] int gender
  )
  => Ok(await _handler.HandleUpdateGenderAsync(customerId, (EGender)gender));

  [HttpPatch("v1/customers/{customerId:guid}/date-of-birth")]
  public async Task<IActionResult> UpdateDateOfBirthAsync(
    [FromRoute] Guid customerId,
    [FromBody] UpdateDateOfBirth newDateOfBirth
  )
  => Ok(await _handler.HandleUpdateDateOfBirthAsync(customerId, newDateOfBirth.DateOfBirth));

}