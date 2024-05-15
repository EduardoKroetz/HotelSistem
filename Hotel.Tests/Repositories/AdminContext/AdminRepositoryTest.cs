using Hotel.Domain.DTOs.AdminContext.AdminDTOs;
using Hotel.Domain.Enums;
using Hotel.Domain.Repositories.AdminContext;
using Hotel.Tests.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Tests.Repositories.AdminContext;

[TestClass]
public class AdminRepositoryTest : BaseRepositoryTest
{
  private static AdminRepository AdminRepository { get; set; } = null!;

  [ClassInitialize]
  public static async Task Startup(TestContext? context)
  {
    await Startup();
    AdminRepository = new AdminRepository(MockConnection.Context);
  }

  [ClassCleanup]
  public static async Task Dispose()
  => await Cleanup();

  [TestMethod]
  public async Task GetByIdAsync_ReturnsWithCorrectParameters()
  {
    var admin = await AdminRepository.GetByIdAsync(Admins[0].Id);

    Assert.IsNotNull(admin);
    Assert.AreEqual(Admins[0].Name.FirstName, admin.FirstName);
    Assert.AreEqual(Admins[0].Name.LastName,admin.LastName);
    Assert.AreEqual(Admins[0].Email.Address,admin.Email);
    Assert.AreEqual(Admins[0].Phone.Number,admin.Phone);
    Assert.AreEqual(Admins[0].Id, admin.Id);
  }

  [TestMethod]
  public async Task GetAsync_ReturnWithCorrectParameters()
  {
    var parameters = new AdminQueryParameters(0, 1, Admins[0].Name.FirstName, null, null,null, null, null, null, null, null, null);
    var admins = await AdminRepository.GetAsync(parameters);

    var admin = admins.ToList()[0];

    Assert.IsNotNull(admin);
    Assert.AreEqual(Admins[0].Name.FirstName, admin.FirstName);
    Assert.AreEqual(Admins[0].Name.LastName, admin.LastName);
    Assert.AreEqual(Admins[0].Email.Address, admin.Email);
    Assert.AreEqual(Admins[0].Phone.Number, admin.Phone);
    Assert.AreEqual(Admins[0].Id, admin.Id);
    Assert.AreEqual(Admins[0].IsRootAdmin, admin.IsRootAdmin);
  }

