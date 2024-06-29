using Hotel.Domain.Data;
using Hotel.Domain.Entities.AdminEntity;
using Hotel.Domain.Entities.PermissionEntity;

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

}
