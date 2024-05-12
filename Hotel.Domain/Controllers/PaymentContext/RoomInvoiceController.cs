using Hotel.Domain.DTOs.Base;
using Hotel.Domain.DTOs.PaymentContext.RoomInvoiceDTOs;
using Hotel.Domain.Handlers.PaymentContext.RoomInvoiceHandlers;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Domain.Controllers.PaymentContext;

public class RoomInvoiceController : ControllerBase
{
  private readonly RoomInvoiceHandler _handler;

  public RoomInvoiceController(RoomInvoiceHandler handler)
  {
    _handler = handler;
  }

  [HttpGet("v1/room-invoices")]
  public async Task<IActionResult> GetAsync(
  [FromBody] RoomInvoiceQueryParameters queryParameters)
  => Ok(await _handler.HandleGetAsync(queryParameters));

  [HttpGet("v1/room-invoices/{Id:guid}")]
  public async Task<IActionResult> GetByIdAsync(
    [FromRoute] Guid id
  )
  => Ok(await _handler.HandleGetByIdAsync(id));


  [HttpPost("v1/room-invoices")]
  public async Task<IActionResult> PostAsync(
    [FromBody] CreateRoomInvoice model
  )
  => Ok(await _handler.HandleCreateAsync(model));


  [HttpDelete("v1/room-invoices/{Id:guid}")]
  public async Task<IActionResult> DeleteAsync(
    [FromRoute] Guid id
  )
  => Ok(await _handler.HandleDeleteAsync(id));

}