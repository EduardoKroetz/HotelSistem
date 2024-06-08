using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.EmployeeContext.EmployeeDTOs;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Handlers.EmployeeContext.EmployeeHandlers;

public partial class EmployeeHandler 
{
  public async Task<Response> HandleUpdateAsync(UpdateEmployee model, Guid id)
  {
    var employee = await _repository.GetEntityByIdAsync(id);
    if (employee == null)
      throw new ArgumentException("Funcionário não encontrado.");

    employee.ChangeName(new Name(model.FirstName,model.LastName));
    employee.ChangePhone(new Phone(model.Phone));
    employee.ChangeGender(model.Gender);
    employee.ChangeDateOfBirth(model.DateOfBirth);
    employee.ChangeAddress(new Address(model.Country,model.City,model.Street,model.Number));
    employee.ChangeSalary(model.Salary ?? employee.Salary);

    _repository.Update(employee);
    await _repository.SaveChangesAsync();

    return new Response(200,"Funcionário atualizado com sucesso!",new { employee.Id });
  }
}