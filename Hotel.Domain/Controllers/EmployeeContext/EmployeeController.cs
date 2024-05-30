using Hotel.Domain.Attributes;
using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.DTOs.EmployeeContext.EmployeeDTOs;
using Hotel.Domain.Enums;
using Hotel.Domain.Handlers.EmployeeContext.EmployeeHandlers;
using Hotel.Domain.Services.Users;
using Hotel.Domain.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Domain.Controllers.EmployeeContext;

[Route("v1/employees")]
[Authorize(Roles = "RootAdmin,Admin,Employee")]
public class EmployeeController : ControllerBase
{
  private readonly EmployeeHandler _handler;

  public EmployeeController(EmployeeHandler handler)
  => _handler = handler;

  //Buscar todos os funcionários
  [HttpGet]
  [AuthorizePermissions([EPermissions.GetEmployees, EPermissions.DefaultEmployeePermission, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> GetAsync([FromBody] EmployeeQueryParameters queryParameters)
    => Ok(await _handler.HandleGetAsync(queryParameters));

  //Buscar funcionário por Id
  [HttpGet("{id:guid}")]
  [AuthorizePermissions([EPermissions.GetEmployee, EPermissions.DefaultEmployeePermission, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    => Ok(await _handler.HandleGetByIdAsync(id));

  //Editar funcionário
  [HttpPut("{id:guid}")]
  [AuthorizePermissions([EPermissions.EditEmployee, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> AdminEditAsync([FromBody] UpdateEmployee model,[FromRoute] Guid id)
    => Ok(await _handler.HandleUpdateAsync(model, id));

  //atribuir responsabilidade a um funcionário
  [HttpPost("{id:guid}/responsabilities/{resId:guid}")]
  [AuthorizePermissions([EPermissions.AssignEmployeeResponsability, EPermissions.DefaultAdminPermission])] //Admin padrão tem acesso
  public async Task<IActionResult> AssignResponsibilityAsync([FromRoute] Guid id,[FromRoute] Guid resId)
    => Ok(await _handler.HandleAssignResponsabilityAsync(id, resId));

  //desatribuir responsabilidade de um funcionário
  [HttpDelete("{id:guid}/responsabilities/{resId:guid}")]
  [AuthorizePermissions([EPermissions.UnassignEmployeeResponsability, EPermissions.DefaultAdminPermission])] //Admin padrão tem acesso
  public async Task<IActionResult> UnassignResponsibilityAsync([FromRoute] Guid id,[FromRoute] Guid resId)
    => Ok(await _handler.HandleUnassignResponsabilityAsync(id, resId));

  //Atribuir permissão a um funcionário
  [HttpPost("{employeeId:guid}/permissions/{permissionId:guid}")]
  [AuthorizePermissions([EPermissions.AdminAssignPermission, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> AssignPermissionAsync([FromRoute] Guid employeeId,[FromRoute] Guid permissionId)
    => Ok(await _handler.HandleAssignPermission(employeeId, permissionId));

  //Desatribuir permissão de um funcionário
  [HttpDelete("{employeeId:guid}/permissions/{permissionId:guid}")]
  [AuthorizePermissions([EPermissions.UnassignEmployeePermission, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> UnassignPermissionAsync([FromRoute] Guid employeeId,[FromRoute] Guid permissionId)
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
    var userId = UserServices.GetIdFromClaim(User);
    return Ok(await _handler.HandleDeleteAsync(userId));
  }

  //Editar funcionário autenticado
  [HttpPut]
  public async Task<IActionResult> EditAsync([FromBody] UpdateEmployee model)
  {
    var userId = UserServices.GetIdFromClaim(User);
    return Ok(await _handler.HandleUpdateAsync(model, userId));
  }

  //Editar campos de um funcionário autenticado

  [HttpPatch("name")]
  public async Task<IActionResult> UpdateNameAsync([FromBody] Name name)
  {
    var userId = UserServices.GetIdFromClaim(User);
    return Ok(await _handler.HandleUpdateNameAsync(userId, name));
  }

  [HttpPatch("email")]
  public async Task<IActionResult> UpdateEmailAsync([FromBody] Email email)
  {
    var userId = UserServices.GetIdFromClaim(User);
    return Ok(await _handler.HandleUpdateEmailAsync(userId, email));
  }

  [HttpPatch("phone")]
  public async Task<IActionResult> UpdatePhoneAsync([FromBody] Phone phone)
  {
    var userId = UserServices.GetIdFromClaim(User);
    return Ok(await _handler.HandleUpdatePhoneAsync(userId, phone));
  }

  [HttpPatch("address")]
  public async Task<IActionResult> UpdateAddressAsync([FromBody] Address address)
  {
    var userId = UserServices.GetIdFromClaim(User);
    return Ok(await _handler.HandleUpdateAddressAsync(userId, address));
  }

  [HttpPatch("gender/{gender:int}")]
  public async Task<IActionResult> UpdateGenderAsync([FromRoute] int gender)
  {
    var userId = UserServices.GetIdFromClaim(User);
    return Ok(await _handler.HandleUpdateGenderAsync(userId, (EGender)gender));
    }

  [HttpPatch("date-of-birth")]
  public async Task<IActionResult> UpdateDateOfBirthAsync([FromBody] UpdateDateOfBirth newDateOfBirth)
  {
    var userId = UserServices.GetIdFromClaim(User);
    return Ok(await _handler.HandleUpdateDateOfBirthAsync(userId, newDateOfBirth.DateOfBirth));
  }
}
