﻿using Hotel.Domain.DTOs;
using Hotel.Domain.Entities.AdminContext.AdminEntity;
using Hotel.Domain.Entities.CustomerContext;
using Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;
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
    if (PasswordHasher.VerifyPassword(password, customer.PasswordHash))
    {
      var token = _tokenService.GenerateToken(customer);
      return new Response(200, "Login efetuado com sucesso!", new { Token = token });
    }
    else
      return new Response(400, "Email ou senha inválidos.");
  }

  //Login com administrador
  public Response UserLogin(string password, Admin admin)
  {
    if (PasswordHasher.VerifyPassword(password, admin.PasswordHash))
    {
      var token = _tokenService.GenerateToken(admin);
      return new Response(200, "Login efetuado com sucesso!", new { Token = token });
    }
    else
      return new Response(400, "Email ou senha inválidos.");
  }

  //Login com funcionário
  public Response UserLogin(string password, Employee employee)
  {
    if (PasswordHasher.VerifyPassword(password, employee.PasswordHash))
    {
      var token = _tokenService.GenerateToken(employee);
      return new Response(200, "Login efetuado com sucesso!", new { Token = token });
    }
    else
      return new Response(400, "Email ou senha inválidos.");
  }
}