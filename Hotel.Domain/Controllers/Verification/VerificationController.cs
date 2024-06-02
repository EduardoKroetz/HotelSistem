using Hotel.Domain.Handlers.Verification;
using Hotel.Domain.Services.VerificationServices;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Domain.Controllers.Verification;

[ApiController]
[Route("v1/verifications")]
public class VerificationController : ControllerBase
{
  private readonly VerificationHandler _verificationHandler;
  private readonly VerificationService _verificationService; //é só para inicializar o Timer que expira os códigos

  public VerificationController(VerificationHandler verificationHandler, VerificationService verificationService)
  {
    _verificationService = verificationService;
    _verificationHandler = verificationHandler;
  }


  [HttpPost("email-code")] 
  public async Task<IActionResult> SendNewEmailCodeAsync([FromQuery] string? email)
  {
    return Ok(await _verificationHandler.HandleSendEmailCodeAsync(email));  
  }
}
