using Hotel.Domain.Entities.EmployeeContext.EmployeeEntity.Interfaces;
using Hotel.Domain.Entities.EmployeeContext.ResponsibilityEntity;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Entities.RoomContext.ServiceEntity;

public partial class Service : IResponsibilitiesMethods
{
  public void AddResponsibility(Responsibility responsibility)
  {
    if (Responsibilities.Contains(responsibility))
      throw new ValidationException("Essa responsabilidade já foi atribuida.");
    Responsibilities.Add(responsibility);
  }

  

  public void RemoveResponsibility(Responsibility responsibility)
  {
    if (!Responsibilities.Remove(responsibility))
      throw new ValidationException("Essa responsabilidade não está atribuida.");
  }
  

}