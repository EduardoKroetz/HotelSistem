using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Entities.CategoryEntity;

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
            throw new ValidationException("Informe o nome da categoria");
    }

    public void ValidateDescription(string description)
    {
        if (string.IsNullOrEmpty(description))
            throw new ValidationException("Informe a descrição da categoria");
    }

    public void ValidateAveragePrice(decimal price)
    {
        if (price < 0)
            throw new ValidationException("O preço médio da categoria não pode ser negativo");
    }
}