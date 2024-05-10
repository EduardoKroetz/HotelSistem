using Hotel.Domain.Entities.AdminContext.AdminEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Repositories;
using Hotel.Domain.ValueObjects;


namespace Hotel.Tests.Repositories;

[TestClass]
public class AdminRepositoryTests : GenericRepositoryTest<Admin,AdminRepository>
{
  private static ConfigMockConnection _mockConnection = null!;
  private static AdminRepository _adminRepository = null!;
  private static Admin _adminToBeCreated = new(new Name("Vinicius", "Pedro"), new Email("joaopedro@gmail.com"), new Phone("+55 (55) 33112-3456"), "1254", EGender.Masculine, DateTime.Now.AddYears(-20), new Address("Brazil", "São Paulo", "Avenida Paulista", 999));
  private static readonly List<Admin> _admins = new() {};
  public AdminRepositoryTests() : base(_adminRepository,_admins,_adminToBeCreated) {}

  //Inicia antes do construtor
  [ClassInitialize]
  public static async Task Setup(TestContext context)
  {
    var mockConnection = new ConfigMockConnection();
    await mockConnection.Initialize();
    _mockConnection = mockConnection;

    _adminRepository = new AdminRepository(_mockConnection.Context);
  }

  [ClassCleanup]
  public static void Cleanup()
  => _mockConnection.Dispose();

  [TestInitialize]
  public async Task SetupTest()
  {
    _admins.AddRange(
      [
        new Admin(new Name("João", "Pedro"), new Email("joaopedro@gmail.com"), new Phone("+55 (55) 33112-3456"), "1234", EGender.Masculine, DateTime.Now.AddYears(-20), new Address("Brazil", "São Paulo", "Avenida Paulista", 999)),
        new Admin(new Name("João", "Pedro"), new Email("joaopedro@gmail.com"), new Phone("+55 (55) 33112-3456"), "1234", EGender.Masculine, DateTime.Now.AddYears(-20), new Address("Brazil", "São Paulo", "Avenida Paulista", 999)),
        new Admin(new Name("Maria", "Silva"), new Email("maria.silva@example.com"), new Phone("+55 (11) 98765-4321"), "senha123", EGender.Feminine, DateTime.Now.AddYears(-25), new Address("Brazil", "Rio de Janeiro", "Rua Copacabana", 123)),
        new Admin(new Name("Carlos", "Oliveira"), new Email("coliveira@example.com"), new Phone("+55 (21) 12345-6789"), "_admin123", EGender.Masculine, DateTime.Now.AddYears(-30), new Address("Brazil", "Brasília", "Quadra 123", 3)),
        new Admin(new Name("Ana", "Santos"), new Email("anasantos@example.com"), new Phone("+55 (31) 98765-4321"), "ana456", EGender.Feminine, DateTime.Now.AddYears(-28), new Address("Brazil", "Belo Horizonte", "Avenida Afonso Pena", 456)),
        new Admin(new Name("Rafael", "Silveira"), new Email("rafaelsilveira@example.com"), new Phone("+55 (19) 98765-4321"), "rafa789", EGender.Masculine, DateTime.Now.AddYears(-32), new Address("Brazil", "Campinas", "Rua Barão de Jaguara", 789))
      ]
    ); ;
    
    _mockConnection.Context.Admins.AddRange(_admins);
    await _mockConnection.Context.SaveChangesAsync();
  }

  [TestCleanup]
  public void CleanupTest()
=> _admins.Clear();


 
  //Testes
  
  [TestMethod]
  public async Task GetByIdAsync_ValidId_ReturnsAdmin()
  {
    var admin = await _adminRepository.GetByIdAsync(_admins[0].Id);

    Assert.IsNotNull(admin);
    Assert.AreEqual("João", admin.FirstName);
  }

  //Testes
  [TestMethod]
  public async Task GetEntitiesAsync_ReturnAdmins()
  {
    var admins = await _adminRepository.GetAsync();

    Assert.IsNotNull(admins);
    Assert.IsTrue(admins.Any());
  }
}