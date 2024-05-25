using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.DTOs.EmployeeContext.EmployeeDTOs;
using Hotel.Domain.Enums;
using Hotel.Domain.Handlers.EmployeeContexty.EmployeeHandlers;
using Hotel.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Domain.Controllers.EmployeeContext;

[Route("v1/employees")]
public class EmployeeController : ControllerBase
{
  private readonly EmployeeHandler _handler;

  public EmployeeController(EmployeeHandler handler)
  => _handler = handler;

  [HttpGet]
  public async Task<IActionResult> GetAsync(
    [FromBody] EmployeeQueryParameters queryParameters)
    => Ok(await _handler.HandleGetAsync(queryParameters));

  [HttpGet("{id:guid}")]
  public async Task<IActionResult> GetByIdAsync(
    [FromRoute] Guid id)
    => Ok(await _handler.HandleGetByIdAsync(id));

  [HttpPut("{id:guid}")]
  public async Task<IActionResult> PutAsync(
    [FromBody] UpdateEmployee model,
    [FromRoute] Guid id)
    => Ok(await _handler.HandleUpdateAsync(model, id));

  [HttpDelete("{id:guid}")]
  public async Task<IActionResult> DeleteAsync(
    [FromRoute] Guid id)
    => Ok(await _handler.HandleDeleteAsync(id));

  [HttpPost("{id:guid}/responsabilities/{resId:guid}")]
  public async Task<IActionResult> AssignResponsibilityAsync(
    [FromRoute] Guid id,
    [FromRoute] Guid resId)
    => Ok(await _handler.HandleAssignResponsabilityAsync(id, resId));

  [HttpDelete("{id:guid}/responsabilities/{resId:guid}")]
  public async Task<IActionResult> UnassignResponsibilityAsync(
    [FromRoute] Guid id,
    [FromRoute] Guid resId)
    => Ok(await _handler.HandleUnassignResponsabilityAsync(id, resId));

  [HttpPatch("{employeeId:guid}/name")]
  public async Task<IActionResult> UpdateNameAsync(
    [FromRoute] Guid employeeId,
    [FromBody] Name name)
    => Ok(await _handler.HandleUpdateNameAsync(employeeId, name));

  [HttpPatch("{employeeId:guid}/email")]
  public async Task<IActionResult> UpdateEmailAsync(
    [FromRoute] Guid employeeId,
    [FromBody] Email email)
    => Ok(await _handler.HandleUpdateEmailAsync(employeeId, email));

  [HttpPatch("{employeeId:guid}/phone")]
  public async Task<IActionResult> UpdatePhoneAsync(
    [FromRoute] Guid employeeId,
    [FromBody] Phone phone)
    => Ok(await _handler.HandleUpdatePhoneAsync(employeeId, phone));

  [HttpPatch("{employeeId:guid}/address")]
  public async Task<IActionResult> UpdateAddressAsync(
    [FromRoute] Guid employeeId,
    [FromBody] Address address)
    => Ok(await _handler.HandleUpdateAddressAsync(employeeId, address));

  [HttpPatch("{employeeId:guid}/gender/{gender:int}")]
  public async Task<IActionResult> UpdateGenderAsync(
    [FromRoute] Guid employeeId,
    [FromRoute] int gender)
    => Ok(await _handler.HandleUpdateGenderAsync(employeeId, (EGender)gender));

  [HttpPatch("{employeeId:guid}/date-of-birth")]
  public async Task<IActionResult> UpdateDateOfBirthAsync(
    [FromRoute] Guid employeeId,
    [FromBody] UpdateDateOfBirth newDateOfBirth)
    => Ok(await _handler.HandleUpdateDateOfBirthAsync(employeeId, newDateOfBirth.DateOfBirth));
}
