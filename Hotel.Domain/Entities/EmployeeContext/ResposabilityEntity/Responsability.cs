using Hotel.Domain.Entities.Base;
using Hotel.Domain.Entities.Interfaces;
using Hotel.Domain.Enums;

namespace Hotel.Domain.Entities.EmployeeContext.ResponsabilityEntity;

public partial class Responsability : Entity, IResponsability
{
  public Responsability(string name, string description, EPriority priority)
  {
    Name = name;
    Description = description;
    Priority = priority;

    Validate();
  }

  public string Name { get; private set; }
  public string Description { get; private set; }
  public EPriority Priority { get; private set; }

}