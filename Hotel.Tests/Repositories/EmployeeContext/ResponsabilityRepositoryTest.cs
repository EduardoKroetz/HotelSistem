using Hotel.Domain.DTOs.EmployeeContext.ResponsabilityDTOs;
using Hotel.Domain.Enums;
using Hotel.Domain.Repositories.EmployeeContext;
using Hotel.Tests.Repositories.Mock;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Tests.Repositories.EmployeeContext;

[TestClass]
public class ResponsabilityRepositoryTest 
{
  private static ResponsabilityRepository ResponsabilityRepository { get; set; } 

  static ResponsabilityRepositoryTest()
  => ResponsabilityRepository = new ResponsabilityRepository(BaseRepositoryTest.MockConnection.Context);
  
  [TestMethod]
  public async Task GetByIdAsync_ReturnsWithCorrectParameters()
  {
    var responsibility = await ResponsabilityRepository.GetByIdAsync(BaseRepositoryTest.Responsabilities[0].Id);

    Assert.IsNotNull(responsibility);
    Assert.AreEqual(BaseRepositoryTest.Responsabilities[0].Id, responsibility.Id);
    Assert.AreEqual(BaseRepositoryTest.Responsabilities[0].Name, responsibility.Name);
    Assert.AreEqual(BaseRepositoryTest.Responsabilities[0].Priority, responsibility.Priority);
    Assert.AreEqual(BaseRepositoryTest.Responsabilities[0].Description, responsibility.Description);

  }

  [TestMethod]
  public async Task GetAsync_ReturnWithCorrectParameters()
  {
    var parameters = new ResponsabilityQueryParameters(0, 100, BaseRepositoryTest.Responsabilities[0].Name, null, null, null, null, null);
    var responsibilities = await ResponsabilityRepository.GetAsync(parameters);

    var responsibility = responsibilities.ToList()[0];

    Assert.IsNotNull(responsibility);
    Assert.AreEqual(BaseRepositoryTest.Responsabilities[0].Id, responsibility.Id);
    Assert.AreEqual(BaseRepositoryTest.Responsabilities[0].Name, responsibility.Name);
    Assert.AreEqual(BaseRepositoryTest.Responsabilities[0].Priority, responsibility.Priority);
    Assert.AreEqual(BaseRepositoryTest.Responsabilities[0].Description, responsibility.Description);
  }

  [TestMethod]
  public async Task GetAsync_WhereNameIncludesPlanejar_ReturnsEmployees()
  {
    var parameters = new ResponsabilityQueryParameters(0, 100, "Planejar", null, null, null, null, null);
    var responsibilities = await ResponsabilityRepository.GetAsync(parameters);

    Assert.IsTrue(responsibilities.Any());
    foreach (var responsibility in responsibilities)
      Assert.IsTrue(responsibility.Name.Contains("Planejar"));
    
  }

  [TestMethod]
  public async Task GetAsync_WherePriorityEqualsLow_ReturnsEmployees()
  {
    var parameters = new ResponsabilityQueryParameters(0, 100, null, EPriority.Low, null, null, null, null);
    var responsibilities = await ResponsabilityRepository.GetAsync(parameters);

    Assert.IsTrue(responsibilities.Any());
    foreach (var responsibility in responsibilities)
      Assert.AreEqual(EPriority.Low,responsibility.Priority);
    
  }

  [TestMethod]
  public async Task GetAsync_WhereEmployeeId_ReturnsEmployees()
  {
    var res = await ResponsabilityRepository.GetEntityByIdAsync(BaseRepositoryTest.Responsabilities[0].Id);
    res?.Employees.Add(BaseRepositoryTest.Employees[0]);
    await BaseRepositoryTest.MockConnection.Context.SaveChangesAsync();

    var parameters = new ResponsabilityQueryParameters(0, 100, null, null, BaseRepositoryTest.Employees[0].Id , null, null, null);
    var responsibilities = await ResponsabilityRepository.GetAsync(parameters);

    Assert.IsTrue(responsibilities.Any());
    foreach (var responsibility in responsibilities)
    {
      var hasEmployee = await BaseRepositoryTest.MockConnection.Context.Responsabilities
        .Where(x => x.Id == responsibility.Id)
        .SelectMany(x => x.Employees)
        .AnyAsync(x => x.Id == BaseRepositoryTest.Employees[0].Id);

      Assert.IsTrue(hasEmployee);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereServiceId_ReturnsEmployees()
  {
    var res = await ResponsabilityRepository.GetEntityByIdAsync(BaseRepositoryTest.Responsabilities[0].Id);
    res?.Services.Add(BaseRepositoryTest.Services[0]);
    await BaseRepositoryTest.MockConnection.Context.SaveChangesAsync();

    var parameters = new ResponsabilityQueryParameters(0, 100, null, null, null,BaseRepositoryTest.Services[0].Id, null, null);
    var responsibilities = await ResponsabilityRepository.GetAsync(parameters);

    Assert.IsTrue(responsibilities.Any());
    foreach (var responsibility in responsibilities)
    {
      var hasService = await BaseRepositoryTest.MockConnection.Context.Responsabilities
        .Where(x => x.Id == responsibility.Id)
        .SelectMany(x => x.Services)
        .AnyAsync(x => x.Id == BaseRepositoryTest.Services[0].Id);

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
      Assert.IsTrue(DateTime.Now.AddDays(-1) < responsibility.CreatedAt);
    
  }

  [TestMethod]
  public async Task GetAsync_WhereCreatedAtLessThanToday_ReturnsEmployees()
  {
    var parameters = new ResponsabilityQueryParameters(0, 100, null, null, null, null, DateTime.Now, "lt");
    var responsibilities = await ResponsabilityRepository.GetAsync(parameters);

    Assert.IsTrue(responsibilities.Any());
    foreach (var responsibility in responsibilities)
      Assert.IsTrue(DateTime.Now > responsibility.CreatedAt);
    
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
