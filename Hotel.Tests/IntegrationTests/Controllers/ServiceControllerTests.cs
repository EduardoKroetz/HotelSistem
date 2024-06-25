using Hotel.Domain.Data;
using Hotel.Domain.DTOs.ServiceDTOs;
using Hotel.Domain.Entities.ServiceEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Services.Interfaces;
using Hotel.Tests.IntegrationTests.Factories;
using Hotel.Tests.IntegrationTests.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Stripe;
using System.Net;
using System.Net.Http.Json;

namespace Hotel.Tests.IntegrationTests.Controllers;

[TestClass]
public class ServiceControllerTests
{
    private static HotelWebApplicationFactory _factory = null!;
    private static HttpClient _client = null!;
    private static HotelDbContext _dbContext = null!;
    private static IStripeService _stripeService = null!;
    private static string _rootAdminToken = null!;
    private const string _baseUrl = "v1/services";
    private static TestService _testService = null!;

    [ClassInitialize]
    public static void ClassInitialize(TestContext? context)
    {
        _factory = new HotelWebApplicationFactory();
        _client = _factory.CreateClient();
        _dbContext = _factory.Services.GetRequiredService<HotelDbContext>();
        _stripeService = _factory.Services.GetRequiredService<IStripeService>();

        _rootAdminToken = _factory.LoginFullAccess().Result;
        _factory.Login(_client, _rootAdminToken);
        _testService = new TestService(_dbContext, _factory, _client, _rootAdminToken);
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
        var body = new EditorService("Room Cleaning", "Room Cleaning", 30.00m, EPriority.Medium, 60);

        //Act
        var response = await _client.PostAsJsonAsync(_baseUrl, body);

        //Assert
        response.EnsureSuccessStatusCode();

        var content = JsonConvert.DeserializeObject<Response<DataStripeProductId>>(await response.Content.ReadAsStringAsync())!;
        var service = await _dbContext.Services.FirstAsync(x => x.Id == content.Data.Id);

        
        Assert.AreEqual(0, content.Errors.Count);
        Assert.AreEqual("Serviço criado com sucesso!", content.Message);

        Assert.AreEqual(body.Name, service.Name);
        Assert.AreEqual(body.Price, service.Price);
        Assert.AreEqual(body.Priority, service.Priority);
        Assert.AreEqual(body.TimeInMinutes, service.TimeInMinutes);
        Assert.IsTrue(service.IsActive);

        var product = await _stripeService.GetProductAsync(service.StripeProductId);

        Assert.AreEqual(body.Name, product.Name);
        Assert.AreEqual(body.Description, product.Description);

        var price = await _stripeService.GetFirstActivePriceByProductId(product.Id);

        Assert.AreEqual(body.Price * 100, price.UnitAmountDecimal);
    }

    [TestMethod]
    public async Task CreateService_WithNameAlreadyExists_ShouldReturn_BAD_REQUEST()
    {
        //Arange
        var factory = new HotelWebApplicationFactory();
        var client = factory.CreateClient();
        var dbContext = factory.Services.GetRequiredService<HotelDbContext>();
        factory.Login(client, _rootAdminToken);

        var seed = new Service("Breakfast Delivery","Breakfast Delivery", 20.00m, EPriority.High, 30);
        await dbContext.Services.AddAsync(seed);
        await dbContext.SaveChangesAsync();

        var body = new EditorService("Breakfast Delivery","Breakfast Delivery", 30.00m, EPriority.High, 30);
        //Act
        var response = await client.PostAsJsonAsync(_baseUrl, body);

        //Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        var content = await _testService.DeserializeResponse<object>(response);

        Assert.AreEqual("Esse nome já está cadastrado.",content.Errors[0]);

        var services = await dbContext.Services.Where(x => x.Name == body.Name).ToListAsync();

        Assert.AreEqual(1, services.Count);
    }


