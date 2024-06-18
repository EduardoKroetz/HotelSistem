using Hotel.Domain.Entities.ResponsibilityEntity;

namespace Hotel.Domain.Entities.EmployeeEntity.Interfaces;

public interface IResponsibilitiesMethods
{
    public void AddResponsibility(Responsibility responsibility);
    public void RemoveResponsibility(Responsibility responsibility);
}