using Hotel.Domain.Attributes;
using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.Enums;
using Hotel.Domain.Handlers.CustomerHandlers;
using Hotel.Domain.Services.UserServices.Interfaces;
using Hotel.Domain.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Domain.Controllers;

[ApiController]
[Route("v1/customers")]
[Authorize(Roles = "RootAdmin,Admin,Employee,Customer")]
public class CustomerController : ControllerBase
{
    private readonly CustomerHandler _handler;
    private readonly IUserService _userService;

    public CustomerController(CustomerHandler handler, IUserService userService)
    {
        _handler = handler;
        _userService = userService;
    }

    // Endpoint para buscar clientes
    [HttpGet]
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
      [FromQuery] string? createdAtOperator
    )
    {
        var queryParameters = new UserQueryParameters(
          skip, take, name, email, phone, gender, dateOfBirth,
          dateOfBirthOperator, createdAt, createdAtOperator
        );

        return Ok(await _handler.HandleGetAsync(queryParameters));
    }


    // Endpoint para buscar cliente por ID
    [HttpGet("{Id:guid}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
      => Ok(await _handler.HandleGetByIdAsync(id));

    // Endpoint para editar cliente (somente para administradores)
    [HttpPut("{Id:guid}")]
    [AuthorizePermissions([EPermissions.EditCustomer, EPermissions.DefaultAdminPermission])]
    public async Task<IActionResult> EditCustomerAsync([FromBody] UpdateUser model, [FromRoute] Guid id)
      => Ok(await _handler.HandleUpdateAsync(model, id));

    // Endpoint para deletar cliente (somente para administradores)
    [HttpDelete("{Id:guid}")]
    [AuthorizePermissions([EPermissions.DeleteCustomer, EPermissions.DefaultAdminPermission])]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
      => Ok(await _handler.HandleDeleteAsync(id));

    // Endpoint para editar o próprio cliente (usando ID do token)
    [HttpPut]
    public async Task<IActionResult> EditAsync([FromBody] UpdateUser model)
    {
        var customerId = _userService.GetUserIdentifier(User);
        return Ok(await _handler.HandleUpdateAsync(model, customerId));
    }

    // Endpoint para deletar o próprio cliente (usando ID do token)
    [HttpDelete]
    public async Task<IActionResult> DeleteAsync()
    {
        var customerId = _userService.GetUserIdentifier(User);
        return Ok(await _handler.HandleDeleteAsync(customerId));
    }

    // Endpoint para editar o nome do cliente (usando ID do token)
    [HttpPatch("name")]
    public async Task<IActionResult> UpdateNameAsync([FromBody] Name name)
    {
        var customerId = _userService.GetUserIdentifier(User);
        return Ok(await _handler.HandleUpdateNameAsync(customerId, name));
    }

    // Endpoint para editar o e-mail do cliente (usando ID do token)
    [HttpPatch("email")]
    public async Task<IActionResult> UpdateEmailAsync([FromBody] Email email)
    {
        var customerId = _userService.GetUserIdentifier(User);
        return Ok(await _handler.HandleUpdateEmailAsync(customerId, email));
    }

    // Endpoint para editar o telefone do cliente (usando ID do token)
    [HttpPatch("phone")]
    public async Task<IActionResult> UpdatePhoneAsync([FromBody] Phone phone)
    {
        var customerId = _userService.GetUserIdentifier(User);
        return Ok(await _handler.HandleUpdatePhoneAsync(customerId, phone));
    }

    // Endpoint para editar o endereço do cliente (usando ID do token)
    [HttpPatch("address")]
    public async Task<IActionResult> UpdateAddressAsync([FromBody] Address address)
    {
        var customerId = _userService.GetUserIdentifier(User);
        return Ok(await _handler.HandleUpdateAddressAsync(customerId, address));
    }

    // Endpoint para editar o gênero do cliente (usando ID do token)
    [HttpPatch("gender/{gender:int}")]
    public async Task<IActionResult> UpdateGenderAsync([FromRoute] int gender)
    {
        var customerId = _userService.GetUserIdentifier(User);
        return Ok(await _handler.HandleUpdateGenderAsync(customerId, (EGender)gender));
    }

    // Endpoint para editar a data de nascimento do cliente (usando ID do token)
    [HttpPatch("date-of-birth")]
    public async Task<IActionResult> UpdateDateOfBirthAsync([FromBody] UpdateDateOfBirth newDateOfBirth)
    {
        var customerId = _userService.GetUserIdentifier(User);
        return Ok(await _handler.HandleUpdateDateOfBirthAsync(customerId, newDateOfBirth.DateOfBirth));
    }
}
