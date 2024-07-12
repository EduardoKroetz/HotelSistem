using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.ReportHandlers;

public partial class ReportHandler
{
    public async Task<Response> HandleDeleteAsync(Guid id, Guid userId)
    {
        var report = await _repository.GetEntityByIdAsync(id)
          ?? throw new NotFoundException("Relatório não encontrado.");

        if (report.EmployeeId != userId)
            throw new UnauthorizedAccessException("Você não tem permissão para deletar relatório alheio!");

        try
        {
            _repository.Delete(report);
            await _repository.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Ocorreu um erro ao deletar um relatório no banco de dados: {ex.Message}");
        }

        return new Response("Relatório deletado com sucesso!", new { id });
    }
}