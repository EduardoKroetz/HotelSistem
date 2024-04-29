using Hotel.Domain.Entities.Interfaces;

namespace Hotel.Domain.Entities;

public class Entity : IEntity
{
  public Guid Id { get; private set; } = Guid.NewGuid();
}