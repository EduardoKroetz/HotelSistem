using Hotel.Domain.Entities.ResponsibilityEntity;
using Hotel.Domain.Entities.EmployeeEntity.Interfaces;

namespace Hotel.Domain.Entities.EmployeeEntity;

public partial class Employee : IResponsibilitiesMethods
{
    public void AddResponsibility(Responsibility responsibility)
    {
        if (Responsibilities.Contains(responsibility))
            throw new ArgumentException("Essa responsabilidade j� est� atribuida.");
        Responsibilities.Add(responsibility);
    }


    public void RemoveResponsibility(Responsibility responsibility)
    {
        if (!Responsibilities.Contains(responsibility))
            throw new ArgumentException("Essa responsabilidade n�o est� atribuida.");
        Responsibilities.Remove(responsibility);
    }


}