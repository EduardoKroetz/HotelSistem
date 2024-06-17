using Hotel.Domain.Attributes;
using Hotel.Domain.DTOs.RoomContext.CategoryDTOs;
using Hotel.Domain.Enums;
using Hotel.Domain.Handlers.RoomContext.CategoryHandlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Domain.Controllers.RoomContext;

[ApiController]
[Route("v1/categories")]
[Authorize(Roles = "RootAdmin,Admin,Employee,Customer")]
public class CategoryController : ControllerBase
{
  private readonly CategoryHandler _handler;

  public CategoryController(CategoryHandler handler)
  => _handler = handler;

  // Endpoint para buscar as categorias
  [HttpGet]
  public async Task<IActionResult> GetAsync(
    [FromQuery] int? skip,
    [FromQuery] int? take,
    [FromQuery] string? name,
    [FromQuery] decimal? averagePrice,
    [FromQuery] string? averagePriceOperator,
    [FromQuery] Guid? roomId
  )
  {
    var queryParameters = new CategoryQueryParameters(
        skip, take, name, averagePrice, averagePriceOperator, roomId
    );

    return Ok(await _handler.HandleGetAsync(queryParameters));
  }


  // Endpoint para buscar uma categoria por ID
  [HttpGet("{Id:guid}")]
  public async Task<IActionResult> GetByIdAsync(
    [FromRoute]Guid id)
    => Ok(await _handler.HandleGetByIdAsync(id));

  // Endpoint para atualizar uma categoria (acesso com permissão)
  [HttpPut("{Id:guid}")]
  [AuthorizePermissions([EPermissions.EditCategory, EPermissions.DefaultEmployeePermission, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> PutAsync(
    [FromBody]EditorCategory model,
    [FromRoute]Guid id)
    => Ok(await _handler.HandleUpdateAsync(model,id));

  // Endpoint para criar uma nova categoria (acesso com permissão)
  [HttpPost]
  [AuthorizePermissions([EPermissions.CreateCategory, EPermissions.DefaultEmployeePermission, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> PostAsync(
    [FromBody]EditorCategory model)
    => Ok(await _handler.HandleCreateAsync(model));

  // Endpoint para deletar uma categoria (acesso com permissão)
  [HttpDelete("{Id:guid}")]
  [AuthorizePermissions([EPermissions.DeleteCategory, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> DeleteAsync(
    [FromRoute]Guid id)
    => Ok(await _handler.HandleDeleteAsync(id));
  
}