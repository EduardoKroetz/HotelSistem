using Hotel.Domain.DTOs;
using Hotel.Domain.DTOs.RoomContext.ReportDTOs;
using Hotel.Domain.Entities.RoomContext.ReportEntity;
using Hotel.Domain.Handlers.Interfaces;
using Hotel.Domain.Repositories.Interfaces;

namespace Hotel.Domain.Handlers.RoomContext.ReportHandlers;

public partial class ReportHandler : IHandler
{
  private readonly IReportRepository  _repository;
  private readonly IEmployeeRepository  _employeeRepository;
  public ReportHandler(IReportRepository repository, IEmployeeRepository employeeRepository)
  {
    _employeeRepository = employeeRepository;
    _repository = repository;
  }


  public async Task<Response<object>> HandleCreateAsync(CreateReport model)
  {
    var employee = await _employeeRepository.GetEntityByIdAsync(model.EmployeeId);
    if (employee == null)
      throw new ArgumentException("Funcionário não encontrado.");

    var report = new Report(model.Summary,model.Description,model.Priority,employee,model.Resolution);

    await _repository.CreateAsync(report);
    await _repository.SaveChangesAsync();

    return new Response<object>(200,"Relatório criado.",new { report.Id });
  }
}