using Hotel.Domain.Entities.EmployeeContext.Interfaces;
using Hotel.Domain.Entities.EmployeeContext.ResponsabilityEntity;

namespace Hotel.Domain.Entities.RoomContext.ServiceEntity;

public partial class Service : IResponsabilitiesMethods
{
  public void AddResponsability(Responsability responsability)
  => Responsabilities.Add(responsability);
  

  public void RemoveResponsability(Responsability responsability)
  => Responsabilities.Remove(responsability);


}