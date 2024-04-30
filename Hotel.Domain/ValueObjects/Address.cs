using Hotel.Domain.Exceptions;
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

  public bool IsValid { get; private set; } = false;

  public void Validate()
  {
    if (string.IsNullOrEmpty(Country) || string.IsNullOrEmpty(City) || string.IsNullOrEmpty(Street))
      throw new ValidationException("Preencha todos os campos."); 
    if (Number <= 0)
      throw new ValidationException("O número da casa não pode ser menor que 0."); 
    IsValid = true;
  }
}