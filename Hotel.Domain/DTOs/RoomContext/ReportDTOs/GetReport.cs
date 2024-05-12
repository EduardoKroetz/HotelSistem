using Hotel.Domain.DTOs.Interfaces;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.RoomContext.ReportDTOs;

public class GetReport : IDataTransferObject 
{
  public GetReport(Guid id ,string summary, string description, EPriority priority, string resolution, Guid employeeId, EStatus status)
  {
    Id = id;
    Summary = summary;
    Description = description;
    Priority = priority;
    Resolution = resolution;
    EmployeeId = employeeId;
    Status = status;
  }

  public Guid Id { get; set; }
  public string Summary { get; set; }
  public EStatus Status { get; set; }
  public string Description { get; set; }
  public EPriority Priority { get; set; }
  public string Resolution { get; set; }
  public Guid EmployeeId { get; set; }
}

