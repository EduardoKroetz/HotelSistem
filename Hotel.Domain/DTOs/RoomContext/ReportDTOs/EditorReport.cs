using Hotel.Domain.DTOs.Interfaces;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.RoomContext.ReportDTOs;

public class EditorReport : IDataTransferObject 
{
  public EditorReport(string summary, string description, EPriority priority, Guid employeeId, string resolution = "")
  {
    Summary = summary;
    Description = description;
    Priority = priority;
    Resolution = resolution;
    EmployeeId = employeeId;
  }

  public string Summary { get; private set; }
  public string Description { get; private set; }
  public EPriority Priority { get; private set; }
  public string Resolution { get; private set; }
  public Guid EmployeeId { get; private set; }
}

