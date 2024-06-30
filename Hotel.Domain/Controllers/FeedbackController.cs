using Hotel.Domain.DTOs.FeedbackDTOs;
using Hotel.Domain.Handlers.FeedbackHandlers;
using Hotel.Domain.Services.UserServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Domain.Controllers;

[ApiController]
[Route("v1/feedbacks")]
[Authorize(Roles = "RootAdmin,Admin,Employee,Customer")]
public class FeedbackController : ControllerBase
{
    private readonly FeedbackHandler _handler;
    private readonly IUserService _userService;

    public FeedbackController(FeedbackHandler handler, IUserService userService)
    {
        _handler = handler;
        _userService = userService;
    }


    // Endpoint para buscar feedbacks
    [HttpGet]
    public async Task<IActionResult> GetAsync(
      [FromQuery] int? skip,
      [FromQuery] int? take,
      [FromQuery] DateTime? createdAt,
      [FromQuery] string? createdAtOperator,
      [FromQuery] string? comment,
      [FromQuery] int? rate,
      [FromQuery] string? rateOperator,
      [FromQuery] int? likes,
      [FromQuery] string? likesOperator,
      [FromQuery] int? dislikes,
      [FromQuery] string? dislikesOperator,
      [FromQuery] DateTime? updatedAt,
      [FromQuery] string? updatedAtOperator,
      [FromQuery] Guid? customerId,
      [FromQuery] Guid? reservationId,
      [FromQuery] Guid? roomId
    )
    {
        var queryParameters = new FeedbackQueryParameters
        {
            Skip = skip,
            Take = take,
            CreatedAt = createdAt,
            CreatedAtOperator = createdAtOperator,
            Comment = comment,
            Rate = rate,
            RateOperator = rateOperator,
            Likes = likes,
            LikesOperator = likesOperator,
            Dislikes = dislikes,
            DislikesOperator = dislikesOperator,
            UpdatedAt = updatedAt,
            UpdatedAtOperator = updatedAtOperator,
            CustomerId = customerId,
            ReservationId = reservationId,
            RoomId = roomId
        };


        return Ok(await _handler.HandleGetAsync(queryParameters));
    }


    // Endpoint para buscar feedback por ID
    [HttpGet("{Id:guid}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
      => Ok(await _handler.HandleGetByIdAsync(id));

    // Endpoint para criar um novo feedback
    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] CreateFeedback model)
    {
        var userId = _userService.GetUserIdentifier(User);
        return Ok(await _handler.HandleCreateAsync(model, userId));
    }

    // Endpoint para atualizar um feedback
    [HttpPut("{Id:guid}")]
    public async Task<IActionResult> PutAsync([FromBody] UpdateFeedback model, [FromRoute] Guid id)
    {
        var customerId = _userService.GetUserIdentifier(User);
        return Ok(await _handler.HandleUpdateAsync(model, id, customerId));
    }

    // Endpoint para deletar um feedback
    [HttpDelete("{Id:guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        var customerId = _userService.GetUserIdentifier(User);
        return Ok(await _handler.HandleDeleteAsync(id, customerId));
    }

    // Endpoint para atualizar a avaliação (rate) de um feedback
    [HttpPatch("rate/{Id:guid}")]
    public async Task<IActionResult> UpdateRateAsync([FromRoute] Guid id, [FromBody] UpdateRate update)
    {
        var customerId = _userService.GetUserIdentifier(User);
        return Ok(await _handler.HandleUpdateRateAsync(id, update.Rate, customerId));
    }

    // Endpoint para atualizar o comentário de um feedback
    [HttpPatch("comment/{Id:guid}")]
    public async Task<IActionResult> UpdateCommentAsync([FromRoute] Guid id, [FromBody] UpdateComment updateComment)
    {
        var customerId = _userService.GetUserIdentifier(User);
        return Ok(await _handler.HandleUpdateCommentAsync(id, updateComment.Comment, customerId));
    }

    // Rotas para adicionar/remover likes e dislikes em um feedback
    [HttpPatch("add-like/{feedbackId:guid}")]
    public async Task<IActionResult> AddLikeAsync([FromRoute] Guid feedbackId)
    {
        var customerId = _userService.GetUserIdentifier(User);
        return Ok(await _handler.HandleAddLikeAsync(feedbackId, customerId));
    }


    [HttpPatch("remove-like/{feedbackId:guid}")]
    public async Task<IActionResult> RemoveLikeAsync([FromRoute] Guid feedbackId)
    {
        var customerId = _userService.GetUserIdentifier(User);
        return Ok(await _handler.HandleRemoveLikeAsync(feedbackId, customerId));
    }


    [HttpPatch("add-dislike/{feedbackId:guid}")]
    public async Task<IActionResult> AddDeslikeAsync([FromRoute] Guid feedbackId)
    {
        var customerId = _userService.GetUserIdentifier(User);
        return Ok(await _handler.HandleAddDeslikeAsync(feedbackId, customerId));
    }

    [HttpPatch("remove-dislike/{feedbackId:guid}")]
    public async Task<IActionResult> RemoveDeslikeAsync([FromRoute] Guid feedbackId)
    {
        var customerId = _userService.GetUserIdentifier(User);
        return Ok(await _handler.HandleRemoveDeslikeAsync(feedbackId, customerId));
    }
}
