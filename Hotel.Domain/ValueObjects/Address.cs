using Hotel.Domain.Exceptions;
using Hotel.Domain.ValueObjects.Base;

namespace Hotel.Domain.ValueObjects;

public class Address : ValueObject
{
  public Address(string? country, string? city, string? street, int? number)
  {
    Country = country;
    City = city;
    Street = street;
    Number = number;

    Validate();
  }

  public string? Country { get; private set; }
  public string? City { get; private set; }
  public string? Street { get; private set; }
  public int? Number { get; private set; }

}