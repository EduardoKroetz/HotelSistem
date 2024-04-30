using System.Text.RegularExpressions;
using Hotel.Domain.Exceptions;
using Hotel.Domain.ValueObjects.Interfaces;

namespace Hotel.Domain.ValueObjects;

public class Email : IValueObject
{
  public Email(string address)
  {
      Address = address;
  }

  public string Address { get; private set; }
  public bool IsValid { get; private set; } = false;

    public void Validate()
  {
    var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").IsMatch(Address);
    if (!regex)
      throw new ValidationException("Informe o email em um formato válido.");
    IsValid = true;
  }
}