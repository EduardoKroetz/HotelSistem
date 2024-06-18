using Hotel.Domain.Entities.AdminEntity;
using Hotel.Domain.Entities.Base.Interfaces;
using Hotel.Domain.Entities.EmployeeEntity;

namespace Hotel.Domain.Entities.Interfaces;

public interface IPermission : IEntity
{
    string Name { get; }
    string Description { get; }
    bool IsActive { get; }

    ICollection<Admin> Admins { get; }
    ICollection<Employee> Employees { get; }
}
