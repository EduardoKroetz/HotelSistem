
using Hotel.Domain.Entities.AdminContext.AdminEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Repositories.AdminContext;
using Hotel.Domain.ValueObjects;
using Hotel.Tests.Repositories.Mock;

namespace Hotel.Tests.Repositories;


//Testar o repositório genérico criando
//somente um CRUD de Admin
[TestClass]
public class GenericRepositoryTest
{
  private static ConfigMockConnection _mockConnection = null!;
  private static AdminRepository _adminRepository = null!;
  private static readonly List<Admin> _admins = 
  [
    new Admin(new Name("João", "Pedro"), new Email("joaopedro@gmail.com"), new Phone("+55 (55) 33112-3456"), "1234", EGender.Masculine, DateTime.Now.AddYears(-20), new Address("Brazil", "São Paulo", "Avenida Paulista", 999)),
    new Admin(new Name("João", "Pedro"), new Email("joaopedro@gmail.com"), new Phone("+55 (55) 33112-3456"), "1234", EGender.Masculine, DateTime.Now.AddYears(-20), new Address("Brazil", "São Paulo", "Avenida Paulista", 999)),
    new Admin(new Name("Maria", "Silva"), new Email("maria.silva@example.com"), new Phone("+55 (11) 98765-4321"), "senha123", EGender.Feminine, DateTime.Now.AddYears(-25), new Address("Brazil", "Rio de Janeiro", "Rua Copacabana", 123)),
    new Admin(new Name("Carlos", "Oliveira"), new Email("coliveira@example.com"), new Phone("+55 (21) 12345-6789"), "_admin123", EGender.Masculine, DateTime.Now.AddYears(-30), new Address("Brazil", "Brasília", "Quadra 123", 3)),
    new Admin(new Name("Ana", "Santos"), new Email("anasantos@example.com"), new Phone("+55 (31) 98765-4321"), "ana456", EGender.Feminine, DateTime.Now.AddYears(-28), new Address("Brazil", "Belo Horizonte", "Avenida Afonso Pena", 456)),
    new Admin(new Name("Rafael", "Silveira"), new Email("rafaelsilveira@example.com"), new Phone("+55 (19) 98765-4321"), "rafa789", EGender.Masculine, DateTime.Now.AddYears(-32), new Address("Brazil", "Campinas", "Rua Barão de Jaguara", 789))
  ];

  public async static Task<ConfigMockConnection> InitializeMockConnection()
  {
    var mockConnection = new ConfigMockConnection();
    await mockConnection.Initialize();
    return mockConnection;
  }

  [ClassInitialize]
  public static async Task Setup(TestContext context)
  {
    _mockConnection = await InitializeMockConnection();

    _mockConnection.Context.Admins.AddRange(_admins);
    await _mockConnection.Context.SaveChangesAsync();

    _adminRepository = new AdminRepository(_mockConnection.Context);
  }

  [ClassCleanup]
  public static void Cleanup()
  => _mockConnection.Dispose();


  [TestMethod]
  public async Task CreateAsync_MustCreate()
  {
    var admin = new Admin(new Name("Vinicius", "Santos"), new Email("viniciuos@gmail.com"), new Phone("+55 (55) 33112-3456"), "1234", EGender.Masculine, DateTime.Now.AddYears(-20), new Address("Brazil", "São Paulo", "Avenida Paulista", 999));
    await _adminRepository.CreateAsync(admin);
    await _adminRepository.SaveChangesAsync();

    var createdEntity = await _adminRepository.GetEntityByIdAsync(admin.Id);

    Assert.IsNotNull(createdEntity);
    Assert.AreEqual(admin.Id, createdEntity?.Id);
  }

  [TestMethod]
  public async Task Delete_MustDelete()
  {
    var adminToBeDeleted = _admins[1];
    _adminRepository.Delete(adminToBeDeleted);
    await _adminRepository.SaveChangesAsync();

    var deletedEntity = await _adminRepository.GetEntityByIdAsync(adminToBeDeleted.Id);
    Assert.IsNull(deletedEntity);
  }

  [TestMethod]
  public async Task DeleteById_MustDelete()
  {
    var adminToBeDeleted = _admins[2];
    _adminRepository.Delete(adminToBeDeleted.Id);
    await _adminRepository.SaveChangesAsync();

    var deletedEntity = await _adminRepository.GetEntityByIdAsync(adminToBeDeleted.Id);
    Assert.IsNull(deletedEntity);
  }

  [TestMethod]
  [ExpectedException(typeof(ArgumentException))]
  public async Task DeleteNonExistEntityById_ExpectedException()
  {
    _adminRepository.Delete(Guid.NewGuid());
    await _adminRepository.SaveChangesAsync();
  }

  [TestMethod]
  public async Task GetEntityByIdAsync_ReturnsEntity()
  {
    var admin = await _adminRepository.GetEntityByIdAsync(_admins[3].Id);
    Assert.IsNotNull(admin);
    Assert.AreEqual(_admins[3].Id, admin.Id);
  }

  [TestMethod]
  public async Task GetEntitiesAsync_ReturnsEntities()
  {
    var admins = await _adminRepository.GetEntitiesAsync();

    Assert.IsNotNull(admins);
    Assert.IsTrue(admins.Any());
  }

  [TestMethod]
  public async Task UpdateEntity_MustApplyEntityChanges()
  {
    var admin = await _adminRepository.GetEntityByIdAsync(_admins[4].Id);
    var date = DateTime.Now.Date.AddDays(1);
    admin?.ChangeCreatedAt(date);

    await _adminRepository.SaveChangesAsync();

    var updatedAdmin = await _adminRepository.GetEntityByIdAsync(_admins[4].Id);

    Assert.IsNotNull(updatedAdmin);
    Assert.AreEqual(date,updatedAdmin.CreatedAt);
  }

}

