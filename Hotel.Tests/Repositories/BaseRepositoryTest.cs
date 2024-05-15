using Hotel.Domain.Entities.CustomerContext;
using Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;
using Hotel.Domain.Entities.Interfaces;
using Hotel.Domain.Entities.ReservationContext.ReservationEntity;
using Hotel.Domain.Entities.RoomContext.CategoryEntity;
using Hotel.Domain.Entities.RoomContext.RoomEntity;
using Hotel.Domain.Entities.RoomContext.ServiceEntity;
using Hotel.Domain.Enums;
using Hotel.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;


namespace Hotel.Tests.Repositories;

abstract public class BaseRepositoryTest
{
  protected static ConfigMockConnection mockConnection = null!;
  public static List<Room> _rooms { get; set; } = [];
  public static List<Reservation> _reservations { get; set; } = [];
  public static List<Category> _categories { get; set; } = null!;
  public static Customer _customer { get; set; } = null!;
  public static Employee _employee { get; set; } = null!;
  public static Service _service { get; set; } = null!;


  public static async Task Startup(TestContext? context)
  {
    mockConnection = await GenericRepositoryTest.InitializeMockConnection();

    await CreateCategories();
    await CreateCustomers();
    await CreateEmployees();
    await CreateServices();
    await CreateRooms();
    await CreateReservations();
  } 

  public static async Task Cleanup()
  {
    mockConnection.Context.RoomInvoices.RemoveRange(await mockConnection.Context.RoomInvoices.ToListAsync());
    mockConnection.Context.Reservations.RemoveRange(await mockConnection.Context.Reservations.ToListAsync());
    mockConnection.Context.Rooms.RemoveRange(await mockConnection.Context.Rooms.ToListAsync());
    mockConnection.Context.Customers.RemoveRange(await mockConnection.Context.Customers.ToListAsync());
    mockConnection.Context.Services.RemoveRange(await mockConnection.Context.Services.ToListAsync());
    mockConnection.Context.Categories.RemoveRange(await mockConnection.Context.Categories.ToListAsync());
    mockConnection.Context.Employees.RemoveRange(await mockConnection.Context.Employees.ToListAsync());

    await mockConnection.Context.SaveChangesAsync();
    mockConnection?.Dispose();
  }

  public static async Task CreateCategories()
  {
    var category = new Category("Quarto básico", "Quarto básico para hospedagem diária.", 45m);

    await mockConnection.Context.Categories.AddRangeAsync(category);
    await mockConnection.Context.SaveChangesAsync();

    _categories = await mockConnection.Context.Categories.ToListAsync();
  }

  public static async Task CreateCustomers()
  {
    var customer = new Customer(new Name("Lucas", "Silveira"), new Email("lucassilveira@example.com"), new Phone("+55 (19) 98765-4311"), "aDAs34sd", EGender.Masculine, DateTime.Now.AddYears(-18), new Address("Brazil", "São Pauki", "Av. Sp", 999));
    
    await mockConnection.Context.Customers.AddAsync(customer);
    await mockConnection.Context.SaveChangesAsync();

    customer = await mockConnection.Context.Customers.FirstOrDefaultAsync(x => x.Id == customer.Id);
    if (customer != null)
      _customer = customer;
  }

  public static async Task CreateServices()
  {
    var service = new Service("Limpeza", 30m, EPriority.Medium, 30);

    await mockConnection.Context.Services.AddAsync(service);
    await mockConnection.Context.SaveChangesAsync();

    service = await mockConnection.Context.Services.FirstOrDefaultAsync(x => x.Id == service.Id);
    if (service != null)
      _service = service;
  }

  public static async Task CreateReservations()
  {
    var reservations = new List<Reservation>()
    {
      new(_rooms[0], DateTime.Now, [_customer],DateTime.Now.AddDays(2)),
      new(_rooms[1], DateTime.Now, [_customer],DateTime.Now.AddDays(8)),
      new(_rooms[0], DateTime.Now, [_customer],DateTime.Now.AddDays(5)),
      new(_rooms[1], DateTime.Now, [_customer],DateTime.Now.AddDays(8)),
      new(_rooms[1], DateTime.Now, [_customer],DateTime.Now.AddDays(5)),
    };

    foreach (var reserv in reservations)
      reserv.AddService(_service);

    await mockConnection.Context.Reservations.AddRangeAsync(reservations);
    await mockConnection.Context.SaveChangesAsync();

    _reservations = await mockConnection.Context.Reservations.ToListAsync();
  }

  public static async Task CreateEmployees()
  {
    var employee = new Employee(new Name("Rafael", "Silveira"), new Email("rafaelsilveira@example.com"), new Phone("+55 (19) 98765-4321"), "rafa789", EGender.Masculine, DateTime.Now.AddYears(-32), new Address("Brazil", "Campinas", "Rua Barão de Jaguara", 789), 2400m);
    await mockConnection.Context.Employees.AddAsync(employee);
    await mockConnection.Context.SaveChangesAsync();

    employee = await mockConnection.Context.Employees.FirstOrDefaultAsync(x => x.Id == employee.Id);
    if (employee != null)
      _employee = employee;
  }

  public static async Task CreateRooms()
  {
    var rooms = new List<Room>()
    {
      new(22, 50m, 3, "Um quarto para hospedagem.", _categories[0].Id),
      new(21, 40m, 4, "Um quarto com vista para a praia.", _categories[0].Id),
    };

    await mockConnection.Context.Rooms.AddRangeAsync(rooms);
    await mockConnection.Context.SaveChangesAsync();

    _rooms = await mockConnection.Context.Rooms.ToListAsync();
  }

}
