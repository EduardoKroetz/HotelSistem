using Hotel.Domain.DTOs.PermissionDTOs;
using Hotel.Domain.Handlers.PermissionHandlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Domain.Controllers;


[ApiController]
//Administradores e funcionários tem permissão.
[Authorize(Roles = "RootAdmin,Admin,Employee")]
public class PermissionController : ControllerBase
{
    private readonly PermissionHandler _handler;

    public PermissionController(PermissionHandler handler)
    => _handler = handler;

    //Buscar as permissões
    [HttpGet("v1/permissions")]
    public async Task<IActionResult> GetAsync(
      [FromQuery] int? skip,
      [FromQuery] int? take,
      [FromQuery] DateTime? createdAt,
      [FromQuery] string? createdAtOperator,
      [FromQuery] string? name,
      [FromQuery] bool? isActive,
      [FromQuery] Guid? adminId
    )
    {
        var queryParameters = new PermissionQueryParameters(
          skip, take, createdAt, createdAtOperator, name, isActive, adminId
        );

        return Ok(await _handler.HandleGetAsync(queryParameters));
    }


    //Buscar as permissões pelo Id
    [HttpGet("v1/permissions/{Id:guid}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
      => Ok(await _handler.HandleGetByIdAsync(id));
}