using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Entities.RoomContext.RoomEntity;

public partial class Room
{
  public override void Validate()
  {
    Category?.Validate();

    ValidateCapacity(Capacity);
    ValidateDescription(Description);
    ValidateNumber(Number);
    ValidatePrice(Price);
    
    base.Validate();
  }

  public void ValidateCapacity(int capacity)
  {
    if (capacity < 1)
      throw new ValidationException("Capacidade mínima do quarto de 1 ultrapassada.");
  }

  public void ValidateNumber(int number)
  {
    if (number < 0)
      throw new ValidationException("O número do quarto não pode ser menor que 0.");
  }
  
  public void ValidateDescription(string description)
  {
    if (description.Length > 2000)
      throw new ValidationException("Limite de 2000 caracteres da descrição foi atingido.");
  }

  public void ValidatePrice(decimal price)
  {
    if (price < 1)
      throw new ValidationException("O preço de quarto não pode ser menor ou igual a zero.");
  }
}