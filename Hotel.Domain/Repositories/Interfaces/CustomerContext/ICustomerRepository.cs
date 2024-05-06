using Hotel.Domain.DTOs.User;
using Hotel.Domain.Entities.CustomerContext;

namespace Hotel.Domain.Repositories.Interfaces;

public interface ICustomerRepository : IRepository<Customer>, IRepositoryQuery<GetUser>
{
}