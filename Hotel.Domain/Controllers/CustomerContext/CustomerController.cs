using Hotel.Domain.Attributes;
using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.Enums;
using Hotel.Domain.Handlers.CustomerContext.CustomerHandlers;
using Hotel.Domain.Services.Users;
using Hotel.Domain.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Domain.Controllers.CustomerContext;

[Route("v1/customers")]
[Authorize(Roles = "RootAdmin,Admin,Employee,Customer")]
public class CustomerController : ControllerBase
{
  private readonly CustomerHandler _handler;

  public CustomerController(CustomerHandler handler)
  => _handler = handler;

  [HttpGet]
  public async Task<IActionResult> GetAsync(
    [FromBody]UserQueryParameters queryParameters)
    => Ok(await _handler.HandleGetAsync(queryParameters));
  
  [HttpGet("{Id:guid}")]
  public async Task<IActionResult> GetByIdAsync(
    [FromRoute]Guid id)
    => Ok(await _handler.HandleGetByIdAsync(id));
  
  //Somente para admins
  [HttpPut("{Id:guid}")]
  [AuthorizePermissions([EPermissions.EditCustomer, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> EditCustomerAsync(
    [FromBody]UpdateUser model,
    [FromRoute]Guid id)
    => Ok(await _handler.HandleUpdateAsync(model,id));

  [HttpDelete("{Id:guid}")]
  [AuthorizePermissions([EPermissions.DeleteCustomer, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> DeleteAsync(
    [FromRoute] Guid id)
    => Ok(await _handler.HandleDeleteAsync(id));

  //Editar o customer com o id do token
  [HttpPut]
  public async Task<IActionResult> EditAsync([FromBody] UpdateUser model)
  {
    var customerId = UserServices.GetIdFromClaim(User);
    return Ok(await _handler.HandleUpdateAsync(model, customerId));
  }

  //Deletar com o id do token
  [HttpDelete]
  public async Task<IActionResult> DeleteAsync()
  {
    var customerId = UserServices.GetIdFromClaim(User);
    return Ok(await _handler.HandleDeleteAsync(customerId));
  }


  //Editar os campos
  [HttpPatch("name")]
  public async Task<IActionResult> UpdateNameAsync([FromBody] Name name )
  {
    var customerId = UserServices.GetIdFromClaim(User);
    return Ok(await _handler.HandleUpdateNameAsync(customerId, name));
  }
     
  [HttpPatch("email")]
  public async Task<IActionResult> UpdateEmailAsync([FromBody] Email email)
  {
    var customerId = UserServices.GetIdFromClaim(User);
    return Ok(await _handler.HandleUpdateEmailAsync(customerId, email));
  }
  
  [HttpPatch("phone")]
  public async Task<IActionResult> UpdatePhoneAsync( [FromBody] Phone phone)
  {
    var customerId = UserServices.GetIdFromClaim(User);
    return Ok(await _handler.HandleUpdatePhoneAsync(customerId, phone));
  }

  [HttpPatch("address")]
  public async Task<IActionResult> UpdateAddressAsync([FromBody] Address address)
  {
    var customerId = UserServices.GetIdFromClaim(User);
    return Ok(await _handler.HandleUpdateAddressAsync(customerId, address));
  }

  [HttpPatch("gender/{gender:int}")]
  public async Task<IActionResult> UpdateGenderAsync([FromRoute] int gender)
  {
    var customerId = UserServices.GetIdFromClaim(User);
    return Ok(await _handler.HandleUpdateGenderAsync(customerId, (EGender)gender));
  } 

  [HttpPatch("date-of-birth")]
  public async Task<IActionResult> UpdateDateOfBirthAsync([FromBody] UpdateDateOfBirth newDateOfBirth)
  {
    var customerId = UserServices.GetIdFromClaim(User);
    return Ok(await _handler.HandleUpdateDateOfBirthAsync(customerId, newDateOfBirth.DateOfBirth));
  }
}