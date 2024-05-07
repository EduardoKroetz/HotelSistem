using Hotel.Domain.Data;
using Hotel.Domain.DTOs.EmployeeContext.ResponsabilityDTOs;
using Hotel.Domain.Entities.EmployeeContext.ResponsabilityEntity;
using Hotel.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Repositories;

public class ResponsabilityRepository : GenericRepository<Responsability> ,IResponsabilityRepository
{
  public ResponsabilityRepository(HotelDbContext context) : base(context) {}

  public async Task<GetReponsability?> GetByIdAsync(Guid id)
  {
    return await _context
      .Responsabilities
      .AsNoTracking()
      .Where(x => x.Id == id)
      .Select(x => new GetReponsability(x.Id,x.Name,x.Description,x.Priority))
      .FirstOrDefaultAsync();
    
  }
  public async Task<IEnumerable<GetReponsability>> GetAsync()
  {
    return await _context
      .Responsabilities
      .AsNoTracking()
      .Select(x => new GetReponsability(x.Id,x.Name,x.Description,x.Priority))
      .ToListAsync();
  }
}