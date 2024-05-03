using System.Text.RegularExpressions;
using Hotel.Domain.Exceptions;
using Hotel.Domain.ValueObjects.Base;

namespace Hotel.Domain.ValueObjects;

public class Email : ValueObject
{
  public Email(string address)
  {
    Address = address;

    Validate();
  }

  public string Address { get; private set; }

  public override void Validate()
  {
    var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").IsMatch(Address);
    if (!regex)
      throw new ValidationException("Informe o email em um formato válido.");

    base.Validate();
  }
}