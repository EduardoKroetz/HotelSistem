using Hotel.Domain.Entities.RoomEntity;
using Hotel.Domain.Exceptions;

namespace Hotel.Tests.UnitTests.Entities;

[TestClass]
public class RoomEntityTest
{
    [TestMethod]
    public void ValidRoom_MustBeValid()
    {
        var room = new Room("Quarto padrão - 931",931, 15m, 5, "Quarto padrão no terceiro andar", TestParameters.Category);
        Assert.IsTrue(room.IsValid);
    }

    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    [DataRow(-50, -1, 0, "")] // tudo errado
    [DataRow(20, 1, 1, "")] //Descrição errada
    [DataRow(10, 0, 1, "Lindo quarto")] //Preço errado
    [DataRow(-1, 1, 1, "Lindo quarto")] //Numero errado
    [DataRow(1, 1, 1, TestParameters.DescriptionMaxCaracteres)] //Descrição errada, limite atingido.
    public void InvalidRoom_ExpectedException(int number, int price, int capacity, string description)
    {
        new Room("Quarto padrão - 931",number, price, capacity, description, TestParameters.Category);
        Assert.Fail();
    }

    [TestMethod]
    public void ChangeRoomNumber_MustBeChanged()
    {
        var room = new Room("Quarto padrão - 931",931, 15m, 5, "Quarto padrão no terceiro andar", TestParameters.Category);
        room.ChangeNumber(1);
        Assert.AreEqual(1, room.Number);
    }

    [TestMethod]
    public void ChangeRoomPrice_MustBeChanged()
    {
        var room = new Room("Quarto padrão - 931",931, 15m, 5, "Quarto padrão no terceiro andar", TestParameters.Category);
        room.ChangePrice(1);
        Assert.AreEqual(1, room.Price);
    }

    [TestMethod]
    public void ChangeRoomCapacity_MustBeChanged()
    {
        var room = new Room("Quarto padrão - 931",931, 15m, 5, "Quarto padrão no terceiro andar", TestParameters.Category);
        room.ChangeCapacity(1);
        Assert.AreEqual(1, room.Capacity);
    }

    [TestMethod]
    public void ChangeRoomStatus_MustBeChanged()
    {
        var room = new Room("Quarto padrão - 931",931, 15m, 5, "Quarto padrão no terceiro andar", TestParameters.Category);
        room.ChangeStatus(Domain.Enums.ERoomStatus.Occupied);
        Assert.AreEqual(Domain.Enums.ERoomStatus.Occupied, room.Status);
    }

    [TestMethod]
    public void AddServiceToRoom_MustBeAdded()
    {
        var room = new Room("Quarto padrão - 931",931, 15m, 5, "Quarto padrão no terceiro andar", TestParameters.Category);
        room.AddService(TestParameters.Service);
        Assert.AreEqual(1, room.Services.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    public void AddSameServiceToRoom_DontAdded()
    {
        var room = new Room("Quarto padrão - 931",931, 15m, 5, "Quarto padrão no terceiro andar", TestParameters.Category);
        room.AddService(TestParameters.Service);
        room.AddService(TestParameters.Service);
        Assert.Fail();
    }

    [TestMethod]
    public void RemoveServiceToRoom_MustBeRemoved()
    {
        var room = new Room("Quarto padrão - 931",931, 15m, 5, "Quarto padrão no terceiro andar", TestParameters.Category);
        room.AddService(TestParameters.Service);
        room.RemoveService(TestParameters.Service);
        Assert.AreEqual(0, room.Services.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    public void RemoveNonExistServiceFromRoom_DoNothing()
    {
        var room = new Room("Quarto padrão - 931",931, 15m, 5, "Quarto padrão no terceiro andar", TestParameters.Category);
        room.RemoveService(TestParameters.Service);
        Assert.AreEqual(0, room.Services.Count);
    }

    [TestMethod]
    public void CreateImageToRoom_MustBeAdded()
    {
        var room = new Room("Quarto padrão - 931",931, 15m, 5, "Quarto padrão no terceiro andar", TestParameters.Category);
        room.CreateImage("url");
        Assert.AreEqual(1, room.Images.Count);
    }

    [TestMethod]
    public void DeleteImageFromRoom_MustBeRemoved()
    {
        var room = new Room("Quarto padrão - 931",931, 15m, 5, "Quarto padrão no terceiro andar", TestParameters.Category);
        var img = room.CreateImage("url");
        room.DeleteImage(img);
        Assert.AreEqual(0, room.Images.Count);
    }

    [TestMethod]
    public void RemoveInexistentImageFromRoom_DoNothing()
    {
        var room = new Room("Quarto padrão - 931",931, 15m, 5, "Quarto padrão no terceiro andar", TestParameters.Category);
        room.DeleteImage(TestParameters.Image);
        Assert.AreEqual(0, room.Images.Count);
    }

}