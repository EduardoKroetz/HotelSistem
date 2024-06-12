using Hotel.Domain.Entities.EmployeeContext.EmployeeEntity.Interfaces;
using Hotel.Domain.Entities.EmployeeContext.ResponsibilityEntity;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Entities.RoomContext.ServiceEntity;

public partial class Service : IResponsabilitiesMethods
{
  public void AddResponsibility(Responsibility responsibility)
  {
    if (Responsabilities.Contains(responsibility))
      throw new ValidationException("Erro de validação: Essa responsabilidade já foi atribuida.");
    Responsabilities.Add(responsibility);
  }

  

  public void RemoveResponsibility(Responsibility responsibility)
  {
    if (!Responsabilities.Remove(responsibility))
      throw new ValidationException("Erro de validação: Essa responsabilidade não está atribuida.");
  }
  

}