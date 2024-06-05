using Hotel.Domain.Attributes;
using Hotel.Domain.DTOs.ReservationContext.ReservationDTOs;
using Hotel.Domain.Enums;
using Hotel.Domain.Handlers.ReservationContext.ReservationHandlers;
using Hotel.Domain.Services.UserServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Domain.Controllers.ReservationContext;

[ApiController]
[Route("v1/reservations")]
[Authorize(Roles = "RootAdmin,Admin,Employee,Customer")]
public class ReservationController : ControllerBase
{
  private readonly ReservationHandler _handler;
  private readonly IUserService _userService;

  public ReservationController(ReservationHandler handler, IUserService userService)
  {
    _handler = handler;
    _userService = userService;
  }

  //Buscar reservas.
  //Somente administradores ou funcion�rios com permiss�o possuem acesso.
  //Administradores e funcion�rios tem acesso por padr�o.
  [HttpGet]
  public async Task<IActionResult> GetAsync([FromBody] ReservationQueryParameters queryParameters)
  => Ok(await _handler.HandleGetAsync(queryParameters));

  //Buscar reserva pelo Id.
  //Todos os usu�rios possuem acesso.
  [HttpGet("{Id:guid}")]
  public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    => Ok(await _handler.HandleGetByIdAsync(id));

  //Criar reserva.
  //Somente clientes podem criar reservas.
  [HttpPost]
  [Authorize(Roles = "Customer")]
  public async Task<IActionResult> PostAsync([FromBody] CreateReservation model)
  {
    var customerId = _userService.GetUserIdentifier(User);
    return Ok(await _handler.HandleCreateAsync(model, customerId));
  }
   
  //Deletar reserva.
  //Somente administradores ou funcion�rios com permiss�o podem acessar.
  //Administradores tem acesso por padr�o.
  [HttpDelete("{Id:guid}")]
  [AuthorizePermissions([EPermissions.DeleteReservation, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
  {
    var customerId = _userService.GetUserIdentifier(User);
    return Ok(await _handler.HandleDeleteAsync(id, customerId));
  } 
    

  //Atualizar check out.
  //Somente administradores ou funcion�rios com permiss�o podem atualizar check out de reservas que n�o s�o suas.
  //Administradores e funcion�rios tem acesso por padr�o.
  [HttpPatch("{Id:guid}/check-out")]
  [AuthorizePermissions([EPermissions.UpdateReservationCheckout, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission], [ERoles.Customer])]
  public async Task<IActionResult> UpdateCheckoutAsync([FromRoute] Guid id,[FromBody] UpdateCheckOut updateCheckOut)
    => Ok(await _handler.HandleUpdateCheckOutAsync(id, updateCheckOut.CheckOut));

  //Atualizar check in.
  //Somente administradores ou funcion�rios com permiss�o podem atualizar check in de reservas que n�o s�o suas.
  //Administradores e funcion�rios tem acesso por padr�o.
  [HttpPatch("{Id:guid}/check-in")]
  [AuthorizePermissions([EPermissions.UpdateReservationCheckIn, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission], [ERoles.Customer])]
  public async Task<IActionResult> UpdateCheckInAsync([FromRoute] Guid id,[FromBody] UpdateCheckIn updateCheckIn)
    => Ok(await _handler.HandleUpdateCheckInAsync(id, updateCheckIn.CheckIn));

  //Adicionar um servi�o a uma reserva, ou seja, o cliente
  //requisitou o servi�o e o servi�o � adicionado atrav�s desse endpoint.
  //Administrador ou funcionarios com permiss�o podem acessar.
  //Administradores e funcion�rios tem acesso por padr�o.
  [HttpPost("{Id:guid}/services/{serviceId:guid}")]
  [AuthorizePermissions([EPermissions.AddServiceToReservation, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission])]
  public async Task<IActionResult> AddServiceAsync([FromRoute] Guid id,[FromRoute] Guid serviceId)
    => Ok(await _handler.HandleAddServiceAsync(id, serviceId));

  //Remover servi�o de uma reserva.
  //Administrador ou funcionarios com permiss�o podem acessar.
  //Somente administradores tem acesso por padr�o.
  [HttpDelete("{Id:guid}/services/{serviceid:guid}")]
  [AuthorizePermissions([EPermissions.RemoveServiceFromReservation, EPermissions.DefaultAdminPermission])]
  public async Task<IActionResult> RemoveServiceAsync([FromRoute] Guid id,[FromRoute] Guid serviceId)
    => Ok(await _handler.HandleRemoveServiceAsync(id, serviceId));

  [HttpPatch("finish/{Id:guid}")]
  public async Task<IActionResult> FinishReservationAsync([FromRoute] Guid Id)
    => Ok(await _handler.HandleFinishReservationAsync(Id));


}