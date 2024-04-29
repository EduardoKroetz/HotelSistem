using Hotel.Domain.ValueObjects.Interfaces;

namespace Hotel.Domain.ValueObjects;

public class Name : IValueObject
{
  public Name(string firstName, string lastName)
  {
    FirstName = firstName;
    LastName = lastName;
  }

  public string FirstName { get; private set; }
  public string LastName { get; private set;}
}