using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.ReportDTOs;
using Hotel.Domain.Entities.ReportEntity;
using Hotel.Domain.Exceptions;
using Hotel.Domain.Handlers.Base.Interfaces;
using Hotel.Domain.Repositories.Interfaces;

namespace Hotel.Domain.Handlers.ReportHandlers;

public partial class ReportHandler : IHandler
{
    private readonly IReportRepository _repository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ILogger<ReportHandler> _logger;  
    public ReportHandler(IReportRepository repository, IEmployeeRepository employeeRepository, ILogger<ReportHandler> logger)
    {
        _employeeRepository = employeeRepository;
        _repository = repository;
        _logger = logger;
    }


    public async Task<Response> HandleCreateAsync(EditorReport model)
    {
        var employee = await _employeeRepository.GetEntityByIdAsync(model.EmployeeId)
          ?? throw new NotFoundException("Funcionário não encontrado.");

        var report = new Report(model.Summary, model.Description, model.Priority, employee, model.Resolution);

        try
        {
            await _repository.CreateAsync(report);
            await _repository.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Ocorreu um erro ao criar um relatório no banco de dados: {ex.Message}");
        }

        return new Response("Relatório criado com sucesso!", new { report.Id });
    }
}