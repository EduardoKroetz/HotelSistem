using Hotel.Domain.Entities.CategoryEntity;
using Hotel.Domain.Exceptions;

namespace Hotel.Tests.UnitTests.Entities;

[TestClass]
public class CategoryEntityTest
{
    [TestMethod]
    public void NewCategoryInstance_MustBeValid()
    {
        var category = new Category("Categoria", "Categoria", 1);

        Assert.IsTrue(category.IsValid);
        Assert.AreEqual("Categoria",category.Name);
        Assert.AreEqual("Categoria", category.Description);
        Assert.AreEqual(1, category.AveragePrice);
    }

    [TestMethod]
    [DataRow("", "", -1, "Informe o nome da categoria")]
    [DataRow("", "Categoria", 1, "Informe o nome da categoria")]
    public void InvalidCategoryParameters_ShouldThrowException(string name, string description, int averagePrice, string errorMessage)
    {
        var exception = Assert.ThrowsException<ValidationException>(() => new Category(name, description, averagePrice));
        Assert.AreEqual(errorMessage, exception.Message);
    }
}