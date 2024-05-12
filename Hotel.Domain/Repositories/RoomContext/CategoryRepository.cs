using Hotel.Domain.Data;
using Hotel.Domain.DTOs.PaymentContext.RoomInvoiceDTOs;
using Hotel.Domain.DTOs.RoomContext.CategoryDTOs;
using Hotel.Domain.Entities.RoomContext.CategoryEntity;
using Hotel.Domain.Extensions;
using Hotel.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Repositories;

public class CategoryRepository : GenericRepository<Category> ,ICategoryRepository
{
  public CategoryRepository(HotelDbContext context) : base(context) {}

  public async Task<GetCategory?> GetByIdAsync(Guid id)
  {
    return await _context
      .Categories
      .AsNoTracking()
      .Where(x => x.Id == id)
      .Select(x => new GetCategory(x.Id,x.Name,x.Description,x.AveragePrice))
      .FirstOrDefaultAsync();
    
  }

  public async Task<IEnumerable<GetCategory>> GetAsync(CategoryQueryParameters queryParameters)
  {
    var query = _context.Categories.AsQueryable();

    if (queryParameters.Name != null)
      query = query.Where(x => x.Name.Contains(queryParameters.Name));

    if (queryParameters.AveragePrice.HasValue)
      query = query.FilterByOperator(queryParameters.AveragePriceOperator, x => x.AveragePrice, queryParameters.AveragePrice);

    if (queryParameters.RoomId.HasValue)
      query = query.Where(x => x.Rooms.Any(x => x.Id == queryParameters.RoomId));

    query = query.BaseQuery(queryParameters);

    return await query.Select(x => new GetCategory(
        x.Id,
        x.Name,
        x.Description,
        x.AveragePrice
    )).ToListAsync();
  }
}