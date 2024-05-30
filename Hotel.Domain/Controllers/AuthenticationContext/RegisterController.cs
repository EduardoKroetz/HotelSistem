using Hotel.Domain.Attributes;
using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.DTOs.EmployeeContext.EmployeeDTOs;
using Hotel.Domain.Enums;
using Hotel.Domain.Handlers.AdminContext.AdminHandlers;
using Hotel.Domain.Handlers.CustomerContext.CustomerHandlers;
using Hotel.Domain.Handlers.EmployeeContext.EmployeeHandlers;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Domain.Controllers.AuthenticationContext;

[ApiController]
[Route("v1/register")]
public class RegisterController : ControllerBase
{
  private readonly AdminHandler _adminHandler;
  private readonly CustomerHandler _customerHandler;
  private readonly EmployeeHandler _employeeHandler;

  public RegisterController(AdminHandler adminHandler, CustomerHandler customerHandler, EmployeeHandler employeeHandler)
  {
    _adminHandler = adminHandler;
    _customerHandler = customerHandler;
    _employeeHandler = employeeHandler;
  }

  //Criar cliente
  [HttpPost("customers")]
  public async Task<IActionResult> RegisterCustomerAsync([FromBody] CreateUser customer)
    => Ok(await _customerHandler.HandleCreateAsync(customer));

  //Criar administrador
  [HttpPost("admins")]
  [AuthorizePermissions([EPermissions.CreateAdmin])]
  public async Task<IActionResult> RegisterAdminAsync([FromBody] CreateUser admin)
    => Ok(await _adminHandler.HandleCreateAsync(admin));

  //Criar funcionário
  [HttpPost("employees")]
  [AuthorizePermissions([EPermissions.CreateEmployee, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> RegisterEmployeeAsync([FromBody] CreateEmployee employee)
    => Ok(await _employeeHandler.HandleCreateAsync(employee));
}
