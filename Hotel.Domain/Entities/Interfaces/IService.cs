
using Hotel.Domain.Entities.Base.Interfaces;
using Hotel.Domain.Entities.EmployeeContext.ResponsibilityEntity;
using Hotel.Domain.Enums;

namespace Hotel.Domain.Entities.Interfaces;

public interface IService : IEntity
{
  string Name { get; }
  decimal Price { get; }
  bool IsActive { get; }
  EPriority Priority { get; }
  int TimeInMinutes { get; }
  ICollection<Responsibility> Responsabilities { get; }
}