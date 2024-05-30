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
[Authorize(Roles = "Admin,RootAdmin")] // Somente admin e root admin tem acesso, porém o admin só vai ter acesso as rotas que ele possui permissão
public class AdminController : ControllerBase
{
  private readonly AdminHandler _handler;

  public AdminController(AdminHandler handler)
  => _handler = handler;

  [HttpGet]
  [AuthorizePermissions([EPermissions.GetAdmins, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> GetAsync(
    [FromBody]AdminQueryParameters queryParameters)
    => Ok(await _handler.HandleGetAsync(queryParameters));
  
  [HttpGet("{Id:guid}")]
  [AuthorizePermissions([EPermissions.GetAdmin, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> GetByIdAsync(
    [FromRoute]Guid id)
    => Ok(await _handler.HandleGetByIdAsync(id));  

  [HttpPut("{Id:guid}")]
  [AuthorizePermissions([EPermissions.EditAdmin])]
  public async Task<IActionResult> PutAsync(
    [FromBody]UpdateUser model,
    [FromRoute]Guid id)
    => Ok(await _handler.HandleUpdateAsync(model,id));
 
  [HttpDelete("{Id:guid}")]
  [AuthorizePermissions([EPermissions.DeleteAdmin])]
  public async Task<IActionResult> DeleteAsync(
    [FromRoute]Guid id)
    => Ok(await _handler.HandleDeleteAsync(id));


  [HttpPost("{adminId:guid}/permissions/{permissionId:guid}")]
  [AuthorizePermissions([EPermissions.AdminAssignPermission])]
  public async Task<IActionResult> AddPermissionAsync(
    [FromRoute] Guid adminId,
    [FromRoute] Guid permissionId)
    => Ok(await _handler.HandleAddPermission(adminId, permissionId));


  [HttpDelete("{adminId:guid}/permissions/{permissionId:guid}")]
  [AuthorizePermissions([EPermissions.AdminUnassignPermission])]
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
  [AuthorizePermissions([EPermissions.AdminEditName , EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> UpdateNameAsync(
    [FromRoute] Guid adminId,
    [FromBody] Name name)
    => Ok(await _handler.HandleUpdateNameAsync(adminId, name));

  [HttpPatch("{adminId:guid}/email")]
  [AuthorizePermissions([EPermissions.AdminEditEmail ,EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> UpdateEmailAsync(
    [FromRoute] Guid adminId,
    [FromBody] Email email)
    => Ok(await _handler.HandleUpdateEmailAsync(adminId, email));

  [HttpPatch("{adminId:guid}/phone")]
  [AuthorizePermissions([EPermissions.AdminEditPhone ,EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> UpdatePhoneAsync(
    [FromRoute] Guid adminId,
    [FromBody] Phone phone)
    => Ok(await _handler.HandleUpdatePhoneAsync(adminId, phone));

  [HttpPatch("{adminId:guid}/address")]
  [AuthorizePermissions([EPermissions.AdminEditAddress ,EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> UpdateAddressAsync(
    [FromRoute] Guid adminId,
    [FromBody] Address address) 
    => Ok(await _handler.HandleUpdateAddressAsync(adminId, address));
  

  [HttpPatch("{adminId:guid}/gender/{gender:int}")]
  [AuthorizePermissions([EPermissions.AdminEditGender ,EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> UpdateGenderAsync(
    [FromRoute] Guid adminId,
    [FromRoute] int gender)
    => Ok(await _handler.HandleUpdateGenderAsync(adminId, (EGender)gender));

  [HttpPatch("{adminId:guid}/date-of-birth")]
  [AuthorizePermissions([EPermissions.AdminEditDateOfBirth ,EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> UpdateDateOfBirthAsync(
    [FromRoute] Guid adminId,
    [FromBody] UpdateDateOfBirth newDateOfBirth)
    => Ok(await _handler.HandleUpdateDateOfBirthAsync(adminId, newDateOfBirth.DateOfBirth));
}