using Hotel.Domain.Data;
using Hotel.Domain.DTOs.EmployeeContext.ResponsabilityDTOs;
using Hotel.Domain.DTOs.RoomContext.ServiceDTOs;
using Hotel.Domain.Entities.RoomContext.ServiceEntity;
using Hotel.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Repositories;

public class ServiceRepository :  GenericRepository<Service> ,IServiceRepository
{
  public ServiceRepository(HotelDbContext context) : base(context) {}

  public async Task<GetService?> GetByIdAsync(Guid id)
  {
    return await _context
      .Services
      .AsNoTracking()
      .Where(x => x.Id == id)
      .Include(x => x.Responsabilities)
      .Select(x => new GetService(
        x.Id,
        x.Name,
        x.Price,
        x.Priority,
        x.IsActive,
        x.TimeInMinutes,
        new List<GetReponsability>(
          x.Responsabilities.Select(
            r => new GetReponsability(r.Id, r.Name, r.Description,r.Priority)
        ))
      ))
      .FirstOrDefaultAsync();
  
  }
  public async Task<IEnumerable<GetServiceCollection>> GetAsync()
  {
    return await _context
      .Services
      .AsNoTracking()
      .Select(x => new GetServiceCollection(       
        x.Id,
        x.Name,
        x.Price,
        x.Priority,
        x.IsActive,
        x.TimeInMinutes
      ))
      .ToListAsync();
  }
}