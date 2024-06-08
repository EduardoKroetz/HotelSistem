using Hotel.Domain.Entities.AdminContext.AdminEntity;
using Hotel.Domain.Entities.AdminContext.PermissionEntity;
using Hotel.Domain.Entities.CustomerContext;
using Hotel.Domain.Entities.CustomerContext.FeedbackEntity;
using Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;
using Hotel.Domain.Entities.EmployeeContext.ResponsabilityEntity;
using Hotel.Domain.Entities.PaymentContext.InvoiceRoomEntity;
using Hotel.Domain.Entities.ReservationContext.ReservationEntity;
using Hotel.Domain.Entities.RoomContext.CategoryEntity;
using Hotel.Domain.Entities.RoomContext.ReportEntity;
using Hotel.Domain.Entities.RoomContext.RoomEntity;
using Hotel.Domain.Entities.RoomContext.ServiceEntity;
using Hotel.Tests.Repositories.Mock.CreateData;
using Microsoft.EntityFrameworkCore;


namespace Hotel.Tests.Repositories.Mock;

public static class BaseRepositoryTest
{
  public static ConfigMockConnection MockConnection = null!;
  public static List<Room> Rooms { get; set; } = [];
  public static List<Room> AvailableRooms { get; set; } = [];
  public static List<Reservation> Reservations { get; set; } = [];
  public static List<Admin> Admins { get; set; } = [];
  public static List<Category> Categories { get; set; } = [];
  public static List<Customer> Customers { get; set; } = [];
  public static List<Employee> Employees { get; set; } = [];
  public static List<Service> Services { get; set; } = [];
  public static List<Permission> Permissions { get; set; } = [];
  public static List<Feedback> Feedbacks { get; set; } = [];
  public static List<Responsability> Responsabilities { get; set; } = [];
  public static List<RoomInvoice> RoomInvoices { get; set; } = [];
  public static List<Reservation> ReservationsToFinish { get; set; } = []; 
  public static List<Report> Reports { get; set; } = [];

  static BaseRepositoryTest()
  => Startup().Wait();

  public static async Task Startup()
  {
    MockConnection = await GenericRepositoryTest.InitializeMockConnection();

    await CreateAdmins.Create();
    await CreatePermissions.Create();
    await CreateCustomers.Create();
    await CreateEmployees.Create();
    await CreateResponsabilities.Create();

    await CreateCategories.Create();
    await CreateReports.Create();
    await CreateServices.Create();

    await CreateRooms.Create();
    await CreateReservations.Create();
    await CreateRoomInvoices.Create();

    await CreateFeedbacks.Create();
  }

  public static async Task Dispose()
  {
    MockConnection.Context.Admins.RemoveRange(await MockConnection.Context.Admins.ToListAsync());
    MockConnection.Context.Employees.RemoveRange(await MockConnection.Context.Employees.ToListAsync());
    MockConnection.Context.Reports.RemoveRange(await MockConnection.Context.Reports.ToListAsync());
    MockConnection.Context.RoomInvoices.RemoveRange(await MockConnection.Context.RoomInvoices.ToListAsync());
    MockConnection.Context.Reservations.RemoveRange(await MockConnection.Context.Reservations.ToListAsync());
    MockConnection.Context.Customers.RemoveRange(await MockConnection.Context.Customers.ToListAsync());
    MockConnection.Context.Services.RemoveRange(await MockConnection.Context.Services.ToListAsync());
    MockConnection.Context.Responsabilities.RemoveRange(await MockConnection.Context.Responsabilities.ToListAsync());
    MockConnection.Context.Feedbacks.RemoveRange(await MockConnection.Context.Feedbacks.ToListAsync());
    MockConnection.Context.Permissions.RemoveRange(await MockConnection.Context.Permissions.ToListAsync());
    MockConnection.Context.Rooms.RemoveRange(await MockConnection.Context.Rooms.ToListAsync());
    MockConnection.Context.Categories.RemoveRange(await MockConnection.Context.Categories.ToListAsync());

    await MockConnection.Context.SaveChangesAsync();
    MockConnection?.Dispose();
  }

}
