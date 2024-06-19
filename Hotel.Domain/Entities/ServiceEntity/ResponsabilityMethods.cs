using Hotel.Domain.Entities.ResponsibilityEntity;
using Hotel.Domain.Entities.EmployeeEntity.Interfaces;
using Hotel.Domain.Exceptions;

namespace Hotel.Domain.Entities.ServiceEntity;

public partial class Service : IResponsibilitiesMethods
{
    public void AddResponsibility(Responsibility responsibility)
    {
        if (Responsibilities.Contains(responsibility))
            throw new ValidationException("Essa responsabilidade já foi atribuida.");
        Responsibilities.Add(responsibility);
    }

    public void RemoveResponsibility(Responsibility responsibility)
    {
        if (!Responsibilities.Remove(responsibility))
            throw new ValidationException("Essa responsabilidade não está atribuida.");
    }


}