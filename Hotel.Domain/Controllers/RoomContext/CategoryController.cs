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

  [HttpGet]
  public async Task<IActionResult> GetAsync(
    [FromBody] CategoryQueryParameters queryParameters)
    => Ok(await _handler.HandleGetAsync(queryParameters));
  
  [HttpGet("{Id:guid}")]
  public async Task<IActionResult> GetByIdAsync(
    [FromRoute]Guid id)
    => Ok(await _handler.HandleGetByIdAsync(id));
  
  [HttpPut("{Id:guid}")]
  [AuthorizeRoleOrPermissions([EPermissions.EditCategory, EPermissions.DefaultEmployeePermission, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> PutAsync(
    [FromBody]EditorCategory model,
    [FromRoute]Guid id)
    => Ok(await _handler.HandleUpdateAsync(model,id));

  [HttpPost]
  [AuthorizeRoleOrPermissions([EPermissions.CreateCategory, EPermissions.DefaultEmployeePermission, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> PostAsync(
    [FromBody]EditorCategory model)
    => Ok(await _handler.HandleCreateAsync(model));
  
  
  [HttpDelete("{Id:guid}")]
  [AuthorizeRoleOrPermissions([EPermissions.DeleteCategory, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> DeleteAsync(
    [FromRoute]Guid id)
    => Ok(await _handler.HandleDeleteAsync(id));
  
}