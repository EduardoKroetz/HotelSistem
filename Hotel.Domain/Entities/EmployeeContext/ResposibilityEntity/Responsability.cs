using Hotel.Domain.Entities.Base;
using Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;
using Hotel.Domain.Entities.Interfaces;
using Hotel.Domain.Entities.RoomContext.ServiceEntity;
using Hotel.Domain.Enums;

namespace Hotel.Domain.Entities.EmployeeContext.ResponsibilityEntity;

public partial class Responsibility : Entity, IResponsibility
{
  internal Responsibility() {}
  public Responsibility(string name, string description, EPriority priority)
  {
    Name = name;
    Description = description;
    Priority = priority;

    Validate();
  }

  public string Name { get; private set; } = string.Empty;
  public string Description { get; private set; } = string.Empty;
  public EPriority Priority { get; private set; }
  public HashSet<Employee> Employees { get; private set; } = [];
  public HashSet<Service> Services { get; private set; } = [];

}