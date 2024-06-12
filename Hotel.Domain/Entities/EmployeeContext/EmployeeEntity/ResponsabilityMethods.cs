using Hotel.Domain.Entities.EmployeeContext.EmployeeEntity.Interfaces;
using Hotel.Domain.Entities.EmployeeContext.ResponsibilityEntity;

namespace Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;

public partial class Employee : IResponsabilitiesMethods
{
  public void AddResponsibility(Responsibility responsibility)
  {
    if (Responsabilities.Contains(responsibility))
      throw new ArgumentException("Essa responsabilidade j� est� atribuida.");
    Responsabilities.Add(responsibility);
  }
 
  
  public void RemoveResponsibility(Responsibility responsibility)
  {
    if (!Responsabilities.Contains(responsibility))
      throw new ArgumentException("Essa responsabilidade n�o est� atribuida.");
    Responsabilities.Remove(responsibility);
  }

  
}