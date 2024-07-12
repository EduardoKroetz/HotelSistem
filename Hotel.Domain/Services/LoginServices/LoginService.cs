using Hotel.Domain.DTOs;
using Hotel.Domain.Entities.AdminEntity;
using Hotel.Domain.Entities.CustomerEntity;
using Hotel.Domain.Entities.EmployeeEntity;
using Hotel.Domain.Services.TokenServices;

namespace Hotel.Domain.Services.LoginServices;

public class LoginService
{
    private readonly TokenService _tokenService;
    private readonly ILogger<LoginService> _logger;

    public LoginService(TokenService tokenService, ILogger<LoginService> logger)
    {
        _tokenService = tokenService;
        _logger = logger;
    }


    //Login com cliente
    public Response UserLogin(string password, Customer customer)
    {
        if (PasswordService.VerifyPassword(password, customer.PasswordHash))
        {
            var token = _tokenService.GenerateToken(customer);
            return new Response("Login efetuado com sucesso!", new { Token = token });
        }
        else
        {
            _logger.LogError($"Cliente falhou ao tentar fazer login com o email {customer.Email.Address}");
            throw new ArgumentException("Email ou senha inválidos.");
        }
    }

    //Login com administrador
    public Response UserLogin(string password, Admin admin)
    {
        if (PasswordService.VerifyPassword(password, admin.PasswordHash))
        {
            var token = _tokenService.GenerateToken(admin);
            return new Response("Login efetuado com sucesso!", new { Token = token });
        }
        else
        {
            _logger.LogError($"Administrador falhou ao tentar fazer login com o email {admin.Email.Address}");
            throw new ArgumentException("Email ou senha inválidos.");
        }
    }

    //Login com funcionário
    public Response UserLogin(string password, Employee employee)
    {
        if (PasswordService.VerifyPassword(password, employee.PasswordHash))
        {
            var token = _tokenService.GenerateToken(employee);
            return new Response("Login efetuado com sucesso!", new { Token = token });
        }
        else
        {
            _logger.LogError($"Funcionário falhou ao tentar fazer login com o email {employee.Email.Address}");
            throw new ArgumentException("Email ou senha inválidos.");
        }
    }
}
