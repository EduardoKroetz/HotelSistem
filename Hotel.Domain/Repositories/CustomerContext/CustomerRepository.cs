using Hotel.Domain.Data;
using Hotel.Domain.Entities.CustomerContext;
using Hotel.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Repositories;

public class CustomerRepository :  UserRepository<Customer> ,ICustomerRepository
{
  public CustomerRepository(HotelDbContext context) : base(context) {}

  public async Task<IEnumerable<Customer>> GetCustomersByListId(List<Guid> CustomersIds)
  {
    return await _context
      .Customers
      .Where(x => CustomersIds.Contains(x.Id))
      .ToListAsync();
  }
}
