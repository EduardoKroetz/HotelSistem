using Hotel.Domain.DTOs;
using Hotel.Domain.Entities.AdminContext.AdminEntity;
using Hotel.Domain.Entities.CustomerContext;
using Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;

namespace Hotel.Domain.Services.Authentication;

public static class LoginService
{
  //Login com cliente
  public static Response<object> UserLogin(string password, Customer customer)
  {
    if (PasswordHasher.VerifyPassword(password, customer.PasswordHash))
    {
      var token = Authentication.GenerateToken(customer);
      return new Response<object>(200, "Login efetuado com sucesso!", new { Token = token });
    }
    else
      return new Response<object>(400, "Email ou senha inválidos.");
  }

  //Login com administrador
  public static Response<object> UserLogin(string password, Admin admin)
  {
    if (PasswordHasher.VerifyPassword(password, admin.PasswordHash))
    {
      var token = Authentication.GenerateToken(admin);
      return new Response<object>(200, "Login efetuado com sucesso!", new { Token = token });
    }
    else
      return new Response<object>(400, "Email ou senha inválidos.");
  }

  //Login com funcionário
  public static Response<object> UserLogin(string password, Employee employee)
  {
    if (PasswordHasher.VerifyPassword(password, employee.PasswordHash))
    {
      var token = Authentication.GenerateToken(employee);
      return new Response<object>(200, "Login efetuado com sucesso!", new { Token = token });
    }
    else
      return new Response<object>(400, "Email ou senha inválidos.");
  }
}
