using Hotel.Domain.Attributes;
using Hotel.Domain.DTOs.PaymentContext.RoomInvoiceDTOs;
using Hotel.Domain.Enums;
using Hotel.Domain.Handlers.PaymentContext.RoomInvoiceHandlers;
using Hotel.Domain.Services.UserServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Domain.Controllers.PaymentContext;

[Route("v1/room-invoices")]
[Authorize(Roles = "RootAdmin,Admin,Employee,Customer")] //Acesso a todos os usuários
public class RoomInvoiceController : ControllerBase
{
  private readonly RoomInvoiceHandler _handler;
  private readonly IUserService _userService;

  public RoomInvoiceController(RoomInvoiceHandler handler, IUserService userService)
  {
    _handler = handler;
    _userService = userService;
  }

  //Buscar faturas de quarto. Somente administradores ou funcionários com permissão possuem acesso.
  [HttpGet]
  [AuthorizePermissions([EPermissions.GetRoomInvoices, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission])]
  public async Task<IActionResult> GetAsync([FromBody] RoomInvoiceQueryParameters queryParameters)
    => Ok(await _handler.HandleGetAsync(queryParameters));

  //Buscar minhas faturas de quarto. Qualquer usuário tem acesso, apesar de que somente clientes podem criar faturas.
  [HttpGet("my")]
  public async Task<IActionResult> GetMyRoomInvoicesAsync(
  [FromBody] RoomInvoiceQueryParameters queryParameters)
  {
    var userId = _userService.GetUserIdentifier(User);
    queryParameters.CustomerId = userId; //Vai filtrar pelo Id do cliente, ainda podendo aplicar o restante dos filtros.
    return Ok(await _handler.HandleGetAsync(queryParameters));
  }

  //Buscar fatura de quarto pelo Id. Somente administradores ou funcionários com permissão possuem acesso.
  [HttpGet("{Id:guid}")]
  [AuthorizePermissions([EPermissions.GetRoomInvoice, EPermissions.DefaultAdminPermission,EPermissions.DefaultEmployeePermission])]
  public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    => Ok(await _handler.HandleGetByIdAsync(id));

  //Deletar fatura de quarto. Somente administradores ou funcionários com permissão possuem acesso.
  [HttpDelete("{Id:guid}")]
  [AuthorizePermissions([EPermissions.DeleteRoomInvoice, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    => Ok(await _handler.HandleDeleteAsync(id));

}