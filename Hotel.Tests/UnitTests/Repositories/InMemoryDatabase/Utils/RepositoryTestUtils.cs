using Hotel.Domain.Data;
using Hotel.Domain.Entities.AdminEntity;
using Hotel.Domain.Entities.CategoryEntity;
using Hotel.Domain.Entities.PermissionEntity;
using Hotel.Domain.Entities.RoomEntity;

namespace Hotel.Tests.UnitTests.Repositories.InMemoryDatabase.Utils;

public class RepositoryTestUtils
{
    private readonly HotelDbContext _dbContext;

    public RepositoryTestUtils(HotelDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Admin> CreateAdminAsync(Admin newAdmin)
    {
        await _dbContext.Admins.AddAsync(newAdmin);
        await _dbContext.SaveChangesAsync();
        return newAdmin;
    }

    public async Task<Permission> CreatePermissionAsync(Permission newPermission)
    {
        await _dbContext.Permissions.AddAsync(newPermission);
        await _dbContext.SaveChangesAsync();
        return newPermission;
    }

    public async Task<Category> CreateCategoryAsync(Category newCategory)
    {
        await _dbContext.Categories.AddAsync(newCategory);
        await _dbContext.SaveChangesAsync();
        return newCategory;
    }

    public async Task<Room> CreateRoomAsync(Room newRoom)
    {
        await _dbContext.Rooms.AddAsync(newRoom);
        await _dbContext.SaveChangesAsync();
        return newRoom;
    }

}
