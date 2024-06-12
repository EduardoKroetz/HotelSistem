using Hotel.Domain.Enums;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Entities.EmployeeContext.ResponsibilityEntity;

public partial class Responsibility 
{
  public override void Validate()
  { 
    ValidateName(Name);
    ValidateDescription(Description);
    ValidatePriority(Priority);

    base.Validate();
  }

  public void ValidateName(string name)
  {
    if (string.IsNullOrEmpty(name))
      throw new ValidationException("Erro de validação: Informe o nome da responsabilidade.");
  }

  public void ValidatePriority(EPriority priority)
  {
    if ((int)priority > 5)
      throw new ValidationException("Erro de validação: Prioridade inválida.");
  }

  public void ValidateDescription(string description)
  {
    if (string.IsNullOrEmpty(description))
      throw new ValidationException("Erro de validação: Informe a descrição da responsabilidade.");
    if (description.Length > 250)
      throw new ValidationException("Erro de validação: A descrição não pode ultrapassar 250 caracteres.");
  }

}