  [TestMethod]
  public async Task GetAsync_WhereNameJoao_ReturnsAdminsJoao()
  {
    var parameters = new AdminQueryParameters(0, 1, Admins[0].Name.FirstName, null, null, null,null, null, null, null, null, null);
    var admins = await AdminRepository.GetAsync(parameters);

    Assert.IsTrue(admins.Any());
    foreach (var admin in admins)
    {
      Assert.IsNotNull(admin);
      Assert.AreEqual(Admins[0].Name.FirstName, admin.FirstName);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereEmailFilter_ReturnsAdmins()
  {
    var parameters = new AdminQueryParameters(0, 100, null, Admins[0].Email.Address, null,null, null, null, null, null, null, null);
    var admins = await AdminRepository.GetAsync(parameters);

    Assert.IsTrue(admins.Any());
    foreach (var admin in admins)
    {
      Assert.IsNotNull(admin);
      Assert.AreEqual(Admins[0].Email.Address, admin.Email);
    }
  }

  [TestMethod]
  public async Task GetAsync_WherePhoneFilter_ReturnsAdmins()
  {
    var parameters = new AdminQueryParameters(0, 100, null,null, Admins[0].Phone.Number, null, null, null, null, null, null, null);
    var admins = await AdminRepository.GetAsync(parameters);

    Assert.IsTrue(admins.Any());
    foreach (var admin in admins)
    {
      Assert.IsNotNull(admin);
      Assert.AreEqual(Admins[0].Phone.Number, admin.Phone);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereGenderFilter_ReturnsAdmins()
  {
    var parameters = new AdminQueryParameters(0, 100, null, null, null, Admins[0].Gender, null, null, null, null, null, null);
    var admins = await AdminRepository.GetAsync(parameters);

    Assert.IsTrue(admins.Any());
    foreach (var admin in admins)
    {
      Assert.IsNotNull(admin);
      Assert.AreEqual(Admins[0].Gender, admin.Gender);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereDateOfBirthGratherThan2000_ReturnsAdmins()
  {
    var parameters = new AdminQueryParameters(0, 100, null, null, null, null, DateTime.Now.AddYears(-24), "gt", null, null, null, null);
    var admins = await AdminRepository.GetAsync(parameters);

    Assert.IsTrue(admins.Any());
    foreach (var admin in admins)
    {
      Assert.IsNotNull(admin);
      Assert.IsTrue(DateTime.Now.AddYears(-24) < admin.DateOfBirth);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereCreatedAtFilter_WhereGratherThanOperator_ReturnsAdmins()
  {
    var parameters = new AdminQueryParameters(0, 100, null, null, null, null, null, null, DateTime.Now.AddDays(-1), "gt", null, null);
    var admins = await AdminRepository.GetAsync(parameters);

    Assert.IsTrue(admins.Any());
    foreach (var admin in admins)
    {
      Assert.IsNotNull(admin);
      Assert.IsTrue(DateTime.Now.AddDays(-1) < admin.CreatedAt);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereCreatedAtFilter_WhereLessThanOperator_ReturnsAdmins()
  {
    var parameters = new AdminQueryParameters(0, 100, null, null, null, null, null, null, DateTime.Now.AddDays(1), "lt", null, null);
    var admins = await AdminRepository.GetAsync(parameters);

    Assert.IsTrue(admins.Any());
    foreach (var admin in admins)
    {
      Assert.IsNotNull(admin);
      Assert.IsTrue(DateTime.Now.AddDays(1) > admin.CreatedAt);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereCreatedAtFilter_WhereEqualsOperator_ReturnsAdmins()
  {
    var parameters = new AdminQueryParameters(0, 100, null, null, null, null, null, null, Admins[0].CreatedAt, "eq", null, null);
    var admins = await AdminRepository.GetAsync(parameters);

    Assert.IsTrue(admins.Any());
    foreach (var admin in admins)
    {
      Assert.IsNotNull(admin);
      Assert.AreEqual(Admins[0].CreatedAt,admin.CreatedAt);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereIsRootAdminEqualsFalse_ReturnsAdmins()
  {
    var parameters = new AdminQueryParameters(0, 100, null, null, null, null, null, null, null, null, false, null);
    var admins = await AdminRepository.GetAsync(parameters);

    Assert.IsTrue(admins.Any());
    foreach (var admin in admins)
    {
      Assert.IsNotNull(admin);
      Assert.AreEqual(false, admin.IsRootAdmin);
    }
  }

  [TestMethod]
  public async Task GetAsync_WherePermissionId_ReturnsAdmins()
  {
    Admins[0].AddPermission(TestParameters.Permission);
    await MockConnection.Context.SaveChangesAsync();
    var parameters = new AdminQueryParameters(0, 100, null, null, null, null, null, null, null,null, null, TestParameters.Permission.Id);
    var admins = await AdminRepository.GetAsync(parameters);

  
    Assert.IsTrue(admins.Any());
    foreach (var admin in admins)
    {
      Assert.IsNotNull(admin);

      var hasPermission = await MockConnection.Context.Admins
        .Where(x => x.Id == admin.Id)
        .SelectMany(x => x.Permissions)
        .AnyAsync(x => x.Id == TestParameters.Permission.Id);

      Assert.IsTrue(hasPermission);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereEmailIncludesExample_And_PhoneIncludes55_And_IsRootAdminEqualsFalse_And_GenderEqualsMasculine_ReturnsAdmins()
  {
    var parameters = new AdminQueryParameters(0, 100, null,"example", "55", EGender.Masculine, null, null,null, null, false, null);
    var admins = await AdminRepository.GetAsync(parameters);
  
    Assert.IsTrue(admins.Any());

    foreach (var admin in admins)
    {
      Assert.IsNotNull(admin);
      Assert.IsTrue(admin.Email.Contains("example"));
      Assert.IsTrue(admin.Phone.Contains("55"));
      Assert.AreEqual(EGender.Masculine,admin.Gender);
      Assert.AreEqual(false, admin.IsRootAdmin);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereNameIncludesR_And_DateOfBirthLessThan31Years_And_CreatedAtLessThanTomorrow_ReturnsAdmins()
  {
    var parameters = new AdminQueryParameters(0, 100, "R", null, null, null, DateTime.Now.AddYears(-31), "lt", DateTime.Now.AddDays(1), "lt", null, null);
    
    var admins = await AdminRepository.GetAsync(parameters);

    Assert.IsTrue(admins.Any());

    foreach (var admin in admins)
    {
      Assert.IsNotNull(admin);
      Assert.IsTrue(admin.FirstName.Contains('R'));
      Assert.IsTrue(DateTime.Now.AddYears(-31) > admin.DateOfBirth);
      Assert.IsTrue(DateTime.Now.AddDays(1) > admin.CreatedAt);
    }
  }
}
