using Hotel.Domain.DTOs.RoomContext.ReportDTOs;
using Hotel.Domain.DTOs.RoomContext.ServiceDTOs;
using Hotel.Domain.Entities.RoomContext.ServiceEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.Repositories.RoomContext;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Tests.Repositories.RoomContext;

[TestClass]
public class ServiceRepositoryTest : BaseRepositoryTest
{
  private static ServiceRepository ServiceRepository { get; set; } = null!;

  [ClassInitialize]
  public static async Task Startup(TestContext? context)
  {
    await Startup();
    ServiceRepository = new ServiceRepository(MockConnection.Context);
  }

  [ClassCleanup]
  public static async Task Dispose()
  => await Cleanup();

  [TestMethod]
  public async Task GetByIdAsync_ReturnsWithCorrectParameters()
  {
    var service = await ServiceRepository.GetByIdAsync(Services[0].Id);

    Assert.IsNotNull(service);
    Assert.AreEqual(Services[0].Id, service.Id);
    Assert.AreEqual(Services[0].Name, service.Name);
    Assert.AreEqual(Services[0].Price, service.Price);
    Assert.AreEqual(Services[0].Priority, service.Priority);
    Assert.AreEqual(Services[0].IsActive, service.IsActive);
    Assert.AreEqual(Services[0].TimeInMinutes, service.TimeInMinutes);
  }

  [TestMethod]
  public async Task GetAsync_ReturnWithCorrectParameters()
  {
    var parameters = new ServiceQueryParameters(0, 100, Services[0].Name, Services[0].Price, null, null, null, null, null, null, null, null, null, null, null);
    var services = await ServiceRepository.GetAsync(parameters);

    var service = services.ToList()[0];

    Assert.IsNotNull(service);
    Assert.AreEqual(Services[0].Id, service.Id);
    Assert.AreEqual(Services[0].Name, service.Name);
    Assert.AreEqual(Services[0].Price, service.Price);
    Assert.AreEqual(Services[0].Priority, service.Priority);
    Assert.AreEqual(Services[0].IsActive, service.IsActive);
    Assert.AreEqual(Services[0].TimeInMinutes, service.TimeInMinutes);
  }


  [TestMethod]
  public async Task GetAsync_WhereNameEquals_ReturnServices()
  {
    var parameters = new ServiceQueryParameters(0, 100, Services[0].Name, null, null, null, null, null, null, null, null, null, null, null, null);
    var services = await ServiceRepository.GetAsync(parameters);

    Assert.IsTrue(services.Any());

    foreach (var service in services)
      Assert.AreEqual(Services[0].Name, service.Name);
    ;
  }

  [TestMethod]
  public async Task GetAsync_WherePriceGratherThan70_ReturnServices()
  {
    var parameters = new ServiceQueryParameters(0, 100, null, 70m, "gt", null, null, null, null, null, null, null, null, null, null);
    var services = await ServiceRepository.GetAsync(parameters);

    Assert.IsTrue(services.Any());

    foreach (var service in services)
      Assert.IsTrue(70 < service.Price);
  }

  [TestMethod]
  public async Task GetAsync_WherePriceLessThan70_ReturnServices()
  {
    var parameters = new ServiceQueryParameters(0, 100, null, 70m, "lt", null, null, null, null, null, null, null, null, null, null);
    var services = await ServiceRepository.GetAsync(parameters);

    Assert.IsTrue(services.Any());

    foreach (var service in services)
      Assert.IsTrue(70 > service.Price);
  }

  [TestMethod]
  public async Task GetAsync_WherePriceEquals70_ReturnServices()
  {
    var parameters = new ServiceQueryParameters(0, 100, null, 70m, "eq", null, null, null, null, null, null, null, null, null, null);
    var services = await ServiceRepository.GetAsync(parameters);

    Assert.IsTrue(services.Any());

    foreach (var service in services)
      Assert.AreEqual(70,service.Price);
  }


  [TestMethod]
  public async Task GetAsync_WherePriorityEqualsMedium_ReturnServices()
  {
    var parameters = new ServiceQueryParameters(0, 100, null, null, null, EPriority.Medium, null, null, null, null, null, null, null, null, null);
    var services = await ServiceRepository.GetAsync(parameters);

    Assert.IsTrue(services.Any());

    foreach (var service in services)
      Assert.AreEqual(EPriority.Medium, service.Priority);
  }


  [TestMethod]
  public async Task GetAsync_WhereIsActiveEqualsTrue_ReturnServices()
  {
    var parameters = new ServiceQueryParameters(0, 100, null, null, null, null, true, null, null, null, null, null, null, null, null);
    var services = await ServiceRepository.GetAsync(parameters);

    Assert.IsTrue(services.Any());

    foreach (var service in services)
      Assert.IsTrue(service.IsActive);
  }

  [TestMethod]
  public async Task GetAsync_WhereIsActiveEqualsFalse_ReturnServices()
  {
    var parameters = new ServiceQueryParameters(0, 100, null, null, null, null, false, null, null, null, null, null, null, null, null);
    var services = await ServiceRepository.GetAsync(parameters);

    Assert.IsTrue(services.Any());

    foreach (var service in services)
      Assert.IsFalse(service.IsActive);
  }

