using Hotel.Domain.DTOs.AdminContext.AdminDTOs;
using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.Enums;
using Hotel.Domain.Handlers.AdminContext.AdminHandlers;
using Hotel.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Domain.Controllers.AdminContext;

public class AdminController : ControllerBase
{
  private readonly AdminHandler _handler;

  public AdminController(AdminHandler handler)
  {
    _handler = handler;
  }

  [HttpGet("v1/admins")]
  public async Task<IActionResult> GetAsync(
    [FromBody]AdminQueryParameters queryParameters)
  => Ok(await _handler.HandleGetAsync(queryParameters));
  
  [HttpGet("v1/admins/{Id:guid}")]
  public async Task<IActionResult> GetByIdAsync(
    [FromRoute]Guid id
  )
  => Ok(await _handler.HandleGetByIdAsync(id));  

  [HttpPut("v1/admins/{Id:guid}")]
  public async Task<IActionResult> PutAsync(
    [FromBody]UpdateUser model,
    [FromRoute]Guid id
  )
  => Ok(await _handler.HandleUpdateAsync(model,id));
 

  [HttpDelete("v1/admins/{Id:guid}")]
  public async Task<IActionResult> DeleteAsync(
    [FromRoute]Guid id
  )
  => Ok(await _handler.HandleDeleteAsync(id));


  [HttpPost("v1/admins/{adminId:guid}/permissions/{permissionId:guid}")]
  public async Task<IActionResult> AddPermissionAsync(
    [FromRoute] Guid adminId,
    [FromRoute] Guid permissionId)
  => Ok(await _handler.HandleAddPermission(adminId, permissionId));


  [HttpDelete("v1/admins/{adminId:guid}/permissions/{permissionId:guid}")]
  public async Task<IActionResult> RemovePermissionAsync(
    [FromRoute] Guid adminId,
    [FromRoute] Guid permissionId)
  => Ok(await _handler.HandleRemovePermission(adminId, permissionId));


  [HttpPost("v1/admins/{adminId:guid}/to-root-admin/{toRootAdminId:guid}")]
  public async Task<IActionResult> ChangeToRootAdminAsync(
    [FromRoute] Guid adminId,
    [FromRoute] Guid toRootAdminId)
  => Ok(await _handler.HandleChangeToRootAdminAsync(adminId,toRootAdminId));

  [HttpPatch("v1/admins/{adminId:guid}/name")]
  public async Task<IActionResult> UpdateNameAsync(
    [FromRoute] Guid adminId,
    [FromBody] Name name
  )
  => Ok(await _handler.HandleUpdateNameAsync(adminId, name));

  [HttpPatch("v1/admins/{adminId:guid}/email")]
  public async Task<IActionResult> UpdateEmailAsync(
    [FromRoute] Guid adminId,
    [FromBody] Email email
  )
  => Ok(await _handler.HandleUpdateEmailAsync(adminId, email));

  [HttpPatch("v1/admins/{adminId:guid}/phone")]
  public async Task<IActionResult> UpdatePhoneAsync(
    [FromRoute] Guid adminId,
    [FromBody] Phone phone
  )
  => Ok(await _handler.HandleUpdatePhoneAsync(adminId, phone));

  [HttpPatch("v1/admins/{adminId:guid}/address")]
  public async Task<IActionResult> UpdateAddressAsync(
    [FromRoute] Guid adminId,
    [FromBody] Address address
  ) 
  => Ok(await _handler.HandleUpdateAddressAsync(adminId, address));
  

  [HttpPatch("v1/admins/{adminId:guid}/gender/{gender:int}")]
  public async Task<IActionResult> UpdateGenderAsync(
    [FromRoute] Guid adminId,
    [FromRoute] int gender
  )
  => Ok(await _handler.HandleUpdateGenderAsync(adminId, (EGender)gender));

  [HttpPatch("v1/admins/{adminId:guid}/date-of-birth")]
  public async Task<IActionResult> UpdateDateOfBirthAsync(
    [FromRoute] Guid adminId,
    [FromBody] UpdateDateOfBirth newDateOfBirth
  )
  => Ok(await _handler.HandleUpdateDateOfBirthAsync(adminId, newDateOfBirth.DateOfBirth));
}