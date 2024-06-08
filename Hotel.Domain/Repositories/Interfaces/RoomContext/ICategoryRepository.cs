using Hotel.Domain.DTOs.RoomContext.CategoryDTOs;
using Hotel.Domain.Entities.RoomContext.CategoryEntity;

namespace Hotel.Domain.Repositories.Interfaces.RoomContext;

public interface ICategoryRepository : IRepository<Category>, IRepositoryQuery<GetCategory, CategoryQueryParameters>
{
  Task<Category?> GetCategoryIncludesRooms(Guid id);
}