using System.Text.RegularExpressions;
using Hotel.Domain.Exceptions;
using Hotel.Domain.ValueObjects.Base;

namespace Hotel.Domain.ValueObjects;

public class Email : ValueObject
{
  public Email(string? address)
  {
    if (string.IsNullOrEmpty(address))
      throw new ValidationException("Email inválido.");

    Validate(address);

    Address = address;
  }

  public string Address { get; private set; }

  public void Validate(string email)
  {
    var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").IsMatch(email);
    if (!regex)
      throw new ValidationException("Informe o email em um formato válido.");

    base.Validate();
  }
}