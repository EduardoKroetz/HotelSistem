using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Entities.AdminContext.PermissionEntity;

public partial class Permission 
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
      throw new ValidationException("Erro de validação: Informe o nome da permissão");
  }

  public void ValidateDescription(string description)
  {
    if (string.IsNullOrEmpty(description))
      throw new ValidationException("Erro de validação: Informe a descrição da permissão");
    if (description.Length > 500)
      throw new ValidationException("Erro de validação: Informe a descrição da permissão com no máximo 500 caracteres.");
  }
}