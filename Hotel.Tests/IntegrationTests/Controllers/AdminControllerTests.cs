using Hotel.Domain.Data;
using Hotel.Domain.DTOs.Base.User;
using Hotel.Domain.Entities.AdminEntity;
using Hotel.Domain.Entities.PermissionEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Services.Permissions;
using Hotel.Domain.Services.TokenServices;
using Hotel.Domain.ValueObjects;
using Hotel.Tests.IntegrationTests.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Hotel.Tests.IntegrationTests.Controllers;

[TestClass]
public class AdminControllerTests
{
    private static HotelWebApplicationFactory _factory = null!;
    private static HttpClient _client = null!;
    private static TokenService _tokenService = null!;
    private static HotelDbContext _dbContext = null!;
    private static string _rootAdminToken = null!;
    private const string _baseUrl = "v1/admins";
    private static Permission _defaultAdminPermission = null!;
    private static List<Permission> _defaultAdminPermissions = null!;
    private static List<Permission> _permissions = null!;

    [ClassInitialize]
    public static void ClassInitialize(TestContext? context)
    {
        _factory = new HotelWebApplicationFactory();
        _client = _factory.CreateClient();
        _dbContext = _factory.Services.GetRequiredService<HotelDbContext>();
        _tokenService = _factory.Services.GetRequiredService<TokenService>();

        _rootAdminToken = _factory.LoginFullAccess().Result;
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _rootAdminToken);

