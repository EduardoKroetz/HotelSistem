using Hotel.Domain.DTOs.AdminContext.PermissionDTOs;
using Hotel.Domain.Entities.AdminContext.AdminEntity;
using Hotel.Domain.Entities.AdminContext.PermissionEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Repositories.AdminContext;
using Hotel.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Tests.Repositories.AdminContext;

[TestClass]
public class PermissionRepositoryTest : BaseRepositoryTest
{
  private List<Permission> _permissions { get; set; } = [];
  private Permission _defaultPermission { get; set; } = null!;
  private PermissionRepository _permissionRepository { get; set; } = null!;

  [TestInitialize]
  public async Task StartupTest()
  {
    await Startup(null);
    _permissionRepository = new PermissionRepository(mockConnection.Context);

    _defaultPermission = new Permission("Criar nova hospedagem", "Criar nova hospedagem");

    _permissions.AddRange(
    [
      _defaultPermission,
      new Permission("Atualizar usuário","Atualizar usuário"),
      new Permission("Criar administrador","Criar administrador"),
      new Permission("Buscar reservas","Buscar reservas"),
      new Permission("Gerar fatura","Gerar fatura")
    ]);

    await mockConnection.Context.Permissions.AddRangeAsync(_permissions);
    await mockConnection.Context.SaveChangesAsync();
  }

  [TestCleanup]
  public async Task CleanupTest()
  {
    mockConnection.Context.Permissions.RemoveRange(_permissions);
    await mockConnection.Context.SaveChangesAsync();
    _permissions.Clear();
  }

  [TestMethod]
  public async Task GetByIdAsync_ReturnsWithCorrectParameters()
  {
    var permission = await _permissionRepository.GetByIdAsync(_defaultPermission.Id);

    Assert.IsNotNull(permission);
    Assert.AreEqual(_defaultPermission.Id, permission.Id);
    Assert.AreEqual(_defaultPermission.Name, permission.Name);
    Assert.AreEqual(_defaultPermission.Description, permission.Description);
    Assert.AreEqual(_defaultPermission.IsActive, permission.IsActive);
  }

  [TestMethod]
  public async Task GetAsync_ReturnWithCorrectParameters()
  {
    var parameters = new PermissionQueryParameters(0, 1, null , null,_defaultPermission.Name, null, null);
    var permissions = await _permissionRepository.GetAsync(parameters);

    var permission = permissions.ToList()[0];

    Assert.IsNotNull(permission);
    Assert.AreEqual(_defaultPermission.Id, permission.Id);
    Assert.AreEqual(_defaultPermission.Name, permission.Name);
    Assert.AreEqual(_defaultPermission.Description, permission.Description);
    Assert.AreEqual(_defaultPermission.IsActive, permission.IsActive);
  }

  [TestMethod]
  public async Task GetAsync_WhereNameIncludesBuscar_ReturnsPermissions()
  {
    var parameters = new PermissionQueryParameters(0, 1, null, null, "Buscar", null, null);
    var permissions = await _permissionRepository.GetAsync(parameters);

    Assert.IsTrue(permissions.Any());
    foreach (var permission in permissions)
    {
      Assert.IsNotNull(permission);
      Assert.IsTrue(permission.Name.Contains("Buscar"));
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereCreatedAtGratherThan2000_ReturnsPermissions()
  {
    var parameters = new PermissionQueryParameters(0, 1, DateTime.Now.AddYears(-24), "gt", null, null, null);
    var permissions = await _permissionRepository.GetAsync(parameters);

    Assert.IsTrue(permissions.Any());
    foreach (var permission in permissions)
    {
      Assert.IsNotNull(permission);
      Assert.IsTrue(DateTime.Now.AddYears(-24) < permission.CreatedAt);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereCreatedAtLessThan2025_ReturnsPermissions()
  {
    var parameters = new PermissionQueryParameters(0, 1, DateTime.Now.AddYears(1), "lt", null, null, null);
    var permissions = await _permissionRepository.GetAsync(parameters);

    Assert.IsTrue(permissions.Any());
    foreach (var permission in permissions)
    {
      Assert.IsNotNull(permission);
      Assert.IsTrue(DateTime.Now.AddYears(1) > permission.CreatedAt);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereCreatedAtEquals_ReturnsPermissions()
  {
    var parameters = new PermissionQueryParameters(0, 1, _defaultPermission.CreatedAt, "eq", null, null, null);
    var permissions = await _permissionRepository.GetAsync(parameters);

    Assert.IsTrue(permissions.Any());
    foreach (var permission in permissions)
    {
      Assert.IsNotNull(permission);
      Assert.AreEqual(_defaultPermission.CreatedAt, permission.CreatedAt);
    }
  }


  [TestMethod]
  public async Task GetAsync_WhereIsActiveEqualsFalse_ReturnsPermissions()
  {
    _permissions[1].Disable();

    await mockConnection.Context.SaveChangesAsync();

    var parameters = new PermissionQueryParameters(0, 1, null, null, null, false, null);
    var permissions = await _permissionRepository.GetAsync(parameters);

    Assert.IsTrue(permissions.Any());
    foreach (var permission in permissions)
    {
      Assert.IsNotNull(permission);
      Assert.IsFalse(permission.IsActive);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereAdminId_ReturnsPermissions()
  {
    var admin = new Admin(new Name("Rafael", "Silveira"), new Email("rafaelsilveira@example.com"), new Phone("+55 (19) 98765-4321"), "rafa789", EGender.Masculine, DateTime.Now.AddYears(-32), new Address("Brazil", "Campinas", "Rua Barão de Jaguara", 789));
    admin.AddPermission(_defaultPermission);

    await mockConnection.Context.Admins.AddAsync(admin);
    await mockConnection.Context.SaveChangesAsync();

    var parameters = new PermissionQueryParameters(0, 1, null, null, null, null, admin.Id);
    var permissions = await _permissionRepository.GetAsync(parameters);


    Assert.IsTrue(permissions.Any());
    foreach (var permission in permissions)
    {
      Assert.IsNotNull(permission);

      var hasPermission = await mockConnection.Context.Permissions
        .Where(x => x.Id == permission.Id)
        .SelectMany(x => x.Admins)
        .AnyAsync(x => x.Id == admin.Id);

      Assert.IsTrue(hasPermission);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereNameIncludesAdmin_And_IsActiveEqualsTrue()
  {
    var parameters = new PermissionQueryParameters(0, 1, null, null, "admin", true, null);
    var permissions = await _permissionRepository.GetAsync(parameters);

    Assert.IsTrue(permissions.Any());

    foreach (var permission in permissions)
    {
      Assert.IsNotNull(permission);
      Assert.IsTrue(permission.Name.Contains("admin"));
      Assert.IsTrue(permission.IsActive);
    }
  }

}
