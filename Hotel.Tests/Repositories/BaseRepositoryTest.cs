using Hotel.Domain.Entities.CustomerContext;
using Hotel.Domain.Entities.ReservationContext.ReservationEntity;
using Hotel.Domain.Entities.RoomContext.CategoryEntity;
using Hotel.Domain.Entities.RoomContext.RoomEntity;
using Hotel.Domain.Enums;
using Hotel.Tests.Entities;

namespace Hotel.Tests.Repositories;

abstract public class BaseRepositoryTest
{
  protected static ConfigMockConnection mockConnection = null!;
  public static Room _room { get; set; } = null!;
  public static Reservation _reservation { get; set; } = null!;
  public static Category _category { get; set; } = null!;
  public static Customer _customer { get; set; } = null!;

  [ClassInitialize]
  public static async Task Startup(TestContext? context)
  {
    mockConnection = await GenericRepositoryTest.InitializeMockConnection();
    _category = new Category("Quarto básico", "Quarto básico para hospedagem diária.", 45m);
    _room = new Room(22, 50m, 3, "Um quarto para hospedagem.", _category.Id);
    _customer = new Customer(TestParameters.Name, TestParameters.Email, TestParameters.Phone, TestParameters.Password, EGender.Masculine, DateTime.Now.AddYears(-18), TestParameters.Address);
    _reservation = new Reservation(_room, DateTime.Now.AddDays(3), [_customer]);

    await mockConnection.Context.Categories.AddAsync(_category);
    await mockConnection.Context.SaveChangesAsync();

    await mockConnection.Context.Rooms.AddAsync(_room);
    await mockConnection.Context.SaveChangesAsync();

    await mockConnection.Context.Customers.AddAsync(_customer);
    await mockConnection.Context.SaveChangesAsync();

    await mockConnection.Context.Reservations.AddAsync(_reservation);
    await mockConnection.Context.SaveChangesAsync();

 
  } 

  [ClassCleanup]
  public static void Cleanup()
  => mockConnection?.Dispose();
}
