using Hotel.Domain.Entities.ResponsibilityEntity;
using Hotel.Domain.Entities.EmployeeEntity.Interfaces;

namespace Hotel.Domain.Entities.EmployeeEntity;

public partial class Employee : IResponsibilitiesMethods
{
    public void AddResponsibility(Responsibility responsibility)
    {
        if (Responsibilities.Contains(responsibility))
            throw new ArgumentException("Essa responsabilidade já está atribuida a esse funcionário");
        Responsibilities.Add(responsibility);
    }


    public void RemoveResponsibility(Responsibility responsibility)
    {
        if (!Responsibilities.Contains(responsibility))
            throw new ArgumentException("Essa responsabilidade não está atribuida a esse funcionário");
        Responsibilities.Remove(responsibility);
    }


}