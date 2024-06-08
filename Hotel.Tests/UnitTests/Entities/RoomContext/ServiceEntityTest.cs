using Hotel.Domain.Entities.EmployeeContext.ResponsabilityEntity;
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
    public void AddSameResponsabilityToService_DontAdd()
    {
        var service = new Service("Preparar e servir o almoço", 25m, EPriority.Medium, 55);
        service.AddResponsability(TestParameters.Responsability);
        service.AddResponsability(TestParameters.Responsability);
        Assert.Fail();
    }

    [TestMethod]
    public void AddResponsabilitiesToServices_MustBeAdded()
    {
        var service = new Service("Preparar e servir o almoço", 25m, EPriority.Medium, 55);
        var responsability = new Responsability("Varrer o chão", "Varrer", EPriority.Trivial);
        service.AddResponsability(responsability);
        service.AddResponsability(TestParameters.Responsability);
        Assert.AreEqual(2, service.Responsabilities.Count);
    }

    [TestMethod]
    public void RemoveResponsabilityFromService_MustBeRemoved()
    {
        var service = new Service("Preparar e servir o almoço", 25m, EPriority.Medium, 55);
        service.AddResponsability(TestParameters.Responsability);
        service.RemoveResponsability(TestParameters.Responsability);
        Assert.AreEqual(0, service.Responsabilities.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    public void RemoveNoneExistsResponsabilityFromService_ExpectedException()
    {
        var service = new Service("Preparar e servir o almoço", 25m, EPriority.Medium, 55);
        service.RemoveResponsability(TestParameters.Responsability);
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