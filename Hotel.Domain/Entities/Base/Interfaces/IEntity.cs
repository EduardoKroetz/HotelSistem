using Hotel.Domain.Entities.Validation;

namespace Hotel.Domain.Entities.Base.Interfaces;

public interface IEntity : IValidation
{
  Guid Id { get; }
}