using Hotel.Domain.Data;
using Hotel.Domain.DTOs.PaymentContext.RoomInvoiceDTOs;
using Hotel.Domain.Entities.RoomContext.CategoryEntity;
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
  public async Task<IEnumerable<GetCategory>> GetAsync()
  {
    return await _context
      .Categories
      .AsNoTracking()
      .Select(x => new GetCategory(x.Id,x.Name,x.Description,x.AveragePrice))
      .ToListAsync();
  }
}