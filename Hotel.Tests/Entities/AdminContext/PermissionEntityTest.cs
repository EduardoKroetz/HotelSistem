
using Hotel.Domain.Entities.AdminContext.PermissionEntity;
using Hotel.Domain.Exceptions;

namespace Hotel.Tests.Entities.AdminContext;

[TestClass]
public class PermissionEntityTest
{
  [TestMethod]
  public void CreatePermission_With_ValidParameters_MustBeValid()
  {
    var permission = new Permission("Permission","Permission");
    Assert.AreEqual(true,permission.IsValid);
  }

  [TestMethod]
  [DataRow("","")]
  [DataRow("permission",TestParameters.DescriptionMaxCaracteres)]
  [DataRow("",TestParameters.DescriptionMaxCaracteres)]
  public void CreatePermission_With_InvalidParameters_ExpectedException(string name, string description)
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
  public void EnablePermission_MustBeEnable()
  {
    var permission = new Permission("Permission","Permission");
    permission.Enable();
    Assert.AreEqual(true,permission.IsActive);
  }

  [TestMethod]
  public void DisablePermission_MustBeDisable()
  {
    var permission = new Permission("Permission","Permission");
    permission.Disable();
    Assert.AreEqual(false,permission.IsActive);
  }
}