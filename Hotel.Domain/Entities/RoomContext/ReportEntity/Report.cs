using Hotel.Domain.Entities.Base;
using Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;
using Hotel.Domain.Enums;

namespace Hotel.Domain.Entities.RoomContext.ReportEntity;

public partial class Report : Entity
{
  public Report(string summary,string description, EPriority priority, Employee employee, string resolution = "")
  {
    Summary = summary;
    Description = description;
    Status = EStatus.Pending;
    Priority = priority;
    Resolution = resolution;
    Employee = employee;
    EmployeeId = employee.Id;

    Validate();
  }
  
  public string Summary { get; private set; }
  public string Description { get; private set; }
  public EStatus Status { get; private set; }
  public EPriority Priority { get; private set; }
  public string Resolution { get; private set; }
  public Guid EmployeeId { get; private set; }
  public Employee? Employee { get; private set; }
}