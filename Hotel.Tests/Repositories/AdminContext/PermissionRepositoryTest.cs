using Hotel.Domain.DTOs.AdminContext.PermissionDTOs;
using Hotel.Domain.Entities.AdminContext.AdminEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Repositories.AdminContext;
using Hotel.Domain.ValueObjects;
using Hotel.Tests.Repositories.Mock;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Tests.Repositories.AdminContext;

[TestClass]
public class PermissionRepositoryTest
{
  private static PermissionRepository PermissionRepository { get; set; }

  static PermissionRepositoryTest()
  => PermissionRepository = new PermissionRepository(BaseRepositoryTest.MockConnection.Context);

  [TestMethod]
  public async Task GetByIdAsync_ReturnsWithCorrectParameters()
  {
    var permission = await PermissionRepository.GetByIdAsync(BaseRepositoryTest.Permissions[0].Id);

    Assert.IsNotNull(permission);
    Assert.AreEqual(BaseRepositoryTest.Permissions[0].Id, permission.Id);
    Assert.AreEqual(BaseRepositoryTest.Permissions[0].Name, permission.Name);
    Assert.AreEqual(BaseRepositoryTest.Permissions[0].Description, permission.Description);
    Assert.AreEqual(BaseRepositoryTest.Permissions[0].IsActive, permission.IsActive);
  }

  [TestMethod]
  public async Task GetAsync_ReturnWithCorrectParameters()
  {
    var parameters = new PermissionQueryParameters(0, 1, null , null,BaseRepositoryTest.Permissions[0].Name, null, null);
    var permissions = await PermissionRepository.GetAsync(parameters);

    var permission = permissions.ToList()[0];

    Assert.IsNotNull(permission);
    Assert.AreEqual(BaseRepositoryTest.Permissions[0].Id, permission.Id);
    Assert.AreEqual(BaseRepositoryTest.Permissions[0].Name, permission.Name);
    Assert.AreEqual(BaseRepositoryTest.Permissions[0].Description, permission.Description);
    Assert.AreEqual(BaseRepositoryTest.Permissions[0].IsActive, permission.IsActive);
  }

  [TestMethod]
  public async Task GetAsync_WhereNameIncludesBuscar_ReturnsPermissions()
  {
    var parameters = new PermissionQueryParameters(0, 1, null, null, "Buscar", null, null);
    var permissions = await PermissionRepository.GetAsync(parameters);

    Assert.IsTrue(permissions.Any());
    foreach (var permission in permissions)
      Assert.IsTrue(permission.Name.Contains("Buscar"));
  }

  [TestMethod]
  public async Task GetAsync_WhereCreatedAtGratherThan2000_ReturnsPermissions()
  {
    var parameters = new PermissionQueryParameters(0, 1, DateTime.Now.AddYears(-24), "gt", null, null, null);
    var permissions = await PermissionRepository.GetAsync(parameters);

    Assert.IsTrue(permissions.Any());
    foreach (var permission in permissions)
      Assert.IsTrue(DateTime.Now.AddYears(-24) < permission.CreatedAt);
  }

  [TestMethod]
  public async Task GetAsync_WhereCreatedAtLessThan2025_ReturnsPermissions()
  {
    var parameters = new PermissionQueryParameters(0, 1, DateTime.Now.AddYears(1), "lt", null, null, null);
    var permissions = await PermissionRepository.GetAsync(parameters);

    Assert.IsTrue(permissions.Any());
    foreach (var permission in permissions)
      Assert.IsTrue(DateTime.Now.AddYears(1) > permission.CreatedAt);
  }

  [TestMethod]
  public async Task GetAsync_WhereCreatedAtEquals_ReturnsPermissions()
  {
    var parameters = new PermissionQueryParameters(0, 1, BaseRepositoryTest.Permissions[0].CreatedAt, "eq", null, null, null);
    var permissions = await PermissionRepository.GetAsync(parameters);

    Assert.IsTrue(permissions.Any());
    foreach (var permission in permissions)
      Assert.AreEqual(BaseRepositoryTest.Permissions[0].CreatedAt, permission.CreatedAt);
  }

  [TestMethod]
  public async Task GetAsync_WhereAdminId_ReturnsPermissions()
  {
    var admin = new Admin(new Name("Rafael", "Silveira"), new Email("rafaelsilv@example.com"), new Phone("+55 (17) 93465-4321"), "rafa789", EGender.Masculine, DateTime.Now.AddYears(-32), new Address("Brazil", "Campinas", "Rua Barão de Jaguara", 789));
    admin.AddPermission(BaseRepositoryTest.Permissions[0]);

    await BaseRepositoryTest.MockConnection.Context.Admins.AddAsync(admin);
    await BaseRepositoryTest.MockConnection.Context.SaveChangesAsync();

    var parameters = new PermissionQueryParameters(0, 1, null, null, null, null, admin.Id);
    var permissions = await PermissionRepository.GetAsync(parameters);


    Assert.IsTrue(permissions.Any());
    foreach (var permission in permissions)
    { 
      var hasPermission = await BaseRepositoryTest.MockConnection.Context.Permissions
        .Where(x => x.Id == permission.Id)
        .SelectMany(x => x.Admins)
        .AnyAsync(x => x.Id == admin.Id);

      Assert.IsTrue(hasPermission);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereNameIncludes_And_IsActiveEqualsTrue()
  {
    var parameters = new PermissionQueryParameters(0, 1, null, null, BaseRepositoryTest.Permissions[0].Name, true, null);
    var permissions = await PermissionRepository.GetAsync(parameters);

    Assert.IsTrue(permissions.Any());

    foreach (var permission in permissions)
    { 
      Assert.IsTrue(permission.Name.Contains(BaseRepositoryTest.Permissions[0].Name));
      Assert.IsTrue(permission.IsActive);
    }
  }

}
