using Hotel.Domain.Entities.ResponsibilityEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Exceptions;


namespace Hotel.Tests.UnitTests.Entities;

[TestClass]
public class ResponsibilityEntityTest
{

    [TestMethod]
    public void ValidResponsibility_MustBeValid()
    {
        var responsibility = new Responsibility("Limpar os quartos", "Limpar", EPriority.Low);
        Assert.AreEqual(true, responsibility.IsValid);
    }

    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    [DataRow("", "")]
    [DataRow("res", "")]
    [DataRow("", "res")]
    [DataRow("res", TestParameters.DescriptionMaxCaracteres)]
    public void InvalidResponsibility_ExpectedException(string name, string description)
    {
        new Responsibility(name, description, EPriority.Low);
        Assert.Fail();
    }

    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    [DataRow("")]
    [DataRow(TestParameters.DescriptionMaxCaracteres)]
    public void ChangeToInvalidResponsibilityDescription_ExpectedException(string description)
    {
        var responsibility = new Responsibility("Responsibility", description, EPriority.Low);
        responsibility.ChangeDescription(description);
        Assert.Fail();
    }

    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    public void ChangeToInvalidResponsibilityName_ExpectedException()
    {
        var responsibility = new Responsibility("Responsibility", "Responsibility", EPriority.Low);
        responsibility.ChangeName("");
        Assert.Fail();
    }

    [TestMethod]
    public void ChangePriorityToMedium_PriorityShouldBeMedium()
    {
        var responsibility = new Responsibility("Responsibility", "Responsibility", EPriority.Low);
        responsibility.ChangePriority(EPriority.Medium);
        Assert.AreEqual(EPriority.Medium, responsibility.Priority);
    }


}