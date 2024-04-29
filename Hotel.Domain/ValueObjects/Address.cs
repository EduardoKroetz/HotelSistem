using Hotel.Domain.ValueObjects.Interfaces;

namespace Hotel.Domain.ValueObjects;

public class Address : IValueObject
{
  public Address(string country, string city, string street, int number)
  {
    Country = country;
    City = city;
    Street = street;
    Number = number;
  }

  public string Country { get; private set; }
  public string City { get; private set; }
  public string Street { get; private set; }
  public int Number { get; private set; }
}