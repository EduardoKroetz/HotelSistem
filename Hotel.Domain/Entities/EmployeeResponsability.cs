using Hotel.Domain.Entities.Base;
using Hotel.Domain.Enums;

namespace Hotel.Domain.Entities;

public class EmployeeResponsability : Entity
{
  public EmployeeResponsability(string name, string description, EPriority priority)
  {
    Name = name;
    Description = description;
    Priority = priority;
  }

  public string Name { get; private set; }
  public string Description { get; private set; }
  public EPriority Priority { get; private set; }
}