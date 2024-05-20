
using Hotel.Domain.Entities.AdminContext.PermissionEntity;
using Hotel.Domain.Exceptions;

namespace Hotel.Tests.Entities.AdminContext;

[TestClass]
public class PermissionEntityTest
{
  [TestMethod]
  public void ValidPermission_MustBeValid()
  {
    var permission = new Permission("Permission","Permission");
    Assert.AreEqual(true,permission.IsValid);
  }

  [TestMethod]
  [DataRow("","")]
  [DataRow("permission",TestParameters.DescriptionMaxCaracteres)]
  [DataRow("",TestParameters.DescriptionMaxCaracteres)]
  public void InvalidPermission_ExpectedException(string name, string description)
  {
    try
    {
      new Permission(name,description);
      Assert.Fail();
    }catch(ValidationException)
    {
      Assert.IsTrue(true);
    }

  }

  [TestMethod]
  [ExpectedException(typeof(InvalidOperationException))]
  public void EnablePermission_WithPermissionAlreadyEnabled_ExpectedException()
  {
    var permission = new Permission("Permission","Permission");
    permission.Enable();
    Assert.Fail();
  }

  [TestMethod]
  public void EnablePermission_WithPermissionDisabled_MustBeEnable()
  {
    var permission = new Permission("Permission", "Permission");
    permission.Disable();
    permission.Enable();
    Assert.IsTrue(permission.IsActive);
  }

  [TestMethod]
  [ExpectedException(typeof(InvalidOperationException))]
  public void EnablePermission_WithPermissionAlreadyDisabled_ExpectedException()
  {
    var permission = new Permission("Permission", "Permission");
    permission.Disable();
    permission.Disable();
    Assert.Fail();
  }

  [TestMethod]
  public void DisablePermission_MustBeDisable()
  {
    var permission = new Permission("Permission","Permission");
    permission.Disable();
    Assert.IsFalse(permission.IsActive);
  }
}