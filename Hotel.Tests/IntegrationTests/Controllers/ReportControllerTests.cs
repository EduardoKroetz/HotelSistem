using Hotel.Domain.Data;
using Hotel.Domain.DTOs.RoomContext.ReportDTOs;
using Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;
using Hotel.Domain.Entities.RoomContext.ReportEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.ValueObjects;
using Hotel.Tests.IntegrationTests.Factories;
using Hotel.Tests.IntegrationTests.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;

namespace Hotel.Tests.IntegrationTests.Controllers;

[TestClass]
public class ReportControllerTests
{
  private static HotelWebApplicationFactory _factory = null!;
  private static HttpClient _client = null!;
  private static HotelDbContext _dbContext = null!;
  private static string _rootAdminToken = null!;
  private const string _baseUrl = "v1/reports";

  [ClassInitialize]
  public static void ClassInitialize(TestContext context)
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
  public async Task CreateReport_ShouldReturn_OK()
  {
    //Arange
    var employee = new Employee(
      new Name("Felipe", "Costa"),
      new Email("felipeCosta@gmail.com"),
      new Phone("+55 (83) 91234-5678"),
      "password21",
      EGender.Masculine,
      DateTime.Now.AddYears(-29),
      new Address("Brazil", "João Pessoa", "PB-2121", 2121)
    );
    await _dbContext.Employees.AddAsync(employee);
    await _dbContext.SaveChangesAsync();

    var body = new EditorReport("Requisição de serviço", "Requisição de serviço 1 no quarto 23", EPriority.Medium, employee.Id);

    //Act
    var response = await _client.PostAsJsonAsync(_baseUrl, body);

    //Assert

    var content = JsonConvert.DeserializeObject<Response<DataId>>(await response.Content.ReadAsStringAsync());
    var report = await _dbContext.Reports.FirstAsync(x => x.Id == content!.Data.Id);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

    Assert.AreEqual(200, content!.Status);
    Assert.AreEqual("Relatório criado com sucesso!", content.Message);
    Assert.AreEqual(0, content.Errors.Count);

    Assert.AreEqual(body.Summary, report.Summary);
    Assert.AreEqual(body.Description, report.Description);
    Assert.AreEqual(body.Priority, report.Priority);
    Assert.AreEqual(body.Resolution, report.Resolution);
    Assert.AreEqual(body.EmployeeId, report.EmployeeId);
    Assert.AreEqual(EStatus.Pending, report.Status);
  }

  [TestMethod]
  public async Task CreateReport_WithNonexistEmployee_ShouldReturn_NOT_FOUND()
  {
    //Arange
    var body = new EditorReport("Limpeza do lobby", "Limpeza profunda do lobby", EPriority.Medium, Guid.NewGuid());

    //Act
    var response = await _client.PostAsJsonAsync(_baseUrl, body);

    //Assert
    var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync());

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

