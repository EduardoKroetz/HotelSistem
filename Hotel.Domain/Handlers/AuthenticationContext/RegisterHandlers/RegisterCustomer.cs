using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.Entities.CustomerContext;
using Hotel.Domain.Handlers.Interfaces;
using Hotel.Domain.Repositories.Interfaces.AdminContext;
using Hotel.Domain.Repositories.Interfaces.CustomerContext;
using Hotel.Domain.Repositories.Interfaces.EmployeeContext;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Handlers.AuthenticationContext.RegisterHandlers;

public partial class RegisterHandler : IHandler
{
  private readonly IAdminRepository _adminRepository;
  private readonly ICustomerRepository _customerRepository;
  private readonly IEmployeeRepository _employeeRepository;

  public RegisterHandler(IAdminRepository adminRepository, ICustomerRepository customerRepository, IEmployeeRepository employeeRepository)
  {
    _adminRepository = adminRepository;
    _customerRepository = customerRepository;
    _employeeRepository = employeeRepository;
  }

  public async Task<Response<Guid>> HandleRegisterCustomer(CreateUser model)
  {
    var customer = new Customer(
      new Name(model.FirstName, model.LastName),
      new Email(model.Email),
      new Phone(model.Phone),
      model.Password,
      model.Gender,
      model.DateOfBirth,
      new Address(model.Country, model.City, model.Street, model.Number)
    );

    await _customerRepository.CreateAsync(customer);
    await _customerRepository.SaveChangesAsync();

    return new Response<Guid>(200, "Cliente criado com sucesso!", customer.Id);
  }
}