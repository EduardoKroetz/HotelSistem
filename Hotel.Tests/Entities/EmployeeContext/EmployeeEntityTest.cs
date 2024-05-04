using Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;
using Hotel.Domain.Entities.EmployeeContext.ResponsabilityEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Exceptions;

namespace Hotel.Tests.Entities.EmployeeContext;

[TestClass]
public class EmployeeEntityTest
{
  private readonly Responsability Responsability = new("Limpar os quartos","Limpar",EPriority.Low);

  [TestMethod]
  public void ValidEmployee_MustBeValid()
  {
    var employee = new Employee(TestParameters.Name,TestParameters.Email,TestParameters.Phone,"password123");
    Assert.IsTrue(employee.IsValid);
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  public void ChangeToInvalidEmployeeSalary_ExpectedException()
  {
    var employee = new Employee(TestParameters.Name,TestParameters.Email,TestParameters.Phone,"password123");
    employee.ChangeSalary(-1);
    Assert.Fail();
  }

  [TestMethod]
  public void ChangeToValidSalary_MustBeChanged()
  {
    var employee = new Employee(TestParameters.Name,TestParameters.Email,TestParameters.Phone,"password123");
    employee.ChangeSalary(0);
    Assert.AreEqual(0,employee.Salary);
  }

  [TestMethod]
  public void AddResponsability_MustBeAdded()
  {
    var employee = new Employee(TestParameters.Name,TestParameters.Email,TestParameters.Phone,"password123");
    employee.AddResponsability(Responsability);
    Assert.AreEqual(1,employee.Responsabilities.Count);
  }

  [TestMethod]
  public void AddSameResponsability_AddJustOne()
  {
    var employee = new Employee(TestParameters.Name,TestParameters.Email,TestParameters.Phone,"password123");
    employee.AddResponsability(Responsability);
    employee.AddResponsability(Responsability);
    Assert.AreEqual(1,employee.Responsabilities.Count);
  }

  [TestMethod]
  public void RemoveResponsability_MustBeRemoved()
  {
    var employee = new Employee(TestParameters.Name,TestParameters.Email,TestParameters.Phone,"password123");
    employee.AddResponsability(Responsability);
    employee.RemoveResponsability(Responsability);
    Assert.AreEqual(0,employee.Responsabilities.Count);
  }

  [TestMethod]
  public void RemoveNonExistingResponsability_DoNothing()
  {
    var employee = new Employee(TestParameters.Name,TestParameters.Email,TestParameters.Phone,"password123");
    employee.RemoveResponsability(Responsability);
    Assert.IsTrue(true);
  }
}