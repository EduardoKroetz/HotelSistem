using Hotel.Domain.DTOs.Interfaces;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.RoomContext.ReportDTOs;

public class CreateReport : IDataTransferObject 
{
  public CreateReport(string summary, string description, EPriority priority, string resolution, Guid employeeId)
  {
    Summary = summary;
    Description = description;
    Priority = priority;
    Resolution = resolution;
    EmployeeId = employeeId;
  }

  public string Summary { get; private set; } = string.Empty;
  public string Description { get; private set; } = string.Empty;
  public EPriority Priority { get; private set; }
  public string Resolution { get; private set; } = string.Empty;
  public Guid EmployeeId { get; private set; 
}
}

