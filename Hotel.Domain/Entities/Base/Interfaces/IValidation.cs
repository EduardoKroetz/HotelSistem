namespace Hotel.Domain.Entities.Base.Interfaces;

public interface IValidation
{
    bool IsValid { get; }
    void Validate();
}