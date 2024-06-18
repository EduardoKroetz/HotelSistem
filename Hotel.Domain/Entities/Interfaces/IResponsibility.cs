using Hotel.Domain.Entities.Base.Interfaces;
using Hotel.Domain.Enums;

namespace Hotel.Domain.Entities.Interfaces;

public interface IResponsibility : IEntity
{
    string Name { get; }
    string Description { get; }
    EPriority Priority { get; }
}
