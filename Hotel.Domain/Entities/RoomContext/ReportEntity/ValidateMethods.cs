using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Entities.RoomContext.ReportEntity;

public partial class Report
{
  public override void Validate()
  {
    ValidateSummary(Summary);
    ValidateDescription(Description);

    base.Validate();
  }


  public void ValidateSummary(string summary)
  {
    if (string.IsNullOrEmpty(summary))
      throw new ValidationException("Erro de validação: O sumário do relatório é obrigatório.");
    if (summary.Length > 50)
      throw new ValidationException("Erro de validação: Limite de 50 caracteres do sumário do relatório foi atingido.");
  }

  public void ValidateDescription(string description)
  {
    if (string.IsNullOrEmpty(description))
      throw new ValidationException("Erro de validação: A descrição do relatório é obrigatória.");
  }

  
}