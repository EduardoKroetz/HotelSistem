using Hotel.Domain.Entities.EmployeeContext.EmployeeEntity.Interfaces;
using Hotel.Domain.Entities.EmployeeContext.ResponsibilityEntity;

namespace Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;

public partial class Employee : IResponsibilitiesMethods
{
  public void AddResponsibility(Responsibility responsibility)
  {
    if (Responsibilities.Contains(responsibility))
      throw new ArgumentException("Essa responsabilidade j� est� atribuida.");
    Responsibilities.Add(responsibility);
  }
 
  
  public void RemoveResponsibility(Responsibility responsibility)
  {
    if (!Responsibilities.Contains(responsibility))
      throw new ArgumentException("Essa responsabilidade n�o est� atribuida.");
    Responsibilities.Remove(responsibility);
  }

  
}