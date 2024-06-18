using Hotel.Domain.Entities.AdminEntity;
using Hotel.Domain.Entities.CustomerEntity;
using Hotel.Domain.Entities.FeedbackEntity;
using Hotel.Domain.Entities.EmployeeEntity;
using Hotel.Domain.Entities.ResponsibilityEntity;
using Hotel.Domain.Entities.PermissionEntity;
using Hotel.Domain.Entities.ReservationEntity;
using Hotel.Domain.Entities.CategoryEntity;
using Hotel.Domain.Entities.ReportEntity;
using Hotel.Domain.Entities.RoomEntity;
using Hotel.Domain.Entities.ServiceEntity;
using Hotel.Tests.UnitTests.Repositories.Mock.CreateData;
using Microsoft.EntityFrameworkCore;
using Hotel.Domain.Entities.InvoiceEntity;


namespace Hotel.Tests.UnitTests.Repositories.Mock;

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
    public static List<Responsibility> Responsibilities { get; set; } = [];
    public static List<Invoice> Invoices { get; set; } = [];
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
        await CreateResponsibilities.Create();

        await CreateCategories.Create();
        await CreateReports.Create();
        await CreateServices.Create();

        await CreateRooms.Create();
        await CreateReservations.Create();
        await CreateInvoices.Create();

        await CreateFeedbacks.Create();
    }

    public static async Task Dispose()
    {
        MockConnection.Context.Admins.RemoveRange(await MockConnection.Context.Admins.ToListAsync());
        MockConnection.Context.Employees.RemoveRange(await MockConnection.Context.Employees.ToListAsync());
        MockConnection.Context.Reports.RemoveRange(await MockConnection.Context.Reports.ToListAsync());
        MockConnection.Context.Invoices.RemoveRange(await MockConnection.Context.Invoices.ToListAsync());
        MockConnection.Context.Reservations.RemoveRange(await MockConnection.Context.Reservations.ToListAsync());
        MockConnection.Context.Customers.RemoveRange(await MockConnection.Context.Customers.ToListAsync());
        MockConnection.Context.Services.RemoveRange(await MockConnection.Context.Services.ToListAsync());
        MockConnection.Context.Responsibilities.RemoveRange(await MockConnection.Context.Responsibilities.ToListAsync());
        MockConnection.Context.Feedbacks.RemoveRange(await MockConnection.Context.Feedbacks.ToListAsync());
        MockConnection.Context.Permissions.RemoveRange(await MockConnection.Context.Permissions.ToListAsync());
        MockConnection.Context.Rooms.RemoveRange(await MockConnection.Context.Rooms.ToListAsync());
        MockConnection.Context.Categories.RemoveRange(await MockConnection.Context.Categories.ToListAsync());

        await MockConnection.Context.SaveChangesAsync();
        MockConnection?.Dispose();
    }

}
