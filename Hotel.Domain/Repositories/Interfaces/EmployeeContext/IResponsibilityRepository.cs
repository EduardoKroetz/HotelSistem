using Hotel.Domain.DTOs.EmployeeContext.ResponsibilityDTOs;
using Hotel.Domain.Entities.EmployeeContext.ResponsibilityEntity;

namespace Hotel.Domain.Repositories.Interfaces.EmployeeContext;

public interface IResponsibilityRepository : IRepository<Responsibility>, IRepositoryQuery<GetResponsibility, ResponsibilityQueryParameters>
{
}