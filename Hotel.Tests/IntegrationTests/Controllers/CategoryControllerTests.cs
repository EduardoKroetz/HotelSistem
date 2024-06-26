using Hotel.Domain.Data;
using Hotel.Domain.DTOs.CategoryDTOs;
using Hotel.Domain.Entities.CategoryEntity;
using Hotel.Tests.IntegrationTests.Factories;
using Hotel.Tests.IntegrationTests.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;

namespace Hotel.Tests.IntegrationTests.Controllers;

[TestClass]
public class CategoryControllerTests
{
    private static HotelWebApplicationFactory _factory = null!;
    private static HttpClient _client = null!;
    private static HotelDbContext _dbContext = null!;
    private static string _rootAdminToken = null!;
    private const string _baseUrl = "v1/categories";

    [ClassInitialize]
    public static void ClassInitialize(TestContext? context)
    {
        _factory = new HotelWebApplicationFactory();
        _client = _factory.CreateClient();
        _dbContext = _factory.Services.GetRequiredService<HotelDbContext>();

        _rootAdminToken = _factory.LoginFullAccess().Result;
        _factory.Login(_client, _rootAdminToken);
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        _factory.Dispose();
    }

    [TestInitialize]
    public void TestInitialize()
    {
        _factory.Login(_client, _rootAdminToken);
    }

    [TestMethod]
    public async Task CreateCategory_ShouldReturn_OK()
    {
        //Arange
        var body = new EditorCategory("Quartos de luxo", "Essa categoria abrange os mais variados quartos de luxo...", 190);

        //Act
        var response = await _client.PostAsJsonAsync(_baseUrl, body);

        //Assert

        var content = JsonConvert.DeserializeObject<Response<DataId>>(await response.Content.ReadAsStringAsync())!;
        var category = await _dbContext.Categories.FirstAsync(x => x.Id == content!.Data.Id);

        Assert.IsNotNull(response);
        response.EnsureSuccessStatusCode();
  
        Assert.AreEqual("Categoria criada com sucesso!", content.Message);
        Assert.AreEqual(0, content.Errors.Count);

        Assert.AreEqual(body.Name, category.Name);
        Assert.AreEqual(body.Description, category.Description);
        Assert.AreEqual(body.AveragePrice, category.AveragePrice);
    }

    [TestMethod]
    public async Task UpdateCategory_ShouldReturn_OK()
    {
        //Arange
        var category = new Category("Padrão", "Essa categoria abrange os mais variados quartos de padrões...", 70);
        await _dbContext.Categories.AddAsync(category);
        await _dbContext.SaveChangesAsync();

        var body = new EditorCategory("Standard", "Essa categoria abrange os mais variados quartos com nível de conforto padrão...", 60);

        //Act
        var response = await _client.PutAsJsonAsync($"{_baseUrl}/{category.Id}", body);

        //Assert

        var content = JsonConvert.DeserializeObject<Response<DataId>>(await response.Content.ReadAsStringAsync())!;
        var updatedCategory = await _dbContext.Categories.FirstAsync(x => x.Id == content!.Data.Id);

        Assert.IsNotNull(response);
        response.EnsureSuccessStatusCode();


        
        Assert.AreEqual("Categoria atualizada com sucesso!", content.Message);
        Assert.AreEqual(0, content.Errors.Count);

        Assert.AreEqual(body.Name, updatedCategory.Name);
        Assert.AreEqual(body.Description, updatedCategory.Description);
        Assert.AreEqual(body.AveragePrice, updatedCategory.AveragePrice);
    }

    [TestMethod]
    public async Task DeleteCategory_ShouldReturn_OK()
    {
        //Arange
        var category = new Category("Basic", "Essa categoria abrange os mais variados quartos básicos...", 40);
        await _dbContext.Categories.AddAsync(category);
        await _dbContext.SaveChangesAsync();

        //Act
        var response = await _client.DeleteAsync($"{_baseUrl}/{category.Id}");

        //Assert
        var content = JsonConvert.DeserializeObject<Response<DataId>>(await response.Content.ReadAsStringAsync())!;
        var exists = await _dbContext.Categories.AnyAsync(x => x.Id == content!.Data.Id);

        Assert.IsNotNull(response);
        response.EnsureSuccessStatusCode();

        
        Assert.AreEqual("Categoria deletada com sucesso!", content.Message);
        Assert.AreEqual(0, content.Errors.Count);

        Assert.IsFalse(exists);
    }

    [TestMethod]
    public async Task GetCategories_ShouldReturn_OK()
    {
        //Arange
        var category = new Category("Premium", "Categoria de quartos nível premium", 350);
        await _dbContext.Categories.AddAsync(category);
        await _dbContext.SaveChangesAsync();

        //Act
        var response = await _client.GetAsync(_baseUrl);

        //Assert
        var content = JsonConvert.DeserializeObject<Response<List<GetCategory>>>(await response.Content.ReadAsStringAsync())!;

        Assert.IsNotNull(response);
        response.EnsureSuccessStatusCode();

        
        Assert.AreEqual("Sucesso!", content.Message);
        Assert.AreEqual(0, content.Errors.Count);

        foreach (var categ in content.Data)
        {
            Assert.IsNotNull(categ.Id);
            Assert.IsNotNull(categ.Name);
            Assert.IsNotNull(categ.Description);
            Assert.IsNotNull(categ.AveragePrice);
        }

    }

    [TestMethod]
    public async Task GetCategoryById_ShouldReturn_OK()
    {
        //Arange
        var category = new Category("Deluxe", "Categoria de quartos nível deluxe", 270);
        await _dbContext.Categories.AddAsync(category);
        await _dbContext.SaveChangesAsync();

        //Act
        var response = await _client.GetAsync($"{_baseUrl}/{category.Id}");

        //Assert
        var content = JsonConvert.DeserializeObject<Response<GetCategory>>(await response.Content.ReadAsStringAsync())!;

        Assert.IsNotNull(response);
        response.EnsureSuccessStatusCode();

        
        Assert.AreEqual("Sucesso!", content.Message);
        Assert.AreEqual(0, content.Errors.Count);

        Assert.AreEqual(category.Id, content.Data.Id);
        Assert.AreEqual(category.Name, content.Data.Name);
        Assert.AreEqual(category.Description, content.Data.Description);
        Assert.AreEqual(category.AveragePrice, content.Data.AveragePrice);
    }

}
