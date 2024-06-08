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

  // Endpoint para buscar todos os quartos
  [HttpGet]
  public async Task<IActionResult> GetAsync([FromBody] RoomQueryParameters queryParameters)
    => Ok(await _handler.HandleGetAsync(queryParameters));

  // Endpoint para buscar um quarto por ID
  [HttpGet("{Id:guid}")]
  public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    => Ok(await _handler.HandleGetByIdAsync(id));

  // Endpoint para atualizar um quarto (com permissão)
  [HttpPut("{Id:guid}")]
  [AuthorizePermissions([EPermissions.EditRoom, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission])]
  public async Task<IActionResult> PutAsync([FromBody] EditorRoom model, [FromRoute] Guid id)
    => Ok(await _handler.HandleUpdateAsync(model, id));

  // Endpoint para criar um novo quarto (com permissão)
  [HttpPost]
  [AuthorizePermissions([EPermissions.CreateRoom, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission])]
  public async Task<IActionResult> PostAsync([FromBody] EditorRoom model)
    => Ok(await _handler.HandleCreateAsync(model));

  // Endpoint para deletar um quarto (com permissão)
  [HttpDelete("{Id:guid}")]
  [AuthorizePermissions([EPermissions.DeleteRoom, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission])]
  public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    => Ok(await _handler.HandleDeleteAsync(id));

  // Endpoint para adicionar um serviço a um quarto (com permissão)
  [HttpPost("{Id:guid}/services/{serviceId:guid}")]
  [AuthorizePermissions([EPermissions.AddRoomService, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission])]
  public async Task<IActionResult> AddServiceAsync([FromRoute] Guid id, [FromRoute] Guid serviceId)
    => Ok(await _handler.HandleAddServiceAsync(id, serviceId));

  // Endpoint para remover um serviço de um quarto (com permissão)
  [HttpDelete("{Id:guid}/services/{serviceId:guid}")]
  [AuthorizePermissions([EPermissions.RemoveRoomService, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission])]
  public async Task<IActionResult> RemoveServiceAsync([FromRoute] Guid id, [FromRoute] Guid serviceId)
    => Ok(await _handler.HandleRemoveServiceAsync(id, serviceId));

  // Endpoint para atualizar o número de um quarto (com permissão)
  [HttpPatch("{id:guid}/number/{number:int}")]
  [AuthorizePermissions([EPermissions.UpdateRoomNumber, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission])]
  public async Task<IActionResult> UpdateNumberAsync([FromRoute] Guid id, [FromRoute] int number)
    => Ok(await _handler.HandleUpdateNumberAsync(id, number));

  // Endpoint para atualizar a capacidade de um quarto (com permissão)
  [HttpPatch("{id:guid}/capacity/{capacity:int}")]
  [AuthorizePermissions([EPermissions.UpdateRoomCapacity, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission])]
  public async Task<IActionResult> UpdateCapacityAsync([FromRoute] Guid id, [FromRoute] int capacity)
    => Ok(await _handler.HandleUpdateCapacityAsync(id, capacity));

  // Endpoint para atualizar a categoria de um quarto (com permissão)
  [HttpPatch("{id:guid}/categories/{categoryId:guid}")]
  [AuthorizePermissions([EPermissions.UpdateRoomCategory, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission])]
  public async Task<IActionResult> UpdateCategoryAsync([FromRoute] Guid id, [FromRoute] Guid categoryId)
    => Ok(await _handler.HandleUpdateCategoryAsync(id, categoryId));

  // Endpoint para atualizar o preço de um quarto (com permissão)
  [HttpPatch("{id:guid}/price/{price:decimal}")]
  [AuthorizePermissions([EPermissions.UpdateRoomPrice, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission])]
  public async Task<IActionResult> UpdatePriceAsync([FromRoute] Guid id, [FromRoute] decimal price)
    => Ok(await _handler.HandleUpdatePriceAsync(id, price));

  // Endpoint para ativar um quarto
  [HttpPatch("enable/{id:guid}")]
  [AuthorizePermissions([EPermissions.EnableRoom, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission])]
  public async Task<IActionResult> EnableRoomAsync([FromRoute] Guid id)
    => Ok(await _handler.HandleEnableRoom(id));

  // Endpoint para desativar um quarto
  [HttpPatch("disable/{id:guid}")]
  [AuthorizePermissions([EPermissions.DisableRoom, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission])]
  public async Task<IActionResult> DisableRoomAsync([FromRoute] Guid id)
    => Ok(await _handler.HandleDisableRoom(id));

  [HttpPatch("available/{id:guid}")]
  [AuthorizePermissions([EPermissions.AvailableRoomStatus, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission])]
  public async Task<IActionResult> UpdateToAvailableRoomStatusAsync([FromRoute] Guid id)
    => Ok(await _handler.HandleChangeToAvailableStatusAsync(id));

}
