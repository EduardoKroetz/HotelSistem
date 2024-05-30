using Hotel.Domain.DTOs.CustomerContext.FeedbackDTOs;
using Hotel.Domain.Handlers.CustomerContext.FeedbackHandlers;
using Hotel.Domain.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Domain.Controllers.CustomerContext;

[ApiController]
[Route("v1/feedbacks")]
[Authorize(Roles = "RootAdmin,Admin,Employee,Customer")]
public class FeedbackController : ControllerBase
{
  private readonly FeedbackHandler _handler;

  public FeedbackController(FeedbackHandler handler)
  => _handler = handler;

  [HttpGet]
  public async Task<IActionResult> GetAsync([FromBody] FeedbackQueryParameters queryParameters)
    => Ok(await _handler.HandleGetAsync(queryParameters));

  [HttpGet("{Id:guid}")]
  public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    => Ok(await _handler.HandleGetByIdAsync(id));

  [HttpPost]
  public async Task<IActionResult> PostAsync([FromBody] CreateFeedback model)
  {
    var userId = UserServices.GetIdFromClaim(User); //Somente clientes tem acesso à criação de feedbacks
    return Ok(await _handler.HandleCreateAsync(model, userId));
  }

  [HttpPut("{Id:guid}")]
  public async Task<IActionResult> PutAsync([FromBody] UpdateFeedback model,[FromRoute] Guid id)
  {
    var customerId = UserServices.GetIdFromClaim(User);
    return Ok(await _handler.HandleUpdateAsync(model, id, customerId));
  }
 
  [HttpDelete("{Id:guid}")]
  public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
  {
    var customerId = UserServices.GetIdFromClaim(User);
    return Ok(await _handler.HandleDeleteAsync(id, customerId));
  }

  [HttpPatch("{Id:guid}/rate/{rate:int}")]
  public async Task<IActionResult> UpdateRateAsync([FromRoute] Guid id, [FromRoute] int rate)
  {
    var customerId = UserServices.GetIdFromClaim(User);
    return Ok(await _handler.HandleUpdateRateAsync(id, rate, customerId));
  }

  [HttpPatch("{Id:guid}/comment")]
  public async Task<IActionResult> UpdateCommentAsync([FromRoute] Guid id, [FromBody] UpdateComment updateComment)
  {
    var customerId = UserServices.GetIdFromClaim(User);
    return Ok(await _handler.HandleUpdateCommentAsync(id, updateComment.Comment, customerId));
  }



  //Rotas de like e deslike
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