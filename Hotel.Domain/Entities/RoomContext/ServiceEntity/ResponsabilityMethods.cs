using Hotel.Domain.Entities.EmployeeContext.Interfaces;
using Hotel.Domain.Entities.EmployeeContext.ResponsabilityEntity;

namespace Hotel.Domain.Entities.RoomContext.ServiceEntity;

public partial class Service : IResponsabilitiesMethods
{
  public void AddResponsability(Responsability responsability)
  {
    responsability.Validate();
    if (Responsabilities.Contains(responsability))
      throw new ArgumentException("Esta responsabilidade já está atribuida à esse serviço.");
    Responsabilities.Add(responsability);
  }

  public void RemoveResponsability(Responsability responsability)
  {
    if (Responsabilities.Contains(responsability))
      Responsabilities.Remove(responsability);
    else
      throw new ArgumentException("Esta responsabilidade NÃO está atribuida à esse serviço.");
  }

}