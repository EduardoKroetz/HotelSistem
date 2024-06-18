using Hotel.Domain.Data;
using Hotel.Domain.DTOs.ReportDTOs;
using Hotel.Domain.Entities.ReportEntity;
using Hotel.Domain.Extensions;
using Hotel.Domain.Repositories.Base;
using Hotel.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Repositories;

public class ReportRepository : GenericRepository<Report>, IReportRepository
{
    public ReportRepository(HotelDbContext context) : base(context) { }

    public async Task<GetReport?> GetByIdAsync(Guid id)
    {
        return await _context
          .Reports
          .AsNoTracking()
          .Where(x => x.Id == id)
          .Select(x => new GetReport(x.Id, x.Summary, x.Description, x.Priority, x.Resolution, x.EmployeeId, x.Status, x.CreatedAt))
          .FirstOrDefaultAsync();

    }

    public async Task<IEnumerable<GetReport>> GetAsync(ReportQueryParameters queryParameters)
    {
        var query = _context.Reports.AsQueryable();

        if (queryParameters.Summary != null)
            query = query.Where(x => x.Summary.Contains(queryParameters.Summary));

        if (queryParameters.Status != null)
            query = query.Where(x => x.Status == queryParameters.Status);

        if (queryParameters.Priority != null)
            query = query.Where(x => x.Priority == queryParameters.Priority);

        if (queryParameters.EmployeeId.HasValue)
            query = query.Where(x => x.EmployeeId == queryParameters.EmployeeId);

        query = query.BaseQuery(queryParameters);

        return await query.Select(x => new GetReport(
            x.Id,
            x.Summary,
            x.Description,
            x.Priority,
            x.Resolution,
            x.EmployeeId,
            x.Status,
            x.CreatedAt
        )).ToListAsync();
    }
}