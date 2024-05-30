using Hotel.Domain.Attributes;
using Hotel.Domain.DTOs.ReservationContext.ReservationDTOs;
using Hotel.Domain.Enums;
using Hotel.Domain.Handlers.ReservationContext.ReservationHandlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Domain.Controllers.ReservationContext;

[ApiController]
[Route("v1/reservations")]
[Authorize(Roles = "RootAdmin,Admin,Employee,Customer")]
public class ReservationController : ControllerBase
{
  private readonly ReservationHandler _handler;

  public ReservationController(ReservationHandler handler)
  => _handler = handler;

  [HttpGet]
  [AuthorizePermissions([EPermissions.GetReservations, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission])]
  public async Task<IActionResult> GetAsync(
  [FromBody] ReservationQueryParameters queryParameters)
    => Ok(await _handler.HandleGetAsync(queryParameters));

  [HttpGet("{Id:guid}")]
  public async Task<IActionResult> GetByIdAsync(
    [FromRoute] Guid id)
    => Ok(await _handler.HandleGetByIdAsync(id));

  [HttpPost]
  [Authorize(Roles = "Customer")]
  public async Task<IActionResult> PostAsync(
    [FromBody] CreateReservation model)
    => Ok(await _handler.HandleCreateAsync(model));

  [HttpDelete("{Id:guid}")]
  [AuthorizePermissions([EPermissions.DeleteReservation, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> DeleteAsync(
    [FromRoute] Guid id)
    => Ok(await _handler.HandleDeleteAsync(id));

  [HttpPatch("{Id:guid}/check-out")]
  [AuthorizePermissions([EPermissions.UpdateReservationCheckout, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission], [ERoles.Customer])]
  public async Task<IActionResult> UpdateCheckoutAsync(
    [FromRoute] Guid id,
    [FromBody] UpdateCheckOut updateCheckOut)
    => Ok(await _handler.HandleUpdateCheckOutAsync(id, updateCheckOut.CheckOut));

  [HttpPatch("{Id:guid}/check-in")]
  [AuthorizePermissions([EPermissions.UpdateReservationCheckIn, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission], [ERoles.Customer])]
  public async Task<IActionResult> UpdateCheckInAsync(
  [FromRoute] Guid id,
  [FromBody] UpdateCheckIn updateCheckIn)
  => Ok(await _handler.HandleUpdateCheckInAsync(id, updateCheckIn.CheckIn));

  [HttpPost("{Id:guid}/services/{serviceId:guid}")]
  [AuthorizePermissions([EPermissions.AddServiceToReservation, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission])]
  public async Task<IActionResult> AddServiceAsync(
    [FromRoute] Guid id,
    [FromRoute] Guid serviceId)
    => Ok(await _handler.HandleAddServiceAsync(id, serviceId));

  [HttpDelete("{Id:guid}/services/{serviceid:guid}")]
  [AuthorizePermissions([EPermissions.RemoveServiceFromReservation, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> RemoveServiceAsync(
    [FromRoute] Guid id,
    [FromRoute] Guid serviceId)
    => Ok(await _handler.HandleRemoveServiceAsync(id, serviceId));


}