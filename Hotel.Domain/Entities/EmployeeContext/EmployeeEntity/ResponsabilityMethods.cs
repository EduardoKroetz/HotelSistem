using Hotel.Domain.Entities.EmployeeContext.EmployeeEntity.Interfaces;
using Hotel.Domain.Entities.EmployeeContext.ResponsabilityEntity;

namespace Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;

public partial class Employee : IResponsabilitiesMethods
{
  public void AddResponsability(Responsability responsability)
  {
    if (Responsabilities.Contains(responsability))
      throw new ArgumentException("Essa responsabilidade j� est� atribuida.");
    Responsabilities.Add(responsability);
  }
 
  
  public void RemoveResponsability(Responsability responsability)
  {
    if (!Responsabilities.Contains(responsability))
      throw new ArgumentException("Essa responsabilidade n�o est� atribuida.");
    Responsabilities.Remove(responsability);
  }

  
}