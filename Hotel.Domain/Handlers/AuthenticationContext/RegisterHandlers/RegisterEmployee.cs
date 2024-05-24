using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.EmployeeContext.EmployeeDTOs;
using Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Handlers.AuthenticationContext.RegisterHandlers;

public partial class RegisterHandler
{
  public async Task<Response<Guid>> HandleRegisterEmployee(CreateEmployee model)
  {
    var employee = new Employee(
      new Name(model.FirstName, model.LastName),
      new Email(model.Email),
      new Phone(model.Phone),
      model.Password, //Hashear a senha no construtor
      model.Gender,
      model.DateOfBirth,
      new Address(model.Country, model.City, model.Street, model.Number),
      model.Salary
    );

    await _employeeRepository.CreateAsync(employee);
    await _employeeRepository.SaveChangesAsync();

    return new Response<Guid>(200, "Funcionário cadastrado com sucesso!", employee.Id);
  }
}