using Hotel.Domain.Entities.EmployeeContext.ResponsabilityEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Exceptions;


namespace Hotel.Tests.Entities.EmplyeeContext;

[TestClass]
public class ResponsabilityEntityTest
{

  [TestMethod]
  public void CreateValidResponsability_MustBeValid()
  {
    var responsability = new Responsability("Limpar os quartos","Limpar",EPriority.Low);
    Assert.AreEqual(true,responsability.IsValid);
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  [DataRow("","")]
  [DataRow("res","")]
  [DataRow("","res")]
  [DataRow("res",TestParameters.DescriptionMaxCaracteres)]
  public void CreateInvalidResponsability_ExpectedException(string name,string description)
  {
    new Responsability(name,description,EPriority.Low);
    Assert.Fail();
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  [DataRow("")]
  [DataRow(TestParameters.DescriptionMaxCaracteres)]
  public void ChangeDescription_WithInvalidValues_ExpectedException(string description)
  {
    var responsability = new Responsability("Responsability",description,EPriority.Low);
    responsability.ChangeDescription(description);
    Assert.Fail();
  }
  
  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  [DataRow("")]
  public void ChangeName_WithInvalidValues_ExpectedException(string name)
  {
    var responsability = new Responsability(name,"Responsability",EPriority.Low);
    responsability.ChangeName(name);
    Assert.Fail();
  }

  [TestMethod]
  public void ChangePriorityTo_Medium_PriorityMustBeMedium()
  {
    var responsability = new Responsability("Responsability","Responsability",EPriority.Low);
    responsability.ChangePriority(EPriority.Medium);
    Assert.AreEqual(EPriority.Medium,responsability.Priority);
  }

  
}