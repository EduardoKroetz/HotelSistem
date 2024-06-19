using Hotel.Domain.Data;
using Hotel.Domain.DTOs.ServiceDTOs;
using Hotel.Domain.Entities.ServiceEntity;
using Hotel.Domain.Enums;
using Hotel.Tests.IntegrationTests.Factories;
using Hotel.Tests.IntegrationTests.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;

namespace Hotel.Tests.IntegrationTests.Controllers;

[TestClass]
public class ServiceControllerTests
{
    private static HotelWebApplicationFactory _factory = null!;
    private static HttpClient _client = null!;
    private static HotelDbContext _dbContext = null!;
    private static string _rootAdminToken = null!;
    private const string _baseUrl = "v1/services";

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
    public async Task CreateService_ShouldReturn_OK()
    {
        //Arange
        var body = new EditorService("Room Cleaning", 30.00m, EPriority.Medium, 60);

        //Act
        var response = await _client.PostAsJsonAsync(_baseUrl, body);

        //Assert
        response.EnsureSuccessStatusCode();

        var content = JsonConvert.DeserializeObject<Response<DataId>>(await response.Content.ReadAsStringAsync())!;
        var service = await _dbContext.Services.FirstAsync(x => x.Id == content.Data.Id);

        
        Assert.AreEqual(0, content.Errors.Count);
        Assert.AreEqual("Serviço criado com sucesso!", content.Message);

        Assert.AreEqual(body.Name, service.Name);
        Assert.AreEqual(body.Price, service.Price);
        Assert.AreEqual(body.Priority, service.Priority);
        Assert.AreEqual(body.TimeInMinutes, service.TimeInMinutes);
        Assert.IsTrue(service.IsActive);
    }

    [TestMethod]
    public async Task CreateService_WithNameAlreadyExists_ShouldReturn_BAD_REQUEST()
    {
        //Arange
        var factory = new HotelWebApplicationFactory();
        var client = factory.CreateClient();
        var dbContext = factory.Services.GetRequiredService<HotelDbContext>();
        factory.Login(client, _rootAdminToken);

        var seed = new Service("Breakfast Delivery", 20.00m, EPriority.High, 30);
        await dbContext.Services.AddAsync(seed);
        await dbContext.SaveChangesAsync();

        var body = new EditorService("Breakfast Delivery", 30.00m, EPriority.High, 30);
        //Act
        var response = await client.PostAsJsonAsync(_baseUrl, body);

        //Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;

       
        Assert.IsTrue(content!.Errors.Any(x => x.Equals("Esse nome já está cadastrado.")));
    }

    [TestMethod]
    public async Task UpdateService_ShouldReturn_OK()
    {
        //Arange
        var seed = new Service("Laundry Service", 25.00m, EPriority.Medium, 120);
        await _dbContext.Services.AddAsync(seed);
        await _dbContext.SaveChangesAsync();

        var body = new EditorService("Laundry Service", 27.01m, EPriority.Low, 50);

        //Act
        var response = await _client.PutAsJsonAsync($"{_baseUrl}/{seed.Id}", body);

        //Assert
        response.EnsureSuccessStatusCode();

        var content = JsonConvert.DeserializeObject<Response<DataId>>(await response.Content.ReadAsStringAsync())!;
        var service = await _dbContext.Services.FirstAsync(x => x.Id == content.Data.Id);

        
        Assert.AreEqual(0, content.Errors.Count);
        Assert.AreEqual("Serviço atualizado com sucesso!", content.Message);

        Assert.AreEqual(body.Name, service.Name);
        Assert.AreEqual(body.Price, service.Price);
        Assert.AreEqual(body.Priority, service.Priority);
        Assert.AreEqual(body.TimeInMinutes, service.TimeInMinutes);
        Assert.IsTrue(service.IsActive);
    }

    [TestMethod]
    public async Task UpdateNonexistService_ShouldReturn_NOT_FOUND()
    {
        //Arange
        var body = new EditorService("Laundry Service", 27.01m, EPriority.Low, 50);

        //Act
        var response = await _client.PutAsJsonAsync($"{_baseUrl}/{Guid.NewGuid()}", body);

        //Assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;

       
        Assert.IsTrue(content!.Errors.Any(x => x.Equals("Serviço não encontrado.")));
    }

