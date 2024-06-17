using Hotel.Domain.Entities.EmployeeContext.ResponsibilityEntity;
using Hotel.Domain.Entities.RoomContext.ServiceEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Exceptions;
using Hotel.Tests.UnitTests.Entities;

namespace Hotel.Tests.UnitTests.Entities.RoomContext;

[TestClass]
public class ServiceEntityTest
{
    [TestMethod]
    public void ValidService_MustBeValid()
    {
        var service = new Service("Preparar e servir o almoço", 25m, EPriority.Medium, 55);
        Assert.AreEqual(true, service.IsValid);
    }

    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    [DataRow("Limpeza de quarto", 1, 0)]
    [DataRow("Limpeza de quarto", 0, 30)]
    [DataRow("", 1, 1)]
    [DataRow("Limpeza de quarto", 999, 0)]
    public void InvalidServiceParameters_ExpectedException(string name, int price, int timeInMinutes)
    {
        new Service(name, price, EPriority.Medium, timeInMinutes);
        Assert.Fail();
    }

    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    public void AddSameResponsibilityToService_DontAdd()
    {
        var service = new Service("Preparar e servir o almoço", 25m, EPriority.Medium, 55);
        service.AddResponsibility(TestParameters.Responsibility);
        service.AddResponsibility(TestParameters.Responsibility);
        Assert.Fail();
    }

    [TestMethod]
    public void AddResponsibilitiesToServices_MustBeAdded()
    {
        var service = new Service("Preparar e servir o almoço", 25m, EPriority.Medium, 55);
        var responsibility = new Responsibility("Varrer o chão", "Varrer", EPriority.Trivial);
        service.AddResponsibility(responsibility);
        service.AddResponsibility(TestParameters.Responsibility);
        Assert.AreEqual(2, service.Responsibilities.Count);
    }

    [TestMethod]
    public void RemoveResponsibilityFromService_MustBeRemoved()
    {
        var service = new Service("Preparar e servir o almoço", 25m, EPriority.Medium, 55);
        service.AddResponsibility(TestParameters.Responsibility);
        service.RemoveResponsibility(TestParameters.Responsibility);
        Assert.AreEqual(0, service.Responsibilities.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    public void RemoveNoneExistsResponsibilityFromService_ExpectedException()
    {
        var service = new Service("Preparar e servir o almoço", 25m, EPriority.Medium, 55);
        service.RemoveResponsibility(TestParameters.Responsibility);
        Assert.Fail();
    }

    [TestMethod]
    public void ChangeNameOfService_MustBeChanged()
    {
        var service = new Service("Preparar e servir o almoço", 25m, EPriority.Medium, 55);
        service.ChangeName("Banana");
        Assert.AreEqual("Banana", service.Name);
    }

    [TestMethod]
    public void ChangePriceOfService_MustBeChanged()
    {
        var service = new Service("Preparar e servir o almoço", 25m, EPriority.Medium, 55);
        service.ChangePrice(10m);
        Assert.AreEqual(10m, service.Price);
    }

    [TestMethod]
    public void DisableService_MustBeDisabled()
    {
        var service = new Service("Preparar e servir o almoço", 25m, EPriority.Medium, 55);
        service.Disable();
        Assert.AreEqual(false, service.IsActive);
    }

    [TestMethod]
    public void EnableService_MustBeEnable()
    {
        var service = new Service("Preparar e servir o almoço", 25m, EPriority.Medium, 55);
        service.Disable();
        service.Enable();
        Assert.AreEqual(true, service.IsActive);
    }

    [TestMethod]
    public void ChangePriorityOfService_MustBeChanged()
    {
        var service = new Service("Preparar e servir o almoço", 25m, EPriority.Medium, 55);
        service.ChangePriority(EPriority.Critical);
        Assert.AreEqual(EPriority.Critical, service.Priority);
    }

    [TestMethod]
    public void ChangeTimeOfService_MustBeChanged()
    {
        var service = new Service("Preparar e servir o almoço", 25m, EPriority.Medium, 55);
        service.ChangeTime(10);
        Assert.AreEqual(10, service.TimeInMinutes);
    }

}