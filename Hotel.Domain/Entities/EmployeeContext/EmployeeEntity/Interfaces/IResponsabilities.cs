using Hotel.Domain.Entities.EmployeeContext.ResponsibilityEntity;

namespace Hotel.Domain.Entities.EmployeeContext.EmployeeEntity.Interfaces;

public interface IResponsibilitiesMethods
{
  public void AddResponsibility(Responsibility responsibility);
  public void RemoveResponsibility(Responsibility responsibility);
}