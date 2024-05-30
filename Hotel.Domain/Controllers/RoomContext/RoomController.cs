using Hotel.Domain.Attributes;
using Hotel.Domain.DTOs.RoomContext.RoomDTOs;
using Hotel.Domain.Enums;
using Hotel.Domain.Handlers.RoomContext.RoomHandlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Domain.Controllers.RoomContext;

[ApiController]
[Route("v1/rooms")]
[Authorize(Roles = "RootAdmin,Admin,Employee,Customer")]
public class RoomController : ControllerBase
{
  private readonly RoomHandler _handler;

  public RoomController(RoomHandler handler)
  => _handler = handler;

  [HttpGet]
  public async Task<IActionResult> GetAsync(
    [FromBody] RoomQueryParameters queryParameters)
    => Ok(await _handler.HandleGetAsync(queryParameters));

  [HttpGet("{Id:guid}")]
  public async Task<IActionResult> GetByIdAsync(
    [FromRoute] Guid id)
    => Ok(await _handler.HandleGetByIdAsync(id));

  [HttpPut("{Id:guid}")]
  [AuthorizeRoleOrPermissions([EPermissions.EditRoom, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission])]
  public async Task<IActionResult> PutAsync(
    [FromBody] EditorRoom model,
    [FromRoute] Guid id)
    => Ok(await _handler.HandleUpdateAsync(model, id));

  [HttpPost]
  [AuthorizeRoleOrPermissions([EPermissions.CreateRoom, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission])]
  public async Task<IActionResult> PostAsync(
    [FromBody] EditorRoom model)
    => Ok(await _handler.HandleCreateAsync(model));


  [HttpDelete("{Id:guid}")]
  [AuthorizeRoleOrPermissions([EPermissions.DeleteRoom, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission])]
  public async Task<IActionResult> DeleteAsync(
    [FromRoute] Guid id)
    => Ok(await _handler.HandleDeleteAsync(id));

  [HttpPost("{Id:guid}/services/{serviceId:guid}")]
  [AuthorizeRoleOrPermissions([EPermissions.AddServiceToRoom, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission])]
  public async Task<IActionResult> AddServiceAsync(
    [FromRoute] Guid id,
    [FromRoute] Guid serviceId)
    => Ok(await _handler.HandleAddServiceAsync(id, serviceId));

  [HttpDelete("{Id:guid}/services/{serviceId:guid}")]
  [AuthorizeRoleOrPermissions([EPermissions.RemoveServiceToRoom, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission])]
  public async Task<IActionResult> RemoveServiceAsync(
    [FromRoute] Guid id,
    [FromRoute] Guid serviceId)
    => Ok(await _handler.HandleRemoveServiceAsync(id, serviceId));


  [HttpPatch("{id:guid}/number/{number:int}")]
  [AuthorizeRoleOrPermissions([EPermissions.UpdateRoomNumber, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission])]
  public async Task<IActionResult> UpdateNumberAsync(
    [FromRoute] Guid id,
    [FromRoute] int number)
    => Ok(await _handler.HandleUpdateNumberAsync(id, number));

  [HttpPatch("{id:guid}/capacity/{capacity:int}")]
  [AuthorizeRoleOrPermissions([EPermissions.UpdateRoomCapacity, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission])]
  public async Task<IActionResult> UpdateCapacityAsync(
    [FromRoute] Guid id,
    [FromRoute] int capacity)
    => Ok(await _handler.HandleUpdateCapacityAsync(id, capacity));

  [HttpPatch("{id:guid}/categories/{categoryId:guid}")]
  [AuthorizeRoleOrPermissions([EPermissions.UpdateRoomCategory, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission])]
  public async Task<IActionResult> UpdateCategoryAsync(
    [FromRoute] Guid id,
    [FromRoute] Guid categoryId)
    => Ok(await _handler.HandleUpdateCategoryAsync(id, categoryId));

  [HttpPatch("{id:guid}/price/{price:decimal}")]
  [AuthorizeRoleOrPermissions([EPermissions.UpdateRoomNumber, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission])]
  public async Task<IActionResult> UpdatePriceAsync(
    [FromRoute] Guid id,
    [FromRoute] decimal price)
    => Ok(await _handler.HandleUpdatePriceAsync(id, price));
}