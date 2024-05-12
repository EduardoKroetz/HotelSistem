using Hotel.Domain.Entities.EmployeeContext.EmployeeEntity.Interfaces;
using Hotel.Domain.Entities.EmployeeContext.ResponsabilityEntity;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Entities.RoomContext.ServiceEntity;

public partial class Service : IResponsabilitiesMethods
{
  public void AddResponsability(Responsability responsability)
  {
    if (Responsabilities.Contains(responsability))
      throw new ValidationException("Erro de validação: Essa responsabilidade já foi atribuida.");
    Responsabilities.Add(responsability);
  }

  

  public void RemoveResponsability(Responsability responsability)
  {
    if (!Responsabilities.Remove(responsability))
      throw new ValidationException("Erro de validação: Responsabilidade não está atribuida.");
  }
  


}