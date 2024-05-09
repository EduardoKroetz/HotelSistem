using Hotel.Domain.Entities.AdminContext.AdminEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Repositories;
using Hotel.Domain.ValueObjects;


namespace Hotel.Tests.Repositories;

[TestClass]
public class AdminRepositoryTests : GenericRepositoryTest<Admin,AdminRepository>
{
  private static ConfigMockConnection _mockConnection;
  private static AdminRepository _adminRepository;
  private static readonly Admin _admin1 = new (new Name("João", "Pedro"), new Email("joaopedro@gmail.com"), new Phone("+55 (55) 33112-3456"), "1234", EGender.Masculine, DateTime.Now.AddYears(-20), new Address("Brazil", "São Paulo", "Avenida Paulista", 999));
  private static readonly Admin _admin2 = new (new Name("João", "Pedro"), new Email("joaopedro@gmail.com"), new Phone("+55 (55) 33112-3456"), "1234", EGender.Masculine, DateTime.Now.AddYears(-20), new Address("Brazil", "São Paulo", "Avenida Paulista", 999));
  private static readonly Admin _admin3 = new (new Name("Maria", "Silva"), new Email("maria.silva@example.com"), new Phone("+55 (11) 98765-4321"), "senha123", EGender.Feminine, DateTime.Now.AddYears(-25), new Address("Brazil", "Rio de Janeiro", "Rua Copacabana", 123));
  private static readonly Admin _admin4 = new (new Name("Carlos", "Oliveira"), new Email("coliveira@example.com"), new Phone("+55 (21) 12345-6789"), "_admin123", EGender.Masculine, DateTime.Now.AddYears(-30), new Address("Brazil", "Brasília", "Quadra 123", 3));
  private static readonly Admin _admin5 = new (new Name("Ana", "Santos"), new Email("anasantos@example.com"), new Phone("+55 (31) 98765-4321"), "ana456", EGender.Feminine, DateTime.Now.AddYears(-28), new Address("Brazil", "Belo Horizonte", "Avenida Afonso Pena", 456));
  private static readonly Admin _admin6 = new (new Name("Rafael", "Silveira"), new Email("rafaelsilveira@example.com"), new Phone("+55 (19) 98765-4321"), "rafa789", EGender.Masculine, DateTime.Now.AddYears(-32), new Address("Brazil", "Campinas", "Rua Barão de Jaguara", 789));
  private static readonly List<Admin> _admins = new() { _admin1,_admin2,_admin3, _admin4, _admin5};
  public AdminRepositoryTests() : base(_adminRepository,_admins,_admin6) {}

  [ClassInitialize]
  public static async Task Setup(TestContext context)
  {
    var mockConnection = new ConfigMockConnection();
    await mockConnection.Initialize();
    _mockConnection = mockConnection;

    _adminRepository = new AdminRepository(_mockConnection.Context);

    _admin1.ChangeId(new Guid("ab9f7b47-4349-47c9-8add-4c0360dbe051"));
    _admin2.ChangeId(new Guid("ab9f7b47-4349-47c9-8add-4c0360dbe052"));
    _admin3.ChangeId(new Guid("ab9f7b47-4349-47c9-8add-4c0360dbe053"));
    _admin4.ChangeId(new Guid("ab9f7b47-4349-47c9-8add-4c0360dbe054"));
    _admin5.ChangeId(new Guid("ab9f7b47-4349-47c9-8add-4c0360dbe055"));
    _admin6.ChangeId(new Guid("ab9f7b47-4349-47c9-8add-4c0360dbe056"));

    _mockConnection.Context.Admins.AddRange(_admins);

    _mockConnection.Context.SaveChanges();
  }

  [TestMethod]
  public async Task GetByIdAsync_ValidId_ReturnsAdmin()
  {
    var admin = await _adminRepository.GetByIdAsync(_admin1.Id);

    Assert.IsNotNull(admin);
    Assert.AreEqual("João", admin.FirstName);
  }

  [ClassCleanup]
  public static void Cleanup()
  => _mockConnection.Dispose();
}