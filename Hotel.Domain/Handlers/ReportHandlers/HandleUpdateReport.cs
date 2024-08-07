using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.ReportDTOs;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Handlers.ReportHandlers;

public partial class ReportHandler
{
    public async Task<Response> HandleUpdateAsync(EditorReport model, Guid id)
    {
        var report = await _repository.GetEntityByIdAsync(id)
          ?? throw new NotFoundException("Relatório não encontrado.");

        report.ChangeSummary(model.Summary);
        report.ChangeDescription(model.Description);
        report.ChangePriority(model.Priority);
        report.ChangeResolution(model.Resolution);

        var employee = await _employeeRepository.GetEntityByIdAsync(model.EmployeeId)
          ?? throw new NotFoundException("Funcionário não encontrado.");

        report.ChangeEmployee(employee);

        try
        {
            _repository.Update(report);
            await _repository.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Ocorreu um erro ao atualizar um relatório no banco de dados: {ex.Message}");
        }

        return new Response("Relatório atualizado com sucesso!", new { report.Id });
    }
}