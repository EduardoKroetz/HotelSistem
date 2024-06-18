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
    public ReportHandler(IReportRepository repository, IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
        _repository = repository;
    }


    public async Task<Response> HandleCreateAsync(EditorReport model)
    {
        var employee = await _employeeRepository.GetEntityByIdAsync(model.EmployeeId)
          ?? throw new NotFoundException("Funcionário não encontrado.");

        var report = new Report(model.Summary, model.Description, model.Priority, employee, model.Resolution);

        await _repository.CreateAsync(report);
        await _repository.SaveChangesAsync();

        return new Response(200, "Relatório criado com sucesso!", new { report.Id });
    }
}