    [TestMethod]
    public async Task CreateService_WithInvalidStripeParameters_ShouldReturn_BAD_REQUEST_and_DONT_CREATE()
    {
        //Arange
        var body = new EditorService("Serviço 123[}]adaopo3I#489kxhjakxaxçalz34-[[alxkaxlçaçlaooi2iaieajKLDHajkxhakhkjcAHKfhaKEgkgfiuAJKLxbnakbskvgaodhiavghddljk.fgieryfygahkhvgsukoldfgyifgafagsdukasgcjhabchgfiuasgdkagcagiatedkufaxgcysdgfyiagdahjgcvayugfyegfajhgcydfgiyegjahgvruifgeiuryfaweuktyeyedyaridijrietzjrietzjkrieedyardiojrietzeduardokroetyz", "Serviço 123", 259999999999999999999999999.00m, EPriority.Medium, 120);

        //Act
        var response = await _client.PostAsJsonAsync(_baseUrl, body);

        //Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        var content = await _testService.DeserializeResponse<object>(response);

        Assert.AreEqual("Um erro ocorreu ao criar o serviço no stripe", content.Errors[0]);

        var serviceHasCreated = await _dbContext.Services.AnyAsync(x => x.Name == body.Name);
        Assert.IsFalse(serviceHasCreated);

        //Check if product has created on Stripe
        var productListOptions = new ProductListOptions
        {
            Created = new DateRangeOptions
            {
                GreaterThanOrEqual = DateTime.UtcNow.Date,
                LessThan = DateTime.UtcNow.Date.AddDays(1)
            }
        };

        var productService = new ProductService();
        var products = await productService.ListAsync(productListOptions);

        var productHasCreated = products.Any(x => x.Name == body.Name && x.Description == body.Description);

        Assert.IsFalse(productHasCreated);
    }


    [TestMethod]
    public async Task UpdateService_ShouldReturn_OK()
    {
        //Arange
        var newService = new EditorService("Laundry Service", "Laundry Service", 25.00m, EPriority.Medium, 120);
        var createServiceResponse = await _client.PostAsJsonAsync(_baseUrl, newService);
        var serviceId = JsonConvert.DeserializeObject<Response<DataId>>(await createServiceResponse.Content.ReadAsStringAsync())!.Data.Id;
        var service = await _dbContext.Services.FirstAsync(x => x.Id == serviceId);

        var body = new EditorService("Laundry Service", "Laundry", 27.01m, EPriority.Low, 50);

        //Act
        var response = await _client.PutAsJsonAsync($"{_baseUrl}/{serviceId}", body);

        //Assert
        response.EnsureSuccessStatusCode();

        var content = JsonConvert.DeserializeObject<Response<DataId>>(await response.Content.ReadAsStringAsync())!;
        var updatedService = await _dbContext.Services.FirstAsync(x => x.Id == content.Data.Id);

        
        Assert.AreEqual(0, content.Errors.Count);
        Assert.AreEqual("Serviço atualizado com sucesso!", content.Message);

        Assert.AreEqual(body.Name, updatedService.Name);
        Assert.AreEqual(body.Description, updatedService.Description);
        Assert.AreEqual(body.Price, updatedService.Price);
        Assert.AreEqual(body.Priority, updatedService.Priority);
        Assert.AreEqual(body.TimeInMinutes, updatedService.TimeInMinutes);
        Assert.IsTrue(updatedService.IsActive);
    }


    [TestMethod]
    public async Task UpdateService_WithInvalidStripeId_ShouldReturn_BAD_REQUEST_and_DONT_UPDATE()
    {
        //Arange
        var newService = new Service("Serviço 9213", "Serviço 12345", 259.00m, EPriority.Medium, 120);
        await _dbContext.Services.AddAsync(newService);
        await _dbContext.SaveChangesAsync();


        var body = new EditorService("Serviço 921", "Serviço 921", 239.00m, EPriority.High, 140);
        //Act
        var response = await _client.PutAsJsonAsync($"{_baseUrl}/{newService.Id}", body);

        //Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        var content = await _testService.DeserializeResponse<object>(response);

        Assert.AreEqual("Ocorreu um erro ao atualizar o produto no Stripe", content.Errors[0]);
    }

