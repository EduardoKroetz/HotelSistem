using Hotel.Domain.Exceptions;
using Hotel.Domain.ValueObjects.Base;

namespace Hotel.Domain.ValueObjects;

public class Address : ValueObject
{
  public Address(string country, string city, string street, int number)
  {
    Country = country;
    City = city;
    Street = street;
    Number = number;

    Validate();
  }

  public string Country { get; private set; }
  public string City { get; private set; }
  public string Street { get; private set; }
  public int Number { get; private set; }

  public override void Validate()
  {
    if (string.IsNullOrEmpty(Country) || string.IsNullOrEmpty(City) || string.IsNullOrEmpty(Street))
      throw new ValidationException("Informe os os campos de endereço."); 
    if (Number <= 0)
      throw new ValidationException("O número da casa não pode ser menor que 0."); 

    base.Validate();
  }
}