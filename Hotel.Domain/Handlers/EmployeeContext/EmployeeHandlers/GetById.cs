using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.EmployeeContext.EmployeeDTOs;

namespace Hotel.Domain.Handlers.EmployeeContext.EmployeeHandlers;

public partial class EmployeeHandler 
{
  public async Task<Response<GetEmployee>> HandleGetByIdAsync(Guid id)
  {
    var employee = await _repository.GetByIdAsync(id);
    if (employee == null)
      throw new ArgumentException("Funcionário não encontrado.");
    
    return new Response<GetEmployee>(200,"Funcionário encontrado.", employee);
  }
}