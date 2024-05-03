using Hotel.Domain.Entities.RoomContext.CategoryEntity;
using Hotel.Domain.Exceptions;

namespace Hotel.Tests.Entities.RoomContext;

[TestClass]
public class CategoryEntityTest
{
  [TestMethod]
  public void ValidCategory_MustBeValid()
  {
    var category = new Category("Categoria","Categoria",1);
    Assert.AreEqual(true,category.IsValid);
  }

  [TestMethod]
  [ExpectedException(typeof(ValidationException))]
  [DataRow("","",-1)]
  [DataRow("Categoria","",1)]
  [DataRow("","Categoria",1)]
  [DataRow("Categoria","Categoria",-1)]
  public void InvalidCategoryParameters_ExpectedException(string name,string description,int averagePrice)
  {
    var category = new Category(name,description,averagePrice);
    Assert.AreEqual(true,category.IsValid);
  }
}