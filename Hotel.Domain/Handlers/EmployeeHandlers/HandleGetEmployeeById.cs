using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.EmployeeDTOs;

namespace Hotel.Domain.Handlers.EmployeeHandlers;

public partial class EmployeeHandler
{
    public async Task<Response<GetEmployee>> HandleGetByIdAsync(Guid id)
    {
        var employee = await _repository.GetByIdAsync(id);
        if (employee == null)
            throw new ArgumentException("Funcionário não encontrado.");

        return new Response<GetEmployee>("Sucesso!", employee);
    }
}