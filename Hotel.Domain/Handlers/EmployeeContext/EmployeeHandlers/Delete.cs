using Hotel.Domain.DTOs;

namespace Hotel.Domain.Handlers.EmployeeContexty.EmployeeHandlers;

public partial class EmployeeHandler
{
  public async Task<Response<object>> HandleDeleteAsync(Guid id)
  {
    _repository.Delete(id);
    await _repository.SaveChangesAsync();
    return new Response<object>(200,"Funcionário deletado.", new { id });
  }
}