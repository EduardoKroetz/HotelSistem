using Hotel.Domain.Data;
using Hotel.Domain.Entities.RoomContext.CategoryEntity;
using Hotel.Domain.Repositories.Interfaces;

namespace Hotel.Domain.Repositories;

public class CategoryRepository : GenericRepository<Category> ,ICategoryRepository
{
  public CategoryRepository(HotelDbContext context) : base(context) {}

}