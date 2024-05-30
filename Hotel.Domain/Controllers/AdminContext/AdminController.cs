using Hotel.Domain.Attributes;
using Hotel.Domain.DTOs.AdminContext.AdminDTOs;
using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.Enums;
using Hotel.Domain.Handlers.AdminContext.AdminHandlers;
using Hotel.Domain.Services.Users;
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

  //Buscar os administradores
  [HttpGet]
  [AuthorizePermissions([EPermissions.GetAdmins, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> GetAsync([FromBody]AdminQueryParameters queryParameters)
    => Ok(await _handler.HandleGetAsync(queryParameters));

  //Buscar o administrador pelo id
  [HttpGet("{Id:guid}")]
  [AuthorizePermissions([EPermissions.GetAdmin, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> GetByIdAsync([FromRoute]Guid id)
    => Ok(await _handler.HandleGetByIdAsync(id));

  //Editar administrador
  [HttpPut("{Id:guid}")]
  [AuthorizePermissions([EPermissions.EditAdmin])]
  public async Task<IActionResult> PutAsync([FromBody]UpdateUser model,[FromRoute]Guid id)
    => Ok(await _handler.HandleUpdateAsync(model,id));

  //Deletar administrador
  [HttpDelete("{Id:guid}")]
  [AuthorizePermissions([EPermissions.DeleteAdmin])]
  public async Task<IActionResult> DeleteAsync([FromRoute]Guid id)
    => Ok(await _handler.HandleDeleteAsync(id));

  //Atribuir permissão
  [HttpPost("{adminId:guid}/permissions/{permissionId:guid}")]
  [AuthorizePermissions([EPermissions.AdminAssignPermission])]
  public async Task<IActionResult> AddPermissionAsync([FromRoute] Guid adminId, [FromRoute] Guid permissionId)
    => Ok(await _handler.HandleAddPermission(adminId, permissionId));

  //Desatribuir permissão
  [HttpDelete("{adminId:guid}/permissions/{permissionId:guid}")]
  [AuthorizePermissions([EPermissions.AdminUnassignPermission])]
  public async Task<IActionResult> RemovePermissionAsync([FromRoute] Guid adminId, [FromRoute] Guid permissionId)
    => Ok(await _handler.HandleRemovePermission(adminId, permissionId));

  //Trocar administrador para administrador raiz
  [HttpPost("to-root-admin/{toRootAdminId:guid}")]
  [Authorize(Roles = "RootAdmin")]
  public async Task<IActionResult> ChangeToRootAdminAsync( [FromRoute] Guid toRootAdminId)
  {
    var adminId = UserServices.GetIdFromClaim(User);
    return Ok(await _handler.HandleChangeToRootAdminAsync(adminId, toRootAdminId));
  }

  //Endpoint para deletar o admin logado
  [HttpDelete]
  public async Task<IActionResult> DeleteAsync()
  {
    var adminId = UserServices.GetIdFromClaim(User);
    return Ok(await _handler.HandleDeleteAsync(adminId));
  }
    
  //Endpoint para editar o admin logado
  [HttpPut]
  public async Task<IActionResult> PutAsync([FromBody] UpdateUser model)
  {
    var adminId = UserServices.GetIdFromClaim(User);
    return Ok(await _handler.HandleUpdateAsync(model, adminId));
  }
  
  //Endpoints para editar campos específicos do admin logado
  [HttpPatch("name")]
  public async Task<IActionResult> UpdateNameAsync([FromBody] Name name)
  {
    var adminId = UserServices.GetIdFromClaim(User);
    return Ok(await _handler.HandleUpdateNameAsync(adminId, name));
  }

  [HttpPatch("email")]
  public async Task<IActionResult> UpdateEmailAsync([FromBody] Email email)
  {
    var adminId = UserServices.GetIdFromClaim(User);
    return Ok(await _handler.HandleUpdateEmailAsync(adminId, email));
  } 

  [HttpPatch("phone")]
  public async Task<IActionResult> UpdatePhoneAsync([FromBody] Phone phone)
  {
    var adminId = UserServices.GetIdFromClaim(User);
    return Ok(await _handler.HandleUpdatePhoneAsync(adminId, phone));
  }  

  [HttpPatch("address")]
  public async Task<IActionResult> UpdateAddressAsync([FromBody] Address address) 
  {
    var adminId = UserServices.GetIdFromClaim(User);
    return Ok(await _handler.HandleUpdateAddressAsync(adminId, address));
  }

  [HttpPatch("gender/{gender:int}")]
  public async Task<IActionResult> UpdateGenderAsync([FromRoute] int gender)
  {
    var adminId = UserServices.GetIdFromClaim(User);
    return Ok(await _handler.HandleUpdateGenderAsync(adminId, (EGender)gender));
  } 

  [HttpPatch("date-of-birth")]
  public async Task<IActionResult> UpdateDateOfBirthAsync([FromBody] UpdateDateOfBirth newDateOfBirth)
  {
    var adminId = UserServices.GetIdFromClaim(User);
    return Ok(await _handler.HandleUpdateDateOfBirthAsync(adminId, newDateOfBirth.DateOfBirth));
  }  
}