using Hotel.Domain.DTOs.EmployeeContext.ResponsabilityDTOs;
using Hotel.Domain.Enums;
using Hotel.Domain.Repositories.EmployeeContext;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Tests.Repositories.EmployeeContext;

[TestClass]
public class ResponsabilityRepositoryTest : BaseRepositoryTest
{
  private static ResponsabilityRepository ResponsabilityRepository { get; set; } = null!;

  [ClassInitialize]
  public static async Task Startup(TestContext? context)
  {
    await Startup(null);
    ResponsabilityRepository = new ResponsabilityRepository(MockConnection.Context);
  }

  [ClassCleanup]
  public static async Task Dispose()
  => await Cleanup();

  [TestMethod]
  public async Task GetByIdAsync_ReturnsWithCorrectParameters()
  {
    var responsibility = await ResponsabilityRepository.GetByIdAsync(Responsabilities[0].Id);

    Assert.IsNotNull(responsibility);
    Assert.AreEqual(Responsabilities[0].Id, responsibility.Id);
    Assert.AreEqual(Responsabilities[0].Name, responsibility.Name);
    Assert.AreEqual(Responsabilities[0].Priority, responsibility.Priority);
    Assert.AreEqual(Responsabilities[0].Description, responsibility.Description);

  }

  [TestMethod]
  public async Task GetAsync_ReturnWithCorrectParameters()
  {
    var parameters = new ResponsabilityQueryParameters(0, 100, "Limpeza de quarto", null, null, null, null, null);
    var responsibilities = await ResponsabilityRepository.GetAsync(parameters);

    var responsibility = responsibilities.ToList()[0];

    Assert.IsNotNull(responsibility);
    Assert.AreEqual(Responsabilities[0].Id, responsibility.Id);
    Assert.AreEqual(Responsabilities[0].Name, responsibility.Name);
    Assert.AreEqual(Responsabilities[0].Priority, responsibility.Priority);
    Assert.AreEqual(Responsabilities[0].Description, responsibility.Description);
  }

  [TestMethod]
  public async Task GetAsync_WhereNameIncludesPlanejar_ReturnsEmployees()
  {
    var parameters = new ResponsabilityQueryParameters(0, 100, "Planejar", null, null, null, null, null);
    var responsibilities = await ResponsabilityRepository.GetAsync(parameters);

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
    var responsibilities = await ResponsabilityRepository.GetAsync(parameters);

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
    var res = await ResponsabilityRepository.GetEntityByIdAsync(Responsabilities[0].Id);
    res?.Employees.Add(Employees[0]);
    await MockConnection.Context.SaveChangesAsync();

    var parameters = new ResponsabilityQueryParameters(0, 100, null, null, Employees[0].Id , null, null, null);
    var responsibilities = await ResponsabilityRepository.GetAsync(parameters);

    Assert.IsTrue(responsibilities.Any());
    foreach (var responsibility in responsibilities)
    {
      var hasEmployee = await MockConnection.Context.Responsabilities
        .Where(x => x.Id == responsibility.Id)
        .SelectMany(x => x.Employees)
        .AnyAsync(x => x.Id == Employees[0].Id);

      Assert.IsTrue(hasEmployee);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereServiceId_ReturnsEmployees()
  {
    var res = await ResponsabilityRepository.GetEntityByIdAsync(Responsabilities[0].Id);
    res?.Services.Add(Services[0]);
    await MockConnection.Context.SaveChangesAsync();

    var parameters = new ResponsabilityQueryParameters(0, 100, null, null, null, Services[0].Id, null, null);
    var responsibilities = await ResponsabilityRepository.GetAsync(parameters);

    Assert.IsTrue(responsibilities.Any());
    foreach (var responsibility in responsibilities)
    {
      var hasService = await MockConnection.Context.Responsabilities
        .Where(x => x.Id == responsibility.Id)
        .SelectMany(x => x.Services)
        .AnyAsync(x => x.Id == Services[0].Id);

      Assert.IsTrue(hasService);
    }
  }


  [TestMethod]
  public async Task GetAsync_WhereCreatedAtGratherThanYesterday_ReturnsEmployees()
  {
    var parameters = new ResponsabilityQueryParameters(0, 100, null, null, null, null, DateTime.Now.AddDays(-1), "gt");
    var responsibilities = await ResponsabilityRepository.GetAsync(parameters);

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
    var responsibilities = await ResponsabilityRepository.GetAsync(parameters);

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
    var responsibilities = await ResponsabilityRepository.GetAsync(parameters);

    Assert.IsTrue(responsibilities.Any());

    foreach (var responsibility in responsibilities)
    {
      Assert.IsNotNull(responsibility);
      Assert.IsTrue(responsibility.Name.Contains("Organizar"));
      Assert.AreEqual(EPriority.Medium, responsibility.Priority);
    }
  }




}
