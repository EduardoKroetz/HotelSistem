using Hotel.Domain.Entities.Base.Interfaces;
using Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;
using Hotel.Domain.Enums;

namespace Hotel.Domain.Entities.Interfaces;

public interface IReport : IEntity
{
  string Summary { get; }
  string Description { get; }
  EStatus Status { get; }
  EPriority Priority { get; }
  string Resolution { get; }
  Guid EmployeeId { get; }
  Employee? Employee { get; }
}
