using Hotel.Domain.DTOs;
using Hotel.Domain.Handlers.Base.Interfaces;

namespace Hotel.Domain.Handlers.EmployeeHandlers;

public partial class EmployeeHandler : IHandler
{
    public async Task<Response> HandleAssignResponsibilityAsync(Guid id, Guid ResponsibilityId)
    {
        var employee = await _repository.GetEmployeeIncludesResponsibilities(id);
        if (employee == null)
            throw new ArgumentException("Funcionário não encontrado.");

        var Responsibility = await _responsibilityRepository.GetEntityByIdAsync(ResponsibilityId);
        if (Responsibility == null)
            throw new ArgumentException("Responsabilidade não encontrada.");

        employee.AddResponsibility(Responsibility);

        await _repository.SaveChangesAsync();

        return new Response(200, "Responsabilidade atribuida com sucesso!");
    }
}