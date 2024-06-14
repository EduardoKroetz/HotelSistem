using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Entities.RoomContext.ServiceEntity;

public partial class Service
{
  public override void Validate()
  {
    ValidateName(Name);
    ValidatePrice(Price);
    ValidateTimeInMinutes(TimeInMinutes);
    ValidatePriority((int)Priority);

    base.Validate();
  }

  public void ValidatePrice(decimal price)
  {
    if (price < 1)
      throw new ValidationException("O preço do serviço não pode ser menor ou igual a zero.");
  }

  public void ValidateTimeInMinutes(int minutes)
  {
    if (minutes <= 0)
      throw new ValidationException("O tempo do serviço em minutos não pode ser menor ou igual a zero.");
  }
  
  
  public void ValidateName(string name)
  {
    if (string.IsNullOrEmpty(name))
      throw new ValidationException("O nome do serviço é obrigatório.");
  }

  public void ValidatePriority(int priority) 
  {
    if (priority > 5 || priority < 1)
      throw new ValidationException("Prioridade inválida.");

  }

}