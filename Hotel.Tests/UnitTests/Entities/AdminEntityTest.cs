using Hotel.Domain.Entities.AdminEntity;
using Hotel.Domain.Entities.PermissionEntity;
using Hotel.Domain.Exceptions;

namespace Hotel.Tests.UnitTests.Entities;

[TestClass]
public class AdminEntityTest
{
    public readonly Permission Permission = new("Permission", "Permission");

    [TestMethod]
    public void NewAdminInstance_MustBeValid()
    {
        var admin = new Admin(TestParameters.Name, TestParameters.Email, TestParameters.Phone, "123");

        Assert.IsFalse(admin.IsRootAdmin);
        Assert.IsTrue(admin.IsValid);
        Assert.AreEqual(TestParameters.Name, admin.Name);
        Assert.AreEqual(TestParameters.Email, admin.Email);
        Assert.AreEqual(TestParameters.Phone, admin.Phone);
        Assert.AreNotEqual("123", admin.PasswordHash);
        Assert.AreEqual(0, admin.Permissions.Count);
        Assert.IsNull(admin.Address);
        Assert.IsNull(admin.DateOfBirth);
        Assert.IsTrue(admin.IncompleteProfile);
    }

    [TestMethod]
    public void ChangeAdminToRootAdmin_WithOutRootAdmin_ShouldThrowException()
    {
        var admin = new Admin(TestParameters.Name, TestParameters.Email, TestParameters.Phone, TestParameters.Password);
        var exception = Assert.ThrowsException<ValidationException>(() => admin.ChangeToRootAdmin(TestParameters.Admin));
    }

    [TestMethod]
    public void AddPermission_MustBeAdded()
    {
        var admin = new Admin(TestParameters.Name, TestParameters.Email, TestParameters.Phone, TestParameters.Password);
        admin.AddPermission(Permission);

        Assert.AreEqual(1, admin.Permissions.Count);
    }

    [TestMethod]
    public void AddSamePermission_ShouldThrowException()
    {
        var admin = new Admin(TestParameters.Name, TestParameters.Email, TestParameters.Phone, TestParameters.Password);
        admin.AddPermission(Permission);
        var exception = Assert.ThrowsException<ValidationException>(() => admin.AddPermission(Permission));
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
    public void RemoveNonExistingPermission_ShouldThrowException()
    {
        var admin = new Admin(TestParameters.Name, TestParameters.Email, TestParameters.Phone, TestParameters.Password);
        var exception = Assert.ThrowsException<ValidationException>(() => admin.RemovePermission(Permission));
        Assert.AreEqual(0, admin.Permissions.Count);
    }
}
