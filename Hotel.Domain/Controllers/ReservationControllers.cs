using Hotel.Domain.Attributes;
using Hotel.Domain.DTOs.PaymentDTOs;
using Hotel.Domain.DTOs.ReservationDTOs;
using Hotel.Domain.Enums;
using Hotel.Domain.Handlers.ReservationHandlers;
using Hotel.Domain.Services.UserServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Domain.Controllers;

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
    //Somente administradores ou funcionários com permissão possuem acesso.
    //Administradores e funcionários tem acesso por padrão.
    [HttpGet]
    public async Task<IActionResult> GetAsync(
      [FromQuery] int? skip,
      [FromQuery] int? take,
      [FromQuery] TimeSpan? timeHosted,
      [FromQuery] string? timeHostedOperator,
      [FromQuery] decimal? dailyRate,
      [FromQuery] string? dailyRateOperator,
      [FromQuery] DateTime? checkIn,
      [FromQuery] string? checkInOperator,
      [FromQuery] DateTime? checkOut,
      [FromQuery] string? checkOutOperator,
      [FromQuery] EReservationStatus? status,
      [FromQuery] int? capacity,
      [FromQuery] string? capacityOperator,
      [FromQuery] Guid? roomId,
      [FromQuery] Guid? customerId,
      [FromQuery] Guid? invoiceId,
      [FromQuery] Guid? serviceId,
      [FromQuery] DateTime? createdAt,
      [FromQuery] string? createdAtOperator,
      [FromQuery] DateTime? expectedCheckIn,
      [FromQuery] string? expectedCheckInOperator,
      [FromQuery] DateTime? expectedCheckOut,
      [FromQuery] string? expectedCheckOutOperator,
      [FromQuery] TimeSpan? expectedTimeHosted,
      [FromQuery] string? expectedTimeHostedOperator
    )
    {
        var queryParameters = new ReservationQueryParameters
        {
            Skip = skip,
            Take = take,
            TimeHosted = timeHosted,
            TimeHostedOperator = timeHostedOperator,
            DailyRate = dailyRate,
            DailyRateOperator = dailyRateOperator,
            CheckIn = checkIn,
            CheckInOperator = checkInOperator,
            CheckOut = checkOut,
            CheckOutOperator = checkOutOperator,
            Status = status,
            Capacity = capacity,
            CapacityOperator = capacityOperator,
            RoomId = roomId,
            CustomerId = customerId,
            InvoiceId = invoiceId,
            ServiceId = serviceId,
            CreatedAt = createdAt,
            CreatedAtOperator = createdAtOperator,
            ExpectedCheckIn = expectedCheckIn,
            ExpectedCheckInOperator = expectedCheckInOperator,
            ExpectedCheckOut = expectedCheckOut,
            ExpectedCheckOutOperator = expectedCheckOutOperator,
            ExpectedTimeHosted = expectedTimeHosted,
            ExpectedTimeHostedOperator = expectedTimeHostedOperator
        };


        return Ok(await _handler.HandleGetAsync(queryParameters));
    }


    //Buscar reserva pelo Id.
    //Todos os usuários possuem acesso.
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
    //Somente administradores ou funcionários com permissão podem acessar.
    //Administradores tem acesso por padrão.
    [HttpDelete("{Id:guid}")]
    [AuthorizePermissions([EPermissions.DeleteReservation, EPermissions.DefaultAdminPermission])]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        var customerId = _userService.GetUserIdentifier(User);
        return Ok(await _handler.HandleDeleteAsync(id, customerId));
    }


    //Atualizar check out.
    //Somente administradores ou funcionários com permissão podem atualizar check out de reservas que não são suas.
    //Administradores e funcionários tem acesso por padrão.
    [HttpPatch("expected-check-out/{Id:guid}")]
    [AuthorizePermissions([EPermissions.UpdateReservationCheckout, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission], [ERoles.Customer])]
    public async Task<IActionResult> UpdateCheckoutAsync([FromRoute] Guid id, [FromBody] UpdateExpectedCheckOut update)
      => Ok(await _handler.HandleUpdateExpectedCheckOutAsync(id, update.ExpectedCheckOut));

    //Atualizar check in.
    //Somente administradores ou funcionários com permissão podem atualizar check in de reservas que não são suas.
    //Administradores e funcionários tem acesso por padrão.
    [HttpPatch("expected-check-in/{Id:guid}")]
    [AuthorizePermissions([EPermissions.UpdateReservationCheckIn, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission], [ERoles.Customer])]
    public async Task<IActionResult> UpdateCheckInAsync([FromRoute] Guid id, [FromBody] UpdateExpectedCheckIn update)
      => Ok(await _handler.HandleUpdateExpectedCheckInAsync(id, update.ExpectedCheckIn));

    //Adicionar um serviço a uma reserva, ou seja, o cliente
    //requisitou o serviço e o serviço é adicionado através desse endpoint.
    //Administrador ou funcionarios com permissão podem acessar.
    //Administradores e funcionários tem acesso por padrão.
    [HttpPost("{Id:guid}/services/{serviceId:guid}")]
    [AuthorizePermissions([EPermissions.AddServiceToReservation, EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission])]
    public async Task<IActionResult> AddServiceAsync([FromRoute] Guid id, [FromRoute] Guid serviceId)
      => Ok(await _handler.HandleAddServiceAsync(id, serviceId));

    //Remover serviço de uma reserva.
    //Administrador ou funcionarios com permissão podem acessar.
    //Somente administradores tem acesso por padrão.
    [HttpDelete("{Id:guid}/services/{serviceid:guid}")]
    [AuthorizePermissions([EPermissions.RemoveServiceFromReservation, EPermissions.DefaultAdminPermission])]
    public async Task<IActionResult> RemoveServiceAsync([FromRoute] Guid id, [FromRoute] Guid serviceId)
      => Ok(await _handler.HandleRemoveServiceAsync(id, serviceId));

    [HttpPost("check-in/{Id:guid}")]
    [AuthorizePermissions([EPermissions.ReservationCheckIn,EPermissions.DefaultAdminPermission, EPermissions.DefaultEmployeePermission])]
    public async Task<IActionResult> ReservationCheckInAsync([FromRoute] Guid Id, [FromBody] CardOptions cardOptions)
    {
        return Ok(await _handler.HandleReservationCheckInAsync(Id, cardOptions.TokenId));
    }

    [HttpPost("finish/{Id:guid}")]
    public async Task<IActionResult> FinishReservationAsync([FromRoute] Guid Id)
    {
        var customerId = _userService.GetUserIdentifier(User);
        return Ok(await _handler.HandleFinishReservationAsync(Id, customerId));
    }

    [HttpPost("cancel/{Id:guid}")]
    public async Task<IActionResult> CancelReservationAsync([FromRoute] Guid Id)
    {
        var customerId = _userService.GetUserIdentifier(User);
        return Ok(await _handler.HandleCancelReservationAsync(Id, customerId));
    }

    [HttpGet("total-amount")]
    public async Task<IActionResult> GetTotalAmount([FromQuery] DateTime checkIn, [FromQuery] DateTime checkOut, [FromQuery] decimal dailyRate, [FromQuery] string? servicesIds)
    => Ok(await _handler.GetTotalAmount(checkIn, checkOut, dailyRate, servicesIds));



}