using Hotel.Domain.DTOs.Base;
using Hotel.Domain.DTOs.CustomerContext.FeedbackDTOs;
using Hotel.Domain.Handlers.CustomerContext.FeedbackHandlers;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Domain.Controllers.CustomerContext;

public class FeedbackController : ControllerBase
{
  private readonly FeedbackHandler _handler;

  public FeedbackController(FeedbackHandler handler)
  {
    _handler = handler;
  }

  [HttpGet("v1/feedbacks")]
  public async Task<IActionResult> GetAsync(
  [FromBody] FeedbackQueryParameters queryParameters)
  => Ok(await _handler.HandleGetAsync(queryParameters));

  [HttpGet("v1/feedbacks/{Id:guid}")]
  public async Task<IActionResult> GetByIdAsync(
    [FromRoute] Guid id
  )
  => Ok(await _handler.HandleGetByIdAsync(id));


  [HttpPost("v1/feedbacks")]
  public async Task<IActionResult> PostAsync(
    [FromBody] CreateFeedback model
  )
  => Ok(await _handler.HandleCreateAsync(model));



  [HttpPut("v1/feedbacks/{Id:guid}")]
  public async Task<IActionResult> PutAsync(
    [FromBody] UpdateFeedback model,
    [FromRoute] Guid id
  )
  => Ok(await _handler.HandleUpdateAsync(model, id));


  [HttpDelete("v1/feedbacks/{Id:guid}")]
  public async Task<IActionResult> DeleteAsync(
    [FromRoute] Guid id
  )
  => Ok(await _handler.HandleDeleteAsync(id));


  [HttpPatch("v1/feedbacks/add-like/{Id:guid}")]
  public async Task<IActionResult> AddLikeAsync(
    [FromRoute] Guid id
  )
  => Ok(await _handler.HandleAddLikeAsync(id));


  [HttpPatch("v1/feedbacks/remove-like/{Id:guid}")]
  public async Task<IActionResult> RemoveLikeAsync(
    [FromRoute] Guid id
  )
  => Ok(await _handler.HandleRemoveLikeAsync(id));


  [HttpPatch("v1/feedbacks/add-deslike/{Id:guid}")]
  public async Task<IActionResult> AddDeslikeAsync(
    [FromRoute] Guid id
  )
  => Ok(await _handler.HandleAddDeslikeAsync(id));


  [HttpPatch("v1/feedbacks/remove-deslike/{Id:guid}")]
  public async Task<IActionResult> RemoveDeslikeAsync(
    [FromRoute] Guid id
  )
  => Ok(await _handler.HandleRemoveDeslikeAsync(id));


  [HttpPatch("v1/feedbacks/{Id:guid}/rate")]
  public async Task<IActionResult> UpdateRateAsync(
    [FromRoute] Guid id,
    [FromBody]UpdateRate updateRate
  )
  => Ok(await _handler.HandleUpdateRateAsync(id, updateRate));


  [HttpPatch("v1/feedbacks/{Id:guid}/comment")]
  public async Task<IActionResult> UpdateCommentAsync(
    [FromRoute] Guid id,
    [FromBody]UpdateComment updateComment
  )
  => Ok(await _handler.HandleUpdateCommentAsync(id, updateComment));

}