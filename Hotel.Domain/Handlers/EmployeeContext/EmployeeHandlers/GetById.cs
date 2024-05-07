using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.User;

namespace Hotel.Domain.Handlers.EmployeeContexty.EmployeeHandlers;

public partial class EmployeeHandler 
{
  public async Task<Response<GetUser>> HandleGetByIdAsync(Guid id)
  {
    var permission = await _repository.GetByIdAsync(id);
    if (permission == null)
      throw new ArgumentException("Funcionário não encontrado.");
    
    return new Response<GetUser>(200,"Funcionário encontrado.", permission);
  }
}