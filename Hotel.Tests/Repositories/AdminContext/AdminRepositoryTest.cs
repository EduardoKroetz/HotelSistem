using Hotel.Domain.DTOs.AdminContext.AdminDTOs;
using Hotel.Domain.Entities.AdminContext.AdminEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Repositories.AdminContext;
using Hotel.Domain.ValueObjects;
using Hotel.Tests.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Tests.Repositories.AdminContext;

[TestClass]
public class AdminRepositoryTest : BaseRepositoryTest
{
  private List<Admin> _admins { get; set; } = [];
  private Admin _defaultAdmin { get; set; } = null!;
  private AdminRepository _adminRepository { get; set; } = null!;

  [TestInitialize]
  public async Task StartupTest()
  {
    await Startup(null);
    _adminRepository = new AdminRepository(mockConnection.Context);

    _defaultAdmin = new Admin(new Name("João", "Pedro"), new Email("joaopedro@gmail.com"), new Phone("+55 (55) 33112-3456"), "1234", EGender.Masculine, DateTime.Now.AddYears(-20), new Address("Brazil", "São Paulo", "Avenida Paulista", 999));

    _admins.AddRange(
    [
      _defaultAdmin,
      new Admin(new Name("Maria", "Silva"), new Email("maria.silva@example.com"), new Phone("+55 (11) 98765-4321"), "senha123", EGender.Feminine, DateTime.Now.AddYears(-25), new Address("Brazil", "Rio de Janeiro", "Rua Copacabana", 123)),
      new Admin(new Name("Carlos", "Oliveira"), new Email("coliveira@example.com"), new Phone("+55 (21) 12345-6789"), "_admin123", EGender.Masculine, DateTime.Now.AddYears(-30), new Address("Brazil", "Brasília", "Quadra 123", 3)),
      new Admin(new Name("Ana", "Santos"), new Email("anasantos@example.com"), new Phone("+55 (31) 98765-4321"), "ana456", EGender.Feminine, DateTime.Now.AddYears(-28), new Address("Brazil", "Belo Horizonte", "Avenida Afonso Pena", 456)),
      new Admin(new Name("Rafael", "Silveira"), new Email("rafaelsilveira@example.com"), new Phone("+55 (19) 98765-4321"), "rafa789", EGender.Masculine, DateTime.Now.AddYears(-32), new Address("Brazil", "Campinas", "Rua Barão de Jaguara", 789))
    ]);

    await mockConnection.Context.Admins.AddRangeAsync(_admins);
    await mockConnection.Context.SaveChangesAsync();    
  }

  [TestCleanup]
  public async Task CleanupTest()
  {
    mockConnection.Context.Admins.RemoveRange(_admins);
    await mockConnection.Context.SaveChangesAsync();
    _admins.Clear();
  }

  [TestMethod]
  public async Task GetByIdAsync_ReturnsWithCorrectParameters()
  {
    var admin = await _adminRepository.GetByIdAsync(_defaultAdmin.Id);

    Assert.IsNotNull(admin);
    Assert.AreEqual(_defaultAdmin.Name.FirstName, admin.FirstName);
    Assert.AreEqual(_defaultAdmin.Name.LastName,admin.LastName);
    Assert.AreEqual(_defaultAdmin.Email.Address,admin.Email);
    Assert.AreEqual(_defaultAdmin.Phone.Number,admin.Phone);
    Assert.AreEqual(_defaultAdmin.Id, admin.Id);
  }

  [TestMethod]
  public async Task GetAsync_ReturnWithCorrectParameters()
  {
    var parameters = new AdminQueryParameters(0, 1, _defaultAdmin.Name.FirstName, null, null, null, null, null, null, null, null);
    var admins = await _adminRepository.GetAsync(parameters);

    var admin = admins.ToList()[0];

    Assert.IsNotNull(admin);
    Assert.AreEqual(_defaultAdmin.Name.FirstName, admin.FirstName);
    Assert.AreEqual(_defaultAdmin.Name.LastName, admin.LastName);
    Assert.AreEqual(_defaultAdmin.Email.Address, admin.Email);
    Assert.AreEqual(_defaultAdmin.Phone.Number, admin.Phone);
    Assert.AreEqual(_defaultAdmin.Id, admin.Id);
    Assert.AreEqual(_defaultAdmin.IsRootAdmin, admin.IsRootAdmin);
  }

