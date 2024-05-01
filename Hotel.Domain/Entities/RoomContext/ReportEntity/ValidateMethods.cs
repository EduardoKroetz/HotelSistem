using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Entities.RoomContext.ReportEntity;

public partial class Report
{
  public override void Validate()
  {
    Employee?.Validate();
    ValidateSummary(Summary);
    ValidateDescription(Description);

    base.Validate();
  }


  public void ValidateSummary(string summary)
  {
    if (string.IsNullOrEmpty(summary))
      throw new ValidationException("Informe o sumário do relatório.");
    if (summary.Length > 100)
      throw new ValidationException("Limite de 100 caracteres do sumário do relatório foi atingido.");
  }

  public void ValidateDescription(string description)
  {
    if (string.IsNullOrEmpty(description))
      throw new ValidationException("Informe a descrição do relatório.");
  }

  
}