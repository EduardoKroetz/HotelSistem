using Hotel.Domain.Data;
using Hotel.Domain.DTOs.PermissionDTOs;
using Hotel.Domain.Entities.AdminEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Repositories;
using Hotel.Domain.ValueObjects;
using Hotel.Tests.UnitTests.Repositories.InMemoryDatabase.Utils;
using Hotel.Tests.UnitTests.Repositories.InMemoryDatabase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Hotel.Domain.Entities.PermissionEntity;

namespace Hotel.Tests.UnitTests.Repositories;

[TestClass]
public class PermissionRepositoryTest
{
    private readonly PermissionRepository _permissionRepository;
    private readonly HotelDbContext _dbContext;
    private readonly RepositoryTestUtils _utils;
    private static readonly AsyncLocal<IDbContextTransaction?> _currentTransaction = new AsyncLocal<IDbContextTransaction?>();

    public PermissionRepositoryTest()
    {
        var sqliteDatabase = new SqliteDatabase();
        _dbContext = sqliteDatabase.Context;
        _permissionRepository = new PermissionRepository(_dbContext);
        _utils = new RepositoryTestUtils(_dbContext);
    }

    [TestInitialize]
    public async Task Initialize()
    {
        _currentTransaction.Value = await _dbContext.Database.BeginTransactionAsync();
    }

    [TestCleanup]
    public async Task Cleanup()
    {
        if (_currentTransaction.Value != null)
        {
            await _currentTransaction.Value.RollbackAsync();
            await _currentTransaction.Value.DisposeAsync();
            _currentTransaction.Value = null;
        }
    }

    private async Task<Permission> CreatePermissionAsync()
    {
        return await _utils.CreatePermissionAsync(new Permission("Deletar usuário", "Deletar usuário"));
    }

    [TestMethod]
    public async Task GetByIdAsync_ReturnsWithCorrectParameters()
    {
        //Arrange
        var newPermission = await CreatePermissionAsync();

        //Act
        var permission = await _permissionRepository.GetByIdAsync(newPermission.Id);

        //Assert
        Assert.IsNotNull(permission);
        Assert.AreEqual(newPermission.Id, permission.Id);
        Assert.AreEqual(newPermission.Name, permission.Name);
        Assert.AreEqual(newPermission.Description, permission.Description);
        Assert.AreEqual(newPermission.IsActive, permission.IsActive);
    }

    [TestMethod]
    public async Task GetAsync_ReturnWithCorrectParameters()
    {
        // Arrange
        var newPermission = await CreatePermissionAsync();
        var parameters = new PermissionQueryParameters { Name = newPermission.Name };

        // Act
        var permissions = await _permissionRepository.GetAsync(parameters);
        var permission = permissions.ToList().FirstOrDefault();

        // Assert
        Assert.IsNotNull(permission);
        Assert.AreEqual(newPermission.Id, permission.Id);
        Assert.AreEqual(newPermission.Name, permission.Name);
        Assert.AreEqual(newPermission.Description, permission.Description);
        Assert.AreEqual(newPermission.IsActive, permission.IsActive);
    }

    [TestMethod]
    public async Task GetAsync_WhereNameIncludesDeletar_ReturnsPermissions()
    {
        // Arrange
        var newPermission = await CreatePermissionAsync();
        var parameters = new PermissionQueryParameters { Name = "Deletar" };

        // Act
        var permissions = await _permissionRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(permissions.Any());
        foreach (var permission in permissions)
        {
            Assert.IsTrue(permission.Name.Contains("Deletar"));
        }
    }

    [TestMethod]
    public async Task GetAsync_WhereCreatedAtGratherThan2000_ReturnsPermissions()
    {
        // Arrange
        var newPermission = await CreatePermissionAsync();
        var parameters = new PermissionQueryParameters { CreatedAt = DateTime.Now.AddYears(-24), CreatedAtOperator = "gt" };

        // Act
        var permissions = await _permissionRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(permissions.Any());
        foreach (var permission in permissions)
        {
            Assert.IsTrue(DateTime.Now.AddYears(-24) < permission.CreatedAt);
        }
    }

    [TestMethod]
    public async Task GetAsync_WhereCreatedAtLessThan2025_ReturnsPermissions()
    {
        // Arrange
        var newPermission = await CreatePermissionAsync();
        var parameters = new PermissionQueryParameters { CreatedAt = DateTime.Now.AddYears(1), CreatedAtOperator = "lt" };

        // Act
        var permissions = await _permissionRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(permissions.Any());
        foreach (var permission in permissions)
        {
            Assert.IsTrue(DateTime.Now.AddYears(1) > permission.CreatedAt);
        }
    }

    [TestMethod]
    public async Task GetAsync_WhereCreatedAtEquals_ReturnsPermissions()
    {
        // Arrange
        var newPermission = await CreatePermissionAsync();
        var parameters = new PermissionQueryParameters { CreatedAt = newPermission.CreatedAt, CreatedAtOperator = "eq" };

        // Act
        var permissions = await _permissionRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(permissions.Any());
        foreach (var permission in permissions)
        {
            Assert.AreEqual(newPermission.CreatedAt, permission.CreatedAt);
        }
    }

    [TestMethod]
    public async Task GetAsync_WhereAdminId_ReturnsPermissions()
    {
        // Arrange
        var newPermission = await CreatePermissionAsync();
        var admin = new Admin(new Name("Rafael", "Silveira"), new Email("rafaelsilv@example.com"), new Phone("+55 (17) 93465-4321"), "rafa789", EGender.Masculine, DateTime.Now.AddYears(-32), new Address("Brazil", "Campinas", "Rua Barão de Jaguara", 789));
        admin.AddPermission(newPermission);
        await _dbContext.Admins.AddAsync(admin);
        await _dbContext.SaveChangesAsync();

        var parameters = new PermissionQueryParameters { AdminId = admin.Id };

        // Act
        var permissions = await _permissionRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(permissions.Any());
        foreach (var permission in permissions)
        {
            var hasPermission = await _dbContext.Permissions
                .Where(x => x.Id == permission.Id)
                .SelectMany(x => x.Admins)
                .AnyAsync(x => x.Id == admin.Id);

            Assert.IsTrue(hasPermission);
        }
    }

    [TestMethod]
    public async Task GetAsync_WhereNameIncludes_And_IsActiveEqualsTrue()
    {
        // Arrange
        var newPermission = await CreatePermissionAsync();
        var parameters = new PermissionQueryParameters { Name = newPermission.Name, IsActive = true };

        // Act
        var permissions = await _permissionRepository.GetAsync(parameters);

        // Assert
        Assert.IsTrue(permissions.Any());
        foreach (var permission in permissions)
        {
            Assert.IsTrue(permission.Name.Contains(newPermission.Name));
            Assert.IsTrue(permission.IsActive);
        }
    }
}
