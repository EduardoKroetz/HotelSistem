using Hotel.Domain.DTOs.ResponsibilityDTOs;
using Hotel.Domain.Entities.ResponsibilityEntity;

namespace Hotel.Domain.Repositories.Interfaces;

public interface IResponsibilityRepository : IRepository<Responsibility>, IRepositoryQuery<GetResponsibility, ResponsibilityQueryParameters>
{
}