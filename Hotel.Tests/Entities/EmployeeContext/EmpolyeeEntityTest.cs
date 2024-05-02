using Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;
using Hotel.Domain.Entities.EmployeeContext.ResponsabilityEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Exceptions;
using Hotel.Domain.ValueObjects;

namespace Hotel.Tests.Entities.EmplyeeContext;

[TestClass]
public class EmployeeEntityTest
{
  private readonly Responsability OtherResponsability = new("Limpar os quartos","Limpar",EPriority.Low);

  [TestMethod]
  public void CreateValidEmployee_MustBeValid()
  {
    var employee = new Employee(TestParameters.Name,TestParameters.Email,TestParameters.Phone,"password123",TestParameters.Responsability);
    Assert.AreEqual(true,employee.IsValid);
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  public void CreateInvalidEmployee_ExpectedException()
  {
    new Employee(new Name("",""),new Email(""),new Phone(""),"",TestParameters.Responsability);
    Assert.Fail();
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  public void ChangeSalary_With_InvalidValue_ExpectedException()
  {
    var employee = new Employee(TestParameters.Name,TestParameters.Email,TestParameters.Phone,"password123",TestParameters.Responsability);
    employee.ChangeSalary(-1);
    Assert.Fail();
  }

  [TestMethod]
  public void ChangeSalary_With_ValidValue_MustBeChanged()
  {
    var employee = new Employee(TestParameters.Name,TestParameters.Email,TestParameters.Phone,"password123",TestParameters.Responsability);
    employee.ChangeSalary(0);
    Assert.AreEqual(0,employee.Salary);
  }

  [TestMethod]
  public void Add_With_ValidValue_MustBeChanged()
  {
    var employee = new Employee(TestParameters.Name,TestParameters.Email,TestParameters.Phone,"password123",TestParameters.Responsability);
    employee.ChangeSalary(0);
    Assert.AreEqual(0,employee.Salary);
  }

  [TestMethod]
  public void AddResponsability_MustBeAdded()
  {
    var employee = new Employee(TestParameters.Name,TestParameters.Email,TestParameters.Phone,"password123",TestParameters.Responsability);
    employee.AddResponsability(OtherResponsability);
    Assert.AreEqual(2,employee.Responsabilities.Count);
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  public void AddSameResponsability_ExpectedException()
  {
    var employee = new Employee(TestParameters.Name,TestParameters.Email,TestParameters.Phone,"password123",TestParameters.Responsability);
    employee.AddResponsability(TestParameters.Responsability);
    Assert.Fail();
  }

  [TestMethod]
  public void RemoveExistingResponsability_ExpectedException()
  {
    var employee = new Employee(TestParameters.Name,TestParameters.Email,TestParameters.Phone,"password123",TestParameters.Responsability);
    employee.RemoveResponsability(TestParameters.Responsability);
    Assert.AreEqual(0,employee.Responsabilities.Count);
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  public void RemoveResponsability_Without_Existing_ExpectedException()
  {
    var employee = new Employee(TestParameters.Name,TestParameters.Email,TestParameters.Phone,"password123",TestParameters.Responsability);
    employee.RemoveResponsability(OtherResponsability);
    Assert.Fail();
  }
}