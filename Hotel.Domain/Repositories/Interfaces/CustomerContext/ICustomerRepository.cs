using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.Entities.CustomerContext;
using Hotel.Domain.Repositories.Base.Interfaces;

namespace Hotel.Domain.Repositories.Interfaces.CustomerContext;

public interface ICustomerRepository : IRepository<Customer>, IRepositoryQuery<GetUser, UserQueryParameters>, IUserRepository<Customer>
{
  public Task<IEnumerable<Customer>> GetCustomersByListId(List<Guid> CustomersIds);
}