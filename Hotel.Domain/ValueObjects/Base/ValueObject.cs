using Hotel.Domain.ValueObjects.Interfaces;

namespace Hotel.Domain.ValueObjects.Base;

public class ValueObject : IValueObject
{
    public bool IsValid { get; private set; }

    public virtual void Validate()
    {
        IsValid = true;
    }
}