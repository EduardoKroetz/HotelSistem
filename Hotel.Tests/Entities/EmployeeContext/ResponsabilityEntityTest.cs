using Hotel.Domain.Entities.EmployeeContext.ResponsabilityEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Exceptions;


namespace Hotel.Tests.Entities.EmployeeContext;

[TestClass]
public class ResponsabilityEntityTest
{

  [TestMethod]
  public void ValidResponsability_MustBeValid()
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
  public void InvalidResponsability_ExpectedException(string name,string description)
  {
    new Responsability(name,description,EPriority.Low);
    Assert.Fail();
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  [DataRow("")]
  [DataRow(TestParameters.DescriptionMaxCaracteres)]
  public void ChangeToInvalidResponsabilityDescription_ExpectedException(string description)
  {
    var responsability = new Responsability("Responsability",description,EPriority.Low);
    responsability.ChangeDescription(description);
    Assert.Fail();
  }
  
  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  public void ChangeToInvalidResponsabilityName_ExpectedException()
  {
    var responsability = new Responsability("Responsability","Responsability",EPriority.Low);
    responsability.ChangeName("");
    Assert.Fail();
  }

  [TestMethod]
  public void ChangePriorityToMedium_PriorityShouldBeMedium()
  {
    var responsability = new Responsability("Responsability","Responsability",EPriority.Low);
    responsability.ChangePriority(EPriority.Medium);
    Assert.AreEqual(EPriority.Medium,responsability.Priority);
  }

  
}