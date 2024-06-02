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
    var containPlus = new Regex(@"^\+").IsMatch(Number);
    if (!containPlus)
      Number = "+" + Number;

    var regex = new Regex(@"^\+55\d{2}9\d{8}$").IsMatch(Number);
    if (!regex)
        throw new ValidationException("Informe o telefone em um formato válido.");
    base.Validate();
}

}