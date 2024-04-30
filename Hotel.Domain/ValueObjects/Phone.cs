using System.Text.RegularExpressions;
using Hotel.Domain.Exceptions;
using Hotel.Domain.ValueObjects.Interfaces;

namespace Hotel.Domain.ValueObjects;

public class Phone : IValueObject
{
  public string Number { get; private set; }
  public bool IsValid { get; private set; } = false;

    public void Validate()
  {
    var regex = new Regex(@"^\+\d{2}\s\d{5}-\d{4}$").IsMatch(Number);
    if (!regex)
      throw new ValidationException("Informe o telefone em um formato válido.");
    IsValid = true;
  }
}