using Hotel.Domain.Attributes;
using Hotel.Domain.DTOs.AdminDTOs;
using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.Enums;
using Hotel.Domain.Handlers.AdminHandlers;
using Hotel.Domain.Services.UserServices.Interfaces;
using Hotel.Domain.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Domain.Controllers;

[Route("v1/admins")]
[Authorize(Roles = "Admin,RootAdmin")] // Somente admin e root admin tem acesso, porém o admin só vai ter acesso as rotas que ele possui permissão
public class AdminController : ControllerBase
{
    private readonly AdminHandler _handler;
    private readonly IUserService _userService;

    public AdminController(AdminHandler handler, IUserService userService)
    {
        _handler = handler;
        _userService = userService;
    }


    //Buscar os administradores
    //Administradores com permissão podem acessar.
    //Administradores podem acessar por padrão.
    [HttpGet]
    [AuthorizePermissions([EPermissions.GetAdmins, EPermissions.DefaultAdminPermission])]
    public async Task<IActionResult> GetAsync(
      [FromQuery] int? skip,
      [FromQuery] int? take,
      [FromQuery] string? name,
      [FromQuery] string? email,
      [FromQuery] string? phone,
      [FromQuery] EGender? gender,
      [FromQuery] DateTime? dateOfBirth,
      [FromQuery] string? dateOfBirthOperator,
      [FromQuery] DateTime? createdAt,
      [FromQuery] string? createdAtOperator,
      [FromQuery] bool? isRootAdmin,
      [FromQuery] Guid? permissionId
    )
    {
        var queryParameters = new AdminQueryParameters(
            skip, take, name, email, phone, gender, dateOfBirth,
            dateOfBirthOperator, createdAt, createdAtOperator, isRootAdmin, permissionId
        );

        return Ok(await _handler.HandleGetAsync(queryParameters));
    }

    //Buscar o administrador pelo id.
    //Administradores com permissão podem acessar.
    //Administradores podem acessar por padrão.
    [HttpGet("{Id:guid}")]
    [AuthorizePermissions([EPermissions.GetAdmin, EPermissions.DefaultAdminPermission])]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
      => Ok(await _handler.HandleGetByIdAsync(id));

    //Editar administrador
    //Administradores com permissão podem acessar.
    [HttpPut("{Id:guid}")]
    [AuthorizePermissions([EPermissions.EditAdmin])]
    public async Task<IActionResult> PutAsync([FromBody] UpdateUser model, [FromRoute] Guid id)
      => Ok(await _handler.HandleUpdateAsync(model, id));

    //Deletar administrador
    //Administradores com permissão podem acessar.
    [HttpDelete("{Id:guid}")]
    [AuthorizePermissions([EPermissions.DeleteAdmin])]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
      => Ok(await _handler.HandleDeleteAsync(id));

    //Atribuir permissão
    //Administradores com permissão podem acessar.
    [HttpPost("{adminId:guid}/permissions/{permissionId:guid}")]
    [AuthorizePermissions([EPermissions.AdminAssignPermission])]
    public async Task<IActionResult> AddPermissionAsync([FromRoute] Guid adminId, [FromRoute] Guid permissionId)
      => Ok(await _handler.HandleAddPermission(adminId, permissionId));

    //Desatribuir permissão
    //Administradores com permissão podem acessar.
    [HttpDelete("{adminId:guid}/permissions/{permissionId:guid}")]
    [AuthorizePermissions([EPermissions.AdminUnassignPermission])]
    public async Task<IActionResult> RemovePermissionAsync([FromRoute] Guid adminId, [FromRoute] Guid permissionId)
      => Ok(await _handler.HandleRemovePermission(adminId, permissionId));

    //Trocar administrador para administrador raiz
    //Somente administrador raiz.
    [HttpPost("to-root-admin/{toRootAdminId:guid}")]
    [Authorize(Roles = "RootAdmin")]
    public async Task<IActionResult> ChangeToRootAdminAsync([FromRoute] Guid toRootAdminId)
    {
        var adminId = _userService.GetUserIdentifier(User);
        return Ok(await _handler.HandleChangeToRootAdminAsync(adminId, toRootAdminId));
    }

    //Endpoint para deletar o admin autenticado.
    //Administradores autenticados podem acessar.
    [HttpDelete]
    public async Task<IActionResult> DeleteAsync()
    {
        var adminId = _userService.GetUserIdentifier(User);
        return Ok(await _handler.HandleDeleteAsync(adminId));
    }

    //Endpoint para editar o admin autenticado.
    //Administradores autenticados podem acessar.
    [HttpPut]
    public async Task<IActionResult> PutAsync([FromBody] UpdateUser model)
    {
        var adminId = _userService.GetUserIdentifier(User);
        return Ok(await _handler.HandleUpdateAsync(model, adminId));
    }

    //Editar nome do administrador.
    //Administradores autenticados podem acessar.
    [HttpPatch("name")]
    public async Task<IActionResult> UpdateNameAsync([FromBody] Name name)
    {
        var adminId = _userService.GetUserIdentifier(User);
        return Ok(await _handler.HandleUpdateNameAsync(adminId, name));
    }

    //Editar email do administrador.
    //Administradores autenticados podem acessar.
    [HttpPatch("email")]
    public async Task<IActionResult> UpdateEmailAsync([FromBody] Email email)
    {
        var adminId = _userService.GetUserIdentifier(User);
        return Ok(await _handler.HandleUpdateEmailAsync(adminId, email));
    }

    //Editar telefone do administrador.
    //Administradores autenticados podem acessar.
    [HttpPatch("phone")]
    public async Task<IActionResult> UpdatePhoneAsync([FromBody] Phone phone)
    {
        var adminId = _userService.GetUserIdentifier(User);
        return Ok(await _handler.HandleUpdatePhoneAsync(adminId, phone));
    }

    //Editar endereço do administrador.
    //Administradores autenticados podem acessar.
    [HttpPatch("address")]
    public async Task<IActionResult> UpdateAddressAsync([FromBody] Address address)
    {
        var adminId = _userService.GetUserIdentifier(User);
        return Ok(await _handler.HandleUpdateAddressAsync(adminId, address));
    }

    //Editar gênero do administrador.
    //Administradores autenticados podem acessar.
    [HttpPatch("gender")]
    public async Task<IActionResult> UpdateGenderAsync([FromBody] UpdateGender updateGender)
    {
        var adminId = _userService.GetUserIdentifier(User);
        return Ok(await _handler.HandleUpdateGenderAsync(adminId, (EGender)updateGender.Gender));
    }

    //Editar data de nascimento do administrador.
    //Administradores autenticados podem acessar.
    [HttpPatch("date-of-birth")]
    public async Task<IActionResult> UpdateDateOfBirthAsync([FromBody] UpdateDateOfBirth newDateOfBirth)
    {
        var adminId = _userService.GetUserIdentifier(User);
        return Ok(await _handler.HandleUpdateDateOfBirthAsync(adminId, newDateOfBirth.DateOfBirth));
    }
}