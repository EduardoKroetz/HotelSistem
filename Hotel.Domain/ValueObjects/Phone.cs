using System.Text.RegularExpressions;
using Hotel.Domain.Exceptions;
using Hotel.Domain.ValueObjects.Base;

namespace Hotel.Domain.ValueObjects;

public class Phone : ValueObject
{
  public Phone(string number)
  {
    Number = number;
    
    Validate();
  }

  public string Number { get; private set; }

  public override void Validate()
  {
    var regex = new Regex(@"^\+\d{2,3}\s\(\d{2,3}\)\s\d{5}-\d{4}$").IsMatch(Number);
    if (!regex)
        throw new ValidationException("Informe o telefone em um formato válido.");
    base.Validate();
}

}