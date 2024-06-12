using Hotel.Domain.Entities.Base.Interfaces;
using Hotel.Domain.Entities.EmployeeContext.ResponsibilityEntity;

namespace Hotel.Domain.Entities.Interfaces;
public interface IEmployee : IUser
{
  decimal? Salary { get; }
  HashSet<Responsibility> Responsabilities { get; }
}
