using Hotel.Domain.Exceptions;
using Hotel.Domain.ValueObjects.Base;

namespace Hotel.Domain.ValueObjects;

public class Name : ValueObject
{
  public Name(string firstName, string lastName)
  {
    FirstName = firstName;
    LastName = lastName;

    Validate();
  }

  public string FirstName { get; private set; }
  public string LastName { get; private set;}

  public override void Validate()
  {
    if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName))
      throw new ValidationException("Informe o primeiro e o segundo nome.");
    
    base.Validate();
  }

  public string GetFullName()
  => $"{FirstName} {LastName}";
  
}