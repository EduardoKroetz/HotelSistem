using Hotel.Domain.DTOs.AuthenticationContext;
using Hotel.Domain.Handlers.LoginHandlers;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Domain.Controllers;

[ApiController]
[Route("v1/login")]
public class LoginController : ControllerBase
{
    private readonly LoginHandler _loginHandler;

    public LoginController(LoginHandler loginHandler)
    => _loginHandler = loginHandler;

    //Fazer login com qualquer usuário
    [HttpPost]
    public async Task<IActionResult> LoginAsync([FromBody] LoginDTO loginDto)
      => Ok(await _loginHandler.HandleLogin(loginDto.Email, loginDto.Password));

}
