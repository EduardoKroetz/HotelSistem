using Hotel.Domain.DTOs;
using Hotel.Domain.Handlers.Base.Interfaces;
using Hotel.Domain.Repositories.Interfaces;
using Hotel.Domain.Services.LoginServices;

namespace Hotel.Domain.Handlers.LoginHandlers;

public partial class LoginHandler : IHandler
{
    private readonly IAdminRepository _adminRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly LoginService _loginService;


    public LoginHandler(IAdminRepository adminRepository, ICustomerRepository customerRepository, IEmployeeRepository employeeRepository, LoginService loginService)
    {
        _adminRepository = adminRepository;
        _customerRepository = customerRepository;
        _employeeRepository = employeeRepository;
        _loginService = loginService;
    }

    public async Task<Response> HandleLogin(string email, string password)
    {
        //Verificar se tem um cliente com o email
        var customer = await _customerRepository.GetEntityByEmailAsync(email);
        if (customer != null)
            return _loginService.UserLogin(password, customer);

        else
        {
            //Verificar se tem um admininistrador com o email
            var admin = await _adminRepository.GetEntityByEmailAsync(email);
            if (admin != null)
                return _loginService.UserLogin(password, admin);


            //Verificar se tem um funcionário com o email
            else
            {
                var employee = await _employeeRepository.GetEntityByEmailAsync(email);
                if (employee != null)
                    return _loginService.UserLogin(password, employee);

                throw new ArgumentException("Email ou senha inválidos.");
            }
        }

    }


}
