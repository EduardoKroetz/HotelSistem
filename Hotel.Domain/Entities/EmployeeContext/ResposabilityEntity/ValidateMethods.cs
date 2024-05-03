using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Entities.EmployeeContext.ResponsabilityEntity;

public partial class Responsability 
{
  public override void Validate()
  { 
    ValidateName(Name);
    ValidateDescription(Description);

    base.Validate();
  }

  public void ValidateName(string name)
  {
    if (string.IsNullOrEmpty(name))
      throw new ValidationException("Erro de validação: Informe o nome da responsabilidade.");
  }

  public void ValidateDescription(string description)
  {
    if (string.IsNullOrEmpty(description))
      throw new ValidationException("Erro de validação: Informe a descrição da responsabilidade.");
    if (description.Length > 500)
      throw new ValidationException("Erro de validação: A descrição não pode ultrapassar 500 caracteres.");
  }

}