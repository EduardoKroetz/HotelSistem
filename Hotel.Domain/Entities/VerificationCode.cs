using Hotel.Domain.Entities.Base;

namespace Hotel.Domain.Entities;

public class VerificationCode : Entity
{
  public VerificationCode()
  {
    var randomNumber = new Random().Next(100000, 1000000);
    Code = randomNumber.ToString();
  }
  public string Code { get; private set; }
}
