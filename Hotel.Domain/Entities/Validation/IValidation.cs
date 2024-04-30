namespace Hotel.Domain.Entities.Validation;

public interface IValidation
{
  bool IsValid { get; }
  void Validate();
}