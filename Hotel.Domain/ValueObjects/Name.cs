using Hotel.Domain.Exceptions;
using Hotel.Domain.ValueObjects.Interfaces;

namespace Hotel.Domain.ValueObjects;

public class Name : IValueObject
{
  public Name(string firstName, string lastName)
  {
    FirstName = firstName;
    LastName = lastName;

    Validate();
  }

  public string FirstName { get; private set; }
  public string LastName { get; private set;}

  public bool IsValid { get; private set; } = false;

  public void Validate()
  {
    if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName))
      throw new ValidationException("Preencha todos os campos");
    IsValid = true;
  }
}