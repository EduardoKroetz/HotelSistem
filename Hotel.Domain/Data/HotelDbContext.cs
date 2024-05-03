using Hotel.Domain.Entities.AdminContext.AdminEntity;
using Hotel.Domain.Entities.AdminContext.PermissionEntity;
using Hotel.Domain.Entities.CustomerContext;
using Hotel.Domain.Entities.CustomerContext.FeedbackEntity;
using Hotel.Domain.Entities.EmployeeContext.EmployeeEntity;
using Hotel.Domain.Entities.EmployeeContext.ResponsabilityEntity;
using Hotel.Domain.Entities.PaymentContext.InvoiceRoomEntity;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Domain.Data;

public class HotelDbContext : DbContext
{
  public DbSet<Admin> Admins { get; set; }
  public DbSet<Permission> Permissions { get; set; }
  public DbSet<Customer> Customers { get; set; }
  public DbSet<Feedback> Feedbacks { get; set; }
  public DbSet<Employee> Employees { get; set; }
  public DbSet<Responsability> Responsabilities { get; set; }
  public DbSet<InvoiceRoom> InvoiceRooms { get; set; }
  public DbSet<InvoiceRoom> RoomInvoices { get; set; }
}