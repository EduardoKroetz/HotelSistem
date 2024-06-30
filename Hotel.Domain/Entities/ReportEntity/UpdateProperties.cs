using Hotel.Domain.Entities.EmployeeEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Entities.ReportEntity;

public partial class Report
{
    public void ChangeSummary(string summary)
    {
        ValidateSummary(summary);
        Summary = summary;
    }

    public void ChangeDescription(string description)
    {
        ValidateDescription(description);
        Description = description;
    }

    public void ChangePriority(EPriority priority)
    {
        ValidatePriority((int)priority);
        Priority = priority;
    }

    public void ChangeEmployee(Employee employee)
    => Employee = employee;

    public void ChangeResolution(string resolution)
    => Resolution = resolution;

    public void Finish()
    {
        if (Status == EStatus.Cancelled)
            throw new ValidationException("N�o � poss�vel finalizar um relat�rio cancelado");
        if (Status == EStatus.Finish)
            throw new ValidationException("O relat�rio j� est� finalizado");

        if (Status == EStatus.Pending)
            Status = EStatus.Finish;
        else
            throw new ValidationException("S� � poss�vel finalizar com o status 'Pendente'.");
    }


    public void Cancel()
    {
        if (Status == EStatus.Cancelled)
            throw new ValidationException("O relat�rio j� est� cancelado");
        if (Status == EStatus.Finish)
            throw new ValidationException("N�o � poss�vel cancelar um relat�rio finalizado");

        if (Status == EStatus.Pending)
            Status = EStatus.Cancelled;
        else
            throw new ValidationException("S� � poss�vel cancelar com o status 'Pendente'.");
    }
}