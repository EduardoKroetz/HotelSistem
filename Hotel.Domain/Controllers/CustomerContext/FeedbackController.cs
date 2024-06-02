using Hotel.Domain.DTOs.CustomerContext.FeedbackDTOs;
using Hotel.Domain.Handlers.CustomerContext.FeedbackHandlers;
using Hotel.Domain.Services.UserServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Domain.Controllers.CustomerContext;

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
  public async Task<IActionResult> GetAsync([FromBody] FeedbackQueryParameters queryParameters)
    => Ok(await _handler.HandleGetAsync(queryParameters));

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
  [HttpPatch("{Id:guid}/rate/{rate:int}")]
  public async Task<IActionResult> UpdateRateAsync([FromRoute] Guid id, [FromRoute] int rate)
  {
    var customerId = _userService.GetUserIdentifier(User);
    return Ok(await _handler.HandleUpdateRateAsync(id, rate, customerId));
  }

  // Endpoint para atualizar o comentário de um feedback
  [HttpPatch("{Id:guid}/comment")]
  public async Task<IActionResult> UpdateCommentAsync([FromRoute] Guid id, [FromBody] UpdateComment updateComment)
  {
    var customerId = _userService.GetUserIdentifier(User);
    return Ok(await _handler.HandleUpdateCommentAsync(id, updateComment.Comment, customerId));
  }

  // Rotas para adicionar/remover likes e dislikes em um feedback
  [HttpPatch("add-like/{Id:guid}")]
  public async Task<IActionResult> AddLikeAsync([FromRoute] Guid id)
    => Ok(await _handler.HandleAddLikeAsync(id));

  [HttpPatch("remove-like/{Id:guid}")]
  public async Task<IActionResult> RemoveLikeAsync([FromRoute] Guid id)
    => Ok(await _handler.HandleRemoveLikeAsync(id));

  [HttpPatch("add-deslike/{Id:guid}")]
  public async Task<IActionResult> AddDeslikeAsync([FromRoute] Guid id)
    => Ok(await _handler.HandleAddDeslikeAsync(id));

  [HttpPatch("remove-deslike/{Id:guid}")]
  public async Task<IActionResult> RemoveDeslikeAsync([FromRoute] Guid id)
    => Ok(await _handler.HandleRemoveDeslikeAsync(id));
}
