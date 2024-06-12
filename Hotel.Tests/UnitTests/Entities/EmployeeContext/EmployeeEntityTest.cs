using Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;
using Hotel.Domain.Entities.EmployeeContext.ResponsibilityEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Exceptions;
using Hotel.Tests.UnitTests.Entities;

namespace Hotel.Tests.UnitTests.Entities.EmployeeContext;

[TestClass]
public class EmployeeEntityTest
{
    private readonly Responsibility Responsibility = new("Limpar os quartos", "Limpar", EPriority.Low);

    [TestMethod]
    public void ValidEmployee_MustBeValid()
    {
        var employee = new Employee(TestParameters.Name, TestParameters.Email, TestParameters.Phone, "password123");
        Assert.IsTrue(employee.IsValid);
    }

    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    public void ChangeToInvalidEmployeeSalary_ExpectedException()
    {
        var employee = new Employee(TestParameters.Name, TestParameters.Email, TestParameters.Phone, "password123");
        employee.ChangeSalary(-1);
        Assert.Fail();
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
        Assert.AreEqual(1, employee.Responsabilities.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AddSameResponsibility_ExpectedException()
    {
        var employee = new Employee(TestParameters.Name, TestParameters.Email, TestParameters.Phone, "password123");
        employee.AddResponsibility(Responsibility);
        employee.AddResponsibility(Responsibility);
        Assert.Fail();
    }

    [TestMethod]
    public void RemoveResponsibility_MustBeRemoved()
    {
        var employee = new Employee(TestParameters.Name, TestParameters.Email, TestParameters.Phone, "password123");
        employee.AddResponsibility(Responsibility);
        employee.RemoveResponsibility(Responsibility);
        Assert.AreEqual(0, employee.Responsabilities.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void RemoveNonExistingResponsibility_ExpectedException()
    {
        var employee = new Employee(TestParameters.Name, TestParameters.Email, TestParameters.Phone, "password123");
        employee.RemoveResponsibility(Responsibility);
        Assert.Fail();
    }


    [TestMethod]
    public void AssignValidPermission_MustBeAssigned()
    {
        var employee = new Employee(TestParameters.Name, TestParameters.Email, TestParameters.Phone, TestParameters.Password);
        employee.AssignPermission(TestParameters.Permission);

        Assert.AreEqual(1, employee.Permissions.Count);
    }


    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    public void AssignSamePermission_ExpectedException()
    {
        var employee = new Employee(TestParameters.Name, TestParameters.Email, TestParameters.Phone, TestParameters.Password);
        employee.AssignPermission(TestParameters.Permission);
        employee.AssignPermission(TestParameters.Permission);
        Assert.AreEqual(1, employee.Permissions.Count);
    }

    [TestMethod]
    public void UnassignExistPermission_MustBeUnassign()
    {
        var employee = new Employee(TestParameters.Name, TestParameters.Email, TestParameters.Phone, TestParameters.Password);
        employee.AssignPermission(TestParameters.Permission);
        employee.UnassignPermission(TestParameters.Permission);

        Assert.AreEqual(0, employee.Permissions.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    public void UnassignNonExistingPermission_ExpectedException()
    {
        var employee = new Employee(TestParameters.Name, TestParameters.Email, TestParameters.Phone, TestParameters.Password);
        employee.UnassignPermission(TestParameters.Permission);
        Assert.AreEqual(0, employee.Permissions.Count);
    }
}