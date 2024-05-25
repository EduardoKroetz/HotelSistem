using Hotel.Domain.DTOs;
using Hotel.Domain.Handlers.Interfaces;
using Hotel.Domain.Repositories.Interfaces.AdminContext;
using Hotel.Domain.Repositories.Interfaces.CustomerContext;
using Hotel.Domain.Repositories.Interfaces.EmployeeContext;
using Hotel.Domain.Services.Authentication;

namespace Hotel.Domain.Handlers.AuthenticationContext.LoginHandlers;

public partial class LoginHandler : IHandler
{
  private readonly IAdminRepository _adminRepository;
  private readonly ICustomerRepository _customerRepository;
  private readonly IEmployeeRepository _employeeRepository;

  public LoginHandler(IAdminRepository adminRepository, ICustomerRepository customerRepository, IEmployeeRepository employeeRepository)
  {
    _adminRepository = adminRepository;
    _customerRepository = customerRepository;
    _employeeRepository = employeeRepository;
  }

  public async Task<Response<object>> HandleLogin(string email, string password)
  {
    //Verificar se tem um cliente com o email
    var customer = await _customerRepository.GetEntityByEmailAsync(email);
    if (customer != null)
      return LoginService.UserLogin(password, customer);
    
    else
    {
      //Verificar se tem um admininistrador com o email
      var admin = await _adminRepository.GetEntityByEmailAsync(email);
      if (admin != null) 
        return LoginService.UserLogin(password, admin);
      

      //Verificar se tem um funcionário com o email
      else
      {
        var employee = await _employeeRepository.GetEntityByEmailAsync(email);
        if (employee != null) 
          return LoginService.UserLogin(password ,employee);

        return new Response<object>(400, "Email ou senha inválidos.");
      }
    }
    
  }


}