  [TestMethod]
  public async Task GetAsync_WhereTimeInMinutesGratherThan30_ReturnServices()
  {
    var parameters = new ServiceQueryParameters(0, 100, null, null, null, null, null, 30, "gt", null, null, null, null, null, null);
    var services = await ServiceRepository.GetAsync(parameters);

    Assert.IsTrue(services.Any());

    foreach (var service in services)
      Assert.IsTrue(30 < service.TimeInMinutes);
  }

  [TestMethod]
  public async Task GetAsync_WhereTimeInMinutesLessThan30_ReturnServices()
  {
    var parameters = new ServiceQueryParameters(0, 100, null, null, null, null, null, 30, "lt", null, null, null, null, null, null);
    var services = await ServiceRepository.GetAsync(parameters);

    Assert.IsTrue(services.Any());

    foreach (var service in services)
      Assert.IsTrue(30 > service.TimeInMinutes);
  }


  [TestMethod]
  public async Task GetAsync_WhereTimeInMinutesEquals30_ReturnServices()
  {
    var parameters = new ServiceQueryParameters(0, 100, null, null, null, null, null, 30, "eq", null, null, null, null, null, null);
    var services = await ServiceRepository.GetAsync(parameters);

    Assert.IsTrue(services.Any());

    foreach (var service in services)
      Assert.AreEqual(30,service.TimeInMinutes);
  }


  [TestMethod]
  public async Task GetAsync_WhereResponsabilityId_ReturnServices()
  {
    var parameters = new ServiceQueryParameters(0, 100, null, null, null, null, null, null, null, Responsabilities[0].Id, null, null, null, null, null);
    var services = await ServiceRepository.GetAsync(parameters);

    Assert.IsTrue(services.Any());

    foreach (var service in services)
    {
      var hasResponsability = await MockConnection.Context.Services
        .Where(x => x.Id == service.Id)
        .SelectMany(x => x.Responsabilities)
        .AnyAsync(x => x.Id == Responsabilities[0].Id);

      Assert.IsTrue(hasResponsability);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereReservationId_ReturnServices()
  {
    var parameters = new ServiceQueryParameters(0, 100, null, null, null, null, null, null, null, null, Reservations[0].Id, null, null, null, null);
    var services = await ServiceRepository.GetAsync(parameters);

    Assert.IsTrue(services.Any());

    foreach (var service in services)
    {
      var hasReservation = await MockConnection.Context.Services
        .Where(x => x.Id == service.Id)
        .SelectMany(x => x.Reservations)
        .AnyAsync(x => x.Id == Reservations[0].Id);

      Assert.IsTrue(hasReservation);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereRoomInvoiceId_ReturnServices()
  {
    var parameters = new ServiceQueryParameters(0, 100, null, null, null, null, null, null, null, null, null, RoomInvoices[0].Id, null, null, null);
    var services = await ServiceRepository.GetAsync(parameters);

    Assert.IsTrue(services.Any());

    foreach (var service in services)
    {
      var hasRoomInvoice = await MockConnection.Context.Services
        .Where(x => x.Id == service.Id)
        .SelectMany(x => x.RoomInvoices)
        .AnyAsync(x => x.Id == RoomInvoices[0].Id);

      Assert.IsTrue(hasRoomInvoice);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereRoomId_ReturnServices()
  {
    var parameters = new ServiceQueryParameters(0, 100, null, null, null, null, null, null, null, null, null, null, Rooms[0].Id, null, null);
    var services = await ServiceRepository.GetAsync(parameters);

    Assert.IsTrue(services.Any());

    foreach (var service in services)
    {
      var hasRoom = await MockConnection.Context.Services
        .Where(x => x.Id == service.Id)
        .SelectMany(x => x.Rooms)
        .AnyAsync(x => x.Id == Rooms[0].Id);

      Assert.IsTrue(hasRoom);
    }
  }

  [TestMethod]
  public async Task GetAsync_WhereCreatedAtGratherThanYesterday_ReturnServices()
  {
    var parameters = new ServiceQueryParameters(0, 100, null, null, null, null, null, null, null, null, null, null, null, DateTime.Now.AddDays(-1), "gt");
    var services = await ServiceRepository.GetAsync(parameters);

    Assert.IsTrue(services.Any());
    foreach (var service in services)
      Assert.IsTrue(DateTime.Now.AddDays(-1) < service.CreatedAt);

  }

  [TestMethod]
  public async Task GetAsync_WhereCreatedAtLessThanToday_ReturnServices()
  {
    var parameters = new ServiceQueryParameters(0, 100, null, null, null, null, null, null, null, null, null, null, null, DateTime.Now.AddDays(1), "lt");
    var services = await ServiceRepository.GetAsync(parameters);

    Assert.IsTrue(services.Any());
    foreach (var service in services)
      Assert.IsTrue(DateTime.Now.AddDays(1) > service.CreatedAt);

  }

  [TestMethod]
  public async Task GetAsync_WhereCreatedAtEquals_ReturnServices()
  {
    var parameters = new ServiceQueryParameters(0, 100, null, null, null, null, null, null, null, null, null, null, null, Services[0].CreatedAt, "eq");
    var services = await ServiceRepository.GetAsync(parameters);

    Assert.IsTrue(services.Any());
    foreach (var service in services)
      Assert.AreEqual(Employees[0].CreatedAt.Date, service.CreatedAt.Date);
  }



}
