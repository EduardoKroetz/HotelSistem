using Hotel.Domain.Entities.Base;
using Hotel.Domain.Entities.EmployeeContext;
using Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;
using Hotel.Domain.Enums;

namespace Hotel.Domain.Entities.RoomContext.ReportEntity;

public class Report : Entity
{
  public Report(string description, EPriority priority, Employee employee, string resolution = "")
  {
    Description = description;
    Status = EStatus.Pending;
    Priority = priority;
    Resolution = resolution;
    Employee = employee;
    EmployeeId = employee.Id;
  }

  public string Description { get; private set; }
  public EStatus Status { get; private set; }
  public EPriority Priority { get; private set; }
  public string Resolution { get; private set; }
  public Guid EmployeeId { get; private set; }
  public Employee? Employee { get; private set; }

  public void ChangeDescription(string description)
  => Description = description;
  public void ChangePriority(EPriority priority)
  => Priority = priority;
  public void ChangeEmployee(Employee employee)
  => Employee = employee;
  public void ChangeResolution(string resolution)
  => Resolution = resolution;

}