using Hotel.Domain.Entities.EmployeeEntity;
using Hotel.Domain.Entities.ResponsibilityEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Exceptions;

namespace Hotel.Tests.UnitTests.Entities;

[TestClass]
public class EmployeeEntityTest
{
    private readonly Responsibility Responsibility = new("Limpar os quartos", "Limpar", EPriority.Low);

    [TestMethod]
    public void NewEmployeeInstance_MustBeValid()
    {
        var employee = new Employee(TestParameters.Name, TestParameters.Email, TestParameters.Phone, "password123");
        Assert.IsTrue(employee.IsValid);
    }

    [TestMethod]
    public void ChangeToInvalidEmployeeSalary_ShouldThrowException()
    {
        var employee = new Employee(TestParameters.Name, TestParameters.Email, TestParameters.Phone, "password123");
        var exception = Assert.ThrowsException<ValidationException>(() => employee.ChangeSalary(-1));
    }

    [TestMethod]
    public void ChangeToValidSalary_MustBeChanged()
    {
        var employee = new Employee(TestParameters.Name, TestParameters.Email, TestParameters.Phone, "password123");
        employee.ChangeSalary(0);
        Assert.AreEqual(0, employee.Salary);
    }

    [TestMethod]
    public void AddResponsibility_MustBeAdded()
    {
        var employee = new Employee(TestParameters.Name, TestParameters.Email, TestParameters.Phone, "password123");
        employee.AddResponsibility(Responsibility);
        Assert.AreEqual(1, employee.Responsibilities.Count);
    }

    [TestMethod]
    public void AddSameResponsibility_ShouldThrowException()
    {
        var employee = new Employee(TestParameters.Name, TestParameters.Email, TestParameters.Phone, "password123");
        employee.AddResponsibility(Responsibility);
        var exception = Assert.ThrowsException<ArgumentException>(() => employee.AddResponsibility(Responsibility));
        Assert.AreEqual(1, employee.Responsibilities.Count);
    }

    [TestMethod]
    public void RemoveResponsibility_MustBeRemoved()
    {
        var employee = new Employee(TestParameters.Name, TestParameters.Email, TestParameters.Phone, "password123");
        employee.AddResponsibility(Responsibility);
        employee.RemoveResponsibility(Responsibility);
        Assert.AreEqual(0, employee.Responsibilities.Count);
    }

    [TestMethod]
    public void RemoveNonExistingResponsibility_ShouldThrowException()
    {
        var employee = new Employee(TestParameters.Name, TestParameters.Email, TestParameters.Phone, "password123");
        var exception = Assert.ThrowsException<ArgumentException>(() => employee.RemoveResponsibility(Responsibility));
    }

    [TestMethod]
    public void AssignValidPermission_MustBeAssigned()
    {
        var employee = new Employee(TestParameters.Name, TestParameters.Email, TestParameters.Phone, TestParameters.Password);
        employee.AssignPermission(TestParameters.Permission);
        Assert.AreEqual(1, employee.Permissions.Count);
    }

    [TestMethod]
    public void AssignSamePermission_ShouldThrowException()
    {
        var employee = new Employee(TestParameters.Name, TestParameters.Email, TestParameters.Phone, TestParameters.Password);
        employee.AssignPermission(TestParameters.Permission);
        var exception = Assert.ThrowsException<ValidationException>(() => employee.AssignPermission(TestParameters.Permission));
        Assert.AreEqual(1, employee.Permissions.Count);
    }

    [TestMethod]
    public void UnassignExistPermission_MustBeUnassigned()
    {
        var employee = new Employee(TestParameters.Name, TestParameters.Email, TestParameters.Phone, TestParameters.Password);
        employee.AssignPermission(TestParameters.Permission);
        employee.UnassignPermission(TestParameters.Permission);

        Assert.AreEqual(0, employee.Permissions.Count);
    }

    [TestMethod]
    public void UnassignNonExistingPermission_ShouldThrowException()
    {
        var employee = new Employee(TestParameters.Name, TestParameters.Email, TestParameters.Phone, TestParameters.Password);
        var exception = Assert.ThrowsException<ValidationException>(() => employee.UnassignPermission(TestParameters.Permission));
    }
}
