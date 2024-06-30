using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Entities.ReportEntity;

public partial class Report
{
    public override void Validate()
    {
        ValidateSummary(Summary);
        ValidateDescription(Description);
        ValidatePriority((int)Priority);

        base.Validate();
    }


    public void ValidateSummary(string summary)
    {
        if (string.IsNullOrEmpty(summary))
            throw new ValidationException("Informe o sumário");
    }

    public void ValidateDescription(string description)
    {
        if (string.IsNullOrEmpty(description))
            throw new ValidationException("A descrição do relatório é obrigatória.");
    }

    public void ValidatePriority(int priority)
    {
        if (priority > 5 || priority < 1)
            throw new ValidationException("Prioridade inválida.");

    }


}