  [TestMethod]
  public async Task GetAsync_WhereNameJoao_ReturnsAdminsJoao()
  {
    var parameters = new AdminQueryParameters(0, 1, _defaultAdmin.Name.FirstName, null, null, null, null, null, null, null, null);
    var admins = await _adminRepository.GetAsync(parameters);

    foreach (var admin in admins)
    {
      Assert.IsNotNull(admin);
      Assert.AreEqual(_defaultAdmin.Name.FirstName, admin.FirstName);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereEmailFilter_ReturnsAdmins()
  {
    var parameters = new AdminQueryParameters(0, 100, null, _defaultAdmin.Email.Address, null, null, null, null, null, null, null);
    var admins = await _adminRepository.GetAsync(parameters);

    foreach (var admin in admins)
    {
      Assert.IsNotNull(admin);
      Assert.AreEqual(_defaultAdmin.Email.Address, admin.Email);
    }
  }

  [TestMethod]
  public async Task GetAsync_WherePhoneFilter_ReturnsAdmins()
  {
    var parameters = new AdminQueryParameters(0, 100, null,null, _defaultAdmin.Phone.Number, null, null, null, null, null, null);
    var admins = await _adminRepository.GetAsync(parameters);

    foreach (var admin in admins)
    {
      Assert.IsNotNull(admin);
      Assert.AreEqual(_defaultAdmin.Phone.Number, admin.Phone);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereGenderFilter_ReturnsAdmins()
  {
    var parameters = new AdminQueryParameters(0, 100, null, null, null, _defaultAdmin.Gender, null, null, null, null, null);
    var admins = await _adminRepository.GetAsync(parameters);

    foreach (var admin in admins)
    {
      Assert.IsNotNull(admin);
      Assert.AreEqual(_defaultAdmin.Gender, admin.Gender);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereDateOfBirthFilter_ReturnsAdmins()
  {
    var parameters = new AdminQueryParameters(0, 100, null, null, null, null, _defaultAdmin.DateOfBirth, null, null, null, null);
    var admins = await _adminRepository.GetAsync(parameters);

    foreach (var admin in admins)
    {
      Assert.IsNotNull(admin);
      Assert.AreEqual(_defaultAdmin.DateOfBirth, admin.DateOfBirth);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereCreatedAtFilter_WhereGratherThanOperator_ReturnsAdmins()
  {
    var parameters = new AdminQueryParameters(0, 100, null, null, null, null, null, DateTime.Now.AddDays(-1), "gt", null, null);
    var admins = await _adminRepository.GetAsync(parameters);

    foreach (var admin in admins)
    {
      Assert.IsNotNull(admin);
      Assert.IsTrue(DateTime.Now.AddDays(-1) < admin.CreatedAt);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereCreatedAtFilter_WhereLessThanOperator_ReturnsAdmins()
  {
    var parameters = new AdminQueryParameters(0, 100, null, null, null, null, null, DateTime.Now.AddDays(1), "lt", null, null);
    var admins = await _adminRepository.GetAsync(parameters);

    foreach (var admin in admins)
    {
      Assert.IsNotNull(admin);
      Assert.IsTrue(DateTime.Now.AddDays(1) > admin.CreatedAt);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereCreatedAtFilter_WhereEqualsOperator_ReturnsAdmins()
  {
    var parameters = new AdminQueryParameters(0, 100, null, null, null, null, null, _defaultAdmin.CreatedAt, "eq", null, null);
    var admins = await _adminRepository.GetAsync(parameters);

    foreach (var admin in admins)
    {
      Assert.IsNotNull(admin);
      Assert.AreEqual(_defaultAdmin.CreatedAt,admin.CreatedAt);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereIsRootAdminEqualsFalse_ReturnsAdmins()
  {
    var parameters = new AdminQueryParameters(0, 100, null, null, null, null, null, null, null, false, null);
    var admins = await _adminRepository.GetAsync(parameters);

    foreach (var admin in admins)
    {
      Assert.IsNotNull(admin);
      Assert.AreEqual(false, admin.IsRootAdmin);
    }
  }

  [TestMethod]
  public async Task GetAsync_WherePermissionId_ReturnsAdmins()
  {
    _defaultAdmin.AddPermission(TestParameters.Permission);
    var parameters = new AdminQueryParameters(0, 100, null, null, null, null, null, null, null, null, TestParameters.Permission.Id);
    var admins = await _adminRepository.GetAsync(parameters);

  
    foreach (var admin in admins)
    {
      Assert.IsNotNull(admin);

      var hasPermission = await mockConnection.Context.Admins
        .Where(x => x.Id == admin.Id)
        .SelectMany(x => x.Permissions)
        .AnyAsync(x => x.Id == TestParameters.Permission.Id);

      Assert.IsTrue(hasPermission);
    }
  }
}
