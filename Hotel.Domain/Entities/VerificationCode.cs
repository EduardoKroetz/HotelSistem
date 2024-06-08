using Hotel.Domain.Entities.Base;
using Hotel.Domain.ValueObjects;

namespace Hotel.Domain.Entities;

public class VerificationCode : Entity
{
  public VerificationCode()
  {
    Code = GenerateCode();
  }

  public VerificationCode(Email email)
  {
    Code = GenerateCode();
    Email = email;
  }

  public VerificationCode(Phone phone)
  {
    Code = GenerateCode();
    Phone = phone;
  }


  public VerificationCode(string code, Email? email = null, Phone? phone = null)
  {
    if (code.Length != 6)
      throw new ArgumentException("Código inválido");

    Code = code;
    Email = email;
    Phone = phone;
  }

  public string Code { get; private set; }
  public Email? Email { get; private set; }
  public Phone? Phone { get; private set; }

  private Func<string> GenerateCode = () => new Random().Next(100000, 1000000).ToString();
}
