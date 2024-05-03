using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Entities.RoomContext.RoomEntity;

public partial class Room
{
  public override void Validate()
  {
    ValidateCapacity(Capacity);
    ValidateDescription(Description);
    ValidateNumber(Number);
    ValidatePrice(Price);
    
    base.Validate();
  }

  public void ValidateCapacity(int capacity)
  {
    if (capacity < 1)
      throw new ValidationException("Erro de validação: A capacidade de pessoas do quarto deve ser maior que 0.");
  }

  public void ValidateNumber(int number)
  {
    if (number < 0)
      throw new ValidationException("Erro de validação: O número do quarto não pode ser menor que 0.");
  }
  
  public void ValidateDescription(string description)
  {
    if (string.IsNullOrEmpty(description))
      throw new ValidationException("Erro de validação: A descrição do quarto é obrigatória.");
    if (description.Length > 500)
      throw new ValidationException("Erro de validação: Limite de 500 caracteres da descrição foi atingido.");
  }

  public void ValidatePrice(decimal price)
  {
    if (price < 1)
      throw new ValidationException("Erro de validação: O preço do quarto não pode ser menor ou igual a zero.");
  }
}