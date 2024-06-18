using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.Entities.CustomerEntity;
using Hotel.Domain.Repositories.Base.Interfaces;

namespace Hotel.Domain.Repositories.Interfaces;

public interface ICustomerRepository : IRepository<Customer>, IRepositoryQuery<GetUser, UserQueryParameters>, IUserRepository<Customer>
{
    public Task<IEnumerable<Customer>> GetCustomersByListId(List<Guid> CustomersIds);
}