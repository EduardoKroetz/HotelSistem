using Hotel.Domain.Attributes;
using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.DTOs.EmployeeDTOs;
using Hotel.Domain.Enums;
using Hotel.Domain.Handlers.EmployeeHandlers;
using Hotel.Domain.Services.UserServices.Interfaces;
using Hotel.Domain.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Domain.Controllers;

[Route("v1/employees")]
[Authorize(Roles = "RootAdmin,Admin,Employee")]
public class EmployeeController : ControllerBase
{
    private readonly EmployeeHandler _handler;
    private readonly IUserService _userService;

    public EmployeeController(EmployeeHandler handler, IUserService userService)
    {
        _handler = handler;
        _userService = userService;
    }


    //Buscar todos os funcionários
    [HttpGet]
    [AuthorizePermissions([EPermissions.GetEmployees, EPermissions.DefaultEmployeePermission, EPermissions.DefaultAdminPermission])]
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
      [FromQuery] decimal? salary,
      [FromQuery] string? salaryOperator
    )
    {
        var queryParameters = new EmployeeQueryParameters
        {
            Skip = skip,
            Take = take,
            Name = name,
            Email = email,
            Phone = phone,
            Gender = gender,
            DateOfBirth = dateOfBirth,
            DateOfBirthOperator = dateOfBirthOperator,
            CreatedAt = createdAt,
            CreatedAtOperator = createdAtOperator,
            Salary = salary,
            SalaryOperator = salaryOperator
        };


        return Ok(await _handler.HandleGetAsync(queryParameters));
    }


    //Buscar funcionário por Id
    [HttpGet("{id:guid}")]
    [AuthorizePermissions([EPermissions.GetEmployee, EPermissions.DefaultEmployeePermission, EPermissions.DefaultAdminPermission])]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
      => Ok(await _handler.HandleGetByIdAsync(id));

    //Editar funcionário
    [HttpPut("{id:guid}")]
    [AuthorizePermissions([EPermissions.EditEmployee, EPermissions.DefaultAdminPermission])]
    public async Task<IActionResult> AdminEditAsync([FromBody] UpdateEmployee model, [FromRoute] Guid id)
      => Ok(await _handler.HandleUpdateAsync(model, id));

    //atribuir responsabilidade a um funcionário
    [HttpPost("{id:guid}/responsibilities/{resId:guid}")]
    [AuthorizePermissions([EPermissions.AssignEmployeeResponsibility, EPermissions.DefaultAdminPermission])] //Admin padrão tem acesso
    public async Task<IActionResult> AssignResponsibilityAsync([FromRoute] Guid id, [FromRoute] Guid resId)
      => Ok(await _handler.HandleAssignResponsibilityAsync(id, resId));

    //desatribuir responsabilidade de um funcionário
    [HttpDelete("{id:guid}/responsibilities/{resId:guid}")]
    [AuthorizePermissions([EPermissions.UnassignEmployeeResponsibility, EPermissions.DefaultAdminPermission])] //Admin padrão tem acesso
    public async Task<IActionResult> UnassignResponsibilityAsync([FromRoute] Guid id, [FromRoute] Guid resId)
      => Ok(await _handler.HandleUnassignResponsibilityAsync(id, resId));

    //Atribuir permissão a um funcionário
    [HttpPost("{employeeId:guid}/permissions/{permissionId:guid}")]
    [AuthorizePermissions([EPermissions.AssignEmployeePermission, EPermissions.DefaultAdminPermission])]
    public async Task<IActionResult> AssignPermissionAsync([FromRoute] Guid employeeId, [FromRoute] Guid permissionId)
      => Ok(await _handler.HandleAssignPermission(employeeId, permissionId));

    //Desatribuir permissão de um funcionário
    [HttpDelete("{employeeId:guid}/permissions/{permissionId:guid}")]
    [AuthorizePermissions([EPermissions.UnassignEmployeePermission, EPermissions.DefaultAdminPermission])]
    public async Task<IActionResult> UnassignPermissionAsync([FromRoute] Guid employeeId, [FromRoute] Guid permissionId)
      => Ok(await _handler.HandleUnassignPermission(employeeId, permissionId));

    //Deletar funcionário
    [HttpDelete("{id:guid}")]
    [AuthorizePermissions([EPermissions.DeleteEmployee, EPermissions.DefaultAdminPermission])]
    public async Task<IActionResult> AdminDeleteAsync([FromRoute] Guid id)
      => Ok(await _handler.HandleDeleteAsync(id));

    //Deletar funcionário autenticado
    [HttpDelete]
    public async Task<IActionResult> DeleteAsync()
    {
        var userId = _userService.GetUserIdentifier(User);
        return Ok(await _handler.HandleDeleteAsync(userId));
    }

    //Editar funcionário autenticado
    [HttpPut]
    public async Task<IActionResult> EditAsync([FromBody] UpdateEmployee model)
    {
        var userId = _userService.GetUserIdentifier(User);
        return Ok(await _handler.HandleUpdateAsync(model, userId));
    }

    //Editar campos de um funcionário autenticado

    [HttpPatch("name")]
    public async Task<IActionResult> UpdateNameAsync([FromBody] Name name)
    {
        var userId = _userService.GetUserIdentifier(User);
        return Ok(await _handler.HandleUpdateNameAsync(userId, name));
    }

    [HttpPatch("email")]
    public async Task<IActionResult> UpdateEmailAsync([FromBody] Email email)
    {
        var userId = _userService.GetUserIdentifier(User);
        return Ok(await _handler.HandleUpdateEmailAsync(userId, email));
    }

    [HttpPatch("phone")]
    public async Task<IActionResult> UpdatePhoneAsync([FromBody] Phone phone)
    {
        var userId = _userService.GetUserIdentifier(User);
        return Ok(await _handler.HandleUpdatePhoneAsync(userId, phone));
    }

    [HttpPatch("address")]
    public async Task<IActionResult> UpdateAddressAsync([FromBody] Address address)
    {
        var userId = _userService.GetUserIdentifier(User);
        return Ok(await _handler.HandleUpdateAddressAsync(userId, address));
    }

    [HttpPatch("gender")]
    public async Task<IActionResult> UpdateGenderAsync([FromBody] UpdateGender updateGender)
    {
        var userId = _userService.GetUserIdentifier(User);
        return Ok(await _handler.HandleUpdateGenderAsync(userId, (EGender)updateGender.Gender));
    }

    [HttpPatch("date-of-birth")]
    public async Task<IActionResult> UpdateDateOfBirthAsync([FromBody] UpdateDateOfBirth newDateOfBirth)
    {
        var userId = _userService.GetUserIdentifier(User);
        return Ok(await _handler.HandleUpdateDateOfBirthAsync(userId, newDateOfBirth.DateOfBirth));
    }
}
