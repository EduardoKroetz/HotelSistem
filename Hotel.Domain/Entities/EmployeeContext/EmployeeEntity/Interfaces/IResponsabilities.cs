using Hotel.Domain.Entities.EmployeeContext.ResponsabilityEntity;

namespace Hotel.Domain.Entities.EmployeeContext.Interfaces;

public interface IResponsabilitiesMethods
{
  public void AddResponsability(Responsability responsability);
  public void RemoveResponsability(Responsability responsability);
}