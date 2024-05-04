using Hotel.Domain.Entities.Base.Interfaces;

namespace Hotel.Domain.Entities.Interfaces;

public interface IPermission : IEntity
{
  string Name { get; }
  string Description { get; }
  bool IsActive { get; }
}
