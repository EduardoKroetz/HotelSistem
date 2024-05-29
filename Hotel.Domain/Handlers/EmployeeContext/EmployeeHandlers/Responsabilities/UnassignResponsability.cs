using Hotel.Domain.DTOs;
using Hotel.Domain.Handlers.Interfaces;

namespace Hotel.Domain.Handlers.EmployeeContext.EmployeeHandlers;

public partial class EmployeeHandler : IHandler
{
  public async Task<Response<object>> HandleUnassignResponsabilityAsync(Guid id, Guid responsabilityId)
  {
    var employee = await _repository.GetEmployeeIncludesResponsabilities(id);
    if (employee == null)
      throw new ArgumentException("Funcionário não encontrado.");

    var responsability = await _responsabilityRepository.GetEntityByIdAsync(responsabilityId);
    if (responsability == null)
      throw new ArgumentException("Responsabilidade não encontrada.");

    employee.RemoveResponsability(responsability);

    await _repository.SaveChangesAsync();

    return new Response<object>(200, "Responsabilidade desatribuida.");
  }
}