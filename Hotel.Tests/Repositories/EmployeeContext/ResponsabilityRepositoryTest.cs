using Hotel.Domain.DTOs.EmployeeContext.ResponsabilityDTOs;
using Hotel.Domain.Entities.EmployeeContext.ResponsabilityEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Repositories.EmployeeContext;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Tests.Repositories.EmployeeContext;

[TestClass]
public class ResponsabilityRepositoryTest : BaseRepositoryTest
{
  private List<Responsability> _responsabilities { get; set; } = [];
  private Responsability _defaultResponsibility { get; set; } = null!;
  private ResponsabilityRepository _responsabilityRepository { get; set; } = null!;

  [TestInitialize]
  public async Task StartupTest()
  {
    await Startup(null);
    _responsabilityRepository = new ResponsabilityRepository(mockConnection.Context);

    _defaultResponsibility = new Responsability("Limpeza de quarto","Limpar os quartos após a hospedagem",EPriority.Medium);

    _responsabilities.AddRange(
    [
      _defaultResponsibility,
      new Responsability("Secretária","Secretária",EPriority.Medium),
      new Responsability("Atender a chamadas de serviço","Atender a chamadas de serviço",EPriority.High),
      new Responsability("Assistência geral", "Assistência geral", EPriority.Medium),
      new Responsability("Cozinheiro","Cozinheiro",EPriority.High),
      new Responsability("Organizar arquivos", "Organizar arquivos", EPriority.Trivial),
      new Responsability("Atualizar registros", "Atualizar registros", EPriority.Low),
      new Responsability("Gerenciar crises", "Gerenciar crises", EPriority.Critical),
      new Responsability("Redigir documentos", "Redigir documentos", EPriority.Low),
      new Responsability("Planejar eventos", "Planejar eventos", EPriority.Trivial),
      new Responsability("Supervisionar equipe", "Supervisionar equipe", EPriority.Critical),
      new Responsability("Desenvolver estratégias de marketing", "Desenvolver estratégias de marketing", EPriority.High),
      new Responsability("Analisar dados financeiros", "Analisar dados financeiros", EPriority.Medium),
      new Responsability("Manter relacionamento com clientes", "Manter relacionamento com clientes", EPriority.Low),
      new Responsability("Treinar novos funcionários", "Treinar novos funcionários", EPriority.Critical),
      new Responsability("Gerenciar inventário", "Gerenciar inventário", EPriority.Low),
      new Responsability("Planejar a logística de entrega", "Planejar a logística de entrega", EPriority.Medium),
      new Responsability("Implementar políticas de segurança", "Implementar políticas de segurança", EPriority.Critical),
      new Responsability("Apoiar a equipe de TI", "Apoiar a equipe de TI", EPriority.Trivial),
      new Responsability("Preparar relatórios mensais", "Preparar relatórios mensais", EPriority.High),
      new Responsability("Organizar reuniões de equipe", "Organizar reuniões de equipe", EPriority.Medium)
    ]);

    await mockConnection.Context.Responsabilities.AddRangeAsync(_responsabilities);
    await mockConnection.Context.SaveChangesAsync();
  }

  [TestCleanup]
  public async Task CleanupTest()
  {
    mockConnection.Context.Responsabilities.RemoveRange(_responsabilities);
    await mockConnection.Context.SaveChangesAsync();
    _responsabilities.Clear();
  }

  [TestMethod]
  public async Task GetByIdAsync_ReturnsWithCorrectParameters()
  {
    var responsibility = await _responsabilityRepository.GetByIdAsync(_defaultResponsibility.Id);

    Assert.IsNotNull(responsibility);
    Assert.AreEqual(_defaultResponsibility.Id, responsibility.Id);
    Assert.AreEqual(_defaultResponsibility.Name, responsibility.Name);
    Assert.AreEqual(_defaultResponsibility.Priority, responsibility.Priority);
    Assert.AreEqual(_defaultResponsibility.Description, responsibility.Description);

  }

  [TestMethod]
  public async Task GetAsync_ReturnWithCorrectParameters()
  {
    var parameters = new ResponsabilityQueryParameters(0, 100, "Limpeza de quarto", null, null, null, null, null);
    var responsibilities = await _responsabilityRepository.GetAsync(parameters);

    var responsibility = responsibilities.ToList()[0];

    Assert.IsNotNull(responsibility);
    Assert.AreEqual(_defaultResponsibility.Id, responsibility.Id);
    Assert.AreEqual(_defaultResponsibility.Name, responsibility.Name);
    Assert.AreEqual(_defaultResponsibility.Priority, responsibility.Priority);
    Assert.AreEqual(_defaultResponsibility.Description, responsibility.Description);
  }

