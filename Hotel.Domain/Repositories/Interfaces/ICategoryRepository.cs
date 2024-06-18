using Hotel.Domain.DTOs.CategoryDTOs;
using Hotel.Domain.Entities.CategoryEntity;

namespace Hotel.Domain.Repositories.Interfaces;

public interface ICategoryRepository : IRepository<Category>, IRepositoryQuery<GetCategory, CategoryQueryParameters>
{
    Task<Category?> GetCategoryIncludesRooms(Guid id);
}