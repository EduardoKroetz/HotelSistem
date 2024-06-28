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
        Assert.AreEqual("Sal�rio deve ser maior ou igual a zero", exception.Message);
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
        Assert.AreEqual("Essa responsabilidade j� est� atribuida a esse funcion�rio", exception.Message);
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
        Assert.AreEqual("Essa responsabilidade n�o est� atribuida a esse funcion�rio", exception.Message);
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
        Assert.AreEqual("Essa permiss�o j� foi associada a esse funcion�rio", exception.Message);
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
        Assert.AreEqual("Essa permiss�o n�o est� associada a esse funcion�rio", exception.Message);
    }
}
