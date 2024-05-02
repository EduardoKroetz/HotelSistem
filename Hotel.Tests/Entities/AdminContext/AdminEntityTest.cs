using Hotel.Domain.Entities.AdminContext.AdminEntity;
using Hotel.Domain.Entities.AdminContext.PermissionEntity;
using Hotel.Domain.Exceptions;
using Hotel.Domain.ValueObjects;

namespace Hotel.Tests.Entities.AdminContext;

[TestClass]
public class AdminEntityTest
{
  public readonly Permission Permission = new("Permission","Permission");

  [TestMethod]
  public void CreateAdmin_WithValidParameters_MustBeValid()
  {
    var admin = new Admin(TestParameters.Name,TestParameters.Email,TestParameters.Phone,TestParameters.Password);
    Assert.AreEqual(true, admin.IsValid);
  }

  [TestMethod]
  public void ChangeAdmin_To_RootAdmin_WithOut_RootAdmin_MustBeInvalid()
  {
    var admin = new Admin(TestParameters.Name,TestParameters.Email,TestParameters.Phone,TestParameters.Password);
    admin.ChangeToRootAdmin(TestParameters.Admin);

    Assert.AreEqual(false,admin.IsRootAdmin);
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  public void CreateAdmin_With_InvalidParameters_ExpectedExeception()
  {
    new Admin(new Name("",""),new Email(""),new Phone(""),"");
  }

  [TestMethod]
  public void AddPermission_With_ValidPermission_MustBeValid()
  {
    var admin = new Admin(TestParameters.Name,TestParameters.Email,TestParameters.Phone,TestParameters.Password);
    admin.AddPermission(Permission);

    Assert.AreEqual(1,admin.Permissions.Count);
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  public void AddPermission_With_InvalidPermission_ExpectedException()
  {
    var admin = new Admin(TestParameters.Name,TestParameters.Email,TestParameters.Phone,TestParameters.Password);
    var permission = new Permission("","");
    admin.AddPermission(permission);
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  public void AddPermission_With_DisabledPermission_ExpectedException()
  {
    var admin = new Admin(TestParameters.Name,TestParameters.Email,TestParameters.Phone,TestParameters.Password);
    var permission = new Permission("Permission 1","Permission 1");
    permission.Disable();
    admin.AddPermission(permission);
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  public void AddPermission_With_AlreadyAddedPermission_ExpectedException()
  {
    var admin = new Admin(TestParameters.Name,TestParameters.Email,TestParameters.Phone,TestParameters.Password);
    admin.AddPermission(Permission);
    admin.AddPermission(Permission);
  }

  [TestMethod]
  public void RemovePermission_With_AddedPermission_MustBeValid()
  {
    var admin = new Admin(TestParameters.Name,TestParameters.Email,TestParameters.Phone,TestParameters.Password);
    admin.AddPermission(Permission);
    admin.RemovePermission(Permission);

    Assert.AreEqual(0, admin.Permissions.Count);
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  public void RemovePermission_Without_AddedPermission_ExpectedException()
  {
    var admin = new Admin(TestParameters.Name,TestParameters.Email,TestParameters.Phone,TestParameters.Password);
    admin.RemovePermission(Permission);
  }
}