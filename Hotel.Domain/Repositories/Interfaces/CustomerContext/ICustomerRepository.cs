using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.Entities.CustomerContext;

namespace Hotel.Domain.Repositories.Interfaces.CustomerContext;

public interface ICustomerRepository : IRepository<Customer>, IRepositoryQuery<GetUser, UserQueryParameters>
{
  public Task<IEnumerable<Customer>> GetCustomersByListId(List<Guid> CustomersIds);
}