using Hotel.Domain.Data;
using Hotel.Domain.DTOs.EmployeeContext.ResponsibilityDTOs;
using Hotel.Domain.Entities.EmployeeContext.ResponsibilityEntity;
using Hotel.Domain.Enums;
using Hotel.Tests.IntegrationTests.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Hotel.Tests.IntegrationTests.Controllers;

[TestClass]
public class ResponsibilityControllerTests
{
  private static HotelWebApplicationFactory _factory = null!;
  private static HttpClient _client = null!;
  private static HotelDbContext _dbContext = null!;
  private static string _rootAdminToken = null!; //Allows access all endpoints
  private const string _baseUrl = "v1/responsibilities";

  [ClassInitialize]
  public static void ClassInitialize(TestContext context)
  {
    _factory = new HotelWebApplicationFactory();
    _client = _factory.CreateClient();
    _dbContext = _factory.Services.GetRequiredService<HotelDbContext>();

    _rootAdminToken = _factory.LoginFullAccess().Result;
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _rootAdminToken);
  }

  [ClassCleanup]
  public static void ClassCleanup()
  {
    _factory.Dispose();
  }

  [TestInitialize]
  public void TestInitialize()
  {
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _rootAdminToken);
  }

  [TestMethod]
  public async Task CreateResponsibility_ShouldReturn_OK()
  {
    //Arrange
    var body = new EditorResponsibility("Receptionist", "Atender os hóspedes na recepção e fornecer informações", EPriority.High);

    //Act
    var response = await _client.PostAsJsonAsync(_baseUrl,body);

    var responsibility = await _dbContext.Responsibilities.FirstOrDefaultAsync(x => x.Name.Equals(body.Name));

    //Assert
    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    Assert.AreEqual(body.Name, responsibility!.Name);
    Assert.AreEqual(body.Description, responsibility.Description);
    Assert.AreEqual(body.Priority, responsibility.Priority);
  }


  [TestMethod]
  public async Task GetResponsibilities_ShouldReturn_OK()
  {
    //Arrange
    var responsibility = new Responsibility("Maintenance", "Realizar reparos e manutenção no hotel", EPriority.High);

    await _dbContext.Responsibilities.AddAsync(responsibility);
    await _dbContext.SaveChangesAsync();

    //Act
    var response = await _client.GetAsync($"{_baseUrl}?take=1&name=responsibility");

    //Assert
    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
  }

  [TestMethod]
  public async Task GetResponsibilityById_ShouldReturn_OK()
  {
    //Arrange
    var responsibility = new Responsibility("Housekeeping", "Limpeza e organização dos quartos e áreas comuns", EPriority.Medium);

    await _dbContext.Responsibilities.AddAsync(responsibility);
    await _dbContext.SaveChangesAsync();

    //Act
    var response = await _client.GetAsync($"{_baseUrl}/{responsibility.Id}");

    //Assert
    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
  }

  [TestMethod]
  public async Task UpdateResponsibility_ShouldReturn_OK()
  {    
    //Arrange
    var responsibility = new Responsibility("Chef", "Preparar refeições para os hóspedes", EPriority.High);

    await _dbContext.Responsibilities.AddAsync(responsibility);
    await _dbContext.SaveChangesAsync();

    var body = new EditorResponsibility("Waiter", "Servir alimentos e bebidas no restaurante do hotel", EPriority.Medium);

    //Act
    var response = await _client.PutAsJsonAsync($"{_baseUrl}/{responsibility.Id}", body);

    var updatedResponsibility = await _dbContext.Responsibilities.FirstOrDefaultAsync(x => x.Name.Equals(body.Name));

    //Assert
    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    Assert.AreEqual(body.Name, updatedResponsibility!.Name);
    Assert.AreEqual(body.Description, updatedResponsibility.Description);
    Assert.AreEqual(body.Priority, updatedResponsibility.Priority);
  }

  [TestMethod]
  public async Task DeleteResponsibility_ShouldReturn_OK()
  {
    //Arrange
    var responsibility = new Responsibility("Concierge", "Ajudar os hóspedes com reservas e informações sobre a área local", EPriority.Medium);

    await _dbContext.Responsibilities.AddAsync(responsibility);
    await _dbContext.SaveChangesAsync();

    //Act
    var response = await _client.DeleteAsync($"{_baseUrl}/{responsibility.Id}");

    var exists = await _dbContext.Responsibilities.AnyAsync(x => x.Id == responsibility.Id);

    //Assert
    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    Assert.IsFalse(exists);
  }
}
