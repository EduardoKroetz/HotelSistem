using Hotel.Domain.DTOs;
using Hotel.Domain.Entities.AdminEntity;
using Hotel.Domain.Entities.CustomerEntity;
using Hotel.Domain.Entities.EmployeeEntity;
using Hotel.Domain.Services.TokenServices;

namespace Hotel.Domain.Services.LoginServices;

public class LoginService
{
    private readonly TokenService _tokenService;

    public LoginService(TokenService tokenService)
    {
        _tokenService = tokenService;
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
            throw new ArgumentException("Email ou senha inválidos.");
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
            throw new ArgumentException("Email ou senha inválidos.");
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
            throw new ArgumentException("Email ou senha inválidos.");
    }
}
