using Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;
using Hotel.Domain.Enums;

namespace Hotel.Domain.Entities.RoomContext.ReportEntity;

public partial class Report
{
  public void ChangeSummary(string summary)
  {
    ValidateSummary(summary);
    Summary = summary;
  }
  
  public void ChangeDescription(string description)
  {
    ValidateDescription(description);
    Description = description;
  }

  public void ChangePriority(EPriority priority)
  => Priority = priority;
  public void ChangeEmployee(Employee employee)
  => Employee = employee;
    
  public void ChangeResolution(string resolution)
  => Resolution = resolution;

}