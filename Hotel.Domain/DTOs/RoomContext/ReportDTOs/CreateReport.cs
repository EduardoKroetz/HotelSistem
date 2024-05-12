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

  public string Summary { get; set; }
  public string Description { get; set; }
  public EPriority Priority { get; set; }
  public string Resolution { get; set; }
  public Guid EmployeeId { get; set; 
}
}

