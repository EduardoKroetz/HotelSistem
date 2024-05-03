using Hotel.Domain.Enums;

namespace Hotel.Domain.Entities.EmployeeContext.ResponsabilityEntity;

public partial class Responsability 
{
  public void ChangeName(string name)
  {
    ValidateName(name);
    Name = name;
  }


  public void ChangeDescription(string description)
  {
    ValidateDescription(description);
    Description = description;
  }

  public void ChangePriority(EPriority priority)
  => Priority = priority;

}