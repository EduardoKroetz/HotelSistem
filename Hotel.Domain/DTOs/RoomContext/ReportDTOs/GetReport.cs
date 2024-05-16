using Hotel.Domain.DTOs.Interfaces;
using Hotel.Domain.Enums;

namespace Hotel.Domain.DTOs.RoomContext.ReportDTOs;

public class GetReport : IDataTransferObject 
{
  public GetReport(Guid id ,string summary, string description, EPriority priority, string resolution, Guid employeeId, EStatus status, DateTime createdAt)
  {
    Id = id;
    Summary = summary;
    Description = description;
    Priority = priority;
    Resolution = resolution;
    EmployeeId = employeeId;
    Status = status;
    CreatedAt = createdAt;
  }

  public Guid Id { get; private set; }
  public string Summary { get; private set; }
  public EStatus Status { get; private set; }
  public string Description { get; private set; }
  public EPriority Priority { get; private set; }
  public string Resolution { get; private set; }
  public Guid EmployeeId { get; private set; }
  public DateTime CreatedAt { get; private set; }
}

