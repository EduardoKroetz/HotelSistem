using Hotel.Domain.Entities.Base;
using Hotel.Domain.Enums;

namespace Hotel.Domain.Entities;

public class Report : Entity
{
  public Report(string description, EStatus status, EPriority priority, Guid employeeId, string resolution = "")
  {
    Description = description;
    Status = status;
    CreatedAt = DateTime.Now;
    Priority = priority;
    Resolution = resolution;
    EmployeeId = employeeId;
  }

  public string Description { get; private set; }
  public EStatus Status { get; private set; }
  public DateTime CreatedAt { get; private set; }
  public EPriority Priority { get; private set; }
  public string Resolution { get; private set; }
  public Guid EmployeeId { get; private set; }
  public Employee? Employee { get; }
}