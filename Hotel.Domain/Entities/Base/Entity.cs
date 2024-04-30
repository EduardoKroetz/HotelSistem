using Hotel.Domain.Entities.Interfaces;

namespace Hotel.Domain.Entities.Base;

public class Entity : IEntity
{
  public Entity()
  {
    CreatedAt = DateTime.Now;
  }
  public Guid Id { get; private set; } = Guid.NewGuid();
  public DateTime CreatedAt { get; private set;  }
  
}