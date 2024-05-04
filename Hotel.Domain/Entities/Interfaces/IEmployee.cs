using Hotel.Domain.Entities.Base.Interfaces;
using Hotel.Domain.Entities.EmployeeContext.ResponsabilityEntity;

namespace Hotel.Domain.Entities.Interfaces;
public interface IEmployee : IUser
{
  decimal? Salary { get; }
  HashSet<Responsability> Responsabilities { get; }
}