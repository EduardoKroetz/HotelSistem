using Hotel.Domain.DTOs.ReservationContext.ReservationDTOs;
using Hotel.Domain.Handlers.ReservationContext.ReservationHandlers;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Domain.Controllers.ReservationContext;

public class ReservationController : ControllerBase
{
  private readonly ReservationHandler _handler;

  public ReservationController(ReservationHandler handler)
  => _handler = handler;


  [HttpGet("v1/reservations")]
  public async Task<IActionResult> GetAsync(
  [FromBody] ReservationQueryParameters queryParameters)
  => Ok(await _handler.HandleGetAsync(queryParameters));

  [HttpGet("v1/reservations/{Id:guid}")]
  public async Task<IActionResult> GetByIdAsync(
    [FromRoute] Guid id
  )
  => Ok(await _handler.HandleGetByIdAsync(id));


  [HttpPost("v1/reservations")]
  public async Task<IActionResult> PostAsync(
    [FromBody] CreateReservation model
  )
  => Ok(await _handler.HandleCreateAsync(model));


  [HttpDelete("v1/reservations/{Id:guid}")]
  public async Task<IActionResult> DeleteAsync(
    [FromRoute] Guid id
  )
  => Ok(await _handler.HandleDeleteAsync(id));

  [HttpPatch("v1/reservations/{Id:guid}/checkout")]
  public async Task<IActionResult> UpdateCheckoutAsync(
    [FromRoute] Guid id,
    [FromBody] UpdateCheckOut updateCheckOut
  )
  => Ok(await _handler.HandleUpdateCheckOutAsync(id, updateCheckOut.CheckOut));

  [HttpPost("v1/reservations/{Id:guid}/services/{serviceId:guid}")]
  public async Task<IActionResult> AddServiceAsync(
    [FromRoute] Guid id,
    [FromRoute] Guid serviceId
  )
  => Ok(await _handler.HandleAddServiceAsync(id, serviceId));

  //Somente administradores
  [HttpDelete("v1/reservations/{Id:guid}/services/{serviceid:guid}")]
  public async Task<IActionResult> RemoveServiceAsync(
    [FromRoute] Guid id,
    [FromRoute] Guid serviceId
  )
  => Ok(await _handler.HandleRemoveServiceAsync(id, serviceId));

  [HttpPatch("v1/reservations/{Id:guid}/checkin")]
  public async Task<IActionResult> UpdateCheckInAsync(
    [FromRoute] Guid id,
    [FromBody] UpdateCheckIn updateCheckIn
  )
  => Ok(await _handler.HandleUpdateCheckInAsync(id, updateCheckIn.CheckIn));
}