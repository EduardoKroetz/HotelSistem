using Hotel.Domain.Entities.AdminContext.PermissionEntity;
using Hotel.Domain.Repositories;



namespace Hotel.Tests.Repositories.AdminContext;

[TestClass]
public class PermissionRepositoryTest : GenericRepositoryTest<Permission, PermissionRepository>
{
  private static ConfigMockConnection _mockConnection = null!;
  private static PermissionRepository _permissionRepository = null!;
  private static Permission _permissionToBeCreated = new("Buscar usuários","Buscar usuários");
  private static readonly List<Permission> _permissions = [];
  public PermissionRepositoryTest() : base(_permissionRepository, _permissions, _permissionToBeCreated) { }

  //Inicia antes do construtor
  [ClassInitialize]
  public static async Task Setup(TestContext context)
  {
    var mockConnection = new ConfigMockConnection();
    await mockConnection.Initialize();
    _mockConnection = mockConnection;

    _permissionRepository = new PermissionRepository(_mockConnection.Context);
  }

  [ClassCleanup]
  public static void Cleanup()
  => _mockConnection.Dispose();

  [TestInitialize]
  public async Task SetupTest()
  {
    _permissions.AddRange(
      [
        new Permission("Criar cliente","Criar cliente"),
        new Permission("Criar admin","Criar admin"),
        new Permission("Buscar reservas","buscar reservas"),
        new Permission("Atualizar funcionário","Atualizar funcionário"),
        new Permission("Deletar funcionário","Deletar funcionário"),
      ]
    );
    ;

    _mockConnection.Context.Permissions.AddRange(_permissions);
    await _mockConnection.Context.SaveChangesAsync();
  }

  [TestCleanup]
  public void CleanupTest()
  => _permissions.Clear();



  //Testes

  [TestMethod]
  public async Task GetByIdAsync_ValidId_ReturnsPermission()
  {
    var permission = await _permissionRepository.GetByIdAsync(_permissions[0].Id);

    Assert.IsNotNull(permission);
    Assert.AreEqual("Criar cliente", permission.Name);
  }

  //Testes
  [TestMethod]
  public async Task GetAsync_ReturnsPermissions()
  {
    var permissions = await _permissionRepository.GetAsync();

    Assert.IsNotNull(permissions);
    Assert.IsTrue(permissions.Any());
  }
}