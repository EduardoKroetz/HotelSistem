using Hotel.Domain.Data;
using Hotel.Domain.Entities.AdminEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Repositories;
using Hotel.Domain.ValueObjects;
using Hotel.Tests.UnitTests.Repositories.InMemoryDatabase.Utils;
using Hotel.Tests.UnitTests.Repositories.InMemoryDatabase;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Tests.UnitTests.Repositories;

// Testar o repositório genérico criando
// somente um CRUD de Admin
[TestClass]
public class GenericRepositoryTest
{
    private readonly AdminRepository _adminRepository;
    private readonly HotelDbContext _dbContext;
    private readonly RepositoryTestUtils _utils;
    private static readonly AsyncLocal<IDbContextTransaction?> _currentTransaction = new AsyncLocal<IDbContextTransaction?>();

    public GenericRepositoryTest()
    {
        var sqliteDatabase = new SqliteDatabase();
        _dbContext = sqliteDatabase.Context;
        _adminRepository = new AdminRepository(_dbContext);
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

    [TestMethod]
    public async Task CreateAsync_MustCreate()
    {
        // Arrange
        var admin = new Admin(new Name("Vinicius", "Santos"), new Email("viniciuos@gmail.com"), new Phone("+55 (55) 33112-3456"), "1234", EGender.Masculine, DateTime.Now.AddYears(-20), new Address("Brazil", "São Paulo", "Avenida Paulista", 999));
        await _adminRepository.CreateAsync(admin);
        await _adminRepository.SaveChangesAsync();

        // Act
        var createdEntity = await _adminRepository.GetEntityByIdAsync(admin.Id);

        // Assert
        Assert.IsNotNull(createdEntity);
        Assert.AreEqual(admin.Id, createdEntity?.Id);
    }

    [TestMethod]
    public async Task Delete_MustDelete()
    {
        // Arrange
        var admin = await _utils.CreateAdminAsync(new Admin(new Name("Vinicius", "Santos"), new Email("viniciuos@gmail.com"), new Phone("+55 (55) 33112-3456"), "1234", EGender.Masculine, DateTime.Now.AddYears(-20), new Address("Brazil", "São Paulo", "Avenida Paulista", 999)));

        // Act
        _adminRepository.Delete(admin);
        await _adminRepository.SaveChangesAsync();

        // Assert
        var deletedEntity = await _dbContext.Admins.FirstOrDefaultAsync(x => x.Id == admin.Id);
        Assert.IsNull(deletedEntity);
    }

    [TestMethod]
    public async Task DeleteById_MustDelete()
    {
        // Arrange
        var admin = await _utils.CreateAdminAsync(new Admin(new Name("João", "Silva"), new Email("joao.silva@gmail.com"), new Phone("+55 (11) 99876-5432"), "1234", EGender.Masculine, DateTime.Now.AddYears(-25), new Address("Brazil", "Rio de Janeiro", "Rua das Flores", 123)));

        // Act
        _adminRepository.Delete(admin.Id);
        await _adminRepository.SaveChangesAsync();

        // Assert
        var deletedEntity = await _adminRepository.GetEntityByIdAsync(admin.Id);
        Assert.IsNull(deletedEntity);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public async Task DeleteNonExistEntityById_ExpectedException()
    {
        // Act
        _adminRepository.Delete(Guid.NewGuid());
        await _adminRepository.SaveChangesAsync();
    }

    [TestMethod]
    public async Task GetEntityByIdAsync_ReturnsEntity()
    {
        // Arrange
        var admin = await _utils.CreateAdminAsync(new Admin(new Name("Ana", "Souza"), new Email("ana.souza@gmail.com"), new Phone("+55 (21) 98765-4321"), "1234", EGender.Feminine, DateTime.Now.AddYears(-30), new Address("Brazil", "Belo Horizonte", "Rua das Palmeiras", 456)));

        // Act
        var retrievedAdmin = await _adminRepository.GetEntityByIdAsync(admin.Id);

        // Assert
        Assert.IsNotNull(retrievedAdmin);
        Assert.AreEqual(admin.Id, retrievedAdmin.Id);
    }

    [TestMethod]
    public async Task GetEntitiesAsync_ReturnsEntities()
    {
        // Arrange
        var admin1 = await _utils.CreateAdminAsync(new Admin(new Name("Carlos", "Almeida"), new Email("carlos.almeida@gmail.com"), new Phone("+55 (31) 12345-6789"), "1234", EGender.Masculine, DateTime.Now.AddYears(-35), new Address("Brazil", "Curitiba", "Avenida Brasil", 789)));
        var admin2 = await _utils.CreateAdminAsync(new Admin(new Name("Maria", "Oliveira"), new Email("maria.oliveira@gmail.com"), new Phone("+55 (41) 98765-4321"), "1234", EGender.Feminine, DateTime.Now.AddYears(-28), new Address("Brazil", "Porto Alegre", "Rua das Acácias", 101)));

        // Act
        var admins = await _adminRepository.GetEntitiesAsync();

        // Assert
        Assert.IsNotNull(admins);
        Assert.IsTrue(admins.Any());
    }

    [TestMethod]
    public async Task UpdateEntity_MustApplyEntityChanges()
    {
        // Arrange
        var admin = await _utils.CreateAdminAsync(new Admin(new Name("Lucas", "Fernandes"), new Email("lucas.fernandes@gmail.com"), new Phone("+55 (21) 87654-3210"), "1234", EGender.Masculine, DateTime.Now.AddYears(-40), new Address("Brazil", "Fortaleza", "Rua da Paz", 123)));
        var newName = new Name("Lucas", "Moura");
        admin.ChangeName(newName);

        // Act
        await _adminRepository.SaveChangesAsync();

        // Assert
        var updatedAdmin = await _adminRepository.GetEntityByIdAsync(admin.Id);
        Assert.IsNotNull(updatedAdmin);
        Assert.AreEqual(newName.FirstName, updatedAdmin.Name.FirstName);
        Assert.AreEqual(newName.LastName, updatedAdmin.Name.LastName);
    }
}
