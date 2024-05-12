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

}