using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Entities.RoomContext.CategoryEntity;

public partial class Category
{
  public override void Validate()
  {
    ValidateName(Name);
    ValidateDescription(Description);
    ValidateAveragePrice(AveragePrice);

    base.Validate();
  }

  public void ValidateName(string name)
  {
    if (string.IsNullOrEmpty(name))
      throw new ValidationException("Erro de validação: O nome da categoria é obrigatório.");
  }

  public void ValidateDescription(string description)
  {
    if (string.IsNullOrEmpty(description))
      throw new ValidationException("Erro de validação: A descrição da categoria é obrigatório.");
  }

  public void ValidateAveragePrice(decimal price)
  {
    if (price < 0)
      throw new ValidationException("Erro de validação: O preço médio da categoria não pode ser negativo.");
  }
}