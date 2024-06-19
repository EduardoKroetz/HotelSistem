using Hotel.Domain.DTOs;
using Hotel.Domain.Handlers.Base.Interfaces;

namespace Hotel.Domain.Handlers.EmployeeHandlers;

public partial class EmployeeHandler : IHandler
{
    public async Task<Response> HandleUnassignResponsibilityAsync(Guid id, Guid responsibilityId)
    {
        var employee = await _repository.GetEmployeeIncludesResponsibilities(id);
        if (employee == null)
            throw new ArgumentException("Funcionário não encontrado.");

        var responsibility = await _responsibilityRepository.GetEntityByIdAsync(responsibilityId);
        if (responsibility == null)
            throw new ArgumentException("Responsabilidade não encontrada.");

        employee.RemoveResponsibility(responsibility);

        await _repository.SaveChangesAsync();

        return new Response("Responsabilidade desatribuida com sucesso!");
    }
}