using Hotel.Domain.Entities.EmployeeContext.Interfaces;
using Hotel.Domain.Entities.EmployeeContext.ResponsabilityEntity;
using Hotel.Domain.Exceptions;


namespace Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;

public partial class Employee : IResponsabilitiesMethods
{
  public void AddResponsability(Responsability responsability)
  {
    if (!Responsabilities.Contains(responsability))
      Responsabilities.Add(responsability);
    else
      throw new ValidationException("Erro de validação: Esta responsabilidade já está atribuida à esse funcionário.");
  }

  public void RemoveResponsability(Responsability responsability)
  {
    if (!Responsabilities.Remove(responsability))
      throw new ValidationException("Erro de validação: Responsabilidade não encontrada.");
  }

  
}