    [TestMethod]
    public async Task UpdateNonexistService_ShouldReturn_NOT_FOUND()
    {
        //Arange
        var body = new EditorService("Laundry Service", "Laundry Service", 27.01m, EPriority.Low, 50);

        //Act
        var response = await _client.PutAsJsonAsync($"{_baseUrl}/{Guid.NewGuid()}", body);

        //Assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

        var content = await _testService.DeserializeResponse<object>(response);

       
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

        var guidName = Guid.NewGuid().ToString();
        var updateSeed = new Service(guidName,"Medical Assistance", 50.00m, EPriority.High, 30);
        await dbContext.Services.AddAsync(updateSeed);
        await dbContext.SaveChangesAsync();

        var seed = new Service("Cooking Class", "Cooking Class", 55.00m, EPriority.Medium, 90);
        var createServiceResponse = await client.PostAsJsonAsync(_baseUrl, seed);
        var serviceId = JsonConvert.DeserializeObject<Response<DataId>>(await createServiceResponse.Content.ReadAsStringAsync())!.Data.Id;
        var service = await dbContext.Services.FirstAsync(x => x.Id == serviceId);


        var body = new EditorService(guidName,"Cooking", 30.00m, EPriority.High, 30);
        //Act
        var response = await client.PutAsJsonAsync($"{_baseUrl}/{service.Id}", body);

        //Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        var content = await _testService.DeserializeResponse<object>(response);

       
        Assert.AreEqual("Esse nome já está cadastrado.",content!.Errors[0]);

        //Check if product has updated on Stripe
        var productListOptions = new ProductListOptions
        {
            Created = new DateRangeOptions
            {
                GreaterThanOrEqual = DateTime.UtcNow.Date,
                LessThan = DateTime.UtcNow.Date.AddDays(1)
            }
        };

        var productService = new ProductService();
        var products = await productService.ListAsync(productListOptions);

        var productHasUpdate = products.Any(x => x.Name == guidName);

        Assert.IsFalse(productHasUpdate);
    }

    [TestMethod]
    public async Task DeleteService_ShouldReturn_OK()
    {
        //Arange
        var body = new EditorService("Party Planning","Party Planning", 200.00m, EPriority.Medium, 180);

        var createServiceResponse = await _client.PostAsJsonAsync(_baseUrl,body);
        var serviceId = JsonConvert.DeserializeObject<Response<DataId>>(await createServiceResponse.Content.ReadAsStringAsync())!.Data.Id;
        var serivce = await _dbContext.Services.FirstAsync(x => x.Id == serviceId);

        //Act
        var response = await _client.DeleteAsync($"{_baseUrl}/{serviceId}");

        //Assert
        response.EnsureSuccessStatusCode();

        var content = JsonConvert.DeserializeObject<Response<DataId>>(await response.Content.ReadAsStringAsync())!;
        var exists = await _dbContext.Services.AnyAsync(x => x.Id == content.Data.Id);
        Assert.IsFalse(exists);

        Assert.AreEqual(0, content.Errors.Count);
        Assert.AreEqual("Serviço deletado com sucesso!", content.Message);

        //Check if the service has disabled on Stripe
 
        var productService = new ProductService();
        var product = await productService.GetAsync(serivce.StripeProductId);

        Assert.IsFalse(product.Active);

    }

    [TestMethod]
    public async Task DeleteNonexistService_ShouldReturn_NOT_FOUND()
    {
        //Act
        var response = await _client.DeleteAsync($"{_baseUrl}/{Guid.NewGuid()}");

        //Assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

        var content = await _testService.DeserializeResponse<object>(response);

       
        Assert.IsTrue(content!.Errors.Any(x => x.Equals("Serviço não encontrado.")));
    }

    [TestMethod]
    public async Task DeleteService_WithOutStripeId_ShouldReturn_BAD_REQUEST_and_DONT_DELETE()
    {
        //Arange

        var service = new Service("service 999", "service999", 999, EPriority.Critical, 999);
        await _dbContext.Services.AddAsync(service);
        await _dbContext.SaveChangesAsync();

        //Act
        var response = await _client.DeleteAsync($"{_baseUrl}/{service.Id}");

        //Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        var content = await _testService.DeserializeResponse<object>(response);

        Assert.AreEqual("Ocorreu um erro ao desativar o produto no Stripe", content.Errors[0]);

        var serviceExists = await _dbContext.Services.AnyAsync(x => x.Id == service.Id);
        Assert.IsTrue(serviceExists);
    }



    [TestMethod]
    public async Task GetServices_ShouldReturn_OK()
    {
        //Assert
        var seed = new Service("Fishing Trip","Fishing Trip", 120.00m, EPriority.Low, 180);
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
        var service = new Service("Premium Wifi","Premium Wifi", 10.00m, EPriority.Medium, 9999);
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

        var content = await _testService.DeserializeResponse<object>(response);

       
        Assert.IsTrue(content!.Errors.Any(x => x.Equals("Serviço não encontrado.")));
    }


}