        _defaultAdminPermission = _dbContext.Permissions.AsTracking().First(x => x.Name.Contains("DefaultAdminPermission"));
        _permissions = _dbContext.Permissions.ToListAsync().Result;
        _defaultAdminPermissions = _permissions.Where(x => DefaultAdminPermissions.PermissionsName.Any(y => y.ToString() == x.Name)).ToList();
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
    public async Task GetAdmin_ShouldReturn_OK()
    {
        //Arrange
        var admin = new Admin
        (
          new Name("Jennifer", "Lawrence"),
          new Email("jenniferLawrenceOfficial@gmail.com"),
          new Phone("+44 (20) 97890-1234"),
          "789",
          EGender.Feminine,
          DateTime.Now.AddYears(-30),
          new Address("United States", "Los Angeles", "US-456", 789)
        );

        await _dbContext.Admins.AddAsync(admin);
        await _dbContext.SaveChangesAsync();

        //Act
        var response = await _client.GetAsync($"{_baseUrl}?take=1");

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [TestMethod]
    public async Task GetAdminById_ShouldReturn_OK()
    {
        //Arrange
        var admin = new Admin
        (
          new Name("Ana", "Souza"),
          new Email("anaSouzaOfficial@gmail.com"),
          new Phone("+55 (31) 91234-5678"),
          "789",
          EGender.Feminine,
          DateTime.Now.AddYears(-28),
          new Address("Brazil", "Belo Horizonte", "BR-123", 789)
        );

        await _dbContext.Admins.AddAsync(admin);
        await _dbContext.SaveChangesAsync();

        //Act
        var response = await _client.GetAsync($"{_baseUrl}/{admin.Id}");

        //Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [TestMethod]
    public async Task DeleteAdmin_ShouldReturn_OK()
    {
        //Arrange
        var admin = new Admin
        (
          new Name("João", "Pereira"),
          new Email("joaoPereira@gmail.com"),
          new Phone("+55 (21) 98765-4321"),
          "password2",
          EGender.Masculine,
          DateTime.Now.AddYears(-30),
          new Address("Brazil", "Rio de Janeiro", "RJ-202", 202)
        );

        await _dbContext.Admins.AddAsync(admin);
        await _dbContext.SaveChangesAsync();

        //Act
        var response = await _client.DeleteAsync($"{_baseUrl}/{admin.Id}");

        //Assert
        var wasNotDeleted = await _dbContext.Admins.AnyAsync(x => x.Id == admin.Id);

        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsFalse(wasNotDeleted);
    }

    [TestMethod]
    public async Task DeleteLoggedAdmin_ShouldReturn_OK()
    {
        //Arrange
        var admin = new Admin
        (
          new Name("Emma", "Watson"),
          new Email("emmaWatson@gmail.com"),
          new Phone("+44 (20) 99346-1912"),
          "123",
          EGender.Feminine,
          DateTime.Now.AddYears(-31),
          new Address("United Kingdom", "London", "UK-123", 456)
        );

        var token = _tokenService.GenerateToken(admin);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var dbContext = _factory.Services.GetRequiredService<HotelDbContext>();
        await dbContext.Admins.AddAsync(admin);
        await dbContext.SaveChangesAsync();

        //Act
        var response = await _client.DeleteAsync(_baseUrl);

        //Assert
        var wasNotDeleted = await dbContext.Admins.AnyAsync(x => x.Id == admin.Id);

        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsFalse(wasNotDeleted);
    }

    [TestMethod]
    public async Task AddAdminPermission_ShouldReturn_OK()
    {
        var permission = await _dbContext.Permissions.FirstOrDefaultAsync(x => x.Name.Contains("CreateAdmin"));

        //Arrange
        var admin = new Admin
        (
          new Name("Carlos", "Silva"),
          new Email("carlosSilvl@gmail.com"),
          new Phone("+55 (21) 99876-5432"),
          "456",
          EGender.Masculine,
          DateTime.Now.AddYears(-35),
          new Address("Brazil", "Rio de Janeiro", "BR-789", 123)
        );

        await _dbContext.Admins.AddAsync(admin);
        await _dbContext.SaveChangesAsync();

        //Act
        var response = await _client.PostAsync($"{_baseUrl}/{admin.Id}/permissions/{permission!.Id}", null);

        //Assert
        var adminWithPermissions = await _dbContext.Admins
          .Include(x => x.Permissions)
          .FirstOrDefaultAsync(x => x.Id == admin.Id);

        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual(adminWithPermissions!.Id, admin.Id);
        Assert.AreEqual(adminWithPermissions.Permissions.First().Id, permission.Id);
    }

    [TestMethod]
    public async Task RemoveAdminPermission_ShouldReturn_OK()
    {
        var permission = await _dbContext.Permissions.FirstOrDefaultAsync(x => x.Name.Contains("CreateAdmin"));

        //Arrange
        var admin = new Admin
        (
          new Name("Mariana", "Lima"),
          new Email("marianaLima@gmail.com"),
          new Phone("+55 (11) 91234-4678"),
          "password1",
          EGender.Feminine,
          DateTime.Now.AddYears(-25),
          new Address("Brazil", "São Paulo", "SP-101", 101),
          [permission]
        );

        await _dbContext.Admins.AddAsync(admin);
        await _dbContext.SaveChangesAsync();

        //Act
        var response = await _client.DeleteAsync($"{_baseUrl}/{admin.Id}/permissions/{permission!.Id}");

        //Assert
        var adminWithPermissions = await _dbContext.Admins
          .Where(x => x.Id == admin.Id)
          .Include(x => x.Permissions)
          .FirstOrDefaultAsync();

        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual(adminWithPermissions!.Id, admin.Id);
        Assert.AreEqual(adminWithPermissions.Permissions.Count, 0);
    }

    //[TestMethod]
    //public async Task UpdateToRootAdmin_ShouldReturn_OK()
    //{
    //  //Arrange
    //  var admin = new Admin
    //  (
    //    new Name("Beatriz", "Santos"),
    //    new Email("beatrizSantos@gmail.com"),
    //    new Phone("+55 (31) 99876-5432"),
    //    "password3",
    //    EGender.Feminine,
    //    DateTime.Now.AddYears(-27),
    //    new Address("Brazil", "Belo Horizonte", "MG-303", 303)
    //  );

    //  await _dbContext.Admins.AddAsync(admin);
    //  await _dbContext.SaveChangesAsync();

    //  //Act
    //  var response = await _client.PostAsync($"{_baseUrl}/to-root-admin/{admin.Id}", null);

    //  //Assert
    //  var updatedAdmin = await _dbContext.Admins.FirstOrDefaultAsync(x => x.Id == admin.Id);

    //  Assert.IsNotNull(response);
    //  Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    //  Assert.IsTrue(updatedAdmin!.IsRootAdmin);
    //}

    [TestMethod]
    public async Task UpdateAdmin_ShouldReturn_OK()
    {
        //Arrange
        var admin = new Admin
        (
          new Name("Rafael", "Oliveira"),
          new Email("rafaelOliveira@gmail.com"),
          new Phone("+55 (41) 97654-3210"),
          "password4",
          EGender.Masculine,
          DateTime.Now.AddYears(-32),
          new Address("Brazil", "Curitiba", "PR-404", 404)
        );

        await _dbContext.Admins.AddAsync(admin);
        await _dbContext.SaveChangesAsync();

        var body = new UpdateUser("Jão", "Pedro", "+55 (41) 97654-3210", EGender.Feminine, DateTime.Now.AddYears(-20), "Brazil", "Curitiba", "PR-404", 404);

        //Act
        var response = await _client.PutAsJsonAsync($"{_baseUrl}/{admin.Id}", body);

        //Assert
        var updatedAdmin = await _dbContext.Admins.FirstOrDefaultAsync(x => x.Id == admin.Id);

        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual(updatedAdmin!.Id, admin.Id);
        Assert.AreEqual(updatedAdmin.Name.FirstName, body.FirstName);
        Assert.AreEqual(updatedAdmin.Name.LastName, body.LastName);
        Assert.AreEqual(updatedAdmin.Phone.Number, body.Phone);
        Assert.AreEqual(updatedAdmin.Gender, body.Gender);
        Assert.AreEqual(updatedAdmin.DateOfBirth, body.DateOfBirth);
        Assert.AreEqual(updatedAdmin!.Address!.Country, body.Country);
        Assert.AreEqual(updatedAdmin!.Address.City, body.City);
        Assert.AreEqual(updatedAdmin!.Address!.Number, body.Number);
        Assert.AreEqual(updatedAdmin!.Address.Street, body.Street);
    }


    [TestMethod]
    public async Task UpdateLoggedAdmin_ShouldReturn_OK()
    {
        //Arrange
        var admin = new Admin
        (
          new Name("Camila", "Costa"),
          new Email("camilaCosta@gmail.com"),
          new Phone("+55 (71) 93456-7890"),
          "password5",
          EGender.Feminine,
          DateTime.Now.AddYears(-29),
          new Address("Brazil", "Salvador", "BA-505", 505)
        );

        await _dbContext.Admins.AddAsync(admin);
        await _dbContext.SaveChangesAsync();

        var token = _tokenService.GenerateToken(admin);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var body = new UpdateUser("Jão", "Pedro", "+55 (41) 93651-3210", EGender.Feminine, DateTime.Now.AddYears(-20), "Brazil", "Curitiba", "PR-404", 404);

        //Act
        var response = await _client.PutAsJsonAsync(_baseUrl, body);

        //Assert
        var updatedAdmin = await _dbContext.Admins.FirstOrDefaultAsync(x => x.Id == admin.Id);

        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual(updatedAdmin!.Id, admin.Id);
        Assert.AreEqual(updatedAdmin.Name.FirstName, body.FirstName);
        Assert.AreEqual(updatedAdmin.Name.LastName, body.LastName);
        Assert.AreEqual(updatedAdmin.Phone.Number, body.Phone);
        Assert.AreEqual(updatedAdmin.Gender, body.Gender);
        Assert.AreEqual(updatedAdmin.DateOfBirth, body.DateOfBirth);
        Assert.AreEqual(updatedAdmin!.Address!.Country, body.Country);
        Assert.AreEqual(updatedAdmin!.Address.City, body.City);
        Assert.AreEqual(updatedAdmin!.Address!.Number, body.Number);
        Assert.AreEqual(updatedAdmin!.Address.Street, body.Street);
    }

    [TestMethod]
    public async Task UpdateAdminName_ShouldReturn_OK()
    {
        //Arrange
        var admin = new Admin
        (
          new Name("Lucas", "Ferreira"),
          new Email("lucasFerreira@gmail.com"),
          new Phone("+55 (61) 92345-6789"),
          "password6",
          EGender.Masculine,
          DateTime.Now.AddYears(-28),
          new Address("Brazil", "Brasília", "DF-606", 606)
        );

        await _dbContext.Admins.AddAsync(admin);
        await _dbContext.SaveChangesAsync();

        var token = _tokenService.GenerateToken(admin);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var body = new Name("John", "Wick");

        //Act
        var response = await _client.PatchAsJsonAsync($"{_baseUrl}/name", body);

        //Assert
        var updatedAdmin = await _dbContext.Admins.FirstOrDefaultAsync(x => x.Id == admin.Id);

        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual(updatedAdmin!.Name.FirstName, body.FirstName);
        Assert.AreEqual(updatedAdmin.Name.LastName, body.LastName);
    }

    [TestMethod]
    public async Task UpdateEmailAdmin_ShouldReturn_OK()
    {
        //Arrange
        var admin = new Admin
        (
          new Name("Fernanda", "River"),
          new Email("fernandaRiver@gmail.com"),
          new Phone("+55 (51) 91219-5678"),
          "password7",
          EGender.Feminine,
          DateTime.Now.AddYears(-26),
          new Address("Brazil", "Porto Alegre", "RS-707", 707)
        );

        await _dbContext.Admins.AddAsync(admin);
        await _dbContext.SaveChangesAsync();

        var token = _tokenService.GenerateToken(admin);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var body = new Email("feeRriber@gmail.com");

        //Act
        var response = await _client.PatchAsJsonAsync($"{_baseUrl}/email", body);

        //Assert
        var updatedAdmin = await _dbContext.Admins.FirstOrDefaultAsync(x => x.Id == admin.Id);

        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual(updatedAdmin!.Email.Address, body.Address);
    }

    [TestMethod]
    public async Task UpdatePhoneAdmin_ShouldReturn_OK()
    {
        //Arrange
        var admin = new Admin
        (
          new Name("Michele", "Silva"),
          new Email("micheleSilvaa100@gmail.com"),
          new Phone("+55 (62) 99846-1432"),
          "password8",
          EGender.Masculine,
          DateTime.Now.AddYears(-31),
          new Address("Brazil", "Goiânia", "GO-808", 808)
        );

        await _dbContext.Admins.AddAsync(admin);
        await _dbContext.SaveChangesAsync();

        var token = _tokenService.GenerateToken(admin);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var body = new Phone("+55 (62) 99156-3449");

        //Act
        var response = await _client.PatchAsJsonAsync($"{_baseUrl}/phone", body);

        //Assert
        var updatedAdmin = await _dbContext.Admins.FirstOrDefaultAsync(x => x.Id == admin.Id);

        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual(updatedAdmin!.Phone.Number, body.Number);
    }

    [TestMethod]
    public async Task UpdateAddressAdmin_ShouldReturn_OK()
    {
        //Arrange
        var admin = new Admin
        (
          new Name("Vinicius", "Silva"),
          new Email("viniSilva@gmail.com"),
          new Phone("+55 (62) 91876-3432"),
          "password8",
          EGender.Masculine,
          DateTime.Now.AddYears(-31),
          new Address("Brazil", "Goiânia", "GO-808", 808)
        );

        await _dbContext.Admins.AddAsync(admin);
        await _dbContext.SaveChangesAsync();

        var token = _tokenService.GenerateToken(admin);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var body = new Address("Brazil", "Florianópolis", "SC-909", 909);

        //Act
        var response = await _client.PatchAsJsonAsync($"{_baseUrl}/address", body);

        //Assert
        var updatedAdmin = await _dbContext.Admins.FirstOrDefaultAsync(x => x.Id == admin.Id);

        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual(updatedAdmin!.Address!.Country, body.Country);
        Assert.AreEqual(updatedAdmin!.Address.City, body.City);
        Assert.AreEqual(updatedAdmin!.Address!.Number, body.Number);
        Assert.AreEqual(updatedAdmin!.Address.Street, body.Street);
    }

    [TestMethod]
    public async Task UpdateGenderAdmin_ShouldReturn_OK()
    {
        //Arrange
        var admin = new Admin
        (
          new Name("Gustavo", "Souza"),
          new Email("gustavoSouza@gmail.com"),
          new Phone("+55 (27) 93456-7890"),
          "password10",
          EGender.Masculine,
          DateTime.Now.AddYears(-33),
          new Address("Brazil", "Vitória", "ES-1010", 1010)
        );

        await _dbContext.Admins.AddAsync(admin);
        await _dbContext.SaveChangesAsync();

        var token = _tokenService.GenerateToken(admin);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //Act
        var response = await _client.PatchAsJsonAsync($"{_baseUrl}/gender/2", new { });

        //Assert
        var updatedAdmin = await _dbContext.Admins.FirstOrDefaultAsync(x => x.Id == admin.Id);

        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual(EGender.Feminine, updatedAdmin!.Gender);
    }

    [TestMethod]
    public async Task UpdateDateOfBirthAdmin_ShouldReturn_OK()
    {
        //Arrange
        var admin = new Admin
        (
          new Name("Geovane", "Silva"),
          new Email("geoSilv@gmail.com"),
          new Phone("+55 (27) 93113-7859"),
          "password10",
          EGender.Masculine,
          DateTime.Now.AddYears(-33),
          new Address("Brazil", "Vitória", "ES-1011", 1011)
        );

        await _dbContext.Admins.AddAsync(admin);
        await _dbContext.SaveChangesAsync();

        var token = _tokenService.GenerateToken(admin);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var body = new UpdateDateOfBirth(DateTime.Now.AddYears(-35));

        //Act
        var response = await _client.PatchAsJsonAsync($"{_baseUrl}/date-of-birth", body);

        //Assert
        var updatedAdmin = await _dbContext.Admins.FirstOrDefaultAsync(x => x.Id == admin.Id);

        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual(body.DateOfBirth, updatedAdmin!.DateOfBirth);
    }



    //
    //TESTES DE PERMISSÂO
    //

    [TestMethod]
    [DataRow("GetAdmins", "v1/admins", "GET")]
    [DataRow("GetAdmin", "v1/admins/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "GET")]
    [DataRow("EditAdmin", "v1/admins/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "PUT")]
    [DataRow("DeleteAdmin", "v1/admins/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "DELETE")]
    [DataRow("AdminAssignPermission", "v1/admins/f6c5e02b-a0ae-429e-beb3-d433d51ad414/permissions/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "POST")]
    [DataRow("AdminUnassignPermission", "v1/admins/f6c5e02b-a0ae-429e-beb3-d433d51ad414/permissions/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "DELETE")]
    [DataRow("EditCustomer", "v1/customers/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "PUT")]
    [DataRow("DeleteCustomer", "v1/customers/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "DELETE")]
    [DataRow("CreateAdmin", "v1/register/admins", "POST")]
    [DataRow("GetEmployee", "v1/employees/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "GET")]
    [DataRow("GetEmployees", "v1/employees", "GET")]
    [DataRow("DeleteEmployee", "v1/employees/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "DELETE")]
    [DataRow("EditEmployee", "v1/employees/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "PUT")]
    [DataRow("CreateEmployee", "v1/register/employees", "POST")]
    [DataRow("AssignEmployeeResponsibility", "v1/employees/f6c5e02b-a0ae-429e-beb3-d433d51ad414/responsibilities/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "POST")]
    [DataRow("UnassignEmployeeResponsibility", "v1/employees/f6c5e02b-a0ae-429e-beb3-d433d51ad414/responsibilities/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "DELETE")]
    [DataRow("AssignEmployeePermission", "v1/employees/f6c5e02b-a0ae-429e-beb3-d433d51ad414/permissions/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "POST")]
    [DataRow("UnassignEmployeePermission", "v1/employees/f6c5e02b-a0ae-429e-beb3-d433d51ad414/permissions/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "DELETE")]
    [DataRow("GetResponsibilities", "v1/responsibilities", "GET")]
    [DataRow("GetResponsibility", "v1/responsibilities/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "GET")]
    [DataRow("CreateResponsibility", "v1/responsibilities", "POST")]
    [DataRow("EditResponsibility", "v1/responsibilities/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "PUT")]
    [DataRow("DeleteResponsibility", "v1/responsibilities/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "DELETE")]
    [DataRow("DeleteInvoice", "v1/room-invoices/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "DELETE")]
    [DataRow("GetInvoices", "v1/room-invoices", "GET")]
    [DataRow("GetInvoice", "v1/room-invoices/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "GET")]
    [DataRow("DeleteReservation", "v1/reservations/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "DELETE")]
    [DataRow("UpdateReservationCheckout", "v1/reservations/f6c5e02b-a0ae-429e-beb3-d433d51ad414/check-out", "PATCH")]
    [DataRow("UpdateReservationCheckIn", "v1/reservations/f6c5e02b-a0ae-429e-beb3-d433d51ad414/check-in", "PATCH")]
    [DataRow("AddServiceToReservation", "v1/reservations/f6c5e02b-a0ae-429e-beb3-d433d51ad414/services/e3347565-8ec7-4a3b-be3a-951317bb53dc", "POST")]
    [DataRow("RemoveServiceFromReservation", "v1/reservations/f6c5e02b-a0ae-429e-beb3-d433d51ad414/services/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "DELETE")]
    [DataRow("CreateCategory", "v1/categories", "POST")]
    [DataRow("EditCategory", "v1/categories/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "PUT")]
    [DataRow("DeleteCategory", "v1/categories/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "DELETE")]
    [DataRow("GetReports", "v1/reports", "GET")]
    [DataRow("GetReport", "v1/reports/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "GET")]
    [DataRow("EditReport", "v1/reports/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "PUT")]
    [DataRow("CreateReport", "v1/reports", "POST")]
    [DataRow("FinishReport", "v1/reports/finish/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "PATCH")]
    [DataRow("CreateRoom", "v1/rooms", "POST")]
    [DataRow("EditRoom", "v1/rooms/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "PUT")]
    [DataRow("DeleteRoom", "v1/rooms/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "DELETE")]
    [DataRow("AddRoomService", "v1/rooms/f6c5e02b-a0ae-429e-beb3-d433d51ad414/services/e3347565-8ec7-4a3b-be3a-951317bb53dc", "POST")]
    [DataRow("RemoveRoomService", "v1/rooms/f6c5e02b-a0ae-429e-beb3-d433d51ad414/services/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "DELETE")]
    [DataRow("UpdateRoomNumber", "v1/rooms/f6c5e02b-a0ae-429e-beb3-d433d51ad414/number/1", "PATCH")]
    [DataRow("UpdateRoomCapacity", "v1/rooms/f6c5e02b-a0ae-429e-beb3-d433d51ad414/capacity/2", "PATCH")]
    [DataRow("UpdateRoomCategory", "v1/rooms/f6c5e02b-a0ae-429e-beb3-d433d51ad414/category/62eb01d1-a7ba-4c09-ae5b-5ec6b5071577", "PATCH")]
    [DataRow("UpdateRoomPrice", "v1/rooms/f6c5e02b-a0ae-429e-beb3-d433d51ad414/price", "PATCH")]
    [DataRow("EnableRoom", "v1/rooms/enable/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "PATCH")]
    [DataRow("DisableRoom", "v1/rooms/disable/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "PATCH")]
    [DataRow("GetServices", "v1/services", "GET")]
    [DataRow("GetService", "v1/services/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "GET")]
    [DataRow("UpdateService", "v1/services/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "PUT")]
    [DataRow("CreateService", "v1/services", "POST")]
    [DataRow("DeleteService", "v1/services/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "DELETE")]
    [DataRow("AssignServiceResponsibility", "v1/services/f6c5e02b-a0ae-429e-beb3-d433d51ad414/responsibilities/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "POST")]
    [DataRow("UnassignServiceResponsibility", "v1/services/f6c5e02b-a0ae-429e-beb3-d433d51ad414/responsibilities/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "DELETE")]
    [DataRow("AvailableRoomStatus", "v1/rooms/available/f6c5e02b-a0ae-429e-beb3-d433d51ad414", "PATCH")]
    public void AccessEndpointsWithOutPermission_ShouldReturn_FORBIDDEN(string permissionName, string endpoint, string method)
    {
        var permissions = _permissions.Where(x => x.Name != "DefaultAdminPermission" && x.Name != "DefaultEmployeePermission").ToList();

        //Arrange
        var admin = new Admin
        (
          new Name("Gabriel", "Souz"),
          new Email("gabriSouz@gmail.com"),
          new Phone("+55 (27) 93123-7810"),
          "password10",
          EGender.Masculine,
          DateTime.Now.AddYears(-33),
          new Address("Brazil", "Vitória", "ES-1011", 1011),
          permissions
        );
        try
        {

            _dbContext.Admins.Add(admin);
            _dbContext.SaveChanges();

            var permission = _dbContext.Permissions.FirstOrDefault(x => x.Name.Equals(permissionName));
            admin.RemovePermission(permission!);

            var token = _tokenService.GenerateToken(admin);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //Act
            var response = method switch
            {
                "POST" => _client.PostAsJsonAsync(endpoint, new { }).Result,
                "PUT" => _client.PutAsJsonAsync(endpoint, new { }).Result,
                "GET" => _client.GetAsync(endpoint).Result,
                "DELETE" => _client.DeleteAsync(endpoint).Result,
                "PATCH" => _client.PatchAsync(endpoint, null).Result,
                _ => null!
            };

            //Assert
            Assert.AreEqual(HttpStatusCode.Forbidden, response!.StatusCode);
        }
        finally
        {
            _dbContext.Admins.Remove(admin!);
            _dbContext.SaveChanges();
        }
    }


}
