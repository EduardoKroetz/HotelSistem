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
        if (Status == EStatus.Pending)
            Status = EStatus.Finish;
        else
            throw new ValidationException("Só é possível finalizar com o status 'Pendente'.");
    }


    public void Cancel()
    {
        if (Status == EStatus.Pending)
            Status = EStatus.Cancelled;
        else
            throw new ValidationException("Só é possível cancelar com o status 'Pendente'.");
    }
}