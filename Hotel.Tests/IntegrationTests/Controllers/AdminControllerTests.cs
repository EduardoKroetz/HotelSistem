using Hotel.Domain.Entities.AdminContext.AdminEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.ValueObjects;
using Hotel.Tests.IntegrationTests.Factories;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Headers;

namespace Hotel.Tests.IntegrationTests.Controllers;

[TestClass]
public class AdminControllerTests
{
  private readonly HotelWebApplicationFactory _factory;
  private const string _baseUrl = "v1/admins";
  private readonly HttpClient _client;

  public AdminControllerTests()
  {
    _factory = new HotelWebApplicationFactory();
    _client = _factory.CreateClient();
    var token = _factory.LoginFullAccess().Result;
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
  }


  [TestMethod]
  public async Task GetAdminById_ShouldReturn_OK()
  {
    var admin = new Admin
    (
      new Name("Emma", "Watson"),
      new Email("emmaWatsonOfficial@gmail.com"),
      new Phone("+44 (20) 99346-0912"),
      "123",
      EGender.Feminine,
      DateTime.Now.AddYears(-31),
      new Address("United Kingdom", "London", "UK-123", 456)
    );

    await _factory.DbFixture.DbContext.Admins.AddAsync(admin);
    await _factory.DbFixture.DbContext.SaveChangesAsync();

    var adminn = await _factory.DbFixture.DbContext.Admins.FirstOrDefaultAsync(x => x.Id == admin.Id);

    //Act
    var response = await _client.GetAsync($"{_baseUrl}/{admin.Id}");

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
  }

}
