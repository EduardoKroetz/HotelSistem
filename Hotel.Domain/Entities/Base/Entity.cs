using Hotel.Domain.Entities.Base.Interfaces;
using Hotel.Domain.Entities.Validation;

namespace Hotel.Domain.Entities.Base;

public class Entity : IEntity, IValidation
{
  public Entity()
  {
    CreatedAt = DateTime.Now;
  }
  public Guid Id { get; private set; } = Guid.NewGuid();
  public DateTime CreatedAt { get; private set;  }

  public bool IsValid { get; private set; } = false;

  public virtual void Validate()
  {
    IsValid = true;
  }
}