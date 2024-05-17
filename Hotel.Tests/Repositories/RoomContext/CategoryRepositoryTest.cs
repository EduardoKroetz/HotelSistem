﻿using Hotel.Domain.DTOs.RoomContext.CategoryDTOs;
using Hotel.Domain.Repositories.RoomContext;
using Hotel.Tests.Repositories.Mock;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Tests.Repositories.RoomContext;

[TestClass]
public class CategoryRepositoryTest
{
  private static CategoryRepository CategoryRepository { get; set; } 

  static CategoryRepositoryTest()
  => CategoryRepository = new CategoryRepository(BaseRepositoryTest.MockConnection.Context);
  
  [TestMethod]
  public async Task GetByIdAsync_ReturnsWithCorrectParameters()
  {
    var category = await CategoryRepository.GetByIdAsync(BaseRepositoryTest.Categories[0].Id);

    Assert.IsNotNull(category);
    Assert.AreEqual(BaseRepositoryTest.Categories[0].Id, category.Id);
    Assert.AreEqual(BaseRepositoryTest.Categories[0].Name, category.Name);
    Assert.AreEqual(BaseRepositoryTest.Categories[0].Description, category.Description);
    Assert.AreEqual(BaseRepositoryTest.Categories[0].AveragePrice, category.AveragePrice);
  }

  [TestMethod]
  public async Task GetAsync_ReturnWithCorrectParameters()
  {
    var parameters = new CategoryQueryParameters(0, 1, BaseRepositoryTest.Categories[0].Name,null,null,null);
    var categories = await CategoryRepository.GetAsync(parameters);

    var category = categories.ToList()[0];

    Assert.IsNotNull(category);
    Assert.AreEqual(BaseRepositoryTest.Categories[0].Id, category.Id);
    Assert.AreEqual(BaseRepositoryTest.Categories[0].Name, category.Name);
    Assert.AreEqual(BaseRepositoryTest.Categories[0].Description, category.Description);
    Assert.AreEqual(BaseRepositoryTest.Categories[0].AveragePrice, category.AveragePrice);
  }

  [TestMethod]
  public async Task GetAsync_WhereAveragePriceGratherThan10_ReturnsCategories()
  {
    var parameters = new CategoryQueryParameters(0, 1, null, 10, "gt", null);
    var categories = await CategoryRepository.GetAsync(parameters);

    Assert.IsTrue(categories.Any());

    foreach (var category in categories)
      Assert.IsTrue(10 < category.AveragePrice);
  }

  [TestMethod]
  public async Task GetAsync_WhereAveragePriceLessThan50_ReturnsCategories()
  {
    var parameters = new CategoryQueryParameters(0, 1, null, 50,"lt",null);
    var categories = await CategoryRepository.GetAsync(parameters);

    Assert.IsTrue(categories.Any());

    foreach (var category in categories)
      Assert.IsTrue(50 > category.AveragePrice);
  }

  [TestMethod]
  public async Task GetAsync_WhereAveragePriceEquals_ReturnsCategories()
  {
    var parameters = new CategoryQueryParameters(0, 1, null,BaseRepositoryTest.Categories[0].AveragePrice, "eq", null);
    var categories = await CategoryRepository.GetAsync(parameters);

    Assert.IsTrue(categories.Any());

    foreach (var category in categories)
      Assert.AreEqual(BaseRepositoryTest.Categories[0].AveragePrice, category.AveragePrice);
  }


  [TestMethod]
  public async Task GetAsync_WhereAdminId_ReturnsCategories()
  {
    var parameters = new CategoryQueryParameters(0, 1, null, null, null, BaseRepositoryTest.Rooms[0].Id);
    var categories = await CategoryRepository.GetAsync(parameters);


    Assert.IsTrue(categories.Any());
    foreach (var category in categories)
    {
      var hasCategory = await BaseRepositoryTest.MockConnection.Context.Categories
        .Where(x => x.Id == category.Id)
        .SelectMany(x => x.Rooms)
        .AnyAsync(x => x.Id == BaseRepositoryTest.Rooms[0].Id);

      Assert.IsTrue(hasCategory);
    }
  }
}
