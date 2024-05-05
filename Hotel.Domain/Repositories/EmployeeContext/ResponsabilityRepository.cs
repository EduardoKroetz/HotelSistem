using Hotel.Domain.Data;
using Hotel.Domain.Entities.EmployeeContext.ResponsabilityEntity;
using Hotel.Domain.Repositories.Interfaces;

namespace Hotel.Domain.Repositories;

public class ResponsabilityRepository : GenericRepository<Responsability> ,IResponsabilityRepository
{
  public ResponsabilityRepository(HotelDbContext context) : base(context) {}
}