    [TestMethod]
    public async Task UpdateService_WithNameAlreadyExists_ShouldReturn_BAD_REQUEST()
    {
        //Arange
        var factory = new HotelWebApplicationFactory();
        var client = factory.CreateClient();
        var dbContext = factory.Services.GetRequiredService<HotelDbContext>();
        factory.Login(client, _rootAdminToken);

        var updateSeed = new Service("Medical Assistance", 50.00m, EPriority.High, 30);
        var seed = new Service("Cooking Class", 55.00m, EPriority.Medium, 90);
        await dbContext.Services.AddRangeAsync([updateSeed, seed]);
        await dbContext.SaveChangesAsync();

        var body = new EditorService(seed.Name, 30.00m, EPriority.High, 30);
        //Act
        var response = await client.PutAsJsonAsync($"{_baseUrl}/{updateSeed.Id}", body);

        //Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;

       
        Assert.IsTrue(content!.Errors.Any(x => x.Equals("Esse nome já está cadastrado.")));
    }

    [TestMethod]
    public async Task DeleteService_ShouldReturn_OK()
    {
        //Arange
        var seed = new Service("Party Planning", 200.00m, EPriority.Medium, 180);
        await _dbContext.Services.AddAsync(seed);
        await _dbContext.SaveChangesAsync();

        //Act
        var response = await _client.DeleteAsync($"{_baseUrl}/{seed.Id}");

        //Assert
        response.EnsureSuccessStatusCode();

        var content = JsonConvert.DeserializeObject<Response<DataId>>(await response.Content.ReadAsStringAsync())!;
        var exists = await _dbContext.Services.AnyAsync(x => x.Id == content.Data.Id);

        
        Assert.AreEqual(0, content.Errors.Count);
        Assert.AreEqual("Serviço deletado com sucesso!", content.Message);

        Assert.IsFalse(exists);
    }

    [TestMethod]
    public async Task DeleteNonexistService_ShouldReturn_NOT_FOUND()
    {
        //Act
        var response = await _client.DeleteAsync($"{_baseUrl}/{Guid.NewGuid()}");

        //Assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;

       
        Assert.IsTrue(content!.Errors.Any(x => x.Equals("Serviço não encontrado.")));
    }

    [TestMethod]
    public async Task GetServices_ShouldReturn_OK()
    {
        //Assert
        var seed = new Service("Fishing Trip", 120.00m, EPriority.Low, 180);
        await _dbContext.Services.AddAsync(seed);
        await _dbContext.SaveChangesAsync();

        //Act
        var response = await _client.GetAsync(_baseUrl);

        //Assert
        response.EnsureSuccessStatusCode();

        var content = JsonConvert.DeserializeObject<Response<List<GetService>>>(await response.Content.ReadAsStringAsync())!;

        
        Assert.AreEqual(0, content.Errors.Count);
        Assert.AreEqual("Sucesso!", content.Message);

        Assert.IsTrue(content.Data.Count > 0);
        foreach (var service in content.Data)
        {
            Assert.IsNotNull(service.Id);
            Assert.IsNotNull(service.Name);
            Assert.IsNotNull(service.Price);
            Assert.IsNotNull(service.Priority);
            Assert.IsNotNull(service.CreatedAt);
            Assert.IsNotNull(service.TimeInMinutes);
            Assert.IsNotNull(service.IsActive);
        }
    }

    [TestMethod]
    public async Task GetServiceById_ShouldReturn_OK()
    {
        //Assert
        var service = new Service("Premium Wifi", 10.00m, EPriority.Medium, 9999);
        await _dbContext.Services.AddAsync(service);
        await _dbContext.SaveChangesAsync();

        //Act
        var response = await _client.GetAsync($"{_baseUrl}/{service.Id}");

        //Assert
        response.EnsureSuccessStatusCode();

        var content = JsonConvert.DeserializeObject<Response<GetService>>(await response.Content.ReadAsStringAsync())!;

        
        Assert.AreEqual(0, content.Errors.Count);
        Assert.AreEqual("Sucesso!", content.Message);

        Assert.AreEqual(service.Id, content.Data.Id);
        Assert.AreEqual(service.Name, content.Data.Name);
        Assert.AreEqual(service.Price, content.Data.Price);
        Assert.AreEqual(service.Priority, content.Data.Priority);
        Assert.AreEqual(service.CreatedAt, content.Data.CreatedAt);
        Assert.AreEqual(service.TimeInMinutes, content.Data.TimeInMinutes);
        Assert.AreEqual(service.IsActive, content.Data.IsActive);
    }

    [TestMethod]
    public async Task GetNonexistServiceById_ShouldReturn_NOT_FOUND()
    {
        //Act
        var response = await _client.GetAsync($"{_baseUrl}/{Guid.NewGuid()}");

        //Assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync())!;

       
        Assert.IsTrue(content!.Errors.Any(x => x.Equals("Serviço não encontrado.")));
    }


}