  [TestMethod]
  public async Task GetAsync_WhereNameIncludesPlanejar_ReturnsEmployees()
  {
    var parameters = new ResponsabilityQueryParameters(0, 100, "Planejar", null, null, null, null, null);
    var responsibilities = await _responsabilityRepository.GetAsync(parameters);

    Assert.IsTrue(responsibilities.Any());
    foreach (var responsibility in responsibilities)
    {
      Assert.IsNotNull(responsibility);
      Assert.IsTrue(responsibility.Name.Contains("Planejar"));
    }
  }

  [TestMethod]
  public async Task GetAsync_WherePriorityEqualsLow_ReturnsEmployees()
  {
    var parameters = new ResponsabilityQueryParameters(0, 100, null, EPriority.Low, null, null, null, null);
    var responsibilities = await _responsabilityRepository.GetAsync(parameters);

    Assert.IsTrue(responsibilities.Any());
    foreach (var responsibility in responsibilities)
    {
      Assert.IsNotNull(responsibility);
      Assert.AreEqual(EPriority.Low,responsibility.Priority);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereEmployeeId_ReturnsEmployees()
  {
    var res = await _responsabilityRepository.GetEntityByIdAsync(_defaultResponsibility.Id);
    res?.Employees.Add(_employee);
    await mockConnection.Context.SaveChangesAsync();

    var parameters = new ResponsabilityQueryParameters(0, 100, null, null, _employee.Id , null, null, null);
    var responsibilities = await _responsabilityRepository.GetAsync(parameters);

    Assert.IsTrue(responsibilities.Any());
    foreach (var responsibility in responsibilities)
    {
      var hasEmployee = await mockConnection.Context.Responsabilities
        .Where(x => x.Id == responsibility.Id)
        .SelectMany(x => x.Employees)
        .AnyAsync(x => x.Id == _employee.Id);

      Assert.IsTrue(hasEmployee);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereServiceId_ReturnsEmployees()
  {
    var res = await _responsabilityRepository.GetEntityByIdAsync(_defaultResponsibility.Id);
    res?.Services.Add(_service);
    await mockConnection.Context.SaveChangesAsync();

    var parameters = new ResponsabilityQueryParameters(0, 100, null, null, null, _service.Id, null, null);
    var responsibilities = await _responsabilityRepository.GetAsync(parameters);

    Assert.IsTrue(responsibilities.Any());
    foreach (var responsibility in responsibilities)
    {
      var hasService = await mockConnection.Context.Responsabilities
        .Where(x => x.Id == responsibility.Id)
        .SelectMany(x => x.Services)
        .AnyAsync(x => x.Id == _service.Id);

      Assert.IsTrue(hasService);
    }
  }


  [TestMethod]
  public async Task GetAsync_WhereCreatedAtGratherThanYesterday_ReturnsEmployees()
  {
    var parameters = new ResponsabilityQueryParameters(0, 100, null, null, null, null, DateTime.Now.AddDays(-1), "gt");
    var responsibilities = await _responsabilityRepository.GetAsync(parameters);

    Assert.IsTrue(responsibilities.Any());
    foreach (var responsibility in responsibilities)
    {
      Assert.IsNotNull(responsibility);
      Assert.IsTrue(DateTime.Now.AddDays(-1) < responsibility.CreatedAt);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereCreatedAtLessThanToday_ReturnsEmployees()
  {
    var parameters = new ResponsabilityQueryParameters(0, 100, null, null, null, null, DateTime.Now, "lt");
    var responsibilities = await _responsabilityRepository.GetAsync(parameters);

    Assert.IsTrue(responsibilities.Any());
    foreach (var responsibility in responsibilities)
    {
      Assert.IsNotNull(responsibility);
      Assert.IsTrue(DateTime.Now > responsibility.CreatedAt);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereNameIncludesOrganizar_And_PriorityEqualsMedium_ReturnsEmployees()
  {
    var parameters = new ResponsabilityQueryParameters(0, 100, "Organizar", EPriority.Medium, null, null, null,null);
    var responsibilities = await _responsabilityRepository.GetAsync(parameters);

    Assert.IsTrue(responsibilities.Any());

    foreach (var responsibility in responsibilities)
    {
      Assert.IsNotNull(responsibility);
      Assert.IsTrue(responsibility.Name.Contains("Organizar"));
      Assert.AreEqual(EPriority.Medium, responsibility.Priority);
    }
  }




}
