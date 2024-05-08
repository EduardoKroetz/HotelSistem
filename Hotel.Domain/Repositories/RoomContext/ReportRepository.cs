using Hotel.Domain.Data;
using Hotel.Domain.DTOs.RoomContext.ReportDTOs;
using Hotel.Domain.Entities.RoomContext.ReportEntity;
using Hotel.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Repositories;

public class ReportRepository :  GenericRepository<Report> ,IReportRepository
{
  public ReportRepository(HotelDbContext context) : base(context) {}
 
  public async Task<GetReport?> GetByIdAsync(Guid id)
  {
    return await _context
      .Reports
      .AsNoTracking()
      .Where(x => x.Id == id)
      .Select(x => new GetReport(x.Id,x.Summary, x.Description, x.Priority,x.Resolution,x.EmployeeId))
      .FirstOrDefaultAsync();
    
  }
  public async Task<IEnumerable<GetReport>> GetAsync()
  {
    return await _context
      .Reports
      .AsNoTracking()
      .Select(x => new GetReport(x.Id,x.Summary, x.Description, x.Priority,x.Resolution,x.EmployeeId))
      .ToListAsync();
  }
}