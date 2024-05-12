using Hotel.Domain.Data;
using Hotel.Domain.DTOs.Base.User;
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

  public new async Task<IEnumerable<GetUser>> GetAsync(UserQueryParameters queryParameters)
  {
    var query = base.GetAsync(queryParameters);

    return await query.Select(x => new GetUser( 
      x.Id,
      x.Name.FirstName,
      x.Name.LastName,
      x.Email.Address,
      x.Phone.Number,
      x.CreatedAt
    )).ToListAsync();
  }
}
