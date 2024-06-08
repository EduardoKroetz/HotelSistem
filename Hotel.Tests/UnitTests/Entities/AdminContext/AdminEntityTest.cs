using Hotel.Domain.Entities.AdminContext.AdminEntity;
using Hotel.Domain.Entities.AdminContext.PermissionEntity;
using Hotel.Domain.Exceptions;
using Hotel.Tests.UnitTests.Entities;

namespace Hotel.Tests.UnitTests.Entities.AdminContext;

[TestClass]
public class AdminEntityTest
{
    public readonly Permission Permission = new("Permission", "Permission");

    [TestMethod]
    public void ValidAdmin_MustBeValid()
    {
        var admin = new Admin(TestParameters.Name, TestParameters.Email, TestParameters.Phone, TestParameters.Password);
        Assert.IsTrue(admin.IsValid);
    }

    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    public void ChangeAdminToRootAdmin_WithOutRootAdmin_MustBeInvalid()
    {
        var admin = new Admin(TestParameters.Name, TestParameters.Email, TestParameters.Phone, TestParameters.Password);
        admin.ChangeToRootAdmin(TestParameters.Admin);
        Assert.Fail();
    }

    [TestMethod]
    public void AddPermission_MustBeAdded()
    {
        var admin = new Admin(TestParameters.Name, TestParameters.Email, TestParameters.Phone, TestParameters.Password);
        admin.AddPermission(Permission);

        Assert.AreEqual(1, admin.Permissions.Count);
    }


    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    public void AddSamePermission_ExpectedException()
    {
        var admin = new Admin(TestParameters.Name, TestParameters.Email, TestParameters.Phone, TestParameters.Password);
        admin.AddPermission(Permission);
        admin.AddPermission(Permission);
        Assert.AreEqual(1, admin.Permissions.Count);
    }

    [TestMethod]
    public void RemovePermission_MustBeValid()
    {
        var admin = new Admin(TestParameters.Name, TestParameters.Email, TestParameters.Phone, TestParameters.Password);
        admin.AddPermission(Permission);
        admin.RemovePermission(Permission);

        Assert.AreEqual(0, admin.Permissions.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    public void RemoveNonExistingPermission_ExpectedException()
    {
        var admin = new Admin(TestParameters.Name, TestParameters.Email, TestParameters.Phone, TestParameters.Password);
        admin.RemovePermission(Permission);
        Assert.AreEqual(0, admin.Permissions.Count);
    }
}