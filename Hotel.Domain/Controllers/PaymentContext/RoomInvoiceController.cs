using Hotel.Domain.Attributes;
using Hotel.Domain.DTOs.PaymentContext.RoomInvoiceDTOs;
using Hotel.Domain.Enums;
using Hotel.Domain.Handlers.PaymentContext.RoomInvoiceHandlers;
using Hotel.Domain.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Domain.Controllers.PaymentContext;

[Route("v1/room-invoices")]
[Authorize(Roles = "RootAdmin,Admin,Employee,Customer")]
public class RoomInvoiceController : ControllerBase
{
  private readonly RoomInvoiceHandler _handler;

  public RoomInvoiceController(RoomInvoiceHandler handler)
  => _handler = handler;

  [HttpGet]
  [AuthorizeRoleOrPermissions([EPermissions.GetRoomInvoices, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission])]
  public async Task<IActionResult> GetAsync(
    [FromBody] RoomInvoiceQueryParameters queryParameters)
    => Ok(await _handler.HandleGetAsync(queryParameters));

  [HttpGet("my")]
  public async Task<IActionResult> GetMyRoomInvoicesAsync(
  [FromBody] RoomInvoiceQueryParameters queryParameters)
  {
    var userId = UserServices.GetIdFromClaim(User);
    queryParameters.CustomerId = userId; //Vai filtrar pelo Id do cliente, ainda podendo aplicar o restante dos filtros.
    return Ok(await _handler.HandleGetAsync(queryParameters));
  }

  [HttpGet("{Id:guid}")]
  public async Task<IActionResult> GetByIdAsync(
    [FromRoute] Guid id)
    => Ok(await _handler.HandleGetByIdAsync(id));


  [HttpPost]
  [Authorize(Roles = "Customer")]
  public async Task<IActionResult> PostAsync(
    [FromBody] CreateRoomInvoice model)
    => Ok(await _handler.HandleCreateAsync(model));


  [HttpDelete("{Id:guid}")]
  [AuthorizeRoleOrPermissions([EPermissions.DeleteRoomInvoice, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> DeleteAsync(
    [FromRoute] Guid id)
    => Ok(await _handler.HandleDeleteAsync(id));

}