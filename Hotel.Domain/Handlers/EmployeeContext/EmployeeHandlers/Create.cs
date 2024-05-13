using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.EmployeeContext.EmployeeDTOs;
using Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;
using Hotel.Domain.Handlers.Interfaces;
using Hotel.Domain.Repositories.Interfaces.EmployeeContext;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Handlers.EmployeeContexty.EmployeeHandlers;

public partial class EmployeeHandler : IHandler
{
  private readonly IEmployeeRepository  _repository;
  public EmployeeHandler(IEmployeeRepository repository)
  => _repository = repository;

  public async Task<Response<object>> HandleCreateAsync(CreateEmployee model)
  {
    var employee = new Employee(
      new Name(model.FirstName,model.LastName),
      new Email(model.Email),
      new Phone(model.Phone),
      model.Password,
      model.Gender,
      model.DateOfBirth,
      new Address(model.Country,model.City,model.Street,model.Number),
      model.Salary
    );

    await _repository.CreateAsync(employee);
    await _repository.SaveChangesAsync();

    return new Response<object>(200,"Funcionário foi cadastrado.",new { employee.Id });
  }
}