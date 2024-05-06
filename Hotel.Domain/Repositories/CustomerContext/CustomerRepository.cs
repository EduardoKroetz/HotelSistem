using Hotel.Domain.Data;
using Hotel.Domain.Entities.CustomerContext;
using Hotel.Domain.Repositories.Interfaces;

namespace Hotel.Domain.Repositories;

public class CustomerRepository :  UserRepository<Customer> ,ICustomerRepository
{
  public CustomerRepository(HotelDbContext context) : base(context) {}

}
