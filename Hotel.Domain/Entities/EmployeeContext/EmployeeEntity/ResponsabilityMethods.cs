using Hotel.Domain.Entities.EmployeeContext.Interfaces;
using Hotel.Domain.Entities.EmployeeContext.ResponsabilityEntity;


namespace Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;

public partial class Employee : IResponsabilitiesMethods
{
  public void AddResponsability(Responsability responsability)
  {
    if (!Responsabilities.Contains(responsability))
      Responsabilities.Add(responsability);
    else
      throw new ArgumentException("Esta responsabilidade já está atribuida à esse funcionário.");
  }

  public void RemoveResponsability(Responsability responsability)
  {
    if (Responsabilities.Contains(responsability))
      Responsabilities.Remove(responsability);
    else
      throw new ArgumentException("Esta responsabilidade NÃO está atribuida à esse funcionário.");
  }

  
}