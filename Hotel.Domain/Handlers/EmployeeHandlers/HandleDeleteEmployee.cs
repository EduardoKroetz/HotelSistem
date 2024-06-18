using Hotel.Domain.DTOs;

namespace Hotel.Domain.Handlers.EmployeeHandlers;

public partial class EmployeeHandler
{
    public async Task<Response> HandleDeleteAsync(Guid id)
    {
        _repository.Delete(id);
        await _repository.SaveChangesAsync();
        return new Response(200, "Funcionário deletado com sucesso!");
    }
}