    Assert.AreEqual(404, content!.Status);
    Assert.IsTrue(content.Errors.Any(x => x.Equals("Funcionário não encontrado.")));
  }

  [TestMethod]
  public async Task UpdateReport_ShouldReturn_OK()
  {
    //Arange
    var employee = new Employee(
      new Name("Renata", "Oliveira"),
      new Email("renataOliveira@gmail.com"),
      new Phone("+55 (67) 97654-3210"),
      "password24",
      EGender.Feminine,
      DateTime.Now.AddYears(-26),
      new Address("Brazil", "Campo Grande", "MS-2424", 2424)
    );
    var report = new Report("Manutenção de quarto", "Manutenção do quarto 14", EPriority.High, employee);

    await _dbContext.Employees.AddAsync(employee);
    await _dbContext.Reports.AddAsync(report);
    await _dbContext.SaveChangesAsync();

    var body = new EditorReport("Manutenção de quarto", "Manutenção do quarto 23", EPriority.High, employee.Id);

    //Act
    var response = await _client.PutAsJsonAsync($"{_baseUrl}/{report.Id}", body);

    //Assert

    var content = JsonConvert.DeserializeObject<Response<DataId>>(await response.Content.ReadAsStringAsync());
    var updatedReport = await _dbContext.Reports.FirstAsync(x => x.Id == content!.Data.Id);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

    Assert.AreEqual(200, content!.Status);
    Assert.AreEqual("Relatório atualizado com sucesso!", content.Message);
    Assert.AreEqual(0, content.Errors.Count);

    Assert.AreEqual(body.Summary, updatedReport.Summary);
    Assert.AreEqual(body.Description, updatedReport.Description);
    Assert.AreEqual(body.Priority, updatedReport.Priority);
    Assert.AreEqual(body.Resolution, updatedReport.Resolution);
    Assert.AreEqual(body.EmployeeId, updatedReport.EmployeeId);
    Assert.AreEqual(EStatus.Pending, updatedReport.Status);
  }

  [TestMethod]
  public async Task UpdateNonexistReport_ShouldReturn_NOT_FOUND()
  {
    //Arange
    var body = new EditorReport("Manutenção de quarto", "Manutenção do quarto 22", EPriority.High, Guid.NewGuid());

    //Act
    var response = await _client.PutAsJsonAsync($"{_baseUrl}/{Guid.NewGuid()}", body);

    //Assert
    var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync());

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

    Assert.AreEqual(404, content!.Status);
    Assert.IsTrue(content.Errors.Any(x => x.Equals("Relatório não encontrado.")));
  }

  [TestMethod]
  public async Task UpdateReport_WithNonexistEmployee_ShouldReturn_NOT_FOUND()
  {
    //Arange
    //Arange
    var employee = new Employee(
      new Name("Gabriela", "Moreira"),
      new Email("gabrielaMoreira@gmail.com"),
      new Phone("+55 (61) 92345-6789"),
      "password20",
      EGender.Feminine,
      DateTime.Now.AddYears(-24),
      new Address("Brazil", "Taguatinga", "DF-2020", 2020)
    );
    var report = new Report("Reabastecimento do minibar", "Reabastecer o minibar do quarto 101", EPriority.Low, employee);

    await _dbContext.Employees.AddAsync(employee);
    await _dbContext.Reports.AddAsync(report);
    await _dbContext.SaveChangesAsync();

    var body = new EditorReport("Limpeza do lobby", "Limpeza profunda do lobby", EPriority.Medium, Guid.NewGuid());

    //Act
    var response = await _client.PutAsJsonAsync($"{_baseUrl}/{report.Id}", body);

    //Assert
    var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync());

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

    Assert.AreEqual(404, content!.Status);
    Assert.IsTrue(content.Errors.Any(x => x.Equals("Funcionário não encontrado.")));
  }

  [TestMethod]
  public async Task DeleteMyReport_ShouldReturn_OK()
  {
    //Arange
    var employee = new Employee(
      new Name("Laura", "Nascimento"),
      new Email("lauraNascimento@gmail.com"),
      new Phone("+55 (62) 99887-6543"),
      "password22",
      EGender.Feminine,
      DateTime.Now.AddYears(-28),
      new Address("Brazil", "Anápolis", "GO-2222", 2222)
    );
    var report = new Report("Vazamento de água", "Vazamento de água no quarto 16", EPriority.Critical, employee);

    await _dbContext.Employees.AddAsync(employee);
    await _dbContext.Reports.AddAsync(report);
    await _dbContext.SaveChangesAsync();

    _factory.Login(_client ,employee);

    //Act
    var response = await _client.DeleteAsync($"{_baseUrl}/my/{report.Id}");

    //Assert

    var content = JsonConvert.DeserializeObject<Response<DataId>>(await response.Content.ReadAsStringAsync());
    var exists = await _dbContext.Reports.AnyAsync(x => x.Id == content!.Data.Id);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

    Assert.AreEqual(200, content!.Status);
    Assert.AreEqual("Relatório deletado com sucesso!", content.Message);
    Assert.AreEqual(0, content.Errors.Count);

    Assert.AreEqual(report.Id, content.Data.Id);

    Assert.IsFalse(exists);
  }

  [TestMethod]
  public async Task DeleteNonexistReport_ShouldReturn_NOT_FOUND()
  {
    //Act
    var response = await _client.DeleteAsync($"{_baseUrl}/my/{Guid.NewGuid()}");

    //Assert
    var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync());

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

    Assert.AreEqual(404, content!.Status);
    Assert.IsTrue(content.Errors.Any(x => x.Equals("Relatório não encontrado.")));
  }

  [TestMethod]
  public async Task DeleteMyReport_WithBeingMine_ShouldReturn_FORBIDDEN()
  {
    //Arange
    var employee = new Employee(
      new Name("Marcelo", "Duarte"),
      new Email("marceloDuarte@gmail.com"),
      new Phone("+55 (21) 93456-7890"),
      "password19",
      EGender.Masculine,
      DateTime.Now.AddYears(-34),
      new Address("Brazil", "Niterói", "RJ-1919", 1919)
    );
    var report = new Report("Revisão de saída de emergência", "Verificar e liberar saída de emergência do 1º andar", EPriority.High, employee);

    await _dbContext.Employees.AddAsync(employee);
    await _dbContext.Reports.AddAsync(report);
    await _dbContext.SaveChangesAsync();

    //Act
    var response = await _client.DeleteAsync($"{_baseUrl}/my/{report.Id}");

    //Assert
    var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync());

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);

    Assert.AreEqual(403, content!.Status);
    Assert.IsTrue(content.Errors.Any(x => x.Equals("Você não tem permissão para deletar relatório alheio!")));
  }

  [TestMethod]
  public async Task GetReports_ShouldReturn_OK()
  {
    //Arange
    var employee = new Employee(
      new Name("Vinícius", "Cardoso"),
      new Email("viniciusCardoso@gmail.com"),
      new Phone("+55 (47) 98765-4321"),
      "password23",
      EGender.Masculine,
      DateTime.Now.AddYears(-31),
      new Address("Brazil", "Joinville", "SC-2323", 2323)
    );
    var report = new Report("Reparo no ar condicionado", "Reparar ar condicionado do quarto 307", EPriority.High, employee);

    await _dbContext.Employees.AddAsync(employee);
    await _dbContext.Reports.AddAsync(report);
    await _dbContext.SaveChangesAsync();

    //Act
    var response = await _client.GetAsync(_baseUrl);

    //Assert
    var content = JsonConvert.DeserializeObject<Response<List<GetReport>>>(await response.Content.ReadAsStringAsync());

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

    Assert.AreEqual(200, content!.Status);
    Assert.AreEqual("Sucesso!", content.Message);
    Assert.AreEqual(0, content.Errors.Count);

    foreach (var categ in content.Data)
    {
      Assert.IsNotNull(categ.Id);
      Assert.IsNotNull(categ.Summary);
      Assert.IsNotNull(categ.Description);
      Assert.IsNotNull(categ.Priority);
      Assert.IsNotNull(categ.Status);
      Assert.IsNotNull(categ.Resolution);
      Assert.IsNotNull(categ.EmployeeId);
    }

  }

  [TestMethod]
  public async Task GetReportById_ShouldReturn_OK()
  {
    //Arange
    var employee = new Employee(
      new Name("André", "Pinto"),
      new Email("andrePinto@gmail.com"),
      new Phone("+55 (95) 93456-7890"),
      "password25",
      EGender.Masculine,
      DateTime.Now.AddYears(-33),
      new Address("Brazil", "Boa Vista", "RR-2525", 2525)
    );
    var report = new Report("Reparo no ar condicionado", "Reparar ar condicionado do quarto 307", EPriority.High, employee);

    await _dbContext.Employees.AddAsync(employee);
    await _dbContext.Reports.AddAsync(report);
    await _dbContext.SaveChangesAsync();

    //Act
    var response = await _client.GetAsync($"{_baseUrl}/{report.Id}");

    //Assert

    var content = JsonConvert.DeserializeObject<Response<GetReport>>(await response.Content.ReadAsStringAsync());

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

    Assert.AreEqual(200, content!.Status);
    Assert.AreEqual("Sucesso!", content.Message);
    Assert.AreEqual(0, content.Errors.Count);

    Assert.AreEqual(report.Id, content.Data.Id);
    Assert.AreEqual(report.Summary, content.Data.Summary);
    Assert.AreEqual(report.Description, content.Data.Description);
    Assert.AreEqual(report.Priority, content.Data.Priority);
    Assert.AreEqual(report.Resolution, content.Data.Resolution);
    Assert.AreEqual(report.EmployeeId, content.Data.EmployeeId);
    Assert.AreEqual(EStatus.Pending, content.Data.Status);
  }

  [TestMethod]
  public async Task GetNonexistReportById_ShouldReturn_NOT_FOUND()
  {
    //Act
    var response = await _client.GetAsync($"{_baseUrl}/{Guid.NewGuid()}");

    //Assert
    var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync());

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

    Assert.AreEqual(404, content!.Status);
    Assert.IsTrue(content.Errors.Any(x => x.Equals("Relatório não encontrado.")));
  }

  [TestMethod]
  public async Task FinishReport_ShouldReturn_OK()
  {
    var employee = new Employee(
      new Name("Juliana", "Silva"),
      new Email("julianaSilva@gmail.com"),
      new Phone("+55 (92) 92345-6789"),
      "password26",
      EGender.Feminine,
      DateTime.Now.AddYears(-27),
      new Address("Brazil", "Manaus", "AM-2626", 2626)
    );

    var report = new Report("Reposição de toalhas", "Repor toalhas no quarto 402", EPriority.Low, employee);

    await _dbContext.Employees.AddAsync(employee);
    await _dbContext.Reports.AddAsync(report);
    await _dbContext.SaveChangesAsync();

    //Act
    var response = await _client.PatchAsJsonAsync($"{_baseUrl}/finish/{report.Id}", new {});

    //Assert

    var content = JsonConvert.DeserializeObject<Response<DataId>>(await response.Content.ReadAsStringAsync());
    var updatedReport = await _dbContext.Reports.FirstAsync(x => x.Id == report.Id);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

    Assert.AreEqual(200, content!.Status);
    Assert.AreEqual("Relatório finalizado com sucesso!", content.Message);
    Assert.AreEqual(0, content.Errors.Count);

    Assert.AreEqual(report.Id, content.Data.Id);
    Assert.AreEqual(report.Summary, updatedReport.Summary);
    Assert.AreEqual(report.Description, updatedReport.Description);
    Assert.AreEqual(report.Priority, updatedReport.Priority);
    Assert.AreEqual(report.Resolution, updatedReport.Resolution);
    Assert.AreEqual(report.EmployeeId, updatedReport.EmployeeId);
    Assert.AreEqual(EStatus.Finish, updatedReport.Status);
  }

  [TestMethod]
  public async Task FinishNonexistReport_ShouldReturn_NOT_FOUND()
  {
    //Arange
    var body = new EditorReport("Manutenção de quarto", "Manutenção do quarto 22", EPriority.High, Guid.NewGuid());

    //Act
    var response = await _client.PatchAsJsonAsync($"{_baseUrl}/finish/{Guid.NewGuid()}", body);

    //Assert
    var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync());

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

    Assert.AreEqual(404, content!.Status);
    Assert.IsTrue(content.Errors.Any(x => x.Equals("Relatório não encontrado.")));
  }

  [TestMethod]
  public async Task CancelReport_ShouldReturn_OK()
  {
    var employee = new Employee(
      new Name("Fernando", "Dias"),
      new Email("fernandoDias@gmail.com"),
      new Phone("+55 (98) 91234-5678"),
      "password27",
      EGender.Masculine,
      DateTime.Now.AddYears(-32),
      new Address("Brazil", "São Luís", "MA-2727", 2727)
    );

    var report = new Report("Assistência na recepção", "Ajudar na recepção durante horário de pico", EPriority.Medium, employee);

    await _dbContext.Employees.AddAsync(employee);
    await _dbContext.Reports.AddAsync(report);
    await _dbContext.SaveChangesAsync();

    //Act
    var response = await _client.PatchAsJsonAsync($"{_baseUrl}/cancel/{report.Id}", new { });

    //Assert

    var content = JsonConvert.DeserializeObject<Response<DataId>>(await response.Content.ReadAsStringAsync());
    var updatedReport = await _dbContext.Reports.FirstAsync(x => x.Id == report.Id);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

    Assert.AreEqual(200, content!.Status);
    Assert.AreEqual("Relatório cancelado com sucesso!", content.Message);
    Assert.AreEqual(0, content.Errors.Count);

    Assert.AreEqual(report.Id, content.Data.Id);
    Assert.AreEqual(report.Summary, updatedReport.Summary);
    Assert.AreEqual(report.Description, updatedReport.Description);
    Assert.AreEqual(report.Priority, updatedReport.Priority);
    Assert.AreEqual(report.Resolution, updatedReport.Resolution);
    Assert.AreEqual(report.EmployeeId, updatedReport.EmployeeId);
    Assert.AreEqual(EStatus.Cancelled, updatedReport.Status);
  }

  [TestMethod]
  public async Task CancelNonexistReport_ShouldReturn_NOT_FOUND()
  {
    //Act
    var response = await _client.PatchAsJsonAsync($"{_baseUrl}/cancel/{Guid.NewGuid()}", new { });

    //Assert
    var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync());

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

    Assert.AreEqual(404, content!.Status);
    Assert.IsTrue(content.Errors.Any(x => x.Equals("Relatório não encontrado.")));
  }

  [TestMethod]
  public async Task UpdateReportPriority_ShouldReturn_OK()
  {
    var employee = new Employee(
      new Name("Paulo", "Moura"),
      new Email("pauloMoura@gmail.com"),
      new Phone("+55 (88) 97654-3210"),
      "password29",
      EGender.Masculine,
      DateTime.Now.AddYears(-34),
      new Address("Brazil", "Sobral", "CE-2929", 2929)
    );

    var report = new Report("Verificação de sistema de aquecimento", "Checar sistema de aquecimento do quarto 205", EPriority.High, employee);

    await _dbContext.Employees.AddAsync(employee);
    await _dbContext.Reports.AddAsync(report);
    await _dbContext.SaveChangesAsync();

    var priority = EPriority.Critical;

    //Act
    var response = await _client.PatchAsJsonAsync($"{_baseUrl}/{report.Id}/priority/{(int)priority}", new { });

    //Assert

    var content = JsonConvert.DeserializeObject<Response<DataId>>(await response.Content.ReadAsStringAsync());
    var updatedReport = await _dbContext.Reports.FirstAsync(x => x.Id == report.Id);

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

    Assert.AreEqual(200, content!.Status);
    Assert.AreEqual("Prioridade atualizada com sucesso!", content.Message);
    Assert.AreEqual(0, content.Errors.Count);

    Assert.AreEqual(report.Id, content.Data.Id);
    Assert.AreEqual(report.Summary, updatedReport.Summary);
    Assert.AreEqual(report.Description, updatedReport.Description);
    Assert.AreEqual(priority, updatedReport.Priority);
    Assert.AreEqual(report.Resolution, updatedReport.Resolution);
    Assert.AreEqual(report.EmployeeId, updatedReport.EmployeeId);
    Assert.AreEqual(EStatus.Pending, updatedReport.Status);
  }

  [TestMethod]
  public async Task UpdateNonexistReportPriority_ShouldReturn_NOT_FOUND()
  {
    //Act
    var response = await _client.PatchAsJsonAsync($"{_baseUrl}/{Guid.NewGuid()}/priority/3", new { });

    //Assert
    var content = JsonConvert.DeserializeObject<Response<object>>(await response.Content.ReadAsStringAsync());

    Assert.IsNotNull(response);
    Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

    Assert.AreEqual(404, content!.Status);
    Assert.IsTrue(content.Errors.Any(x => x.Equals("Relatório não encontrado.")));
  }
}
