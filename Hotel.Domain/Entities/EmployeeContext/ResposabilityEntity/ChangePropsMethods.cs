using Hotel.Domain.Enums;

namespace Hotel.Domain.Entities.EmployeeContext.ResponsabilityEntity;

public partial class Responsability 
{
  public void ChangeName(string name)
  => Name = name;

  public void ChangeDescription(string description)
  => Description = description;

  public void ChangePriority(EPriority priority)
  => Priority = priority;

}