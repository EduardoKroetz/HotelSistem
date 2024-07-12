using Hotel.Domain.DTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.ReportHandlers;

public partial class ReportHandler
{
    public async Task<Response> HandleFinishAsync(Guid id)
    {
        var report = await _repository.GetEntityByIdAsync(id)
          ?? throw new NotFoundException("Relatório não encontrado.");

        report.Finish();

        try
        {
            await _repository.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Ocorreu um erro ao atualizar um relatório no banco de dados: {ex.Message}");
        }

        return new Response("Relatório finalizado com sucesso!", new { report.Id });
    }
}