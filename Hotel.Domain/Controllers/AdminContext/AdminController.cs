using Hotel.Domain.Attributes;
using Hotel.Domain.DTOs.AdminContext.AdminDTOs;
using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.Enums;
using Hotel.Domain.Handlers.AdminContext.AdminHandlers;
using Hotel.Domain.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Domain.Controllers.AdminContext;

[Route("v1/admins")]
public class AdminController : ControllerBase
{
  private readonly AdminHandler _handler;

  public AdminController(AdminHandler handler)
  => _handler = handler;

  [HttpGet]
  [AuthorizeRoleOrPermissions([ "Admin", "RootAdmin" ],[ "GetAdmins" ])]
  public async Task<IActionResult> GetAsync(
    [FromBody]AdminQueryParameters queryParameters)
    => Ok(await _handler.HandleGetAsync(queryParameters));
  
  [HttpGet("{Id:guid}")]
  [AuthorizeRoleOrPermissions(["Admin", "RootAdmin"], ["GetAdmin"])]
  public async Task<IActionResult> GetByIdAsync(
    [FromRoute]Guid id)
    => Ok(await _handler.HandleGetByIdAsync(id));  

  [HttpPut("{Id:guid}")]
  [AuthorizeRoleOrPermissions(["Admin", "RootAdmin"], ["GetAdmins"])]
  public async Task<IActionResult> PutAsync(
    [FromBody]UpdateUser model,
    [FromRoute]Guid id)
    => Ok(await _handler.HandleUpdateAsync(model,id));
 
  [HttpDelete("{Id:guid}")]
  [AuthorizeRoleOrPermissions(["Admin", "RootAdmin"], ["DeleteAdmin"])]
  public async Task<IActionResult> DeleteAsync(
    [FromRoute]Guid id)
    => Ok(await _handler.HandleDeleteAsync(id));


  [HttpPost("{adminId:guid}/permissions/{permissionId:guid}")]
  [AuthorizeRoleOrPermissions(["RootAdmin"], ["Admin-AssignPermission"])]
  public async Task<IActionResult> AddPermissionAsync(
    [FromRoute] Guid adminId,
    [FromRoute] Guid permissionId)
    => Ok(await _handler.HandleAddPermission(adminId, permissionId));


  [HttpDelete("{adminId:guid}/permissions/{permissionId:guid}")]
  [AuthorizeRoleOrPermissions([ "RootAdmin"], ["Admin-UnassignPermission"])]
  public async Task<IActionResult> RemovePermissionAsync(
    [FromRoute] Guid adminId,
    [FromRoute] Guid permissionId)
    => Ok(await _handler.HandleRemovePermission(adminId, permissionId));

  [HttpPost("{adminId:guid}/to-root-admin/{toRootAdminId:guid}")]
  [Authorize(Roles = "RootAdmin")]
  public async Task<IActionResult> ChangeToRootAdminAsync(
    [FromRoute] Guid adminId,
    [FromRoute] Guid toRootAdminId)
    => Ok(await _handler.HandleChangeToRootAdminAsync(adminId,toRootAdminId));

  [HttpPatch("{adminId:guid}/name")]
  [AuthorizeRoleOrPermissions(["RootAdmin, Admin"], ["Admin-EditName"])]
  public async Task<IActionResult> UpdateNameAsync(
    [FromRoute] Guid adminId,
    [FromBody] Name name)
    => Ok(await _handler.HandleUpdateNameAsync(adminId, name));

  [HttpPatch("{adminId:guid}/email")]
  [AuthorizeRoleOrPermissions(["RootAdmin, Admin"], ["Admin-EditEmail"])]
  public async Task<IActionResult> UpdateEmailAsync(
    [FromRoute] Guid adminId,
    [FromBody] Email email)
    => Ok(await _handler.HandleUpdateEmailAsync(adminId, email));

  [HttpPatch("{adminId:guid}/phone")]
  [AuthorizeRoleOrPermissions(["RootAdmin, Admin"], ["Admin-EditPhone"])]
  public async Task<IActionResult> UpdatePhoneAsync(
    [FromRoute] Guid adminId,
    [FromBody] Phone phone)
    => Ok(await _handler.HandleUpdatePhoneAsync(adminId, phone));

  [HttpPatch("{adminId:guid}/address")]
  [AuthorizeRoleOrPermissions(["RootAdmin, Admin"], ["Admin-EditAddress"])]
  public async Task<IActionResult> UpdateAddressAsync(
    [FromRoute] Guid adminId,
    [FromBody] Address address) 
    => Ok(await _handler.HandleUpdateAddressAsync(adminId, address));
  

  [HttpPatch("{adminId:guid}/gender/{gender:int}")]
  [AuthorizeRoleOrPermissions(["RootAdmin, Admin"], ["Admin-EditGender"])]
  public async Task<IActionResult> UpdateGenderAsync(
    [FromRoute] Guid adminId,
    [FromRoute] int gender)
    => Ok(await _handler.HandleUpdateGenderAsync(adminId, (EGender)gender));

  [HttpPatch("{adminId:guid}/date-of-birth")]
  [AuthorizeRoleOrPermissions(["RootAdmin, Admin"], ["Admin-EditDateOfBirth"])]
  public async Task<IActionResult> UpdateDateOfBirthAsync(
    [FromRoute] Guid adminId,
    [FromBody] UpdateDateOfBirth newDateOfBirth)
    => Ok(await _handler.HandleUpdateDateOfBirthAsync(adminId, newDateOfBirth.DateOfBirth));
}