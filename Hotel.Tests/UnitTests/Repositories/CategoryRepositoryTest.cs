using Hotel.Domain.Data;
using Hotel.Domain.DTOs.CategoryDTOs;
using Hotel.Domain.Repositories;
using Hotel.Tests.UnitTests.Repositories.InMemoryDatabase.Utils;
using Hotel.Tests.UnitTests.Repositories.InMemoryDatabase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Hotel.Domain.Entities.CategoryEntity;
using Hotel.Domain.Entities.RoomEntity;

namespace Hotel.Tests.UnitTests.Repositories;

[TestClass]
public class CategoryRepositoryTest
{
    private readonly CategoryRepository _categoryRepository;
    private readonly HotelDbContext _dbContext;
    private readonly RepositoryTestUtils _utils;
    private static readonly AsyncLocal<IDbContextTransaction?> _currentTransaction = new AsyncLocal<IDbContextTransaction?>();

    public CategoryRepositoryTest()
    {
        var sqliteDatabase = new SqliteDatabase();
        _dbContext = sqliteDatabase.Context;
        _categoryRepository = new CategoryRepository(_dbContext);
        _utils = new RepositoryTestUtils(_dbContext);
    }

    [TestInitialize]
    public async Task Initialize()
    {
        _currentTransaction.Value = await _dbContext.Database.BeginTransactionAsync();
    }

    [TestCleanup]
    public async Task Cleanup()
    {
        if (_currentTransaction.Value != null)
        {
            await _currentTransaction.Value.RollbackAsync();
            await _currentTransaction.Value.DisposeAsync();
            _currentTransaction.Value = null;
        }
    }

    [TestMethod]
    public async Task GetByIdAsync_ReturnsWithCorrectParameters()
    {
        //Arrange
        var newCategory = await _utils.CreateCategoryAsync(new Category("Luxos", "Luxos", 190));

        //Act
        var category = await _categoryRepository.GetByIdAsync(newCategory.Id);

        //Assert
        Assert.IsNotNull(category);
        Assert.AreEqual(newCategory.Id, category.Id);
        Assert.AreEqual(newCategory.Name, category.Name);
        Assert.AreEqual(newCategory.Description, category.Description);
        Assert.AreEqual(newCategory.AveragePrice, category.AveragePrice);
    }

    [TestMethod]
    public async Task GetAsync_ReturnWithCorrectParameters()
    {
        //Arrange
        var newCategory = await _utils.CreateCategoryAsync(new Category("Luxos", "Luxos", 190));

        //Act
        var parameters = new CategoryQueryParameters(0, 1, newCategory.Name, null, null, null);
        var categories = await _categoryRepository.GetAsync(parameters);

        //Assert
        var category = categories.ToList()[0];

        Assert.IsNotNull(category);
        Assert.AreEqual(newCategory.Id, category.Id);
        Assert.AreEqual(newCategory.Name, category.Name);
        Assert.AreEqual(newCategory.Description, category.Description);
        Assert.AreEqual(newCategory.AveragePrice, category.AveragePrice);
    }

    [TestMethod]
    public async Task GetAsync_WhereAveragePriceGratherThan10_ReturnsCategories()
    {
        //Arrange
        var newCategory = await _utils.CreateCategoryAsync(new Category("Luxos", "Luxos", 190));

        //Act
        var parameters = new CategoryQueryParameters(0, 1, null, 10, "gt", null);
        var categories = await _categoryRepository.GetAsync(parameters);

        //Assert
        Assert.IsTrue(categories.Any());

        foreach (var category in categories)
            Assert.IsTrue(10 < category.AveragePrice);
    }

    [TestMethod]
    public async Task GetAsync_WhereAveragePriceLessThan50_ReturnsCategories()
    {
        //Arrange
        var newCategory = await _utils.CreateCategoryAsync(new Category("Basic", "Basic", 49.9m));

        //Act
        var parameters = new CategoryQueryParameters(0, 1, null, 50, "lt", null);
        var categories = await _categoryRepository.GetAsync(parameters);

        //Assert
        Assert.IsTrue(categories.Any());

        foreach (var category in categories)
            Assert.IsTrue(50 > category.AveragePrice);
    }

    [TestMethod]
    public async Task GetAsync_WhereAveragePriceEquals_ReturnsCategories()
    {
        //Arrange
        var newCategory = await _utils.CreateCategoryAsync(new Category("Luxos", "Luxos", 190));

        //Act
        var parameters = new CategoryQueryParameters(0, 1, null, newCategory.AveragePrice, "eq", null);
        var categories = await _categoryRepository.GetAsync(parameters);

        //Assert
        Assert.IsTrue(categories.Any());

        foreach (var category in categories)
            Assert.AreEqual(newCategory.AveragePrice, category.AveragePrice);
    }

    [TestMethod]
    public async Task GetAsync_WhereAdminId_ReturnsCategories()
    {
        //Arrange
        var newCategory = await _utils.CreateCategoryAsync(new Category("Luxos", "Luxos", 190));
        var newRoom = await _utils.CreateRoomAsync(new Room("Deluxe room", 30, 78.1m, 8, "the Deluxe room is a...", newCategory));

        //Act
        var parameters = new CategoryQueryParameters(0, 1, null, null, null, newRoom.Id);
        var categories = await _categoryRepository.GetAsync(parameters);

        //Assert
        Assert.IsTrue(categories.Any());
        foreach (var category in categories)
        {
            var hasCategory = await _dbContext.Categories
              .Where(x => x.Id == category.Id)
              .SelectMany(x => x.Rooms)
              .AnyAsync(x => x.Id == newRoom.Id);

            Assert.IsTrue(hasCategory);
        }
    }
}
