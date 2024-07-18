using Hotel.Domain.Handlers.VerificationHandlers;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Domain.Controllers;

[ApiController]
[Route("v1/verifications")]
public class VerificationController : ControllerBase
{
    private readonly VerificationHandler _verificationHandler;

    public VerificationController(VerificationHandler verificationHandler)
    {
        _verificationHandler = verificationHandler;
    }


    [HttpPost("email-code")]
    public async Task<IActionResult> SendNewEmailCodeAsync([FromQuery] string? email)
    {
        return Ok(await _verificationHandler.HandleSendEmailCodeAsync(email));
    }
}
